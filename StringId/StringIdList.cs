using EIV_DataPack.Interfaces;

namespace EIV_DataPack.StringId;

public class StringIdList : IGetStringId, IFromStringId
{
    public List<string> StringIds = [];
    public int NotExistsId { get; } = -1;
    public string FromStringId(int id)
    {
        if (id == NotExistsId)
            return string.Empty;
        return StringIds.ElementAt(id);
    }

    public int GetStringId(string str)
    {
        return StringIds.FindIndex(x => x == str);
    }
}
