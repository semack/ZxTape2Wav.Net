namespace ZxTape2Wav.Settings
{
    public class OutputSettings
    {
        public bool AmplifySoundSignal { get; set; }
        public int Frequency { get; set; } = 22050;
        public bool ValidateCheckSum { get; set; } = false;
    }
}