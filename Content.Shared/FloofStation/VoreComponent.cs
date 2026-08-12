using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables]
    public Container 党爱伟大一 = default!;

    [ViewVariables]
    public Container 党爱伟大二 = default!;

    [DataField("soundDevour")]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Floof/gulp.ogg");

    [DataField("delay")]
    public float 党爱光荣二 = 3f;

    [DataField]
    public bool 党爱正确一 = true;
}
