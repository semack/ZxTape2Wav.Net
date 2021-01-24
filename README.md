# zxtap2wav

.net port of https://github.com/raydac/zxtap-to-wav

[![License Apache 2.0](https://img.shields.io/badge/license-Apache%20License%202.0-green.svg)](http://www.apache.org/licenses/LICENSE-2.0)

Easy command line utility to convert [.TAP files (a data format for ZX-Spectrum emulator)](http://fileformats.archiveteam.org/wiki/TAP_(ZX_Spectrum)) into [sound WAV file](https://en.wikipedia.org/wiki/WAV).

# Arguments
```
-a    amplify sound signal
-f int
      frequency of result wav, in Hz (default 22050)
-g int
      time gap between sound blocks, in seconds (default 1)
-i string
      source TAP file
-o string
      target WAV file
-s    add silence before the first file
```
# Example
```
zxtap2wav -i RENEGADE.tap
zxtap2wav -a -i RENEGADE.tap -o RENEGADE.wav -f 44100 -s
```
# How to?

## Make longer silence interval between files in WAV
Just add `-g 2` or `-g 3` to make delay in 2 or 3 seconds.

## Add silence in start of generated WAV file
Use `-s` and silence will be generated in start of WAV file.

## I want 44100 Hz quantized WAV
Use parameter `-f 44100`

## Sound is too silent
Use flag `-a` and generated sound in WAV will be amplified to maximum.

## License
Please see [LICENSE.md](LICENSE.md).

## Contribute
Contributions are welcome. Just open an Issue or submit a PR. 

## Contact
You can reach me via my [email](mailto://semack@gmail.com).

