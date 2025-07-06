namespace EIV_DataPack;

/// <summary>
/// File Metadata struct for easy storing the Meta information.
/// </summary>
public struct FileMetadata
{
    /// <summary>
    /// Creates a new empty <see cref="FileMetadata"/>.
    /// </summary>
    public FileMetadata() { }

    /// <summary>
    /// Should write FileMetadata.
    /// </summary>
    public bool UseMetadata;
    /// <summary>
    /// Unix Mode for Unix system.
    /// </summary>
    public byte UnixMode;
    /// <summary>
    /// Creation time as UTC long.
    /// </summary>
    public long CreationTimeUtc;
    /// <summary>
    /// Last access time as UTC long.
    /// </summary>
    public long LastAccessTimeUtc;
    /// <summary>
    /// Last write time as UTC long.
    /// </summary>
    public long LastWriteTimeUtc;

    /// <inheritdoc/>
    public readonly override string ToString()
    {
        return $"{UseMetadata} {UnixMode} {CreationTimeUtc} {LastAccessTimeUtc} {LastWriteTimeUtc}";
    }

    /// <summary>
    /// Write Metadata into <paramref name="binaryWriter"/>.
    /// </summary>
    /// <param name="binaryWriter">Input writer to write the data.</param>
    /// <param name="version">DataPack version.</param>
    public readonly void WriteMetadata(BinaryWriter binaryWriter, ushort version = Consts.CURRENT_VERSION)
    {
        if (version != Consts.METADATA_VERSION)
            binaryWriter.Write(UseMetadata);
        if (UseMetadata | version == Consts.METADATA_VERSION)
        {
            binaryWriter.Write(UnixMode);
            binaryWriter.Write(CreationTimeUtc);
            binaryWriter.Write(LastAccessTimeUtc);
            binaryWriter.Write(LastWriteTimeUtc);
        }
        binaryWriter.Flush();
    }

    /// <summary>
    /// Creates a <see cref="FileMetadata"/> that populates from <paramref name="binaryReader"/>.
    /// </summary>
    /// <param name="binaryReader">Reader that reads the data from.</param>
    /// <param name="version">DataPack version.</param>
    public FileMetadata(BinaryReader binaryReader, ushort version = Consts.CURRENT_VERSION)
    {
        UseMetadata = version != Consts.METADATA_VERSION || binaryReader.ReadBoolean();
        if (UseMetadata)
        {
            UnixMode = binaryReader.ReadByte();
            CreationTimeUtc = binaryReader.ReadInt64();
            LastAccessTimeUtc = binaryReader.ReadInt64();
            LastWriteTimeUtc = binaryReader.ReadInt64();
        }
    }
}