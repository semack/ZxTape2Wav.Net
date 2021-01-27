using System.IO;

namespace ZxTape2Wav.Blocks.Abstract
{
    internal abstract class BlockBase
    {
        protected BlockBase(int index)
        {
            Index = index;
        }

        protected BlockBase(BinaryReader reader, int index) : this(index)
        {
            LoadData(reader);
        }

        public int Index { get; }
        protected abstract void LoadData(BinaryReader reader);
    }
}