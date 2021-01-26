using System.Linq;

namespace ZxTape2Wav.Helpers
{
    internal static class ByteHelper
    {
        public static byte CalculateCheckSum(byte[] data)
        {
            return data.Aggregate<byte, byte>(0, (current, t) => (byte) (current ^ t));
        }

        public static bool CheckCrc(byte[] data, byte checkSum)
        {
            return checkSum == CalculateCheckSum(data);
        }
    }
}