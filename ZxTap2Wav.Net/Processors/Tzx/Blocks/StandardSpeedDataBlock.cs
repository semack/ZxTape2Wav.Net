using System.IO;

namespace ZxTap2Wav.Net.Processors.Tzx.Blocks
{
    internal class StandardSpeedDataBlock : TurboSpeedDataBlock
    {
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

        public ushort PauseInMs { get; set; }
    }
}