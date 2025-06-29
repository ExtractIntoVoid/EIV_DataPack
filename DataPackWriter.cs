using EIV_DataPack.Interfaces;
using EIV_DataPack.Pack;
using System.Runtime.InteropServices;
using System.Text;

namespace EIV_DataPack;

/// <summary>
/// Writing the DataPack
/// </summary>
public class DataPackWriter(BinaryWriter writer, DataPack dataPack) : IDataPackManipulator
{
    // Delegates for Writer events.
    public delegate void OnFileAddedDelegate(string FileName);
    public delegate void OnDataAddedDelegate(string DataName);
    public delegate void OnDirectoryAddedDelegate(string DirectoryName);
    public delegate void OnSavedDelegate();

    // Events to 3rd Party hooks.
    public event OnFileAddedDelegate? OnFileAdded;
    public event OnDataAddedDelegate? OnDataAdded;
    public event OnDirectoryAddedDelegate? OnDirectoryAdded;
    public event OnSavedDelegate? OnSaved;

    private readonly BinaryWriter Writer = writer;
    public DataPack Pack { get; } = dataPack;
    public bool CanRead => false;
    public bool CanWrite => true;

    public void Close()
    {
        Writer.Flush();
        Writer.Close();
        Writer.Dispose();
    }

    /// <summary>
    /// Save the underlaying DataPack
    /// </summary>
    public void Save()
    {
        Writer.Flush();
        Pack.Pack.Write(Writer);
        Writer.Flush();
        OnSaved?.Invoke();
    }

    /// <summary>
    /// Adding file into the DataPack.
    /// </summary>
    /// <param name="path">Path of the File</param>
    /// <param name="RemovePath">Path to be removed</param>
    public void AddFile(string path, string RemovePath = "", byte[]? extraData = null)
    {
        // We normalize
        string filename = path;
        if (RemovePath != "")
            filename = path.Replace(RemovePath, string.Empty).Replace('\\', '/');
        FileMetadata fileMetadata = new()
        { 
            UseMetadata = Pack.Settings.Metadata.UseMetadata,
            UnixMode = 0,
            CreationTimeUtc = File.GetCreationTimeUtc(path).ToFileTimeUtc(),
            LastAccessTimeUtc = File.GetLastAccessTimeUtc(path).ToFileTimeUtc(),
            LastWriteTimeUtc = File.GetLastWriteTimeUtc(path).ToFileTimeUtc(),
        };
#if NET5_0_OR_GREATER
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            fileMetadata.UnixMode = (byte)File.GetUnixFileMode(path);
        }
#endif
        InternalFileV4 internalFile = new()
        {
            Name = filename,
            Metadata = fileMetadata,
            FileData = Pack.Compressor.Compress(File.ReadAllBytes(path)),
        };
        if (extraData != null)
        {
            internalFile.ExtraData = extraData;
        }
        if (Pack.Settings.StringId.GetStringId != null)
            internalFile.StringId = Pack.Settings.StringId.GetStringId.GetStringId(filename);
        Pack.Pack.Files.Add(internalFile);
        OnFileAdded?.Invoke(filename);
    }

    /// <summary>
    /// Add Directory with included files in the DataPack
    /// </summary>
    /// <param name="path">Path to Files</param>
    /// <param name="Recursive">Should include all directories inside or only selected</param>
    public void AddDirectory(string path, bool Recursive = false, byte[]? extraData = null)
    {
        var files = Directory.GetFiles(path,"*.*", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        foreach (var item in files)
        {
            AddFile(item, path + Path.DirectorySeparatorChar, extraData);
        }
        OnDirectoryAdded?.Invoke(path);
    }

    /// <summary>
    /// Add String Data into the DataPack
    /// </summary>
    /// <param name="dataname">Unique Data Name</param>
    /// <param name="data">Data to be saved</param>
    public void AddData(string dataname, string data, byte[]? extraData = null)
    {
        AddData(dataname, Encoding.UTF8.GetBytes(data), extraData);
    }

    /// <summary>
    /// Add ByteArray Data into the DataPack
    /// </summary>
    /// <param name="dataname">Unique Data Name</param>
    /// <param name="data">Data to be saved</param>
    public void AddData(string dataname, byte[] data, byte[]? extraData = null)
    {
        InternalFileV4 internalFile = new()
        {
            Name = dataname,
            Metadata = new(),
            ExtraData = extraData ?? [],
            FileData = Pack.Compressor.Compress(data),
        };
        if (Pack.Settings.StringId.GetStringId != null)
            internalFile.StringId = Pack.Settings.StringId.GetStringId.GetStringId(dataname);
        Pack.Pack.Files.Add(internalFile);
        OnDataAdded?.Invoke(dataname);
    }
}
