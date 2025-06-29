using EIV_DataPack.Pack;

namespace EIV_DataPack.Interfaces;

/// <summary>
/// Manipulating the DataPack, for reading and writing.
/// </summary>
public interface IDataPackManipulator
{
    public DataPack Pack { get; }
    public bool CanRead { get; }
    public bool CanWrite { get; }
    public void Close();
}
