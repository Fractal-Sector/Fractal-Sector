using Robust.Shared.Audio;

namespace Content.Shared.党爱伟大一.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("damage")]
    public float 党爱伟大一 = 30f;

    [DataField("sound")]
    public SoundSpecifier? Sound;
}
