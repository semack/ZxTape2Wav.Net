using System.IO;

namespace ZxTape2Wav.Blocks.Abstract
{
    internal abstract class BlockBase
    {
        protected BlockBase()
        {
        }

        protected BlockBase(BinaryReader reader)
        {
            LoadData(reader);
        }

        public virtual bool IsValuable { get; } = true;
        public virtual bool IsValid { get; } = true;
        protected abstract void LoadData(BinaryReader reader);
    }
}