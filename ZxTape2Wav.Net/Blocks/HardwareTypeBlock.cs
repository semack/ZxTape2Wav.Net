using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    // HardwareType = 0x33
    internal class HardwareTypeBlock : BlockBase
    {
        public HardwareTypeBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            reader.ReadBytes(l * 3);
        }
    }
}