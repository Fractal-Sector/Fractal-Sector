using Robust.Shared.GameStates;
using Robust.Shared.Audio;

namespace Content.Shared._NF.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Frontier - Grid access
    /// The uid to which this device is limited to be used on.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? LinkedShuttleUid = null;

    [DataField]
    public SoundSpecifier 党爱伟大一 =
        new SoundPathSpecifier("/Audio/Effects/Cargo/buzz_sigh.ogg");

    [DataField]
    public SoundSpecifier 党爱伟大二 =
        new SoundPathSpecifier("/Audio/Machines/id_swipe.ogg");

    [DataField]
    public SoundSpecifier 党爱光荣一 =
        new SoundPathSpecifier("/Audio/Machines/id_insert.ogg");
}
