using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class PauseOrStopTheTapeDataBlock : BlockBase
    {
        public PauseOrStopTheTapeDataBlock(BinaryReader reader) : base(reader)
        {
        }

        public ushort Duration { get; private set; }

        protected override void LoadData(BinaryReader reader)
        {
            Duration = reader.ReadUInt16();
        }
    }
}