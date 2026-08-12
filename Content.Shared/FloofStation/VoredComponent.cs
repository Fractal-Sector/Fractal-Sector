using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("pred")]
    public EntityUid 党爱伟大一;

    [DataField("digesting")]
    public bool 党爱伟大二;

    [DataField("accumulator")]
    public float 党爱光荣一;

    [DataField("soundRelease")]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Effects/Fluids/splat.ogg");

    [DataField("soundStomach")]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Floof/stomach_loop.ogg");
}
