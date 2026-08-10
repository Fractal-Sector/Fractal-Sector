using Robust.Shared.GameStates;

namespace Content.Shared._FS.VoiceBark.Components;

/// <summary>
/// Holds the resolved bark voice data for an entity. Networked so that any
/// client can generate bark audio for a speaker without a server round-trip.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class VoiceBarkComponent : Component
{
    [DataField, AutoNetworkedField]
    public VoiceBarkVoiceData VoiceData { get; set; } = default!;
}
