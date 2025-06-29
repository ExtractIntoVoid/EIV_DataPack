using EIV_DataPack.Interfaces;
using EIV_DataPack.Pack;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace EIV_DataPack;

/// <summary>
/// Reading the DataPack
/// </summary>
public class DataPackReader(BinaryReader reader, DataPack dataPack) : IDataPackManipulator
{
    private readonly BinaryReader Reader = reader;
    public DataPack Pack { get; } = dataPack;

    /// <summary>
    /// Should Throw error or just simply return non breaking values. (0, Empty Array)
    /// </summary>
    public bool ShouldThrow = false;
    public bool CanRead => true;

    public bool CanWrite => false;

    public void Close()
    {
        Reader.Close();
        Reader.Dispose();
    }

    /// <summary>
    /// Reading the File Names inside the DataPack.
    /// </summary>
    /// <param name="ShoulReadMetadata">Indicating should read the File Metadata</param>
    public void ReadFileNames()
    {
        Pack.Pack.Read(Reader, false);
    }

    /// <summary>
    /// Get the File Data from Name.
    /// </summary>
    /// <param name="filename">The Name of the file (can be a path too)</param>
    /// <returns>The Decompressed Data inside the file</returns>
    /// <exception cref="Exception">File not found inside the Archive</exception>
    public byte[] GetFileData(string filename)
    {
        var internalFile = Pack.Pack.Files.FirstOrDefault(x => x.Name == filename.Replace('\\', '/'));
        if (internalFile == null)
        {
            if (ShouldThrow)
                throw new Exception("File not found inside the Archive!");
            return [];
        }
        if (internalFile.FileDataPosition == -1)
            return [];
        Reader.BaseStream.Seek(internalFile.FileDataPosition, SeekOrigin.Begin);
        var datalen = Reader.ReadInt32();
        var databytes = Reader.ReadBytes(datalen);
        return Pack.Compressor.Decompress(databytes);
    }

    /// <summary>
    /// Get the File Length from Name
    /// </summary>
    /// <param name="filename">The Name of the file (can be a path too)</param>
    /// <returns>The Compressed Length of the file.</returns>
    /// <exception cref="Exception">File not found inside the Archive</exception>
    public int GetFileLen(string filename)
    {
        var internalFile = Pack.Pack.Files.FirstOrDefault(x => x.Name == filename.Replace('\\', '/'));
        if (internalFile == null)
        {
            if (ShouldThrow)
                throw new Exception("File not found inside the Archive!");
            return 0;
        }
        if (internalFile.FileDataPosition == -1)
            return 0;
        Reader.BaseStream.Seek(internalFile.FileDataPosition, SeekOrigin.Begin);
        return Reader.ReadInt32();
    }

    /// <summary>
    /// Get the File Data from Index.
    /// </summary>
    /// <param name="FileIndex">The Index for a File</param>
    /// <returns>The Decompressed Data inside the file</returns>
    /// <exception cref="Exception">File not found inside the Archive / File Positon is invalid</exception>
    public byte[] GetFileDataIndex(int FileIndex)
    {
        var internalFile = Pack.Pack.Files.ElementAtOrDefault(FileIndex);
        if (internalFile == null)
        {
            if (ShouldThrow)
                throw new Exception("Your file Index is much larger than the files inside!");
            else
                return [];
        }
        if (internalFile.FileDataPosition == -1)
            return [];
        Reader.BaseStream.Seek(internalFile.FileDataPosition, SeekOrigin.Begin);
        var datalen = Reader.ReadInt32();
        var databytes = Reader.ReadBytes(datalen);
        return Pack.Compressor.Decompress(databytes);
    }

    /// <summary>
    /// Get the File Length from Index
    /// </summary>
    /// <param name="FileIndex">The Index for a File</param>
    /// <returns>The Compressed Length of the file.</returns>
    /// <exception cref="Exception">Your file Index is much larger than the files inside / File Positon is invalid</exception>
    public int GetFileLenIndex(int FileIndex)
    {
        var internalFile = Pack.Pack.Files.ElementAtOrDefault(FileIndex);
        if (internalFile == null)
        {
            if (ShouldThrow)
                throw new Exception("Your file Index is much larger than the files inside!");
            else
                return 0;
        }
        if (internalFile.FileDataPosition == -1)
            return 0;
        Reader.BaseStream.Seek(internalFile.FileDataPosition, SeekOrigin.Begin);
        return Reader.ReadInt32();
    }

    /// <summary>
    /// Export the File to a Location.
    /// </summary>
    /// <param name="FileName">FileName inside the archive to export</param>
    /// <param name="exportLocation">Expoerted location + the file name.</param>
    public void ExportFile(string FileName,
#if NET5_0_OR_GREATER
    [NotNull]
#endif
        string exportLocation)
    {
        if (Path.GetDirectoryName(exportLocation) != null)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(exportLocation)!);
        }
        File.WriteAllBytes(exportLocation, GetFileData(FileName));
        var metadata = GetMetadata(FileName);
#if NET5_0_OR_GREATER
        var unixFileMode = (UnixFileMode)metadata.UnixMode;
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            File.SetUnixFileMode(exportLocation, unixFileMode);
        }
#endif
        File.SetCreationTimeUtc(exportLocation, DateTime.FromFileTimeUtc(metadata.CreationTimeUtc));
        File.SetLastAccessTimeUtc(exportLocation, DateTime.FromFileTimeUtc(metadata.LastAccessTimeUtc));
        File.SetLastWriteTimeUtc(exportLocation, DateTime.FromFileTimeUtc(metadata.LastWriteTimeUtc));
    }

    /// <summary>
    /// Get the FileMetadata from <paramref name="FileName"/>
    /// </summary>
    /// <param name="FileName">The Name of the File</param>
    /// <returns>File Metadata</returns>
    public FileMetadata GetMetadata(string FileName)
    {
        if (Pack.Pack.Files.FirstOrDefault(x => x.Name == FileName.Replace('\\', '/')) is not IPackMetadata internalFile)
            return new();
        return internalFile.Metadata;
    }

    /// <summary>
    /// Get the FileMetadata from <paramref name="Index"/>
    /// </summary>
    /// <param name="Index">Index of the file</param>
    /// <returns>File Metadata</returns>
    public FileMetadata GetMetadataIndex(int Index)
    {
        if (Pack.Pack.Files.ElementAtOrDefault(Index) is not IPackMetadata internalFile)
            return new();
        return internalFile.Metadata;
    }

    /// <summary>
    /// Get the File Data from Name.
    /// </summary>
    /// <param name="filename">The Name of the file (can be a path too)</param>
    /// <returns>The Decompressed Data inside the file</returns>
    /// <exception cref="Exception">File not found inside the Archive</exception>
    public byte[] GetExtraData(string filename)
    {
        if (Pack.Pack.Files.FirstOrDefault(x => x.Name == filename.Replace('\\', '/')) is not IPackExtraData internalFile)
        {
            if (ShouldThrow)
                throw new Exception("File not found inside the Archive!");
            return [];
        }
        if (internalFile.ExtraDataPosition == -1)
            return [];
        Reader.BaseStream.Seek(internalFile.ExtraDataPosition, SeekOrigin.Begin);
        var datalen = Reader.ReadInt32();
        var databytes = Reader.ReadBytes(datalen);
        return Pack.Compressor.Decompress(databytes);
    }

    /// <summary>
    /// Get the File Data from Index.
    /// </summary>
    /// <param name="FileIndex">The Index for a File</param>
    /// <returns>The Decompressed Data inside the file</returns>
    /// <exception cref="Exception">File not found inside the Archive / File Positon is invalid</exception>
    public byte[] GetExtraDataIndex(int FileIndex)
    {
        if (Pack.Pack.Files.ElementAtOrDefault(FileIndex) is not IPackExtraData internalFile)
        {
            if (ShouldThrow)
                throw new Exception("File not found inside the Archive!");
            return [];
        }
        if (internalFile.ExtraDataPosition == -1)
            return [];
        Reader.BaseStream.Seek(internalFile.ExtraDataPosition, SeekOrigin.Begin);
        var datalen = Reader.ReadInt32();
        var databytes = Reader.ReadBytes(datalen);
        return Pack.Compressor.Decompress(databytes);
    }
}
