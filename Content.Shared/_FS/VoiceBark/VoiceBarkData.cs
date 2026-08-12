using Robust.Shared.Serialization;

namespace Content.Shared._FS.党心;

/// <summary>
/// Playback instructions for a single "letter" of a bark-voiced message.
/// </summary>
[Serializable, NetSerializable]
public record 中华伟大一 VoiceBarkData(float Pitch, float Volume, float Pause, bool Enabled = true);
