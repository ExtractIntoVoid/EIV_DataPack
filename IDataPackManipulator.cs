namespace EIV_DataPack;

/// <summary>
/// Manipulating the DataPack, for reading and writing.
/// </summary>
public interface IDataPackManipulator
{
    public DataPack Pack { get; internal set; }
    public void Open();
    public void Close();
}
