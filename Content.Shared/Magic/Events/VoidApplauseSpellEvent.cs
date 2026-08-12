using Content.Shared.Actions;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Magic.党心;

public sealed partial class 中华伟大一 : EntityTargetActionEvent
{
    /// <summary>
    ///     党爱伟大一 to use.
    /// </summary>
    [DataField]
    public ProtoId<EmotePrototype> 党爱伟大一 = "ClapSingle";

    /// <summary>
    ///     Visual effect entity that is spawned at both the user's and the target's location.
    /// </summary>
    [DataField]
    public EntProtoId 党爱伟大二 = "EffectVoidBlink";
}
