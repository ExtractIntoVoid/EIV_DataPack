using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace EIV_DataPack;

/// <summary>
/// A Class for Reading the DataPack
/// </summary>
public class DataPackReader : IDataPackManipulator
{
    BinaryReader Reader;
    public DataPack Pack { get; set; }

    /// <summary>
    /// Should Throw error or just simply return non breaking values. (0, Empty Array)
    /// </summary>
    public bool ShouldThrow = false;
    public int FileNameCount { get; internal set; } = 0;
    internal long ReadedFilesPos = -1;
    internal long MetadataPos = -1; 
    internal long NextDataBlobPos = -1;

    public DataPackReader(BinaryReader reader, DataPack dataPack) 
    {
        Pack = dataPack;
        Reader = reader;
    }

    public void Open()
    {
        FileNameCount = Reader.ReadInt32();
        Reader.BaseStream.Position = 7; // 7 because we readed int32, int16 and a byte (4+2+1)
    }

    public void Close()
    {
        Reader.Close();
        Reader.Dispose();
    }

    /// <summary>
    /// Reading the File Names inside the DataPack.
    /// </summary>
    /// <param name="ShoulReadMetadata">Indicating should read the File Metadata</param>
    public void ReadFileNames(bool ShoulReadMetadata = true)
    {
        Pack.FileNameToData.Clear();
        Pack.FileNames.Clear();
        Reader.BaseStream.Position = 7;
        FileNameCount = Reader.ReadInt32();
        for (int i = 0; i < FileNameCount; i++)
        {
            var filename_len = Reader.ReadInt32();
            var filename = Encoding.UTF8.GetString(Reader.ReadBytes(filename_len));
            Pack.FileNames.Add(filename);
            var start_len = Reader.ReadInt64();
            Pack.FileNameToData.Add(filename, start_len);
        }
        ReadedFilesPos = Reader.BaseStream.Position;
        if (ShoulReadMetadata)
            ReadMetadata();
    }

    /// <summary>
    /// Reading the File MetaData
    /// </summary>
    public void ReadMetadata()
    {
        Pack.FileNameToMetadata.Clear();
        if (Pack.Version > 2)
        {
            MetadataPos = Reader.ReadInt64();
            Reader.BaseStream.Position = MetadataPos - 8;
            NextDataBlobPos = Reader.ReadInt64();
        }
        else
        {
            MetadataPos = -1;
            NextDataBlobPos = -1;
            return;
        }
        int MetadataItems = Reader.ReadInt32();
        for (int i = 0; i < MetadataItems; i++)
        {
            var filename_len = Reader.ReadInt32();
            var filename = Encoding.UTF8.GetString(Reader.ReadBytes(filename_len));
            var metadata = Reader.ReadMetadata();
            Pack.FileNameToMetadata.Add(filename, metadata);
        }
    }

    /// <summary>
    /// Get the File Data from Name.
    /// </summary>
    /// <param name="filename">The Name of the file (can be a path too)</param>
    /// <returns>The Decompressed Data inside the file</returns>
    /// <exception cref="Exception">File not found inside the Archive</exception>
    public byte[] GetFileData(string filename)
    {
        Reader.BaseStream.Position = ReadedFilesPos;
        if (!Pack.FileNameToData.TryGetValue(filename.Replace('\\', '/'), out var data))
        {
            if (ShouldThrow)
                throw new Exception("File not found inside the Archive!");
            return [];
        }
        Reader.BaseStream.Seek(data, SeekOrigin.Begin);
        var datalen = Reader.ReadInt32();
        var databytes = Reader.ReadBytes(datalen);
        return Pack.DeCompress(databytes);
    }

    /// <summary>
    /// Get the File Length from Name
    /// </summary>
    /// <param name="filename">The Name of the file (can be a path too)</param>
    /// <returns>The Compressed Length of the file.</returns>
    /// <exception cref="Exception">File not found inside the Archive</exception>
    public int GetFileLen(string filename)
    {
        Reader.BaseStream.Position = ReadedFilesPos;
        if (!Pack.FileNameToData.TryGetValue(filename.Replace('\\', '/'), out var data))
        {
            if (ShouldThrow)
                throw new Exception("File not found inside the Archive!");
            return 0;
        }
        Reader.BaseStream.Seek(data, SeekOrigin.Begin);
        return Reader.ReadInt32();
    }

    /// <summary>
    /// Get the File Data from Index.
    /// </summary>
    /// <param name="FileIndex">The Index for a File</param>
    /// <returns>The Decompressed Data inside the file</returns>
    /// <exception cref="Exception">File not found inside the Archive / File Positon is invalid</exception>
    public byte[] GetFileData(int FileIndex)
    {
        Reader.BaseStream.Position = 11;
        long len_to_Start = -1;
        if (FileIndex > FileNameCount)
            if (ShouldThrow)
                throw new Exception("Your file Index is much larger than the files inside!");
            else
                return [];
        for (int i = 0; i <= FileIndex; i++)
        {
            var filename_len = Reader.ReadInt32();
            Reader.ReadBytes(filename_len);
            len_to_Start = Reader.ReadInt64();
        }
        if (len_to_Start == -1)
        {
            if (ShouldThrow)
                throw new Exception("File Positon is invalid!");
            return [];
        }
        Reader.BaseStream.Seek(len_to_Start, SeekOrigin.Begin);
        var datalen = Reader.ReadInt32();
        var databytes = Reader.ReadBytes(datalen);
        return Pack.DeCompress(databytes);
    }

    /// <summary>
    /// Get the File Length from Index
    /// </summary>
    /// <param name="FileIndex">The Index for a File</param>
    /// <returns>The Compressed Length of the file.</returns>
    /// <exception cref="Exception">Your file Index is much larger than the files inside / File Positon is invalid</exception>
    public int GetFileLen(int FileIndex)
    {
        Reader.BaseStream.Position = 11;
        long len_to_Start = -1;
        if (FileIndex > FileNameCount)
            if (ShouldThrow)
                throw new Exception("Your file Index is much larger than the files inside!");
            else
                return 0;
        for (int i = 0; i <= FileIndex; i++)
        {
            var filename_len = Reader.ReadInt32();
            Reader.ReadBytes(filename_len);
            len_to_Start = Reader.ReadInt64();
        }
        if (len_to_Start == -1)
        {
            if (ShouldThrow)
                throw new Exception("File Positon is invalid!");
            return 0;
        }
        Reader.BaseStream.Seek(len_to_Start, SeekOrigin.Begin);
        return Reader.ReadInt32();
    }

    /// <summary>
    /// Export the File to a Location.
    /// </summary>
    /// <param name="FileName">FileName inside the archive to export</param>
    /// <param name="exportLocation">Expoerted location + the file name.</param>
    public void ExportFile(string FileName, [NotNull] string exportLocation)
    {
        if (Path.GetDirectoryName(exportLocation) != null)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(exportLocation)!);
        }
        File.WriteAllBytes(exportLocation, GetFileData(FileName));
        if (Pack.Version == 2) //version 2 doesnt have metadata
            return;
        var metadata = GetMetadata(FileName);
        var unixFileMode = (UnixFileMode)metadata.UnixMode;
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            File.SetUnixFileMode(exportLocation, unixFileMode);
        }
        File.SetCreationTimeUtc(exportLocation, DateTime.FromFileTimeUtc(metadata.CreationTimeUtc));
        File.SetLastAccessTimeUtc(exportLocation, DateTime.FromFileTimeUtc(metadata.LastAccessTimeUtc));
        File.SetLastWriteTimeUtc(exportLocation, DateTime.FromFileTimeUtc(metadata.LastWriteTimeUtc));
    }
    
    /// <summary>
    /// Get the FileMetadata from <paramref name="FileName"/>
    /// </summary>
    /// <param name="FileName">The Name of the File</param>
    /// <returns>Empty FileMetadata if <see cref="DataPack.Version"/> > 2 otherwise the Filled FileMetadata</returns>
    public FileMetadata GetMetadata(string FileName)
    {
        if (Pack.Version > 2)
            return new();
        FileMetadata metadata = new();
        Pack.FileNameToMetadata.TryGetValue(FileName, out metadata);
        return metadata;
    }

    /// <summary>
    /// Get the FileMetadata from <paramref name="Index"/>
    /// </summary>
    /// <param name="Index">Index of the file</param>
    /// <returns>Empty FileMetadata if <see cref="DataPack.Version"/> > 2 otherwise the Filled FileMetadata</returns>
    public FileMetadata GetMetadata(int Index)
    {
        if (Pack.Version > 2)
            return new();
        FileMetadata metadata = new();
        if (Pack.FileNameToMetadata.Count < Index)
            return metadata;
        metadata = Pack.FileNameToMetadata.ElementAt(Index).Value;
        return metadata;
    }
}
