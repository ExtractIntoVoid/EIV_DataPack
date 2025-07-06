using EIV_DataPack.Interfaces;
using EIV_DataPack.Pack;

namespace EIV_DataPack.PackFile;

internal class InternalFileV4 : IPackFile, IPackMetadata
{
    public int StringId { get; set; }
    public string Name { get; set; } = string.Empty;
    public long FileDataPosition { get; set; } = -1;
    public byte[] FileData { get; set; } = [];
    public FileMetadata Metadata { get; set; } = new();
    public long ExtraDataPosition { get; set; } = -1;
    public byte[] ExtraData { get; set; } = [];

    public void Read(BinaryReader reader, PackSettings settings, bool HasStringId, bool NoData)
    {
        if (!HasStringId)
            Name = reader.ReadString();
        else
        {
            StringId = reader.ReadInt32();
            if (settings.StringId.FromStringId != null)
                Name = settings.StringId.FromStringId.FromStringId(StringId);
        }
        if (NoData)
        {
            FileDataPosition = reader.BaseStream.Position;
            reader.BaseStream.Position += reader.ReadInt32();
            Metadata = new(reader);
            bool hasExtraData = reader.ReadBoolean();
            if (hasExtraData)
            {
                ExtraDataPosition = reader.BaseStream.Position;
                reader.BaseStream.Position += reader.ReadInt32();
            }

        }
        else
        {
            FileDataPosition = reader.BaseStream.Position;
            int FileDataLen = reader.ReadInt32();
            FileData = reader.ReadBytes(FileDataLen);
            Metadata = new(reader);
            bool hasExtraData = reader.ReadBoolean();
            if (hasExtraData)
            {
                ExtraDataPosition = reader.BaseStream.Position;
                int ExtraDataLen = reader.ReadInt32();
                ExtraData = reader.ReadBytes(ExtraDataLen);
            }
        }
    }

    public void Write(BinaryWriter writer, PackSettings settings, bool useStringId)
    {
        if (!useStringId)
            writer.Write(Name);
        else
        {
            if (settings.StringId.GetStringId != null && StringId == settings.StringId.GetStringId.NotExistsId)
                writer.Write(settings.StringId.GetStringId.GetStringId(Name));
            else
                writer.Write(StringId);
        }

        writer.Write(FileData.Length);
        writer.Write(FileData);
        Metadata.WriteMetadata(writer);
        bool HasExtraData = ExtraData.Length != 0;
        writer.Write(HasExtraData);
        if (HasExtraData)
        {
            writer.Write(ExtraData.Length);
            writer.Write(ExtraData);
        }
    }
}
