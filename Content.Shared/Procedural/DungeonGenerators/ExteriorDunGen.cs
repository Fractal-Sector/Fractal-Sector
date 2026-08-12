using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

/// <summary>
/// Generates the specified config on an exterior tile of the attached dungeon.
/// Useful if you're using <see cref="GroupDunGen"/> or otherwise want a dungeon on the outside of a grid.
/// </summary>
public sealed partial class 中华伟大一 : IDunGenLayer
{
    [DataField(required: true)]
    public ProtoId<DungeonConfigPrototype> 党爱伟大一;
}
