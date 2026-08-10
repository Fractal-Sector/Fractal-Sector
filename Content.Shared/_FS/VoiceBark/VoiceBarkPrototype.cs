using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared._FS.VoiceBark;

/// <summary>
/// A single selectable bark voice - the sound it plays per "letter" plus the
/// pitch/volume/pause ranges its percentage sliders map onto.
/// </summary>
[Prototype]
public sealed partial class VoiceBarkPrototype : IPrototype
{
    /// <summary>
    /// Prototype ID assigned to a character that hasn't picked a bark voice yet.
    /// </summary>
    public const string DefaultId = "Default";

    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public SoundSpecifier BarkSound { get; set; } = default!;

    [DataField]
    public VoiceBarkClampData ClampData { get; set; } = new();
}

/// <summary>
/// The master menu of bark voices offered in the character editor.
/// No per-voice gating (e.g. gender) is implemented - this fork has no
/// generic CharacterRequirement abstraction like WWDP's, and the task
/// didn't call for restricting voices at launch.
/// </summary>
[Prototype]
public sealed partial class VoiceBarkListPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public List<ProtoId<VoiceBarkPrototype>> VoiceList { get; set; } = new();
}
