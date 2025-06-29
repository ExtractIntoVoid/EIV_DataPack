namespace EIV_DataPack.Interfaces;

public interface IStringId
{
    public int NotExistsId { get; }
}

public interface IGetStringId : IStringId
{
    public int GetStringId(string str);
}

public interface IFromStringId : IStringId
{
    public string FromStringId(int id);
}
