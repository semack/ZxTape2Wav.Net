using System.Linq;

namespace ZxTape2Wav.Helpers
{
    internal static class ByteHelper
    {
        public static bool CheckCrc(byte[] data, byte checkSum)
        {
            return checkSum == data.Aggregate<byte, byte>(0, (current, t) => (byte) (current ^ t));
        }
    }
}