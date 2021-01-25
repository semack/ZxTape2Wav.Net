using System.IO;
using System.Text;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class TextDescriptionDataBlock : BlockBase
    {
        public TextDescriptionDataBlock(BinaryReader reader)
        {
            var l = reader.ReadByte();
            Description = Encoding.ASCII.GetString(reader.ReadBytes(l));
        }

        public string Description { get; }
        
        public override bool IsValuable { get; } = false;
    }
}