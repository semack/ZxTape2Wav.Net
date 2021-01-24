namespace ZxTap2Wav.Net
{
    public class Settings
    {
        public bool AmplifySoundSignal { get; set; } = false;
        public bool SilenceOnStart { get; set; } = false;
        public short GapBetweenBlocks { get; set; } = 1;
        public int WavFrequency { get; set; } = 22050;
    }
}