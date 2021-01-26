namespace ZxTape2Wav.Enums
{
    internal enum TzxBlockTypeEnum : byte
    {
        StandardSpeedDataBlock = 0x10,
        TurboSpeedDataBlock = 0x11,
        PureTone = 0x12,
        PulseSequence = 0x13,
        PureDataBlock = 0x14,
        PauseOrStopTheTape = 0x20,
        GroupStart = 0x21,
        GroupEnd = 0x22,
        TextDescription = 0x30,
        ArchiveInfo = 0x32,
        HardwareType = 0x33
    }
}