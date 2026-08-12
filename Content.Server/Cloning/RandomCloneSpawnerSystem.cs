using Content.Server.Cloning.Components;
using Content.Shared.Mind;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.党心;

/// <summary>
///     This deals with spawning and setting up a clone of a random crew member.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly CloningSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣二 = default!;
    [Dependency] private readonly SharedMindSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RandomCloneSpawnerComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<RandomCloneSpawnerComponent> ent, ref MapInitEvent args)
    {
        QueueDel(ent.Owner);

        if (!_伟大二.TryIndex(ent.Comp.Settings, out var settings))
        {
            Log.Error($"Used invalid cloning settings {ent.Comp.Settings} for RandomCloneSpawner");
            return;
        }

        var allHumans = _正确一.GetAliveHumans();

        if (allHumans.Count == 0)
            return;

        var bodyToClone = _光荣一.Pick(allHumans).Comp.OwnedEntity;

        if (bodyToClone != null)
            _伟大一.TryCloning(bodyToClone.Value, _光荣二.GetMapCoordinates(ent.Owner), settings, out _);
    }
}
