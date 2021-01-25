using CommandLine;
using ZxTap2Wav.Net;

namespace ZxTap2Wav
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var settings = new OutputSettings();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async o =>
                {
                    if (o.Amplify)
                        settings.AmplifySoundSignal = true;
                    if (o.Frequency != 0)
                        settings.Frequency = o.Frequency;
                    if (o.Gap != 0)
                        settings.GapBetweenBlocks = o.Gap;
                    if (o.Silence)
                        settings.SilenceOnStart = o.Silence;
                    var tape = await Tape.CreateAsync(o.Input);
                    await tape.SaveWavAsync(o.Output, settings);
                });
        }
    }
}