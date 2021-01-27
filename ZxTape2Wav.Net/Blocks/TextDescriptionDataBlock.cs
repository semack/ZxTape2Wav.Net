using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class TextDescriptionDataBlock : BlockBase
    {
        // TextDescription = 0x30
        public TextDescriptionDataBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        public string Label { get; private set; }

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            Label = new string(reader.ReadChars(l));
        }
    }
}