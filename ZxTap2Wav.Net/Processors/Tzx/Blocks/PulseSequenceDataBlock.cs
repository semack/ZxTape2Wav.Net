using System.IO;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class PulseSequenceDataBlock : BlockBase
    {
        public PulseSequenceDataBlock(BinaryReader reader)
        {
            var l = reader.ReadByte();
            Pulses = new ushort[l];
            for (var i = 0; i < l; i++) Pulses[i] = reader.ReadUInt16();
        }

        public ushort[] Pulses { get; set; }
    }
}