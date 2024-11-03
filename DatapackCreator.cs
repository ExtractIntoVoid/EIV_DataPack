namespace EIV_DataPack;

/// <summary>
/// 
/// </summary>
public class DatapackCreator
{
    // Current DataPack Version
    public const ushort DATAPACK_VERSION = 3;
    // Last Supported DataPack Version
    public const ushort LAST_SUPPORTED_DATAPACK_VERSION = 2;
    // File Magic as string
    public const string MAGIC = "eivp"; //65 69 76 70
    // File Magic as Int
    public const int MagicInt = 1886808421;
    // DataPack Manipulator, for reading and writing
    internal IDataPackManipulator Manipulator;
    internal DatapackCreator(Stream stream, bool IsRead, CompressionType compressionType = CompressionType.Deflate)
    {
        if (IsRead)
        {
            BinaryReader reader = new BinaryReader(stream);
            if (reader.ReadInt32() != MagicInt)
                throw new Exception("Wrong file readed!");
            ushort version = reader.ReadUInt16();
            if (version < LAST_SUPPORTED_DATAPACK_VERSION)
                throw new Exception("Version no longer supported! Version: " + version);
            Manipulator = new DataPackReader(reader, new()
            { 
                Compression = (CompressionType)reader.ReadByte(),
                Version = version,
            });
        }
        else
        {
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(MagicInt);
            writer.Write(DATAPACK_VERSION);
            writer.Write((byte)compressionType);
            Manipulator = new DataPackWriter(writer, new()
            { 
                Compression = compressionType,
                Version = DATAPACK_VERSION
            });
        }
        Manipulator.Open();
    }

    /// <summary>
    /// Create a File for Writing.
    /// </summary>
    /// <param name="Filename">Name of the File</param>
    /// <param name="compressionType">Type of Compression</param>
    /// <returns>The created Datapack</returns>
    public static DatapackCreator Create(string Filename, CompressionType compressionType = CompressionType.Deflate)
    {
        return Create(File.OpenWrite(Filename), compressionType);
    }

    /// <summary>
    /// Open a FileStream for Writing.
    /// </summary>
    /// <param name="fileStream">Stream of the File for writing</param>
    /// <param name="compressionType">Type of Compression</param>
    /// <returns>The created Datapack</returns>
    public static DatapackCreator Create(FileStream fileStream, CompressionType compressionType = CompressionType.Deflate)
    {
        return new DatapackCreator(fileStream, false, compressionType);
    }

    /// <summary>
    /// Open a File for Reading.
    /// </summary>
    /// <param name="Filename">Name of the File</param>
    /// <returns>The opened Datapack</returns>
    public static DatapackCreator Read(string Filename)
    {
        return Read(File.OpenRead(Filename));
    }

    /// <summary>
    /// Open a File for Reading.
    /// </summary>
    /// <param name="fileStream">Stream of the File for reading</param>
    /// <returns>The opened Datapack</returns>
    public static DatapackCreator Read(FileStream fileStream)
    {
        return new DatapackCreator(fileStream, true);
    }

    /// <summary>
    /// Check the DataPack is Readable
    /// </summary>
    /// <returns>True if Readable</returns>
    public bool CanRead()
    {
        return Manipulator is DataPackReader reader && reader != null;
    }

    /// <summary>
    /// Check the DataPack is Writable
    /// </summary>
    /// <returns>True if Writable</returns>
    public bool CanWrite()
    {
        return Manipulator is DataPackWriter writer && writer != null;
    }

    /// <summary>
    /// Get The DataPack Reader
    /// </summary>
    /// <returns>Null if not readable</returns>
    public DataPackReader? GetReader()
    {
        if (!CanRead())
            return null;
        return Manipulator as DataPackReader;
    }

    /// <summary>
    /// Get The DataPack Writer
    /// </summary>
    /// <returns>Null if not writable</returns>
    public DataPackWriter? GetWriter()
    {
        if (!CanWrite())
            return null;
        return Manipulator as DataPackWriter;
    }

    /// <summary>
    /// Close the underlaying Manipulator.
    /// </summary>
    public void Close()
    {
        Manipulator.Close();
    }
}
