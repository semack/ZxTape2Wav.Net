using System.IO;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class TurboSpeedDataBlock : BlockBase
    {
        public TurboSpeedDataBlock()
        {
        }

        public TurboSpeedDataBlock(BinaryReader reader)
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
            var dl = d[2] << (16 + d[1]) << (8 + d[0]);
            Data = reader.ReadBytes(dl);
        }

        public ushort PilotPulseLen { get; set; }
        public ushort FirstSyncLen { get; set; }
        public ushort SecondSyncLen { get; set; }
        public ushort ZeroLen { get; set; }
        public ushort OneLen { get; set; }
        public ushort PilotLen { get; set; }
        public byte Rem { get; set; }
        public ushort TailMs { get; set; }
        public byte[] Data { get; set; }
    }
}