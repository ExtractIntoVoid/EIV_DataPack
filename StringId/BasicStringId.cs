using EIV_DataPack.Interfaces;

namespace EIV_DataPack.StringId;

public class BasicStringId : IGetStringId
{
    public virtual int NotExistsId => 12;

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
