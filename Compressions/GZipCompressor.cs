using EIV_DataPack.Interfaces;
using System.IO.Compression;

namespace EIV_DataPack.Compressions;

/// <summary>
/// Compressor for <see cref="CompressionType.GZip"/>
/// </summary>
public class GZipCompressor : ICompressor
{
    /// <inheritdoc/>
    public CompressionType Type => CompressionType.GZip;

    /// <inheritdoc/>
    public byte CustomCompressionType => 0;

    /// <inheritdoc/>
    public byte[] Compress(byte[] data)
    {
        using MemoryStream mem = new(data);
        using MemoryStream out_stream = new();
        using GZipStream stream = new(out_stream, CompressionMode.Compress);
        mem.CopyTo(stream);
        stream.Dispose();
        mem.Dispose();
        return out_stream.ToArray();
    }

    /// <inheritdoc/>
    public byte[] Decompress(byte[] data)
    {
        MemoryStream mem = new(data);
        MemoryStream out_stream = new();
        GZipStream stream = new(mem, CompressionMode.Decompress);
        stream.CopyTo(out_stream);
        stream.Dispose();
        mem.Dispose();
        return out_stream.ToArray();
    }
}
