# ZxTape2Wav.Net [![License Apache 2.0](https://img.shields.io/badge/license-Apache%20License%202.0-green.svg)](http://www.apache.org/licenses/LICENSE-2.0)

Extended .NET port of [zxtap-to-wav](https://github.com/raydac/zxtap-to-wav) implemented in [Go-lang](https://en.wikipedia.org/wiki/Go_(programming_language)).

![release](https://github.com/semack/ZxTape2Wave.Net/workflows/release/badge.svg?branch=master) ![development](https://github.com/semack/zxtap2wav/workflows/development/badge.svg?branch=development) ![nuget](https://github.com/semack/ZxTape2Wave.Net/workflows/nuget/badge.svg?branch=nuget) 

Easy library & command line utility  to convert [.TAP](http://fileformats.archiveteam.org/wiki/TAP_(ZX_Spectrum)) / [.TZX](http://fileformats.archiveteam.org/wiki/TZX) files (a data format for ZX-Spectrum emulator) into [sound WAV file](https://en.wikipedia.org/wiki/WAV).

## Library
### Installation
Before using of the library [Nuget Package](https://www.nuget.org/packages/ZxTape2Wav.Net/) must be installed.
```
Install-Package ZxTape2Wav.Net
```

## Command line tool
Please build the solution before using the tool.

### Arguments 
```
-a    amplify sound signal
-f int
      frequency of result wav, in Hz (default 22050)
-i string
      source TAP file
-o string
      target WAV file
-v    validate data blocks checksum
```
### Example of usage
```
ZxTape2Wav -i -v RENEGADE.tzx
ZxTape2Wav -a -i RENEGADE.tap -o RENEGADE.wav -f 44100
```
## How to?

### I want 44100 Hz quantized WAV
Use parameter `-f 44100`

### Sound is too silent
Use flag `-a` and generated sound in WAV will be amplified to maximum.

## License
Please see [LICENSE.md](LICENSE.md).

## Contribute
Contributions are welcome. Just open an Issue or submit a PR. 

## Contact
You can reach me via my [email](mailto://semack@gmail.com).

## Thanks
Many thanks especially to [Igor Maznitsa](https://github.com/raydac) for his [library](https://github.com/raydac/zxtap-to-wav) as a source for ideas.


