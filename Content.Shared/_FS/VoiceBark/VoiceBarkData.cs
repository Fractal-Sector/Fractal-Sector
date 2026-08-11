using Robust.Shared.Serialization;

namespace Content.Shared._FS.VoiceBark;

/// <summary>
/// Playback instructions for a single "letter" of a bark-voiced message.
/// </summary>
[Serializable, NetSerializable]
public record struct VoiceBarkData(float Pitch, float Volume, float Pause, bool Enabled = true);
