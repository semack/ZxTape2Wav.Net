using System.IO;
using System.Text;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class ArchiveInfoDataBlock : BlockBase
    {
        public ArchiveInfoDataBlock(BinaryReader reader)
        {
            var l = reader.ReadInt16();
            Description = Encoding.ASCII.GetString(reader.ReadBytes(l));
        }

        public string Description { get; }
        
        public override bool IsValuable { get; } = false;
    }
}