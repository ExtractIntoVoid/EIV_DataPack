namespace EIV_DataPack.Interfaces;

internal interface IPack
{
    public ushort Version { get; }

    public List<IPackFile> Files { get; }

    public void Read(BinaryReader reader, bool NoData = false);

    public void Write(BinaryWriter writer);
}
