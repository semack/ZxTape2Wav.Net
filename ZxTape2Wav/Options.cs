using CommandLine;

namespace ZxTape2Wav
{
    public class Options
    {
        [Option('a', "amplify", Required = false, HelpText = "amplify sound signal")]
        public bool Amplify { get; set; }

        [Option('f', "frequency", Required = false, HelpText = "frequency of result wav, in Hz (default 22050)")]
        public int Frequency { get; set; }

        [Option('i', "input", Required = true, HelpText = "source TAP/TZX file")]
        public string Input { get; set; }

        [Option('o', "output", Required = false, HelpText = "target WAV file")]
        public string Output { get; set; }
    }
}