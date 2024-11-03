using System.IO.Compression;

namespace EIV_DataPack;

/// <summary>
/// EIV Version for storing files in one file instead of separate.
/// </summary>
public class DataPack
{
    /// <summary>
    /// A <see cref="CompressionType"/> for current <see cref="DataPack"/>
    /// </summary>
    public CompressionType Compression { get; internal set; }
    /// <summary>
    /// Version indicator for current <see cref="DataPack"/>
    /// </summary>
    public ushort Version { get; internal set; }
    /// <summary>
    /// Name of the files that the current <see cref="DataPack"/> holding.
    /// </summary>
    public List<string> FileNames = [];
    // Map for FileName->Data position
    internal Dictionary<string, long> FileNameToData = [];
    // Map for FileName->Data, only used in Writer.
    internal Dictionary<string, byte[]> FileNameToBytes = [];
    // Map for FileName->FileMetadata
    internal Dictionary<string, FileMetadata> FileNameToMetadata = [];

    /// <summary>
    /// Compress the input <paramref name="data"/> with the desired <see cref="Compression"/>
    /// </summary>
    /// <param name="data">Data as Bytes</param>
    /// <returns>Compressed <paramref name="data"/></returns>
    public byte[] Compress(byte[] data)
    {
        MemoryStream mem = new(data);
        MemoryStream out_stream = new();
        switch (Compression)
        {
            case CompressionType.None:
                return data;
            case CompressionType.Deflate:
                DeflateStream deflateStream = new(out_stream, CompressionMode.Compress);
                mem.CopyTo(deflateStream);
                deflateStream.Dispose();
                mem.Dispose();
                return out_stream.ToArray();
            case CompressionType.GZip:
                GZipStream gZipStream = new(out_stream, CompressionMode.Compress);
                mem.CopyTo(gZipStream);
                gZipStream.Dispose();
                mem.Dispose();
                return out_stream.ToArray();
            case CompressionType.ZLib:
                ZLibStream zLibStream = new(out_stream, CompressionMode.Compress);
                mem.CopyTo(zLibStream);
                zLibStream.Dispose();
                mem.Dispose();
                return out_stream.ToArray();
            case CompressionType.Brotli:
                BrotliStream brotliStream = new(out_stream, CompressionMode.Compress);
                mem.CopyTo(brotliStream);
                brotliStream.Dispose();
                mem.Dispose();
                return out_stream.ToArray();
            default:
                return data;
        }
    }

    /// <summary>
    /// DeCompress the input <paramref name="data"/> with the desired <see cref="Compression"/>
    /// </summary>
    /// <param name="data">Data as Bytes</param>
    /// <returns>DeCompressed <paramref name="data"/></returns>
    public byte[] DeCompress(byte[] data)
    {
        MemoryStream mem = new(data);
        MemoryStream out_stream = new();
        switch (Compression)
        {
            case CompressionType.None:
                return data;
            case CompressionType.Deflate:
                DeflateStream deflateStream = new(mem, CompressionMode.Decompress);
                deflateStream.CopyTo(out_stream);
                deflateStream.Dispose();
                mem.Dispose();
                return out_stream.ToArray();
            case CompressionType.GZip:
                GZipStream gZipStream = new(mem, CompressionMode.Decompress);
                gZipStream.CopyTo(out_stream);
                gZipStream.Dispose();
                mem.Dispose();
                return out_stream.ToArray();
            case CompressionType.ZLib:
                ZLibStream zLibStream = new(mem, CompressionMode.Decompress);
                zLibStream.CopyTo(out_stream);
                zLibStream.Dispose();
                mem.Dispose();
                return out_stream.ToArray();
            case CompressionType.Brotli:
                BrotliStream brotliStream = new(mem, CompressionMode.Decompress);
                brotliStream.CopyTo(out_stream);
                brotliStream.Dispose();
                mem.Dispose();
                return out_stream.ToArray();
            default:
                return data;
        }
    }
}

/// <summary>
/// Compression Types that supported
/// </summary>
public enum CompressionType : byte
{
    None = 0,
    Deflate,
    GZip,
    ZLib,
    Brotli
}
