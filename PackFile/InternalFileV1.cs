using EIV_DataPack.Interfaces;

namespace EIV_DataPack.PackFile;

/// <summary>
/// DataPack Version 1 never existed!
/// </summary>
internal class InternalFileV1 : IPackFile
{
    public string Name { get; set; } = string.Empty;
    public long FileDataPosition { get; set; }
    public byte[] FileData { get; set; } = [];
}
