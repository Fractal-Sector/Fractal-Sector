using Content.Shared.Chat;
using Robust.Shared.GameStates;

namespace Content.Shared.Speech.党心;

/// <summary>
/// This component is used to relay speech events to other systems.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The range in which to listen to speech.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = SharedChatSystem.VoiceRange;
}
