using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Pack;

internal class InternalPackV1(PackSettings settings) : IPack
{
    public PackSettings Settings => settings;
    public ushort Version => 1;

    public List<IPackFile> Files { get; } = [];

    public void Read(BinaryReader reader, bool NoData = false)
    {
        // V1 of DataPack never released, cant trace back what changed!
        throw new NotSupportedException("Reading V1 is not supported!");
    }

    public void Write(BinaryWriter _)
    {
       throw new NotSupportedException("Writing V1 is not supported!");
    }
}