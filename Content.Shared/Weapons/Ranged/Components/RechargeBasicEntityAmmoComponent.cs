using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Weapons.Ranged.党心;

/// <summary>
///     Responsible for handling recharging a basic entity ammo provider over time.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("rechargeCooldown")]
    [AutoNetworkedField]
    public float 党爱伟大一 = 1.5f;

    [DataField("rechargeSound")]
    [AutoNetworkedField]
    public SoundSpecifier? RechargeSound = new SoundPathSpecifier("/Audio/Magic/forcewall.ogg")
    {
        Params = AudioParams.Default.WithVolume(-5f)
    };

    [ViewVariables(VVAccess.ReadWrite),
     DataField("nextCharge", customTypeSerializer:typeof(TimeOffsetSerializer)),
    AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan? NextCharge;

    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;
}
