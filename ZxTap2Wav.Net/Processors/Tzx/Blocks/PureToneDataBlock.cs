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

        public ushort PulseLen { get; set; }
        public ushort Pluses { get; set; }
    }
}