namespace EIV_DataPack;

public static class Consts
{
    /// <summary>
    /// Current DataPack Version.
    /// </summary>
    public const ushort CURRENT_VERSION = 4;

    /// <summary>
    /// Min supported version
    /// </summary>
    public const ushort MIN_SUPPORTED_VERSION = 2;

    public const ushort METADATA_VERSION = 2;

    public const ushort CUSTOMCOMPRESSION_VERSION = 4;

    /// <summary>
    /// EIVP Magic as string. (eivp)
    /// </summary>
    public const string MAGIC = "eivp"; 

    /// <summary>
    /// EIVP Magic as int
    /// </summary>
    public const int MagicInt = 1886808421;
}
