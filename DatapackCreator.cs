namespace EIV_DataPack;

public class DatapackCreator
{
    public const ushort DATAPACK_VERSION = 3;
    public const ushort LAST_SUPPORTED_DATAPACK_VERSION = 2;
    public const string MAGIC = "eivp"; //65 69 76 70
    public const int MagicInt = 1886808421;
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

    public static DatapackCreator Create(string Filename, CompressionType compressionType = CompressionType.Deflate)
    {
        return Create(File.OpenWrite(Filename), compressionType);
    }

    public static DatapackCreator Create(FileStream fileStream, CompressionType compressionType = CompressionType.Deflate)
    {
        return new DatapackCreator(fileStream, false, compressionType);
    }

    public static DatapackCreator Read(string Filename)
    {
        return Read(File.OpenRead(Filename));
    }

    public static DatapackCreator Read(FileStream fileStream)
    {
        return new DatapackCreator(fileStream, true);
    }

    public bool CanRead()
    {
        return Manipulator is DataPackReader reader && reader != null;
    }

    public bool CanWrite()
    {
        return Manipulator is DataPackWriter writer && writer != null;
    }

    public DataPackReader? GetReader()
    {
        if (!CanRead())
            return null;
        return Manipulator as DataPackReader;
    }

    public DataPackWriter? GetWriter()
    {
        if (!CanWrite())
            return null;
        return Manipulator as DataPackWriter;
    }

    public void Close()
    {
        Manipulator.Close();
    }
}
