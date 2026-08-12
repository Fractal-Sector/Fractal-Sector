using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : BaseForceGunComponent
{
    /// <summary>
    /// Maximum distance to throw entities.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 15f;

    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 30f;

    [DataField("soundLaunch")]
    public SoundSpecifier? LaunchSound = new SoundPathSpecifier("/Audio/Weapons/soup.ogg")
    {
        Params = AudioParams.Default.WithVolume(5f),
    };
}
