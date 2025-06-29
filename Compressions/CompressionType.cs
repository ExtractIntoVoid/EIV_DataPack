namespace EIV_DataPack.Compressions;

/// <summary>
/// Compression Types that supported
/// </summary>
public enum CompressionType : byte
{
    None = 0,
    Deflate,
    GZip,
    /// <summary>
    /// Only works in NET8+
    /// </summary>
    ZLib,
    /// <summary>
    /// Only works in NET8+
    /// </summary>
    Brotli,
    Custom,
}
