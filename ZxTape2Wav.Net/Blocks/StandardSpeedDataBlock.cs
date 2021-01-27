using System.IO;

namespace ZxTape2Wav.Blocks
{
    // StandardSpeedDataBlock = 0x10
    internal class StandardSpeedDataBlock : DataBlock
    {
        public StandardSpeedDataBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        protected override void LoadData(BinaryReader reader)
        {
            TailMs = reader.ReadUInt16();
            base.LoadData(reader);
        }
    }
}