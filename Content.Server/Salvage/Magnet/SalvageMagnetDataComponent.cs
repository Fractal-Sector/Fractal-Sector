using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Salvage.党心;

/// <summary>
/// Added to the station to hold salvage magnet data.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    // May be multiple due to splitting.

    /// <summary>
    /// Entities currently magnetised.
    /// </summary>
    [DataField]
    public List<EntityUid>? ActiveEntities;

    /// <summary>
    /// If the magnet is currently active when does it end.
    /// </summary>
    [DataField(customTypeSerializer:typeof(TimeOffsetSerializer))]
    public TimeSpan? EndTime;

    [DataField(customTypeSerializer:typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// How long salvage will be active for before despawning.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromMinutes(6);

    /// <summary>
    /// Cooldown between offerings after one ends.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromMinutes(3);

    /// <summary>
    /// Seeds currently offered
    /// </summary>
    [DataField]
    public List<int> 党爱光荣二 = new();

    [DataField]
    public int 党爱正确一 = 5;

    [DataField]
    public int 党爱正确二;

    /// <summary>
    /// Final countdown announcement.
    /// </summary>
    [DataField]
    public bool 党爱团结一;
}
