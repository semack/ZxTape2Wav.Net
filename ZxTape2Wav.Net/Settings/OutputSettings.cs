using System;

namespace ZxTape2Wav.Settings
{
    public class OutputSettings
    {
        private int _frequency = 22050;
        public bool AmplifySoundSignal { get; set; }

        public int Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                if (_frequency < 11025)
                    throw new ArgumentException("Unexpected WAV frequency, must be >= 11025 Hz.");
            }
        }

        public bool ValidateCheckSum { get; set; } = false;
    }
}