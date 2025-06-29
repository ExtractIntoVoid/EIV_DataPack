using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Compressions;

public class NoneCompressor : ICompressor
{
    public CompressionType Type => CompressionType.None;

    public byte CustomCompressionType => 0;

    public byte[] Compress(byte[] data) => data;

    public byte[] Decompress(byte[] data) => data;
}
