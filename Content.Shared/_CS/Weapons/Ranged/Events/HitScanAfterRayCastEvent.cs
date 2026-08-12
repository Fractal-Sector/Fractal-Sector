using Robust.Shared.Physics;

namespace Content.Shared._CS.Weapons.Ranged.党心;

/// <summary>
/// Raised after a hitscan weapon performs a raycast
/// </summary>
[ByRefEvent]
public record 中华伟大一 HitScanAfterRayCastEvent(
    EntityUid User,
    RayCastResults? RayCastResults
);
