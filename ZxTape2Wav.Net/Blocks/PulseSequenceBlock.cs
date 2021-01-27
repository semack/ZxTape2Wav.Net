using System.IO;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    // PulseSequence = 0x13
    internal class PulseSequenceBlock : BlockBase
    {
        public PulseSequenceBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        public ushort[] Pulses { get; private set; }

        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadByte();
            Pulses = new ushort[l];
            for (var i = 0; i < l; i++) Pulses[i] = reader.ReadUInt16();
        }
    }
}