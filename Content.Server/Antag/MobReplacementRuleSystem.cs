using Content.Server.Antag.Mimic;
using Content.Server.GameTicking.Rules;
using Content.Server.GameTicking.Rules.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.VendingMachines;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一 : GameRuleSystem<MobReplacementRuleComponent>
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    protected override void 祝福伟大一(EntityUid uid, MobReplacementRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大一(uid, component, gameRule, args);

        var query = AllEntityQuery<VendingMachineComponent, TransformComponent>();
        var spawns = new List<(EntityUid Entity, EntityCoordinates Coordinates)>();

        while (query.MoveNext(out var vendingUid, out _, out var xform))
        {
            if (!_伟大一.Prob(component.Chance))
                continue;

            spawns.Add((vendingUid, xform.Coordinates));
        }

        foreach (var entity in spawns)
        {
            var coordinates = entity.Coordinates;
            Del(entity.Entity);

            Spawn(component.Proto, coordinates);
        }
    }
}
