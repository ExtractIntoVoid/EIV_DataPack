using EIV_DataPack.Compressions;

namespace EIV_DataPack.Interfaces;

/// <summary>
/// Represent a Compressor.
/// </summary>
public interface ICompressor
{
    public CompressionType Type { get; }
    public byte CustomCompressionType { get; }
    public byte[] Compress(byte[] data);
    public byte[] Decompress(byte[] data);
}
