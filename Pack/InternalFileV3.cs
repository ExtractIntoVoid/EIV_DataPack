using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Pack;

internal class InternalFileV3 : IPackFile, IPackMetadata
{
    public string Name { get; set; } = string.Empty;
    public long FileDataPosition { get; set; }
    public long MetadataPosition { get; set; }
    public byte[] FileData { get; set; } = [];
    public FileMetadata Metadata { get; set; } = new();
}
