using EIV_DataPack.Interfaces;

namespace EIV_DataPack.StringId;

/// <summary>
/// A simple <see cref="List{T}"/> of <see cref="string"/> for storing and retrieving strings.
/// </summary>
public class StringIdList : IGetStringId, IFromStringId
{
    /// <summary>
    /// List of string. (Index = string)
    /// </summary>
    public List<string> StringIds = [];

    /// <inheritdoc/>
    public int NotExistsId => -1;

    /// <inheritdoc/>
    public string FromStringId(int id)
    {
        if (id == NotExistsId)
            return string.Empty;
        return StringIds.ElementAt(id);
    }

    /// <inheritdoc/>
    public int GetStringId(string str)
    {
        return StringIds.FindIndex(x => x == str);
    }
}
