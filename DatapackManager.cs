using EIV_DataPack.Compressions;
using EIV_DataPack.Interfaces;

namespace EIV_DataPack;

/// <summary>
/// Manages the reading and writing of Data Pack.
/// </summary>
public class DatapackManager
{
    // DataPack Manipulator, for reading and writing
    internal IDataPackManipulator Manipulator;
    internal DatapackManager(Stream stream)
    {
        BinaryReader reader = new(stream);
        if (reader.ReadInt32() != Consts.MagicInt)
            throw new Exception("Magic MissMatch!");
        ushort version = reader.ReadUInt16();
        if (version < Consts.MIN_SUPPORTED_VERSION)
            throw new Exception($"Version no longer supported! Version: {version}");
        CompressionType compressionType = (CompressionType)reader.ReadByte();
        byte extra = 0;
        if (compressionType == CompressionType.Custom)
        {
            if (version < Consts.CUSTOMCOMPRESSION_VERSION)
                throw new Exception($"Custom compression not supported! Version: {version}");
            extra = reader.ReadByte();
        }
        Manipulator = new DataPackReader(reader, new(version, compressionType, extra));
    }

    internal DatapackManager(Stream stream, CompressionType compressionType = CompressionType.Deflate, byte extra = 0)
    {
        BinaryWriter writer = new(stream);
        writer.Write(Consts.MagicInt);
        writer.Write(Consts.CURRENT_VERSION);
        writer.Write((byte)compressionType);
        if (compressionType == CompressionType.Custom)
            writer.Write(extra);
        Manipulator = new DataPackWriter(writer, new(Consts.CURRENT_VERSION, compressionType, extra));
    }

    /// <summary>
    /// Create a File for Writing.
    /// </summary>
    /// <param name="Filename">Name of the File</param>
    /// <param name="compressionType">Type of Compression</param>
    /// <param name="extra">Extra data.</param>
    /// <returns>The created Datapack</returns>
    public static DatapackManager Create(string Filename, CompressionType compressionType = CompressionType.Deflate, byte extra = 0)
    {
        return Create(File.OpenWrite(Filename), compressionType, extra);
    }

    /// <summary>
    /// Open a FileStream for Writing.
    /// </summary>
    /// <param name="fileStream">Stream of the File for writing</param>
    /// <param name="compressionType">Type of Compression</param>
    /// <param name="extra">Extra data.</param>
    /// <returns>The created Datapack</returns>
    public static DatapackManager Create(FileStream fileStream, CompressionType compressionType = CompressionType.Deflate, byte extra = 0)
    {
        return new DatapackManager(fileStream, compressionType, extra);
    }

    /// <summary>
    /// Open a File for Reading.
    /// </summary>
    /// <param name="Filename">Name of the File</param>
    /// <returns>The opened Datapack</returns>
    public static DatapackManager Read(string Filename)
    {
        return Read(File.OpenRead(Filename));
    }

    /// <summary>
    /// Open a File for Reading.
    /// </summary>
    /// <param name="fileStream">Stream of the File for reading</param>
    /// <returns>The opened Datapack</returns>
    public static DatapackManager Read(FileStream fileStream)
    {
        return new DatapackManager(fileStream);
    }

    /// <summary>
    /// Reading the file from <paramref name="Data"/>.
    /// </summary>
    /// <param name="Data">Data of the File</param>
    /// <returns>The opened Datapack</returns>
    public static DatapackManager Read(byte[] Data)
    {
        return new DatapackManager(new MemoryStream(Data));
    }

    /// <summary>
    /// Get The DataPack Reader
    /// </summary>
    /// <returns>Null if not readable</returns>
    public DataPackReader? GetReader()
    {
        if (!Manipulator.CanRead)
            return null;
        return Manipulator as DataPackReader;
    }

    /// <summary>
    /// Get The DataPack Writer
    /// </summary>
    /// <returns>Null if not writable</returns>
    public DataPackWriter? GetWriter()
    {
        if (!Manipulator.CanWrite)
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
