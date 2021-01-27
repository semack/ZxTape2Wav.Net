using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class GroupStartDataBlock : BlockBase
    {
        public GroupStartDataBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }
        // GroupStart = 0x21


        public string GroupName { get; private set; }

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            GroupName = new string(reader.ReadChars(l));
        }
    }
}