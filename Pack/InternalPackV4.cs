using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Pack;

internal class InternalPackV4(PackSettings settings) : IPack
{
    public PackSettings Settings => settings;
    public ushort Version => 4;
    public List<IPackFile> Files { get; protected set; } = [];

    public void Read(BinaryReader reader, bool NoData = false)
    {
        bool HasStringId = reader.ReadBoolean();
        bool HasGuid = reader.ReadBoolean();
        if (HasGuid)
        {
            Settings.Identifier.UseGuid = HasGuid;
            Settings.Identifier.Id = new(reader.ReadBytes(reader.ReadInt16()));
        }
        int fileCount = reader.ReadInt32();
        Files = new(fileCount);
        for (int i = 0; i < fileCount; i++)
        {
            InternalFileV4 file = new();
            file.Read(reader, settings, HasStringId, NoData);
            Files.Add(file);
        }
    }

    public void Write(BinaryWriter writer)
    {
        bool useStringId = settings.StringId.UseStringId;
        writer.Write(useStringId);
        var id = settings.Identifier;
        writer.Write(id.UseGuid);
        if (id.UseGuid)
        {
            var idBytes = id.Id.ToByteArray();
            writer.Write((short)idBytes.Length);
            writer.Write(idBytes);
        }
        writer.Write(Files.Count);
        foreach (InternalFileV4 item in Files.Cast<InternalFileV4>())
        {
            item.Write(writer, settings, useStringId);
        }
    }
}