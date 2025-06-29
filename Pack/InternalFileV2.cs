using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Pack;

internal class InternalFileV2 : IPackFile
{
    public string Name { get; set; } = string.Empty;
    public long FileDataPosition { get; set; }
    public byte[] FileData { get; set; } = [];
}
