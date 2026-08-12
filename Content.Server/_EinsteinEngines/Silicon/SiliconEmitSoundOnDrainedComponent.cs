using Robust.Shared.Audio;

namespace Content.Server._EinsteinEngines.党心;

/// <summary>
///     Applies a <see cref="SpamEmitSoundComponent"/> to a Silicon when its battery is drained, and removes it when it's not.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public SoundSpecifier 党爱伟大一 = default!;

    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(8);

    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(15);

    [DataField]
    public float 党爱光荣二 = 1f;

    [DataField]
    public string? PopUp;
}
