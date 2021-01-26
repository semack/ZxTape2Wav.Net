namespace ZxTape2Wav.Settings
{
    public class OutputSettings
    {
        public bool AmplifySoundSignal { get; set; } = false;
        public bool SilenceOnStart { get; set; } = false;
        public short GapBetweenBlocks { get; set; } = 1;
        public int Frequency { get; set; } = 22050;
    }
}