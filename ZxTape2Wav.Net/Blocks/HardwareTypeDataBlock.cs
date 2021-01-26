using System.IO;
using System.Reflection;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class HardwareTypeDataBlock: BlockBase
    {
        // HardwareType = 0x33
        public HardwareTypeDataBlock(BinaryReader reader): base(reader)
        {
            
        }
        public override bool IsValuable { get; } = false;
        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            reader.ReadBytes(l*3);
        }
    }
}