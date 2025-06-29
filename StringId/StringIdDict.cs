using EIV_DataPack.Interfaces;

namespace EIV_DataPack.StringId;

public class StringIdDict : IGetStringId, IFromStringId
{
    public Dictionary<string, int> StringIds = [];
    public int NotExistsId { get; } = 0;
    public string FromStringId(int id)
    {
        if (id == NotExistsId)
            return string.Empty;
        KeyValuePair<string, int> stringId = StringIds.FirstOrDefault(x=>x.Value == id);
        if (stringId.Value != id)
            return string.Empty;
        return stringId.Key;
    }

    public int GetStringId(string str)
    {
        if (StringIds.TryGetValue(str, out int id))
            return id;
        return NotExistsId;
    }
}
