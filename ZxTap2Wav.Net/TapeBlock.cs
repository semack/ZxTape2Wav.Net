namespace ZxTap2Wav.Net
{
    internal struct TapeBlock
    {
        public byte[] Data { get; init; }
        public byte CheckSum { get; init; }
    }
}