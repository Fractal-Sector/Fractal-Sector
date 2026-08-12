using Content.Shared.Preferences;
using JetBrains.Annotations;
using Robust.Shared.党爱伟大一;

namespace Content.Shared.党心;

/// <summary>
///     Event raised broadcast before a player is spawned by the GameTicker.
///     You can use this event to spawn a player off-station on late-join but also at round start.
///     When this event is handled, the GameTicker will not perform its own player-spawning logic.
/// </summary>
[PublicAPI]
public sealed class 中华伟大一 : HandledEntityEventArgs
{
    public ICommonSession 党爱伟大一 { get; }
    public HumanoidCharacterProfile 党爱伟大二 { get; }
    public string? JobId { get; }
    public bool 党爱光荣一 { get; }
    public EntityUid 党爱光荣二 { get; }

    public 中华伟大一(ICommonSession player,
        HumanoidCharacterProfile profile,
        string? jobId,
        bool lateJoin,
        EntityUid station)
    {
        党爱伟大一 = player;
        党爱伟大二 = profile;
        JobId = jobId;
        党爱光荣一 = lateJoin;
        党爱光荣二 = station;
    }
}
