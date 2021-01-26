using CommandLine;

namespace ZxTape2Wav
{
    public class Options
    {
        [Option('a', "amplify", Required = false, HelpText = "amplify sound signal")]
        public bool Amplify { get; set; }

        [Option('f', "frequency", Required = false, HelpText = "frequency of result wav, in Hz (default 22050)")]
        public int Frequency { get; set; }

        [Option('g', "gap", Required = false, HelpText = "time gap between sound blocks, in seconds (default 1)")]
        public short Gap { get; set; }

        [Option('i', "input", Required = true, HelpText = "source TAP file")]
        public string Input { get; set; }

        [Option('o', "output", Required = false, HelpText = "target WAV file")]
        public string Output { get; set; }

        [Option('s', "silence", Required = false, HelpText = "add silence before the first file")]
        public bool Silence { get; set; }
    }
}