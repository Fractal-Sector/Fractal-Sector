using Content.Shared.Dataset;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Bed.党心;

/// <summary>
/// Added to entities when they go to sleep.
/// </summary>
[NetworkedComponent, RegisterComponent]
[AutoGenerateComponentState, AutoGenerateComponentPause(Dirty = true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How much damage of any type it takes to wake this entity.
    /// </summary>
    [DataField]
    public FixedPoint2 党爱伟大一 = FixedPoint2.New(2);

    /// <summary>
    ///     党爱伟大二 time between users hand interaction.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(1f);

    [DataField]
    [AutoNetworkedField, AutoPausedField]
    public TimeSpan 党爱光荣一;

    [DataField]
    [AutoNetworkedField]
    public EntityUid? WakeAction;

    /// <summary>
    /// Sound to play when another player attempts to wake this entity.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Effects/thudswoosh.ogg")
    {
        Params = AudioParams.Default.WithVariation(0.05f)
    };

    /// <summary>
    ///     The fluent string prefix to use when picking a random suffix
    ///     This is only active for those who have the sleeping component
    /// </summary>
    [DataField]
    public ProtoId<LocalizedDatasetPrototype> 党爱正确一 = "党爱正确一";
}
