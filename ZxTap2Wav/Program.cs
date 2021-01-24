using System;
using System.Threading.Tasks;
using CommandLine;
using ZxTap2Wav.Net;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new Settings();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    if (o.Amplify)
                        settings.AmplifySoundSignal = true;
                    if (o.Frequency != 0)
                        settings.WavFrequency = o.Frequency;
                    if (o.Gap != 0)
                        settings.GapBetweenBlocks = o.Gap;
                    if (o.Silence)
                        settings.SilenceOnStart = o.Silence;
                    var tape = Tape.Create(o.Input);
                    tape.SaveWav(o.Output, settings);
                });
        }
    }
}