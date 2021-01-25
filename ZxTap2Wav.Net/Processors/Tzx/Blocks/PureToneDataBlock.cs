using System.IO;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class PureToneDataBlock : BlockBase
    {
        public PureToneDataBlock(BinaryReader reader)
        {
            PulseLen = reader.ReadUInt16();
            Pluses = reader.ReadUInt16();
        }

        public ushort PulseLen { get; }
        public ushort Pluses { get; }
        public override bool IsValuable { get; } = true;
    }
}