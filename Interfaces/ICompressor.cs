using EIV_DataPack.Compressions;

namespace EIV_DataPack.Interfaces;

/// <summary>
/// Represent a Compressor.
/// </summary>
public interface ICompressor
{
    /// <summary>
    /// Type of current <see cref="CompressionType"/>.
    /// </summary>
    public CompressionType Type { get; }

    /// <summary>
    /// Custom <see cref="byte"/> type, if <see cref="Type"/> is <see cref="CompressionType.Custom"/>
    /// </summary>
    public byte CustomCompressionType { get; }

    /// <summary>
    /// Compress <paramref name="data"/>.
    /// </summary>
    /// <param name="data">Compressable data.</param>
    /// <returns>Compressed data.</returns>
    public byte[] Compress(byte[] data);

    /// <summary>
    /// Decompress <paramref name="data"/>.
    /// </summary>
    /// <param name="data">Decompressable data.</param>
    /// <returns>Decompressed data.</returns>
    public byte[] Decompress(byte[] data);
}
