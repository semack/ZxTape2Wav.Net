using System.IO;
using System.Threading.Tasks;
using ZxTap2Wav.Net.Settings;

namespace ZxTap2Wav.Net.Processors
{
    internal interface IFormatProcessor
    {
        Task<bool> LoadAsync(Stream stream);
        Task FillWavStreamAsync(Stream stream, OutputSettings settings);
    }
}