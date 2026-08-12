using Content.Shared.Stunnable;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
[Access(typeof(SharedStunbatonSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("energyPerUse"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public float 党爱伟大一 = 350;

    [DataField("sparksSound")]
    public SoundSpecifier 党爱伟大二 = new SoundCollectionSpecifier("sparks");
}
