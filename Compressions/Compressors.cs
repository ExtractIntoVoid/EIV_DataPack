using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Compressions;

/// <summary>
/// Collection of the <see cref="ICompressor"/>'s.
/// </summary>
public static class Compressors
{
    /// <summary>
    /// Custom Function that return a Custom Compressor for <see cref="ICompressor.CustomCompressionType"/>
    /// </summary>
    public static event Func<byte, ICompressor>? GetCompressorEvent;

    /// <summary>
    /// Getting a <see cref="ICompressor"/> from the <paramref name="compressionType"/> and if exists from <paramref name="extra"/>.
    /// </summary>
    /// <param name="compressionType">Desired <see cref="CompressionType"/>.</param>
    /// <param name="extra"><see cref="ICompressor.CustomCompressionType"/> extra byte data.</param>
    /// <returns>A compressor or <see cref="NoneCompressor"/>.</returns>
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
