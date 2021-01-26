using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class GroupEndDataBlock : BlockBase
    {
        public override bool IsValuable { get; } = false;

        protected override void LoadData(BinaryReader reader)
        {
        }
    }
}