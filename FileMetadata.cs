namespace EIV_DataPack;

/// <summary>
/// File Metadata struct for easy storing the Meta information.
/// </summary>
public struct FileMetadata
{
    public FileMetadata() { }

    public bool UseMetadata;
    public byte UnixMode;
    public long CreationTimeUtc;
    public long LastAccessTimeUtc;
    public long LastWriteTimeUtc;

    public readonly override string ToString()
    {
        return $"{UseMetadata} {UnixMode} {CreationTimeUtc} {LastAccessTimeUtc} {LastWriteTimeUtc}";
    }

    public readonly void WriteMetadata(BinaryWriter binaryWriter, ushort version = 4)
    {
        if (version != 3)
            binaryWriter.Write(UseMetadata);
        if (UseMetadata | version == 3)
        {
            binaryWriter.Write(UnixMode);
            binaryWriter.Write(CreationTimeUtc);
            binaryWriter.Write(LastAccessTimeUtc);
            binaryWriter.Write(LastWriteTimeUtc);
        }
        binaryWriter.Flush();
    }

    public FileMetadata(BinaryReader binaryReader, ushort version = 4)
    {
        UseMetadata = version != 3 || binaryReader.ReadBoolean();
        if (UseMetadata)
        {
            UnixMode = binaryReader.ReadByte();
            CreationTimeUtc = binaryReader.ReadInt64();
            LastAccessTimeUtc = binaryReader.ReadInt64();
            LastWriteTimeUtc = binaryReader.ReadInt64();
        }
    }
}