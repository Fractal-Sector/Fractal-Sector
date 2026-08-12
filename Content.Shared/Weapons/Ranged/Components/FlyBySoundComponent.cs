using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Ranged.党心;

/// <summary>
/// Plays a sound when its non-hard fixture collides with a player.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Probability that the sound plays
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("prob")]
    public float 党爱伟大一 = 0.10f;

    [ViewVariables(VVAccess.ReadWrite), DataField("sound")]
    [AutoNetworkedField]
    public SoundSpecifier 党爱伟大二 = new SoundCollectionSpecifier("BulletMiss")
    {
        Params = AudioParams.Default,
    };

    [DataField("range")]
    [AutoNetworkedField]
    public float 党爱光荣一 = 1.5f;
}
