using System.IO.Compression;

namespace EIV_DataPack
{
    public class DataPack
    {
        public CompressionType Compression { get; internal set; }
        public ushort Version { get; internal set; }
        public List<string> FileNames = [];
        internal Dictionary<string, long> FileNameToData = [];
        internal Dictionary<string, byte[]> FileNameToBytes = [];
        internal Dictionary<string, FileMetadata> FileNameToMetadata = [];

        public byte[] Compress(byte[] data)
        {
            MemoryStream mem = new(data);
            MemoryStream out_stream = new();
            switch (Compression)
            {
                case CompressionType.None:
                    return data;
                case CompressionType.Deflate:
                    DeflateStream deflateStream = new(out_stream, CompressionMode.Compress);
                    mem.CopyTo(deflateStream);
                    deflateStream.Dispose();
                    mem.Dispose();
                    return out_stream.ToArray();
                case CompressionType.GZip:
                    GZipStream gZipStream = new(out_stream, CompressionMode.Compress);
                    mem.CopyTo(gZipStream);
                    gZipStream.Dispose();
                    mem.Dispose();
                    return out_stream.ToArray();
                case CompressionType.ZLib:
                    ZLibStream zLibStream = new(out_stream, CompressionMode.Compress);
                    mem.CopyTo(zLibStream);
                    zLibStream.Dispose();
                    mem.Dispose();
                    return out_stream.ToArray();
                case CompressionType.Brotli:
                    BrotliStream brotliStream = new(out_stream, CompressionMode.Compress);
                    mem.CopyTo(brotliStream);
                    brotliStream.Dispose();
                    mem.Dispose();
                    return out_stream.ToArray();
                default:
                    return data;
            }
        }

        public byte[] DeCompress(byte[] data)
        {
            MemoryStream mem = new(data);
            MemoryStream out_stream = new();
            switch (Compression)
            {
                case CompressionType.None:
                    return data;
                case CompressionType.Deflate:
                    DeflateStream deflateStream = new(mem, CompressionMode.Decompress);
                    deflateStream.CopyTo(out_stream);
                    deflateStream.Dispose();
                    mem.Dispose();
                    return out_stream.ToArray();
                case CompressionType.GZip:
                    GZipStream gZipStream = new(mem, CompressionMode.Decompress);
                    gZipStream.CopyTo(out_stream);
                    gZipStream.Dispose();
                    mem.Dispose();
                    return out_stream.ToArray();
                case CompressionType.ZLib:
                    ZLibStream zLibStream = new(mem, CompressionMode.Decompress);
                    zLibStream.CopyTo(out_stream);
                    zLibStream.Dispose();
                    mem.Dispose();
                    return out_stream.ToArray();
                case CompressionType.Brotli:
                    BrotliStream brotliStream = new(mem, CompressionMode.Decompress);
                    brotliStream.CopyTo(out_stream);
                    brotliStream.Dispose();
                    mem.Dispose();
                    return out_stream.ToArray();
                default:
                    return data;
            }
        }
    }

    public enum CompressionType : byte
    {
        None = 0,
        Deflate,
        GZip,
        ZLib,
        Brotli
    }
}
