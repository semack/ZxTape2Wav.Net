namespace ZxTap2Wav.Net.Processors.Tap
{
    internal struct TapBlock
    {
        public byte[] Data { get; init; }
        public byte CheckSum { get; init; }
    }
}