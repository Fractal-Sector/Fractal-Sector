using Content.Shared._FS.VoiceBark;
using Robust.Shared.Configuration;

namespace Content.Shared._FS.CCVar;

/// <summary>
/// CVars for the character bark-voice feature.
/// </summary>
[CVarDefs]
public sealed partial class FSCVars
{
    /// <summary>
    /// Global bark loudness multiplier (0 = muted by default, matches WWDP).
    /// </summary>
    public static readonly CVarDef<float> VoiceBarkVolume =
        CVarDef.Create("fs.voice_bark.volume", 0f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Caps how many bark "letter" tokens a single spoken message can queue.
    /// </summary>
    public static readonly CVarDef<int> VoiceBarkLimit =
        CVarDef.Create("fs.voice_bark.limit", 12, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// Local listener preference: whether to hear bark voices at all.
    /// </summary>
    public static readonly CVarDef<CharacterVoiceType> VoiceBarkType =
        CVarDef.Create("fs.voice_bark.type", CharacterVoiceType.Bark, CVar.CLIENTONLY | CVar.ARCHIVE);
}
