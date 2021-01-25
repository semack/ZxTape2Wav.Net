using System.IO;
using System.Text;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class TextDescriptionDataBlock : BlockBase
    {
        public TextDescriptionDataBlock(BinaryReader reader)
        {
            var l = reader.ReadByte();
            Decription = Encoding.ASCII.GetString(reader.ReadBytes(l));
        }

        public string Decription { get; set; }
    }
}