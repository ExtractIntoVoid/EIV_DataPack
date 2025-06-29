using EIV_DataPack.Pack;

namespace EIV_DataPack.Interfaces;

/// <summary>
/// Manipulating the DataPack, for reading and writing.
/// </summary>
public interface IDataPackManipulator
{
    /// <summary>
    /// The <see cref="DataPack"/>.
    /// </summary>
    public DataPack Pack { get; }
    /// <summary>
    /// Can this Manipulator read.
    /// </summary>
    public bool CanRead { get; }
    /// <summary>
    /// Can this Manipulator write.
    /// </summary>
    public bool CanWrite { get; }
    /// <summary>
    /// Close the Manipulator.
    /// </summary>
    public void Close();
}
