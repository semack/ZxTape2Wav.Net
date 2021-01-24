using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZxTap2Wav.Net
{
    public sealed class Tape
    {
        private static string _fileName;
        private readonly List<TapeBlock> _blocks;

        private Tape(Stream source)
        {
            _blocks = new List<TapeBlock>();

            using var reader = new BinaryReader(source);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var block = ReadTapeBlock(reader);
                if (!IsCrcValid(block))
                    throw new ArgumentException($"Invalid CRC of block #{_blocks.Count + 1}");
                _blocks.Add(block);
            }
        }

        private bool IsCrcValid(TapeBlock block)
        {
            return block.CheckSum == block.Data
                .Aggregate<byte, byte>(0, (current, t) => (byte) (current ^ t));
        }

        public static Tape Create(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException($"File {fileName} not found");

            _fileName = fileName;

            using var stream = new FileStream(fileName, FileMode.Open);
            return new Tape(stream);
        }

        private TapeBlock ReadTapeBlock(BinaryReader reader)
        {
            var array = reader.ReadBytes(2);
            var length = (array[1] << 8) | array[0];
            var result = new TapeBlock
            {
                Data = reader.ReadBytes(length - 1),
                CheckSum = reader.ReadByte()
            };
            return result;
        }


        public void SaveWav(string fileName = null, Settings options = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = Path.ChangeExtension(_fileName, ".wav");

            options ??= new Settings();
            using var writer = new BinaryWriter(new FileStream(fileName, FileMode.Create));
            WriteHeader(writer, _blocks, options);
            var len = 0;
            for (var index = 0; index < _blocks.Count; index++)
            {
                if (index > 0 || options.SilenceOnStart)
                    for (var i = 0; i < options.WavFrequency * options.GapBetweenBlocks; i++)
                    {
                        writer.Write(0x00); // 0x80?
                        len += i;
                    }

                len += SaveSoundData(writer, _blocks[index], options);
            }

            SetLenght(writer, len);
            writer.Flush();
            writer.Close();
        }

        private void SetLenght(BinaryWriter writer, int len)
        {
            writer.Seek(4, 0);
            writer.Write(len + 26);
            writer.Seek(40, 0);
            writer.Write(len);
            ;
        }

        private void WriteHeader(BinaryWriter writer, List<TapeBlock> blocks, Settings options)
        {
            writer.Write(Encoding.ASCII.GetBytes("RIFF"));
            writer.Write(0);
            writer.Write(Encoding.ASCII.GetBytes("WAVEfmt "));
            writer.Write(16);
            writer.Write((short) 1);
            writer.Write((short) 1);
            writer.Write((uint) options.WavFrequency);
            writer.Write((uint) options.WavFrequency);
            writer.Write((short) 2);
            writer.Write((short) 8);
            writer.Write(Encoding.ASCII.GetBytes("data"));
            writer.Write(0);
        }

        private int SaveSoundData(BinaryWriter writer, TapeBlock block, Settings options)
        {
            const int PULSELEN_PILOT = 2168;
            const int PULSELEN_SYNC1 = 667;
            const int PULSELEN_SYNC2 = 735;
            const int PULSELEN_SYNC3 = 954;
            const int IMPULSNUMBER_PILOT_HEADER = 8063;
            const int IMPULSNUMBER_PILOT_DATA = 3223;

            var pilotImpulses = block.Data[0] < 128 ? IMPULSNUMBER_PILOT_HEADER : IMPULSNUMBER_PILOT_DATA;

            byte hi, lo;
            if (options.AmplifySoundSignal)
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
            var result = 0;

            for (var i = 0; i < pilotImpulses; i++)
            {
                result += DoSignal(writer, signalState, PULSELEN_PILOT, options);
                signalState = signalState == hi ? lo : hi;
            }

            if (signalState == lo)
                result += DoSignal(writer, lo, PULSELEN_PILOT, options);

            result += DoSignal(writer, hi, PULSELEN_SYNC1, options);
            result += DoSignal(writer, lo, PULSELEN_SYNC2, options);

            foreach (var t in block.Data)
                result += WriteDataByte(writer, t, hi, lo, options);

            result += WriteDataByte(writer, block.CheckSum, hi, lo, options);
            result += DoSignal(writer, hi, PULSELEN_SYNC3, options);
            return result;
        }

        private int DoSignal(BinaryWriter writer, byte signalLevel, int clks, Settings options)
        {
            var sampleNanoSec = 1000000000D / options.WavFrequency;
            var cpuClkNanoSec = 286D;
            var samples = Math.Round(cpuClkNanoSec * clks / sampleNanoSec);

            for (var i = 0; i < samples; i++) writer.Write(signalLevel);

            return (int) samples;
        }

        private int WriteDataByte(BinaryWriter writer, byte data, byte hi, byte lo, Settings options)
        {
            const int PULSELEN_ZERO = 855;
            const int PULSELEN_ONE = 1710;

            byte mask = 0x80;
            var result = 0;
            while (mask != 0)
            {
                var len = (data & mask) == 0 ? PULSELEN_ZERO : PULSELEN_ONE;
                result += DoSignal(writer, hi, len, options);
                result += DoSignal(writer, lo, len, options);
                mask >>= 1;
            }

            return result;
        }
    }
}