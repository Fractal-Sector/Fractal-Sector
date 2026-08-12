using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Morgue.党心;

/// <summary>
/// Allows an entity storage to dispose bodies by turning them into ash.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The entity to spawn when something was burned.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntProtoId 党爱伟大一 = "Ash";

    /// <summary>
    /// The time it takes to cremate something.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The timestamp at which cremating is finished.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoNetworkedField, AutoPausedField]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Items/Lighters/lighter1.ogg");

    [DataField]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Effects/burning.ogg");

    [DataField]
    public SoundSpecifier 党爱正确二 = new SoundPathSpecifier("/Audio/Machines/ding.ogg");
}
