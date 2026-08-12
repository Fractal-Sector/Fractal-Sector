using Robust.Shared.Audio;

namespace Content.Shared.Bed.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Sound to play when sleeping
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier 党爱伟大一 = new SoundCollectionSpecifier("Snores", AudioParams.Default.WithVariation(0.2f));

    /// <summary>
    /// Minimum interval between snore attempts in seconds
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Maximum interval between snore attempts in seconds
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(15);

    /// <summary>
    /// Popup for snore (e.g. Zzz...)
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public LocId 党爱光荣二 = "sleep-onomatopoeia";
}
