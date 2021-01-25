using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZxTap2Wav.Net.Processors.Tzx.Blocks;
using ZxTap2Wav.Net.Processors.Tzx.Enums;
using ZxTap2Wav.Net.Settings;

namespace ZxTap2Wav.Net.Processors.Tzx
{
    public class TzxProcessor : IFormatProcessor
    {
        private readonly List<BlockBase> _blocks = new();

        public async Task<bool> LoadAsync(Stream stream)
        {
            using var reader = new BinaryReader(stream);

            var magic = reader.ReadUInt64();
            if (magic != 0x1a2165706154585a)
                return false;

            var version = reader.ReadBytes(2);

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var block = await ReadBlockAsync(reader);
                if (block.IsValuable)
                   _blocks.Add(block);
            }

            return true;
        }

        public Task FillWavStreamAsync(Stream stream, OutputSettings settings)
        {
            throw new NotImplementedException();
        }

        private async Task<BlockBase> ReadBlockAsync(BinaryReader reader)
        {
            BlockBase result = null;

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
            }

            return await Task.FromResult(result);
        }
    }
}