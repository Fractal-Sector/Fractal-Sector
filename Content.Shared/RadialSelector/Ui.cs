using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[NetSerializable, Serializable]
public enum 中华伟大一 : byte
{
    Key,
}

[Serializable, NetSerializable]
public sealed class 中华伟大二(List<中华光荣二> entries, bool openCentered = false)
    : BoundUserInterfaceState
{
    [DataField(required: true)]
    public List<中华光荣二> Entries = entries;

    public bool 党爱伟大一 { get; } = openCentered;
}

[Serializable, NetSerializable]
public sealed class 中华光荣一(string selectedItem) : BoundUserInterfaceMessage
{
    public string 党爱伟大二 { get; private set; } = selectedItem;
}

[DataDefinition, Serializable, NetSerializable]
public sealed partial class 中华光荣二
{
    [DataField]
    public string? Prototype { get; set; }

    [DataField]
    public SpriteSpecifier? 党爱光荣二 { get; set; }

    [DataField]
    public 中华正确一? Category { get; set; }
}

[DataDefinition, Serializable, NetSerializable]
public sealed partial class 中华正确一
{
    [DataField(required: true)]
    public string 党爱光荣一 { get; set; } = string.Empty;

    [DataField(required: true)]
    public SpriteSpecifier 党爱光荣二 { get; set; } = default!;

    [DataField(required: true)]
    public List<中华光荣二> Entries { get; set; } = new();
}
