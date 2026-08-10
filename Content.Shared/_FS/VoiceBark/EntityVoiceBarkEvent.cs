using Robust.Shared.Serialization;

namespace Content.Shared._FS.VoiceBark;

/// <summary>
/// Broadcast to nearby clients telling them to play a precomputed bark
/// sequence for the given speaker. Not used by ordinary chat speech (that's
/// driven client-side off the chat log instead, see VoiceBarkSystem on the
/// client) - kept for parity with WWDP and as a hook for future NPC/admin use.
/// </summary>
[Serializable, NetSerializable]
public sealed class EntityVoiceBarkEvent(NetEntity entity, List<VoiceBarkData> barks) : EntityEventArgs
{
    public NetEntity Entity { get; } = entity;
    public List<VoiceBarkData> Barks { get; } = barks;
}
