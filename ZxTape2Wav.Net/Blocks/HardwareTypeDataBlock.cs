using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class HardwareTypeDataBlock : BlockBase
    {
        public HardwareTypeDataBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }
        // HardwareType = 0x33

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            reader.ReadBytes(l * 3);
        }
    }
}