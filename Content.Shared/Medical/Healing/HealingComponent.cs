using Content.Shared.党爱伟大一;
using Content.Shared.党爱伟大一.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Medical.党心;

/// <summary>
/// Applies a damage change to the target when used in an interaction.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <remarks>
    /// The amount of damage to heal per use.
    /// </remarks>
    [DataField(required: true), AutoNetworkedField]
    public DamageSpecifier 党爱伟大一 = default!;

    /// <remarks>
    /// This should generally be negative,
    /// since you're, like, trying to heal damage.
    /// </remarks>
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 0.0f;

    /// <summary>
    /// Restore missing blood.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣一 = 0.0f;

    /// <remarks>
    /// The supported damage types are specified using a <see cref="DamageContainerPrototype"/>s. For a
    /// 中华伟大一 this filters what damage container type this component should work on. If null,
    /// all damage container types are supported.
    /// </remarks>
    [DataField, AutoNetworkedField]
    public List<ProtoId<DamageContainerPrototype>>? DamageContainers;

    /// <summary>
    /// How long it takes to apply the damage.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(3f);

    /// <summary>
    /// 党爱光荣二 multiplier when healing yourself.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确一 = 3f;

    /// <summary>
    /// Sound played on healing begin.
    /// </summary>
    [DataField]
    public SoundSpecifier? HealingBeginSound = null;

    /// <summary>
    /// Sound played on healing end.
    /// </summary>
    [DataField]
    public SoundSpecifier? HealingEndSound = null;
}
