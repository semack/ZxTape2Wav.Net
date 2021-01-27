using System.IO;
using System.Text;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    internal class ArchiveInfoDataBlock : BlockBase
    {
        //  ArchiveInfo = 0x32
        public ArchiveInfoDataBlock(BinaryReader reader, int index) : base(reader, index)
        {
        }

        public string Description { get; private set; }


        protected override void LoadData(BinaryReader reader)
        {
            var l = reader.ReadInt16();
            Description = Encoding.ASCII.GetString(reader.ReadBytes(l));
        }
    }
}