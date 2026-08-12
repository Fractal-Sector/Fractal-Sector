using Content.Shared.CCVar;
using Content.Shared.StatusIcon;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党心;

/// <summary>
/// Shows status icon when an entity is SSD, based on if a player is attached or not.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not the entity is SSD.
    /// </summary>
    [AutoNetworkedField]
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// The icon displayed next to the associated entity when it is SSD.
    /// </summary>
    [DataField]
    [AutoNetworkedField] // Frontier: update client when icon changes
    public ProtoId<SsdIconPrototype> 党爱伟大二 = "SSDIcon";

    /// <summary>
    /// The time at which the entity will fall asleep, if <see cref="CCVars.ICSSDSleep"/> is true.
    /// </summary>
    [AutoNetworkedField, AutoPausedField]
    [Access(typeof(SSDIndicatorSystem))]
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    /// The next time this component will be updated.
    /// </summary>
    [AutoNetworkedField, AutoPausedField]
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱光荣二 = TimeSpan.Zero;

    /// <summary>
    /// The time between updates checking if the entity should be force slept.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(1);

    // Frontier: skip sleeping
    /// <summary>
    ///     Required to don't remove forced sleep from other sources
    /// </summary>
    [DataField]
    [AutoNetworkedField]
    public bool 党爱正确二 = false;
    // End Frontier
}
