using EIV_DataPack.Interfaces;

namespace EIV_DataPack.StringId;

/// <summary>
/// Basic StringId Parser, cannot reverse the name back!
/// </summary>
public class BasicStringId : IGetStringId
{
    /// <inheritdoc/>
    public virtual int NotExistsId => 12;

    /// <inheritdoc/>
    public int GetStringId(string str)
    {
        int id = 12;
        foreach (char c in str)
        {
            id = id * 9 + c;
        }
        return id;
    }
}
