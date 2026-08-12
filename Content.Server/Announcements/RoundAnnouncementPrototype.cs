using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

/// <summary>
/// Used for any announcements on the start of a round.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField("sound")] public SoundSpecifier? Sound;

    [DataField("message")] public string? Message;
}
