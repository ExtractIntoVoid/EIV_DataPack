using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Compressions;

/// <summary>
/// Compression Types that supported
/// </summary>
public enum CompressionType : byte
{
    /// <summary>
    /// No compression.
    /// </summary>
    None = 0,
    /// <summary>
    /// Deflate compression.
    /// </summary>
    Deflate,
    /// <summary>
    /// GZip Compression.
    /// </summary>
    GZip,
    /// <summary>
    /// ZLib compression.
    /// </summary>
    /// <remarks>
    /// Only works in NET8+
    /// </remarks>
    ZLib,
    /// <summary>
    /// Brotli compression.
    /// </summary>
    /// <remarks>
    /// Only works in NET8+
    /// </remarks>
    Brotli,
    /// <summary>
    /// Custom Compression. Use <see cref="ICompressor"/>
    /// </summary>
    Custom,
}
