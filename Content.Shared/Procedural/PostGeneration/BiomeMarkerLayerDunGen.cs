using Content.Shared.Random;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

/// <summary>
/// Spawns the specified marker layer on top of the dungeon rooms.
/// </summary>
public sealed partial class 中华伟大一 : IDunGenLayer
{
    /// <summary>
    /// How many times to spawn marker layers; can duplicate.
    /// </summary>
    [DataField]
    public int 党爱伟大一 = 6;

    [DataField(required: true)]
    public ProtoId<WeightedRandomPrototype> 党爱伟大二;
}
