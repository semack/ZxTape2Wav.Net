using System.IO;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class GroupStartDataBlock : BlockBase
    {
        public GroupStartDataBlock(BinaryReader reader)
        {
            var l = reader.ReadByte();
            reader.ReadBytes(l);
        }

        public override bool IsValuable { get; } = false;
    }
}