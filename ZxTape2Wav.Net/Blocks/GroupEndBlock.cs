using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    // GroupEnd = 0x22
    internal class GroupEndBlock : BlockBase
    {
        public GroupEndBlock(int index) : base(index)
        {
        }

        protected override void LoadData(BinaryReader reader)
        {
        }
    }
}