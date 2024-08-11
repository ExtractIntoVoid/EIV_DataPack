namespace EIV_DataPack;

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
        FileMetadata fileMetadata = new();
        fileMetadata.UnixMode = binaryReader.ReadByte();
        fileMetadata.CreationTimeUtc = binaryReader.ReadInt64();
        fileMetadata.LastAccessTimeUtc = binaryReader.ReadInt64();
        fileMetadata.LastWriteTimeUtc = binaryReader.ReadInt64();
        return fileMetadata;
    }
}
