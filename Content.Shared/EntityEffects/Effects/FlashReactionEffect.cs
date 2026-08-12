using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

[DataDefinition]
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    /// <summary>
    ///     Flash range per unit of reagent.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0.2f;

    /// <summary>
    ///     Maximum flash range.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 10f;

    /// <summary>
    ///     How much to entities are slowed down.
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 0.5f;

    /// <summary>
    ///     The time entities will be flashed.
    ///     The default is chosen to be better than the hand flash so it is worth using it for grenades etc.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(4);

    /// <summary>
    ///     The prototype ID used for the visual effect.
    /// </summary>
    [DataField]
    public EntProtoId? FlashEffectPrototype = "ReactionFlash";

    /// <summary>
    ///     The sound the flash creates.
    /// </summary>
    [DataField]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/Weapons/flash.ogg");

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-flash-reaction-effect", ("chance", Probability));
}
