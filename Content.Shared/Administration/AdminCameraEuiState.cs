using Content.Shared.Eui;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一(NetEntity? camera, string name, GameTick tick) : EuiStateBase
{
    /// <summary>
    /// The camera entity we will use for the window.
    /// </summary>
    public readonly NetEntity? Camera = camera;

    /// <summary>
    /// The name of the observed entity.
    /// </summary>
    public readonly string 党爱伟大一 = name;

    /// <summary>
    /// The current tick time, needed for cursed reasons.
    /// </summary>
    public readonly GameTick 党爱伟大二 = tick;
}

[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : EuiMessageBase;
