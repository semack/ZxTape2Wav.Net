using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ZxTap2Wav.Net.Processors;
using ZxTap2Wav.Net.Processors.Tap;
using ZxTap2Wav.Net.Processors.Tzx;
using ZxTap2Wav.Net.Settings;

namespace ZxTap2Wav.Net
{
    public sealed class Tape
    {
        private readonly string _fileName;
        private readonly IFormatProcessor _processor;

        private Tape(string fileName, IFormatProcessor processor)
        {
            _processor = processor;
            _fileName = fileName;
        }

        public static async Task<Tape> CreateAsync(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException(fileName);

            Tape result = null;

            // register known processors
            var processors = new List<IFormatProcessor>
            {
                new TapProcessor(),
                new TzxProcessor()
            };

            foreach (var processor in processors)
            {
                await using var dataStream = new FileStream(fileName, FileMode.Open);
                if (await processor.LoadAsync(dataStream))
                {
                    result = new Tape(fileName, processor);
                    break;
                }
            }

            if (result == null)
                throw new ArgumentException($"File {fileName} has incompatible format.");

            return result;
        }

        public async Task SaveToWavAsync(string fileName = null, OutputSettings settings = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = Path.ChangeExtension(_fileName, ".wav");

            settings ??= new OutputSettings();

            await _processor.FillWavStreamAsync(new FileStream(fileName, FileMode.Create), settings);
        }
    }
}