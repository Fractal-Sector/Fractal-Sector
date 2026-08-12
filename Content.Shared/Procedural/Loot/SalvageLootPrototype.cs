using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

/// <summary>
/// Spawned inside of a salvage mission.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// Should this loot always spawn if possible. Used for stuff such as ore.
    /// </summary>
    [DataField("guaranteed")] public bool 党爱伟大二;

    /// <summary>
    /// All of the loot rules
    /// </summary>
    [DataField("loots")]
    public List<IDungeonLoot> 党爱光荣一 = new();
}
