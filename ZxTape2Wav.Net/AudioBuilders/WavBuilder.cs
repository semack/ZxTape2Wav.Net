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
            await using var writer = new BinaryWriter(target);

            // reserved for wav header
            const int WAV_HEADER_SIZE = 40;
            writer.Seek(WAV_HEADER_SIZE, SeekOrigin.Begin);

            foreach (var block in blocks)
                if (block is DataBlock dataBlock)
                    await SaveSoundDataAsync(writer, dataBlock, settings);

            var len = (int) writer.BaseStream.Length - WAV_HEADER_SIZE;
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

        private static async Task SaveSoundDataAsync(BinaryWriter writer, DataBlock block, OutputSettings settings)
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

            var signalState = hi;

            for (var i = 0; i < block.PilotLen; i++)
            {
                await DoSignalAsync(writer, signalState, block.PilotPulseLen, settings.Frequency);
                signalState = signalState == hi ? lo : hi;
            }

            // pilot
            if (signalState == lo)
                await DoSignalAsync(writer, lo, block.PilotPulseLen, settings.Frequency);

            await DoSignalAsync(writer, hi, block.FirstSyncLen, settings.Frequency);
            await DoSignalAsync(writer, lo, block.SecondSyncLen, settings.Frequency);

            // writing data
            foreach (var d in block.Data)
                await WriteDataByteAsync(writer, block, d, hi, lo, settings.Frequency);

            await WriteDataByteAsync(writer, block, block.CheckSum, hi, lo, settings.Frequency);

            // last sync
            for (var i = 7; i >= 8 - block.Rem; i--)
            {
                var d = (byte) block.ZeroLen;
                if ((block.CheckSum & (1 << i)) != 0)
                    d = (byte) block.OneLen;
                await WriteDataByteAsync(writer, block, d, hi, lo, settings.Frequency);
                await WriteDataByteAsync(writer, block, d, hi, lo, settings.Frequency);
            }

            // adding pause
            if (block.TailMs > 0)
                for (var i = 0; i < settings.Frequency * (block.TailMs / 1000); i++)
                    writer.Write(0x00);
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