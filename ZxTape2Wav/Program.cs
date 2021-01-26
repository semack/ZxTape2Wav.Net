using CommandLine;
using ZxTape2Wav.Settings;

namespace ZxTape2Wav
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
                    if (o.Silence)
                        settings.SilenceOnStart = o.Silence;
                    var tape = await TapeFile.CreateAsync(o.Input);
                    await tape.SaveToWavAsync(o.Output, settings);
                });
        }
    }
}