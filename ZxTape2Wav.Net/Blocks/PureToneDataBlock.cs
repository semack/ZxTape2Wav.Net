using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class PureToneDataBlock : BlockBase
    {
        // PureTone = 0x12;
        public PureToneDataBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        public ushort PulseLen { get; private set; }
        public ushort Pluses { get; private set; }

        protected override void LoadData(BinaryReader reader)
        {
            PulseLen = reader.ReadUInt16();
            Pluses = reader.ReadUInt16();
        }
    }
}