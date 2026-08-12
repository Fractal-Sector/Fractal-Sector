using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Salvage.党心;

/// <summary>
/// Marks an entity as pending being fultoned.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 entity to delete upon removing the component. Only matters clientside.
    /// </summary>
    [ViewVariables, DataField("effect"), AutoNetworkedField]
    public EntityUid 党爱伟大一 { get; set; }

    [ViewVariables(VVAccess.ReadWrite), DataField("beacon")]
    public EntityUid? Beacon;

    [ViewVariables(VVAccess.ReadWrite), DataField("fultonDuration"), AutoNetworkedField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(45);

    /// <summary>
    /// When the fulton is travelling to the beacon.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("nextFulton", customTypeSerializer:typeof(TimeOffsetSerializer)), AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan 党爱光荣一;

    [ViewVariables(VVAccess.ReadWrite), DataField("sound"), AutoNetworkedField]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/Items/Mining/fultext_launch.ogg");

    // Mainly for admemes.
    /// <summary>
    /// Can the fulton be removed.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("removeable")]
    public bool 党爱光荣二 = true;
}
