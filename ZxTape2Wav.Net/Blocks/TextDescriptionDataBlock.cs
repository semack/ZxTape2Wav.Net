using System.IO;
using System.Text;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class TextDescriptionDataBlock : BlockBase
    {
        public TextDescriptionDataBlock(BinaryReader reader) : base(reader)
        {
        }

        public string Description { get; private set; }

        public override bool IsValuable { get; } = false;

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            Description = Encoding.ASCII.GetString(reader.ReadBytes(l));
        }
    }
}