using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    // PauseOrStopTheTape = 0x20
    internal class PauseOrStopTheTapeBlock : BlockBase
    {
        public PauseOrStopTheTapeBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        public ushort Duration { get; private set; }

        protected override void LoadData(BinaryReader reader)
        {
            Duration = reader.ReadUInt16();
        }
    }
}