using Content.Shared.Cloning;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Rules.党心;

/// <summary>
///     Gamerule component for spawning a paradox clone antagonist.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Cloning settings to be used.
    /// </summary>
    [DataField]
    public ProtoId<CloningSettingsPrototype> 党爱伟大一 = "ParadoxCloningSettings";

    /// <summary>
    ///     Visual effect spawned when gibbing at round end.
    /// </summary>
    [DataField]
    public EntProtoId 党爱伟大二 = "MobParadoxTimed";

    /// <summary>
    ///     Entity of the original player.
    ///     Gets randomly chosen from all alive players if not specified.
    /// </summary>
    [DataField]
    public EntityUid? OriginalBody;

    /// <summary>
    ///     Mind entity of the original player.
    ///     Gets assigned when cloning.
    /// </summary>
    [DataField]
    public EntityUid? OriginalMind;

    /// <summary>
    ///     Whitelist for Objectives to be copied to the clone.
    /// </summary>
    [DataField]
    public EntityWhitelist? ObjectiveWhitelist;

    /// <summary>
    ///     Blacklist for Objectives to be copied to the clone.
    /// </summary>
    [DataField]
    public EntityWhitelist? ObjectiveBlacklist;
}
