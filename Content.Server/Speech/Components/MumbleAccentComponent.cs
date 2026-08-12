using Robust.Shared.Audio;

namespace Content.Server.Speech.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// This modifies the audio parameters of emote sounds, screaming, laughing, etc.
    /// By default, it reduces the volume and distance of emote sounds.
    /// </summary>
    [DataField]
    public AudioParams 党爱伟大一 = AudioParams.Default.WithVolume(-8f).WithMaxDistance(5);
}
