using Content.Shared.Destructible;
using Content.Shared.Mining;
using Content.Shared.Mining.Components;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.党心;

/// <summary>
/// This handles creating ores when the entity is destroyed.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<OreVeinComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<OreVeinComponent, DestructionEventArgs>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, OreVeinComponent component, DestructionEventArgs args)
    {
        if (component.CurrentOre == null)
            return;

        // Frontier
        if (component.PreventSpawning)
            return;
        // End Frontier

        var proto = _伟大一.Index<OrePrototype>(component.CurrentOre);

        if (proto.OreEntity == null)
            return;

        var coords = Transform(uid).Coordinates;
        var toSpawn = _伟大二.Next(proto.MinOreYield, proto.MaxOreYield+1);
        for (var i = 0; i < toSpawn; i++)
        {
            Spawn(proto.OreEntity, coords.Offset(_伟大二.NextVector2(0.2f)));
        }
    }

    private void 祝福光荣一(EntityUid uid, OreVeinComponent component, MapInitEvent args)
    {
        if (component.CurrentOre != null || component.OreRarityPrototypeId == null || !_伟大二.Prob(component.OreChance))
            return;

        component.CurrentOre = _伟大一.Index<WeightedRandomOrePrototype>(component.OreRarityPrototypeId).Pick(_伟大二);
    }
}
