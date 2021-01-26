using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZxTape2Wav.AudioBuilders;
using ZxTape2Wav.Blocks;
using ZxTape2Wav.Blocks.Abstract;
using ZxTape2Wav.Enums;
using ZxTape2Wav.Helpers;
using ZxTape2Wav.Settings;

namespace ZxTape2Wav
{
    public sealed class TapeFile
    {
        private readonly List<BlockBase> _blocks = new();

        private string _fileName;
        private TapeFileTypeEnum _tapeFileType = TapeFileTypeEnum.Unknown;

        private async Task LoadAsync(string fileName)
        {
            _fileName = fileName;

            if (!File.Exists(_fileName))
                throw new FileNotFoundException(_fileName);

            using var reader = new BinaryReader(new FileStream(_fileName, FileMode.Open));

            _tapeFileType = await GetTapeFileTypeAsync(reader);

            if (_tapeFileType == TapeFileTypeEnum.Unknown)
                throw new ArgumentException($"File {fileName} has incompatible format.");

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var block = await ReadBlockAsync(reader);
                _blocks.Add(block);
            }
        }

        private async Task<TapeFileTypeEnum> GetTapeFileTypeAsync(BinaryReader reader)
        {
            // check for Tzx
            var magic = reader.ReadUInt64();
            if (magic == 0x1a2165706154585a)
            {
                //skip version info block
                reader.ReadBytes(2);
                return await Task.FromResult(TapeFileTypeEnum.Tzx);
            }

            // check for Tap
            try
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                var dl = reader.ReadUInt16();
                var data = reader.ReadBytes(dl - 1);
                var checkSum = reader.ReadByte();
                if (ByteHelper.CheckCrc(data, checkSum))
                    return await Task.FromResult(TapeFileTypeEnum.Tap);

                return await Task.FromResult(TapeFileTypeEnum.Unknown);
            }
            finally
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
            }
        }

        private async Task<BlockBase> ReadBlockAsync(BinaryReader reader)
        {
            BlockBase result = null;

            switch (_tapeFileType)
            {
                case TapeFileTypeEnum.Tap:
                    result = new DataBlock(reader);
                    break;
                case TapeFileTypeEnum.Tzx:
                {
                    var blockType = (TzxBlockTypeEnum) reader.ReadByte();

                    switch (blockType)
                    {
                        case TzxBlockTypeEnum.StandardSpeedDataBlock:
                            result = new StandardSpeedDataBlock(reader);
                            break;
                        case TzxBlockTypeEnum.TurboSpeedDataBlock:
                            result = new TurboSpeedDataBlock(reader);
                            break;
                        case TzxBlockTypeEnum.PureTone:
                            result = new PureToneDataBlock(reader);
                            break;
                        case TzxBlockTypeEnum.PureDataBlock:
                            result = new PureDataBlock(reader);
                            break;
                        case TzxBlockTypeEnum.PulseSequence:
                            result = new PulseSequenceDataBlock(reader);
                            break;
                        case TzxBlockTypeEnum.PauseOrStopTheTape:
                            result = new PauseOrStopTheTapeDataBlock(reader);
                            break;
                        case TzxBlockTypeEnum.GroupStart:
                            result = new GroupStartDataBlock(reader);
                            break;
                        case TzxBlockTypeEnum.GroupEnd:
                            result = new GroupEndDataBlock();
                            break;
                        case TzxBlockTypeEnum.TextDescription:
                            result = new TextDescriptionDataBlock(reader);
                            break;
                        case TzxBlockTypeEnum.ArchiveInfo:
                            result = new ArchiveInfoDataBlock(reader);
                            break;
                        case TzxBlockTypeEnum.HardwareType:
                            result = new HardwareTypeDataBlock(reader);
                            break;
                        default:
                            throw new Exception($"Unrecognized block type {blockType:H}");
                    }

                    break;
                }
            }

            return await Task.FromResult(result);
        }

        public static async Task<TapeFile> CreateAsync(string fileName)
        {
            var result = new TapeFile();

            await result.LoadAsync(fileName);
            return result;
        }

        public async Task SaveToWavAsync(string fileName = null, OutputSettings settings = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = Path.ChangeExtension(_fileName, ".wav");

            settings ??= new OutputSettings();

            await WavBuilder.BuildAsync(_blocks, new FileStream(fileName, FileMode.Create), settings);
        }
    }
}