using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class PulseSequenceDataBlock : BlockBase
    {
        public PulseSequenceDataBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }
        // PulseSequence = 0x13


        public ushort[] Pulses { get; private set; }

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            Pulses = new ushort[l];
            for (var i = 0; i < l; i++) Pulses[i] = reader.ReadUInt16();
        }
    }
}