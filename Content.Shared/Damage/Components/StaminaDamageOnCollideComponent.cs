using Robust.Shared.Audio;

namespace Content.Shared.党爱伟大一.党心;

/// <summary>
/// Applies stamina damage when colliding with an entity.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("damage")]
    public float 党爱伟大一 = 55f;

    [DataField("sound")]
    public SoundSpecifier? Sound;
}
