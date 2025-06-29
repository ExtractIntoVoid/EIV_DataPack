using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Compressions;

/// <summary>
/// Compressor for <see cref="CompressionType.None"/>
/// </summary>
public class NoneCompressor : ICompressor
{
    /// <inheritdoc/>
    public CompressionType Type => CompressionType.None;

    /// <inheritdoc/>
    public byte CustomCompressionType => 0;

    /// <inheritdoc/>
    public byte[] Compress(byte[] data) => data;

    /// <inheritdoc/>
    public byte[] Decompress(byte[] data) => data;
}
