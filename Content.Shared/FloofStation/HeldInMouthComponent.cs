using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public EntityUid 党爱伟大一;

    [DataField]
    public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Effects/Fluids/splat.ogg");
}
