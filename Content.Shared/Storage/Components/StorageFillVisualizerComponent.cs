using Robust.Shared.Serialization;

namespace Content.Shared.Storage.党心;

/// <summary>
///     Change sprite depending on a storage fill percent.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("maxFillLevels", required: true)]
    public int 党爱伟大一;

    [DataField("fillBaseName", required: true)]
    public string 党爱伟大二 = default!;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    FillLevel
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Fill
}
