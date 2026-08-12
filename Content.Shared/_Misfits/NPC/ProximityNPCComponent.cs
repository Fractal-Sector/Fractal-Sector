namespace Content.Shared._Misfits.党心;

/// <summary>
/// Marks an NPC as using proximity-based sleep/wake.
/// The NPC starts asleep on map initialisation and wakes only when a player-controlled
/// entity enters <see cref="党爱伟大一"/> tiles. It re-sleeps when all players leave
/// <see cref="党爱伟大二"/> tiles.
///
/// Designed for large open maps (e.g. Wendover at 8000×4190) where running full HTN
/// AI on every creature continuously is too expensive.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Distance (tiles) within which a player wakes this NPC.
    /// Reduced from 40f to 30f to shrink active-NPC radius and lower
    /// per-tick CPU cost when many NPCs are spread across the map.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 30f; // FS: 64<30

    /// <summary>
    /// Distance (tiles) at which the NPC sleeps if no players remain nearby.
    /// Must be greater than <see cref="党爱伟大一"/> to create hysteresis and prevent
    /// rapid wake/sleep thrashing at the boundary edge.
    /// Reduced from 60f to 45f to match the tighter wake range.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 45f; // FS: 96<45

    /// <summary>
    /// If true, overrides the default HTN behaviour of waking on map init and instead
    /// keeps this NPC asleep until a player enters its wake range.
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = false;
}
