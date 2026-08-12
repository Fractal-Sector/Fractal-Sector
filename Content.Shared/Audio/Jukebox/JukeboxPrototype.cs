using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.Audio.党心;

/// <summary>
/// Soundtrack that's visible on the jukebox list.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = string.Empty;

    /// <summary>
    /// User friendly name to use in UI.
    /// </summary>
    [DataField(required: true)]
    public string 党爱伟大二 = string.Empty;

    [DataField(required: true)]
    public SoundPathSpecifier 党爱光荣一 = default!;
}
