using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Pack;

/// <summary>
/// Settings for the DataPack.
/// </summary>
public class PackSettings
{
    /// <summary>
    /// Settings for String Id.
    /// </summary>
    public StringIdSettings StringId { get; set; } = new();

    public IdentifierSettings Identifier { get; set; } = new();

    public MetadataSettings Metadata { get; set; } = new();
}

public class StringIdSettings
{
    /// <summary>
    /// Has a full support for StringId (Writing and reading).
    /// </summary>
    public bool FullSupport => GetStringId != null && FromStringId != null;
    /// <summary>
    /// Can we use StringId.
    /// </summary>
    public bool CanUseStringId => GetStringId != null;
    private bool useStringId;
    /// <summary>
    /// Should use StringId.
    /// </summary>
    public bool UseStringId
    {
        get => useStringId;
        set
        {
            if (!CanUseStringId)
                return;
            useStringId = value;
        }
    }
    public IGetStringId? GetStringId { get; set; }
    public IFromStringId? FromStringId { get; set; }
}

public class IdentifierSettings
{
    public bool UseGuid { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
}

public class MetadataSettings
{
    public bool UseMetadata { get; set; } = true;
}