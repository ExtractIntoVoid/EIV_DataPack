using EIV_DataPack.Interfaces;
using System.Text;

namespace EIV_DataPack.Pack;

internal class InternalPackV2(PackSettings settings) : IPack
{
    public PackSettings Settings => settings;
    public ushort Version => 2;

    public List<IPackFile> Files { get; protected set; } = [];

    public void Read(BinaryReader reader, bool NoData = false)
    {
        int fileCount = reader.ReadInt32();
        Files = new(fileCount);
        for (int i = 0; i < fileCount; i++)
        {
            InternalFileV2 file = new();
            int filename_len = reader.ReadInt32();
            file.Name = Encoding.UTF8.GetString(reader.ReadBytes(filename_len));
            file.FileDataPosition = reader.ReadInt64();
            Files.Add(file);
        }
        if (NoData)
            return;
        foreach (InternalFileV2 file in Files.Cast<InternalFileV2>())
        {
            reader.BaseStream.Seek(file.FileDataPosition, SeekOrigin.Begin);
            file.FileData = reader.ReadBytes(reader.ReadInt32());
        }
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Files.Count);
        foreach (var item in Files)
        {
            var name = Encoding.UTF8.GetBytes(item.Name);
            writer.Write(name.Length);
            writer.Write(name);
            item.FileDataPosition = writer.BaseStream.Position;
            writer.Write((long)0);
        }
        writer.Write((ushort)0); // Some padding I guess
        foreach (var item in Files)
        {
            var pos_start = writer.BaseStream.Position;
            writer.Write(item.FileData.Length);
            writer.Write(item.FileData);
            var pos_end = writer.BaseStream.Position;
            // Writing File Data Position.
            writer.BaseStream.Position = item.FileDataPosition;
            writer.Write(pos_start);
            writer.BaseStream.Position = pos_end;
        }
    }
}