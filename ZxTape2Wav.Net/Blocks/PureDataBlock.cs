using System.IO;

namespace ZxTape2Wav.Blocks
{
    internal class PureDataBlock : StandardSpeedDataBlock
    {
        // PureDataBlock = 0x14
        public PureDataBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        public override bool IsValid => true;

        protected override void LoadData(BinaryReader reader)
        {
            ZeroLen = reader.ReadUInt16();
            OneLen = reader.ReadUInt16();
            Rem = reader.ReadByte();
            TailMs = reader.ReadUInt16();
            var d = reader.ReadBytes(3);
            var dl = (d[2] << 16) + (d[1] << 8) + d[0];
            Data = reader.ReadBytes(dl);
        }
    }
}