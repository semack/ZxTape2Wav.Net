using System.IO;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class PauseOrStopTheTapeDataBlock : BlockBase
    {
        public PauseOrStopTheTapeDataBlock(BinaryReader reader)
        {
            Duration = reader.ReadUInt16();
        }

        public ushort Duration { get; }
        public override bool IsValuable { get; } = true;
    }
}