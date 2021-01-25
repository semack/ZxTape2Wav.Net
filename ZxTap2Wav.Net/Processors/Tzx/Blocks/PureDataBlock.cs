using System.IO;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class PureDataBlock : StandardSpeedDataBlock
    {
        public PureDataBlock(BinaryReader reader)
        {
            ZeroLen = reader.ReadUInt16();
            OneLen = reader.ReadUInt16();
            Rem = reader.ReadByte();
            TailMs = reader.ReadUInt16();
            var d = reader.ReadBytes(3);
            var dl = d[2] << (16 + d[1]) << (8 + d[0]);
            Data = reader.ReadBytes(dl);
        }
    }
}