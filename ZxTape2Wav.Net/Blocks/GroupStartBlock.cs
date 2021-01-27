using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    // GroupStart = 0x21
    internal class GroupStartBlock : BlockBase
    {
        public GroupStartBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        public string GroupName { get; private set; }

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            GroupName = new string(reader.ReadChars(l));
        }
    }
}