using EIV_DataPack.Interfaces;

namespace EIV_DataPack.Pack;

public class PackSettings
{
    public StringIdSettings StringId { get; set; } = new();

    public IdentifierSettings Identifier { get; set; } = new();

    public MetadataSettings Metadata { get; set; } = new();
}

public class StringIdSettings
{
    public bool FullSupport => GetStringId != null && FromStringId != null;
    public bool CanUseStringId => GetStringId != null;
    private bool useStringId;
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