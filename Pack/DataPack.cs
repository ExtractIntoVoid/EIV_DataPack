using EIV_DataPack.Compressions;
using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Pack;

/// <summary>
/// EIV Datapack.
/// </summary>
public class DataPack
{
    public DataPack(ushort version, ICompressor compressor)
    {
        Version = version;
        Compressor = compressor;
        Settings = new();
        Pack = version switch
        {
            1 => new InternalPackV1(Settings),
            2 => new InternalPackV2(Settings),
            3 => new InternalPackV3(Settings),
            4 => new InternalPackV4(Settings),
            _ => new InternalPackV1(Settings),
        };
    }

    public DataPack(ushort version, CompressionType compressionType = CompressionType.Deflate, byte extra = 0) : this(version, Compressors.GetCompressor(compressionType, extra)) { }


    /// <summary>
    /// Version indicator for current <see cref="DataPack"/>
    /// </summary>
    public ushort Version { get; }

    /// <summary>
    /// A Custom <see cref="ICompressor"/> for compressing custom data.
    /// </summary>
    public ICompressor Compressor { get; }

    /// <summary>
    /// Name of the files that the current <see cref="DataPack"/> holding.
    /// </summary>
    public IReadOnlyCollection<string> FileNames
    { 
        get
        {
            return [.. Pack.Files.Select(x => x.Name)];
        }
    }

    /// <summary>
    /// Setting of the DataPack.
    /// </summary>
    public PackSettings Settings { get; } = new();

    internal IPack Pack { get; }
}