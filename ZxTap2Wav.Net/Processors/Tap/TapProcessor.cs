using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZxTap2Wav.Net.Settings;

namespace ZxTap2Wav.Net.Processors.Tap
{
    public sealed class TapProcessor : IFormatProcessor
    {
        private readonly List<TapBlock> _blocks = new();

        public async Task<bool> LoadAsync(Stream source)
        {
            using var reader = new BinaryReader(source);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var block = await ReadTapeBlockAsync(reader);
                if (!IsCrcValid(block))
                    return false;

                _blocks.Add(block);
            }

            return true;
        }

        public async Task FillWavStreamAsync(Stream stream, OutputSettings settings)
        {
            const int WAV_HEADER_SIZE = 40;

            await using var writer = new BinaryWriter(stream);

            // reserve for wav header
            writer.Seek(WAV_HEADER_SIZE, SeekOrigin.Begin);

            var len = 0;
            for (var index = 0; index < _blocks.Count; index++)
            {
                if (index > 0 || settings.SilenceOnStart)
                    for (var i = 0; i < settings.Frequency * settings.GapBetweenBlocks; i++)
                    {
                        writer.Write(0x00); // - silence (original value was 0x80)
                        len++;
                    }

                await SaveSoundDataAsync(writer, _blocks[index], settings);
            }

            len = (int) writer.BaseStream.Length - WAV_HEADER_SIZE;
            await WriteHeaderAsync(writer, len, settings.Frequency);

            writer.Flush();
            writer.Close();
        }

        private bool IsCrcValid(TapBlock block)
        {
            return block.CheckSum == block.Data
                .Aggregate<byte, byte>(0, (current, t) => (byte) (current ^ t));
        }


        private async Task<TapBlock> ReadTapeBlockAsync(BinaryReader reader)
        {
            var array = reader.ReadBytes(2);
            var length = (array[1] << 8) | array[0];

            var result = new TapBlock
            {
                Data = reader.ReadBytes(length - 1),
                CheckSum = reader.ReadByte()
            };
            return await Task.FromResult(result);
        }

        private async Task WriteHeaderAsync(BinaryWriter writer, int len, int frequency)
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

        private async Task SaveSoundDataAsync(BinaryWriter writer, TapBlock block, OutputSettings settings)
        {
            const int PULSELEN_PILOT = 2168;
            const int PULSELEN_SYNC1 = 667;
            const int PULSELEN_SYNC2 = 735;
            const int PULSELEN_SYNC3 = 954;
            const int IMPULSNUMBER_PILOT_HEADER = 8063;
            const int IMPULSNUMBER_PILOT_DATA = 3223;

            var pilotImpulses = block.Data[0] < 128 ? IMPULSNUMBER_PILOT_HEADER : IMPULSNUMBER_PILOT_DATA;

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

            for (var i = 0; i < pilotImpulses; i++)
            {
                await DoSignalAsync(writer, signalState, PULSELEN_PILOT, settings.Frequency);
                signalState = signalState == hi ? lo : hi;
            }

            if (signalState == lo)
                await DoSignalAsync(writer, lo, PULSELEN_PILOT, settings.Frequency);

            await DoSignalAsync(writer, hi, PULSELEN_SYNC1, settings.Frequency);
            await DoSignalAsync(writer, lo, PULSELEN_SYNC2, settings.Frequency);

            foreach (var d in block.Data)
                await WriteDataByteAsync(writer, d, hi, lo, settings.Frequency);

            await WriteDataByteAsync(writer, block.CheckSum, hi, lo, settings.Frequency);
            await DoSignalAsync(writer, hi, PULSELEN_SYNC3, settings.Frequency);
        }

        private static async Task DoSignalAsync(BinaryWriter writer, byte signalLevel, int clks, int frequency)
        {
            var sampleNanoSec = 1000000000D / frequency;
            var cpuClkNanoSec = 286D;
            var samples = Math.Round(cpuClkNanoSec * clks / sampleNanoSec);

            for (var i = 0; i < samples; i++) writer.Write(signalLevel);

            await Task.CompletedTask;
        }

        private static async Task WriteDataByteAsync(BinaryWriter writer, byte data, byte hi, byte lo,
            int frequency)
        {
            const int PULSELEN_ZERO = 855;
            const int PULSELEN_ONE = 1710;

            byte mask = 0x80;

            while (mask != 0)
            {
                var len = (data & mask) == 0 ? PULSELEN_ZERO : PULSELEN_ONE;
                await DoSignalAsync(writer, hi, len, frequency);
                await DoSignalAsync(writer, lo, len, frequency);
                mask >>= 1;
            }
        }
    }
}