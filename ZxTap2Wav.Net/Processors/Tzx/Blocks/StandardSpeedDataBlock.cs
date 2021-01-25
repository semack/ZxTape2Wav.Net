using System.IO;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class StandardSpeedDataBlock : BlockBase
    {
        public ushort PilotPulseLen { get; protected set; }
        public ushort FirstSyncLen { get; protected set; }
        public ushort SecondSyncLen { get; protected set; }
        public ushort ZeroLen { get; protected set; }
        public ushort OneLen { get; protected set; }
        public ushort PilotLen { get; protected set; }
        public byte Rem { get; protected set; }
        public ushort TailMs { get; protected set; }
        public byte[] Data { get; protected set; }

        public StandardSpeedDataBlock()
        {
        }

        public StandardSpeedDataBlock(BinaryReader reader)
        {
            PilotPulseLen = 2168;
            FirstSyncLen = 667;
            SecondSyncLen = 735;
            ZeroLen = 855;
            OneLen = 1710;
            TailMs = 1000;
            Rem = 8;
            PilotLen = 8083;

            TailMs = reader.ReadUInt16();
            
            var dl = reader.ReadUInt16();
            Data = reader.ReadBytes(dl);
            if (Data[0] >= 128)
                PilotLen = 3223;
        }

        public override bool IsValuable { get; } = true;
    }
}