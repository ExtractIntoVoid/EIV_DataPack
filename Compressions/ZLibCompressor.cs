#if NET5_0_OR_GREATER
using EIV_DataPack.Interfaces;
using System.IO.Compression;

namespace EIV_DataPack.Compressions;

public class ZLibCompressor : ICompressor
{
    public CompressionType Type => CompressionType.ZLib;

    public byte CustomCompressionType => 0;

    public byte[] Compress(byte[] data)
    {
        using MemoryStream mem = new(data);
        using MemoryStream out_stream = new();
        using ZLibStream stream = new(out_stream, CompressionMode.Compress);
        mem.CopyTo(stream);
        stream.Dispose();
        mem.Dispose();
        return out_stream.ToArray();
    }

    public byte[] Decompress(byte[] data)
    {
        MemoryStream mem = new(data);
        MemoryStream out_stream = new();
        ZLibStream stream = new(mem, CompressionMode.Decompress);
        stream.CopyTo(out_stream);
        stream.Dispose();
        mem.Dispose();
        return out_stream.ToArray();
    }
}
#endif