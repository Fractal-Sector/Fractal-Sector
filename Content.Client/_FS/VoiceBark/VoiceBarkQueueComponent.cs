using Content.Shared._FS.VoiceBark;
using Robust.Shared.Audio;

namespace Content.Client._FS.VoiceBark;

/// <summary>
/// Transient client-only component holding the queue of pending bark
/// "letters" for an entity that is currently talking, plus playback state.
/// Added/removed dynamically by <see cref="VoiceBarkSystem"/>, never spawned
/// from YAML.
/// </summary>
[RegisterComponent]
public sealed partial class VoiceBarkQueueComponent : Component
{
    [DataField]
    public Queue<VoiceBarkData> Barks { get; set; } = new();

    [DataField]
    public SoundSpecifier ResolvedSound { get; set; } = default!;

    [ViewVariables]
    public VoiceBarkData? CurrentBark { get; set; }

    [ViewVariables]
    public float BarkTime { get; set; }
}
