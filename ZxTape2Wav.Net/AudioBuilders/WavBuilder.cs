using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZxTape2Wav.Blocks;
using ZxTape2Wav.Blocks.Abstract;
using ZxTape2Wav.Settings;

namespace ZxTape2Wav.AudioBuilders
{
    internal static class WavBuilder
    {
        public static async Task BuildAsync(IEnumerable<BlockBase> blocks, Stream target, OutputSettings settings)
        {
            using var writer = new BinaryWriter(target);
            // reserved for wav header
            const int wavHeaderSize = 40;
            writer.Seek(40, SeekOrigin.Begin);

            foreach (var block in blocks)
                await SaveSoundDataAsync(writer, block, settings);

            var len = (int) writer.BaseStream.Length - wavHeaderSize;
            await WriteHeaderAsync(writer, len, settings.Frequency);

            writer.Flush();
            writer.Close();
        }

        private static async Task WriteHeaderAsync(BinaryWriter writer, int len, int frequency)
        {
            writer.Seek(0, SeekOrigin.Begin);
            writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(len + 26);
            writer.Write(Encoding.ASCII.GetBytes("WAVEfmt "));
            writer.Write(16);
            writer.Write((short) 1);
            writer.Write((short) 1);
            writer.Write((uint) frequency);
            writer.Write((uint) frequency);
            writer.Write((short) 1);
            writer.Write((short) 8);
            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(len);
            await Task.CompletedTask;
        }

        private static async Task SaveSoundDataAsync(BinaryWriter writer, BlockBase block, OutputSettings settings)
        {
            byte hi, lo;
            if (settings.AmplifySoundSignal)
            {
                hi = 0xFF;
                lo = 0x00;
            }
            else
            {
                hi = 0xC0;
                lo = 0x40;
            }

            switch (block)
            {
                case DataBlock dataBlock:
                {
                    if (settings.ValidateCheckSum && !dataBlock.IsValid)
                        throw new ArgumentException($"Block #{block.Index} has incorrect CheckSum.");

                    var signalState = hi;

                    for (var i = 0; i < dataBlock.PilotLen; i++)
                    {
                        await DoSignalAsync(writer, signalState, dataBlock.PilotPulseLen, settings.Frequency);
                        signalState = signalState == hi ? lo : hi;
                    }

                    // pilot
                    if (signalState == lo)
                        await DoSignalAsync(writer, lo, dataBlock.PilotPulseLen, settings.Frequency);

                    await DoSignalAsync(writer, hi, dataBlock.FirstSyncLen, settings.Frequency);
                    await DoSignalAsync(writer, lo, dataBlock.SecondSyncLen, settings.Frequency);

                    // writing data
                    foreach (var d in dataBlock.Data)
                        await WriteDataByteAsync(writer, dataBlock, d, hi, lo, settings.Frequency);

                    // last sync
                    for (var i = 7; i >= 8 - dataBlock.Rem; i--)
                    {
                        var len = (byte) dataBlock.ZeroLen;
                        if ((dataBlock.Data[dataBlock.Data.Length - 1] & (1 << i)) != 0)
                            len = (byte) dataBlock.OneLen;
                        await DoSignalAsync(writer, hi, len, settings.Frequency);
                        await DoSignalAsync(writer, lo, len, settings.Frequency);
                    }

                    // adding pause
                    if (dataBlock.TailMs > 0)
                        await WritePauseAsync(writer, dataBlock.TailMs, settings.Frequency);

                    break;
                }
                case PauseOrStopTheTapeBlock pauseOrStopTheTapeDataBlock:
                    await WritePauseAsync(writer, pauseOrStopTheTapeDataBlock.Duration, settings.Frequency);
                    break;
                case PulseSequenceBlock pulseSequenceBlock:
                {
                    foreach (var pulse in pulseSequenceBlock.Pulses)
                    {
                        await DoSignalAsync(writer, hi, pulse, settings.Frequency);
                        await DoSignalAsync(writer, lo, pulse, settings.Frequency);
                    }

                    break;
                }
                case PureToneBlock pureToneBlock:
                {
                    for (var i = 0; i < pureToneBlock.Pluses; i++)
                    {
                        await DoSignalAsync(writer, hi, pureToneBlock.PulseLen, settings.Frequency);
                        await DoSignalAsync(writer, lo, pureToneBlock.PulseLen, settings.Frequency);
                    }

                    break;
                }
            }
        }

        private static async Task WritePauseAsync(BinaryWriter writer, int ms, int frequency)
        {
            for (var i = 0; i < frequency * (ms / 1000); i++)
                writer.Write(0x00);
            await Task.CompletedTask;
        }

        private static async Task DoSignalAsync(BinaryWriter writer, byte signalLevel, int clks, int frequency)
        {
            var sampleNanoSec = 1000000000D / frequency;
            var cpuClkNanoSec = 286D;
            var samples = Math.Round(cpuClkNanoSec * clks / sampleNanoSec);

            for (var i = 0; i < samples; i++) writer.Write(signalLevel);

            await Task.CompletedTask;
        }

        private static async Task WriteDataByteAsync(BinaryWriter writer, DataBlock block, byte data, byte hi, byte lo,
            int frequency)
        {
            byte mask = 0x80;

            while (mask != 0)
            {
                var len = (data & mask) == 0 ? block.ZeroLen : block.OneLen;
                await DoSignalAsync(writer, hi, len, frequency);
                await DoSignalAsync(writer, lo, len, frequency);
                mask >>= 1;
            }
        }
    }
}