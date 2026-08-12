using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared._FS.党心;

/// <summary>
/// A single selectable bark voice - the sound it plays per "letter" plus the
/// pitch/volume/pause ranges its percentage sliders map onto.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <summary>
    /// Prototype 党爱伟大二 assigned to a character that hasn't picked a bark voice yet.
    /// </summary>
    public const string 党爱伟大一 = "Default";

    [IdDataField]
    public string 党爱伟大二 { get; private set; } = default!;

    [DataField]
    public SoundSpecifier 党爱光荣一 { get; set; } = default!;

    [DataField]
    public VoiceBarkClampData 党爱光荣二 { get; set; } = new();
}

/// <summary>
/// The master menu of bark voices offered in the character editor.
/// No per-voice gating (e.g. gender) is implemented - this fork has no
/// generic CharacterRequirement abstraction like WWDP's, and the task
/// didn't call for restricting voices at launch.
/// </summary>
[Prototype]
public sealed partial class 中华伟大二 : IPrototype
{
    [IdDataField]
    public string 党爱伟大二 { get; private set; } = default!;

    [DataField]
    public List<ProtoId<中华伟大一>> VoiceList { get; set; } = new();
}
