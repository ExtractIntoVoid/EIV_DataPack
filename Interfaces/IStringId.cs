namespace EIV_DataPack.Interfaces;

/// <summary>
/// Base interface for parsing string to int and int to string.
/// </summary>
/// <remarks>
/// Use either <see cref="IGetStringId"/> or <see cref="IFromStringId"/>.
/// </remarks>
public interface IStringId
{
    /// <summary>
    /// Id that indicate the string not found.
    /// </summary>
    public int NotExistsId { get; }
}

/// <summary>
/// Getting an <see cref="int"/> from the <see cref="string"/>.
/// </summary>
public interface IGetStringId : IStringId
{
    /// <summary>
    /// Getting an <see cref="int"/> id from <paramref name="str"/>.
    /// </summary>
    /// <param name="str">Input string.</param>
    /// <returns>A StringId.</returns>
    public int GetStringId(string str);
}

/// <summary>
/// Getting an <see cref="string"/> back from <see cref="int"/>.
/// </summary>
public interface IFromStringId : IStringId
{
    /// <summary>
    /// Getting an <see cref="string"/> from <paramref name="id"/>.
    /// </summary>
    /// <param name="id">A StringId.</param>
    /// <returns>A string.</returns>
    public string FromStringId(int id);
}
