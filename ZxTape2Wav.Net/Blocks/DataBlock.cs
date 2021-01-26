using System.IO;
using ZxTape2Wav.Blocks.Abstract;
using ZxTape2Wav.Helpers;

namespace ZxTape2Wav.Blocks
{
    internal class DataBlock : BlockBase
    {
        // Basic Tap data block
        protected DataBlock()
        {
        }

        public DataBlock(BinaryReader reader) : base(reader)
        {
        }

        public ushort PilotPulseLen { get; protected set; }
        public ushort FirstSyncLen { get; protected set; }
        public ushort SecondSyncLen { get; protected set; }
        public ushort ZeroLen { get; protected set; }
        public ushort OneLen { get; protected set; }
        public ushort PilotLen { get; protected set; }
        public byte Rem { get; protected set; }
        public ushort TailMs { get; protected set; }
        public byte[] Data { get; protected set; }
        public byte CheckSum { get; protected set; }

        public override bool IsValid => ByteHelper.CheckCrc(Data, CheckSum);

        protected override void LoadData(BinaryReader reader)
        {
            PilotPulseLen = 2168;
            FirstSyncLen = 667;
            SecondSyncLen = 735;
            ZeroLen = 855;
            OneLen = 1710;
            TailMs = 1000;
            Rem = 8;
            PilotLen = 8083;
            var dl = reader.ReadUInt16();
            Data = reader.ReadBytes(dl - 1);
            CheckSum = reader.ReadByte();
            if (Data[0] >= 128)
                PilotLen = 3223;
        }
    }
}