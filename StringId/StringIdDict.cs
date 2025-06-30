using EIV_DataPack.Interfaces;

namespace EIV_DataPack.StringId;

/// <summary>
/// A simple <see cref="Dictionary{TKey, TValue}"/> for storing and retrieving strings.
/// </summary>
public class StringIdDict : IGetStringId, IFromStringId
{
    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, int> StringIds = [];

    /// <inheritdoc/>
    public int NotExistsId { get; } = 0;

    /// <inheritdoc/>
    public string FromStringId(int id)
    {
        if (id == NotExistsId)
            return string.Empty;
        KeyValuePair<string, int> stringId = StringIds.FirstOrDefault(x=>x.Value == id);
        if (stringId.Value != id)
            return string.Empty;
        return stringId.Key;
    }

    /// <inheritdoc/>
    public int GetStringId(string str)
    {
        if (StringIds.TryGetValue(str, out int id))
            return id;
        return NotExistsId;
    }
}
