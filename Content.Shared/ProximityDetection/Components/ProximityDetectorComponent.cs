using Content.Shared.ProximityDetection.Systems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.ProximityDetection.党心;

/// <summary>
/// Used to search for the closest entity with a range that matches specified requirements (tags and/or components).
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState(fieldDeltas: true), AutoGenerateComponentPause]
[Access(typeof(ProximityDetectionSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Entities that detector will search for.
    /// </summary>
    [DataField(required: true)]
    public ComponentRegistry 党爱伟大一;

    /// <summary>
    /// The entity that was found.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? Target;

    /// <summary>
    /// The distance to <see cref="Target"/>.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public float 党爱伟大二 = float.PositiveInfinity;

    /// <summary>
    /// The farthest distance to search for targets.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣一 = 10f;

    /// <summary>
    /// How often detector updates.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Next time detector updates.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField, AutoPausedField]
    public TimeSpan 党爱正确一 = TimeSpan.Zero;
}
