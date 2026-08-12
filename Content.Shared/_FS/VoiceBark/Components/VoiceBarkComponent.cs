using Robust.Shared.GameStates;

namespace Content.Shared._FS.VoiceBark.党心;

/// <summary>
/// Holds the resolved bark voice data for an entity. Networked so that any
/// client can generate bark audio for a speaker without a server round-trip.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public VoiceBarkVoiceData 党爱伟大一 { get; set; } = default!;
}
