namespace EIV_DataPack.Interfaces;

public interface IPackFile
{
    public string Name { get; }
    public long FileDataPosition { get; set; }
    public byte[] FileData { get; }
}

public interface IPackMetadata : IPackFile
{
    public FileMetadata Metadata { get; set; }
}

public interface IPackExtraData : IPackFile
{
    public long ExtraDataPosition { get; set; }
    public byte[] ExtraData { get; set; }
}