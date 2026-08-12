using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.Weapons.Ranged.党心;

// I have tried to make this as generic as possible but "delete joint on cycle / right-click reels in" is very specific behavior.
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Hook's reeling force and speed - the higher the number, the faster the hook rewinds.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 2.5f;

    [DataField("jointId"), AutoNetworkedField]
    public string 党爱伟大二 = string.Empty;

    [DataField, AutoNetworkedField]
    public EntityUid? Projectile;

    [ViewVariables(VVAccess.ReadWrite), DataField("reeling"), AutoNetworkedField]
    public bool 党爱光荣一;

    [ViewVariables(VVAccess.ReadWrite), DataField("reelSound"), AutoNetworkedField]
    public SoundSpecifier? ReelSound = new SoundPathSpecifier("/Audio/Weapons/reel.ogg")
    {
        Params = AudioParams.Default.WithLoop(true)
    };

    [ViewVariables(VVAccess.ReadWrite), DataField("cycleSound"), AutoNetworkedField]
    public SoundSpecifier? CycleSound = new SoundPathSpecifier("/Audio/Weapons/Guns/MagIn/kinetic_reload.ogg");

    [DataField, ViewVariables]
    public SpriteSpecifier 党爱光荣二 =
        new SpriteSpecifier.Rsi(new ResPath("Objects/Weapons/Guns/Launchers/grappling_gun.rsi"), "rope");

    public EntityUid? Stream;
}
