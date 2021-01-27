using System.IO;

namespace ZxTape2Wav.Blocks
{
    // TurboSpeedDataBlock = 0x11
    internal class TurboSpeedDataBlock : StandardSpeedDataBlock
    {
        public TurboSpeedDataBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        protected override void LoadData(BinaryReader reader)
        {
            PilotPulseLen = reader.ReadUInt16();
            FirstSyncLen = reader.ReadUInt16();
            SecondSyncLen = reader.ReadUInt16();
            ZeroLen = reader.ReadUInt16();
            OneLen = reader.ReadUInt16();
            PilotLen = reader.ReadUInt16();
            Rem = reader.ReadByte();
            TailMs = reader.ReadUInt16();
            var d = reader.ReadBytes(3);
            var dl = (d[2] << 16) + (d[1] << 8) + d[0];
            Data = reader.ReadBytes(dl);
        }
    }
}