using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class GroupEndDataBlock : BlockBase
    {
        public GroupEndDataBlock(int index) : base(index)
        {
        }

        // GroupEnd = 0x22
        protected override void LoadData(BinaryReader reader)
        {
        }
    }
}