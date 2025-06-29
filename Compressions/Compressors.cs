using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Compressions;

public static class Compressors
{
    public static event Func<byte, ICompressor>? GetCompressorEvent;

    public static ICompressor GetCompressor(CompressionType compressionType, byte extra = 0)
    {
        switch (compressionType)
        {
            case CompressionType.Deflate:
                return new DeflateCompressor();
            case CompressionType.GZip:
                return new GZipCompressor();

            case CompressionType.ZLib:
#if NET5_0_OR_GREATER
                return new ZLibCompressor();
#else
                return new NoneCompressor();
#endif
            case CompressionType.Brotli:
#if NET5_0_OR_GREATER
                return new BrotliCompressor();
#else
                return new NoneCompressor();
#endif
            case CompressionType.Custom:
                ICompressor? compressor = GetCompressorEvent?.Invoke(extra);
                if (compressor == null)
                    return new NoneCompressor();
                return compressor;
            case CompressionType.None:
            default:
                return new NoneCompressor();
        }
    }
}
