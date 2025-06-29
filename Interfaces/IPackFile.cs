namespace EIV_DataPack.Interfaces;

/// <summary>
/// Describes a File inside of DataPack.
/// </summary>
public interface IPackFile
{
    /// <summary>
    /// Name of the file.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Position where <see cref="FileData"/> is written into.
    /// </summary>
    public long FileDataPosition { get; set; }
    /// <summary>
    /// Data of the file.
    /// </summary>
    public byte[] FileData { get; }
}

/// <summary>
/// Describes a <see cref="FileMetadata"/> for <see cref="IPackFile"/>.
/// </summary>
public interface IPackMetadata : IPackFile
{
    /// <summary>
    /// Metadata of the file.
    /// </summary>
    public FileMetadata Metadata { get; set; }
}

/// <summary>
/// Describes a <see cref="ExtraData"/> for <see cref="IPackFile"/>.
/// </summary>
public interface IPackExtraData : IPackFile
{
    /// <summary>
    /// Position where <see cref="ExtraData"/> is written into.
    /// </summary>
    public long ExtraDataPosition { get; set; }
    /// <summary>
    /// Extra data associated to the file.
    /// </summary>
    public byte[] ExtraData { get; set; }
}