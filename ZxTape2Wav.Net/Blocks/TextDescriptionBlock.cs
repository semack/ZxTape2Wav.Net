using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    // TextDescription = 0x30
    internal class TextDescriptionBlock : BlockBase
    {
        public TextDescriptionBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        public string Description { get; private set; }

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            Description = new string(reader.ReadChars(l));
        }
    }
}