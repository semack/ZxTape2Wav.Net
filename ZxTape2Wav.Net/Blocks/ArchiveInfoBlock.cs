using System.IO;
using System.Text;
using ZxTape2Wav.Blocks.Abstract;

namespace ZxTape2Wav.Blocks
{
    //  ArchiveInfo = 0x32
    internal class ArchiveInfoBlock : BlockBase
    {
        public ArchiveInfoBlock(BinaryReader reader, int index) : base(reader, index)
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