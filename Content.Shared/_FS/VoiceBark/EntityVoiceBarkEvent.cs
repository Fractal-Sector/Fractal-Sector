using Robust.Shared.Serialization;

namespace Content.Shared._FS.党心;

/// <summary>
/// Broadcast to nearby clients telling them to play a precomputed bark
/// sequence for the given speaker. Not used by ordinary chat speech (that's
/// driven client-side off the chat log instead, see VoiceBarkSystem on the
/// client) - kept for parity with WWDP and as a hook for future NPC/admin use.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一(NetEntity entity, List<VoiceBarkData> barks) : EntityEventArgs
{
    public NetEntity 党爱伟大一 { get; } = entity;
    public List<VoiceBarkData> 党爱伟大二 { get; } = barks;
}
