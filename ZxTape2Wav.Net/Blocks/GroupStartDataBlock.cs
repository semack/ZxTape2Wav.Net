using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class GroupStartDataBlock : BlockBase
    {
        public GroupStartDataBlock(BinaryReader reader) : base(reader)
        {
        }

        public override bool IsValuable { get; } = false;

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            reader.ReadBytes(l);
        }
    }
}