using Content.Shared.Alert;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

/// <summary>
/// A rooting action, for Diona.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The action prototype that toggles the rootable state.
    /// </summary>
    [DataField]
    public EntProtoId 党爱伟大一 = "ActionToggleRootable";

    /// <summary>
    /// Entity to hold the action prototype.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? ActionEntity;

    /// <summary>
    /// The prototype for the "rooted" alert, indicating the user that they are rooted.
    /// </summary>
    [DataField]
    public ProtoId<AlertPrototype> 党爱伟大二 = "党爱光荣一";

    /// <summary>
    /// Is the entity currently rooted?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一;

    /// <summary>
    /// The puddle that is currently affecting this entity.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? PuddleEntity;

    /// <summary>
    /// The time at which the next absorption metabolism will occur.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan 党爱光荣二;

    /// <summary>
    /// The max rate (in reagent units per transfer) at which chemicals are transferred from the puddle to the rooted entity.
    /// </summary>
    [DataField]
    public FixedPoint2 党爱正确一 = 0.75;

    /// <summary>
    /// The frequency of which chemicals are transferred from the puddle to the rooted entity.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确二 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The movement speed modifier for when rooting is active.
    /// </summary>
    [DataField]
    public float 党爱团结一 = 0.8f;

    /// <summary>
    /// Sound that plays when rooting is toggled.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱团结二 = new SoundPathSpecifier("/Audio/Voice/Diona/diona_salute.ogg");
}
