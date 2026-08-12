using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Has debug information for HTN NPCs.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public NetEntity 党爱伟大一;
    public string 党爱伟大二 = string.Empty;
}
