namespace EIV_DataPack;

/// <summary>
/// File Metadata struct for easy storing the data.
/// </summary>
public struct FileMetadata
{
    public byte UnixMode;
    public long CreationTimeUtc;
    public long LastAccessTimeUtc;
    public long LastWriteTimeUtc;

    public override string ToString()
    {
        return $"{UnixMode} {CreationTimeUtc} {LastAccessTimeUtc} {LastWriteTimeUtc}";
    }
}

/// <summary>
/// Static class for FileMetadata operation. (Reading and Writing)
/// </summary>
public static class FileMetadataExt
{
    public static void WriteMetadata(this FileMetadata metadata, BinaryWriter binaryWriter)
    {
        binaryWriter.Write(metadata.UnixMode);
        binaryWriter.Write(metadata.CreationTimeUtc);
        binaryWriter.Write(metadata.LastAccessTimeUtc);
        binaryWriter.Write(metadata.LastWriteTimeUtc);
        binaryWriter.Flush();
    }

    public static FileMetadata ReadMetadata(this BinaryReader binaryReader)
    {
        return new()
        {
            UnixMode = binaryReader.ReadByte(),
            CreationTimeUtc = binaryReader.ReadInt64(),
            LastAccessTimeUtc = binaryReader.ReadInt64(),
            LastWriteTimeUtc = binaryReader.ReadInt64()
        };
    }
}
