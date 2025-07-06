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

    /// <summary>
    /// <see cref="Guid"/> Identifier Settings
    /// </summary>
    public IdentifierSettings Identifier { get; set; } = new();

    /// <summary>
    /// Settings for <see cref="FileMetadata"/>.
    /// </summary>
    public MetadataSettings Metadata { get; set; } = new();
}

/// <summary>
/// Settings for String Id.
/// </summary>
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

    /// <summary>
    /// <see cref="string"/> convertion to <see cref="int"/>.
    /// </summary>
    public IGetStringId? GetStringId { get; set; }
    /// <summary>
    /// <see cref="int"/> convertion to <see cref="string"/>.
    /// </summary>
    public IFromStringId? FromStringId { get; set; }
}

/// <summary>
/// <see cref="Guid"/> Identifier Settings
/// </summary>
public class IdentifierSettings
{
    /// <summary>
    /// Is Pack using <see cref="Id"/>.
    /// </summary>
    public bool UseGuid { get; set; }

    /// <summary>
    /// Id of the File.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
}

/// <summary>
/// Settings for <see cref="FileMetadata"/>.
/// </summary>
public class MetadataSettings
{
    /// <summary>
    /// Should write Metadata of the File.
    /// </summary>
    public bool UseMetadata { get; set; } = true;
}