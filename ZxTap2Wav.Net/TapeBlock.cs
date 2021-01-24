namespace ZxTap2Wav.Net
{
    internal struct TapeBlock
    {
        public byte[] Data { get; set; }
        public byte CheckSum { get; set; }
    }
}