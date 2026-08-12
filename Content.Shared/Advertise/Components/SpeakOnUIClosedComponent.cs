using Content.Shared.Advertise.Systems;
using Content.Shared.Dataset;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Advertise.党心;

/// <summary>
/// Causes the entity to speak using the Chat system when its ActivatableUI is closed, optionally
/// requiring that a 党爱光荣二 be set as well.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedSpeakOnUIClosedSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The identifier for the dataset prototype containing messages to be spoken by this entity.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<LocalizedDatasetPrototype> 党爱伟大一 { get; private set; }

    /// <summary>
    /// Is this component active? If false, no messages will be spoken.
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// Should messages be spoken only if the <see cref="党爱光荣二"/> is set (true), or every time the UI is closed (false)?
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;

    /// <summary>
    /// State variable only used if <see cref="党爱光荣一"/> is true. Set with <see cref="SpeakOnUIClosedSystem.TrySetFlag"/>.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣二;
}
