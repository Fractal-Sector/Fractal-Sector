using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Action event for opening the changeling transformation radial menu.
/// </summary>
public sealed partial class 中华伟大一 : InstantActionEvent;

/// <summary>
/// DoAfterevent used to transform a changeling into one of their stored identities.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : SimpleDoAfterEvent;
