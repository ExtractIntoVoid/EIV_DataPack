using EIV_DataPack.Interfaces;
using EIV_DataPack.Pack;
using System.Text;

namespace EIV_DataPack.PackFile;

internal class InternalPackV3(PackSettings settings) : IPack
{
    public PackSettings Settings => settings;
    public ushort Version => 3;

    public List<IPackFile> Files { get; protected set; } = [];

    public void Read(BinaryReader reader, bool NoData = false)
    {
        int fileCount = reader.ReadInt32();
        Files = new(fileCount);
        for (int i = 0; i < fileCount; i++)
        {
            InternalFileV3 file = new();
            var filename_len = reader.ReadInt32();
            file.Name = Encoding.UTF8.GetString(reader.ReadBytes(filename_len));
            file.FileDataPosition = reader.ReadInt64();
            Files.Add(file);
        }
        long MetadataPos = reader.ReadInt64();
        reader.BaseStream.Position = MetadataPos - 8;
        long NextDataBlobPos = reader.ReadInt64();
        int MetadataItems = reader.ReadInt32();
        for (int i = 0; i < MetadataItems; i++)
        {
            int filename_len = reader.ReadInt32();
            string filename = Encoding.UTF8.GetString(reader.ReadBytes(filename_len));
            InternalFileV3? file = (InternalFileV3?)Files.FirstOrDefault(x=>x.Name == filename);
            if (file == null)
            {
                Console.WriteLine("Seems like your .eivp file corrupt!");
                continue;
            }
            file.Metadata = new(reader, Version);
        }
        if (!NoData)
        {
            foreach (InternalFileV3 file in Files.Cast<InternalFileV3>())
            {
                reader.BaseStream.Seek(file.FileDataPosition, SeekOrigin.Begin);
                int datalen = reader.ReadInt32();
                file.FileData = reader.ReadBytes(datalen);
            }
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
        long metadata_starter_post = writer.BaseStream.Position;
        writer.Write((long)-1); // Position for metadata
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
        writer.Write((long)-1); // Position for "New Data", but we ditched that so thats just an 8 byte long nothing.
        var tmp_pos = writer.BaseStream.Position;
        writer.BaseStream.Position = metadata_starter_post;
        writer.Write(tmp_pos);
        writer.BaseStream.Position = tmp_pos;
        writer.Write(Files.Count);
        foreach (InternalFileV3 item in Files.Cast<InternalFileV3>())
        {
            var name = Encoding.UTF8.GetBytes(item.Name);
            writer.Write(name.Length);
            writer.Write(name);
            item.Metadata.WriteMetadata(writer, Version);
        }
    }
}

/*
* Count
* ListOfName / FileId
* 
* FileIndex
* FileDataSize
* FileData
* FileMetadata
* FileExtraDataSize
* FileExtraData
* 
*/