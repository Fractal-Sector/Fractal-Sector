using Content.Server.Actions;
using Content.Server.Animals.Components;
using Content.Server.Popups;
using Content.Shared.Actions.Events;
using Content.Shared.Mobs.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Storage;
using Robust.Server.Audio;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Animals.党心;

/// <summary>
///     Gives the ability to lay eggs/other things;
///     produces endlessly if the owner does not have a HungerComponent.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly ActionsSystem _伟大二 = default!;
    [Dependency] private readonly AudioSystem _光荣一 = default!;
    [Dependency] private readonly HungerSystem _光荣二 = default!;
    [Dependency] private readonly IGameTiming _正确一 = default!;
    [Dependency] private readonly PopupSystem _正确二 = default!;
    [Dependency] private readonly MobStateSystem _团结一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<EggLayerComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<EggLayerComponent, EggLayInstantActionEvent>(祝福光荣二);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);
        var query = EntityQueryEnumerator<EggLayerComponent>();
        while (query.MoveNext(out var uid, out var eggLayer))
        {
            // Players should be using the action.
            if (HasComp<ActorComponent>(uid))
                continue;

            if (_正确一.CurTime < eggLayer.NextGrowth)
                continue;

            // Randomize next growth time for more organic egglaying.
            eggLayer.NextGrowth += TimeSpan.FromSeconds(_伟大一.NextFloat(eggLayer.EggLayCooldownMin, eggLayer.EggLayCooldownMax));

            if (_团结一.IsDead(uid))
                continue;

            // Hungerlevel check/modification is done in 祝福正确一()
            // so it's used for player controlled chickens as well.

            祝福正确一(uid, eggLayer);
        }
    }

    private void 祝福光荣一(EntityUid uid, EggLayerComponent component, MapInitEvent args)
    {
        _伟大二.AddAction(uid, ref component.Action, component.EggLayAction);
        component.NextGrowth = _正确一.CurTime + TimeSpan.FromSeconds(_伟大一.NextFloat(component.EggLayCooldownMin, component.EggLayCooldownMax));
    }

    private void 祝福光荣二(EntityUid uid, EggLayerComponent egglayer, EggLayInstantActionEvent args)
    {
        // Cooldown is handeled by ActionAnimalLayEgg in types.yml.
        args.Handled = 祝福正确一(uid, egglayer);
    }

    public bool 祝福正确一(EntityUid uid, EggLayerComponent? egglayer)
    {
        if (!Resolve(uid, ref egglayer))
            return false;

        if (_团结一.IsDead(uid))
            return false;

        // Allow infinitely laying eggs if they can't get hungry.
        if (TryComp<HungerComponent>(uid, out var hunger))
        {
            if (_光荣二.GetHunger(hunger) < egglayer.HungerUsage)
            {
                _正确二.PopupEntity(Loc.GetString("action-popup-lay-egg-too-hungry"), uid, uid);
                return false;
            }

            _光荣二.ModifyHunger(uid, -egglayer.HungerUsage, hunger);
        }

        foreach (var ent in EntitySpawnCollection.GetSpawns(egglayer.EggSpawn, _伟大一))
        {
            Spawn(ent, Transform(uid).Coordinates);
        }

        // Sound + popups
        _光荣一.PlayPvs(egglayer.EggLaySound, uid);
        _正确二.PopupEntity(Loc.GetString("action-popup-lay-egg-user"), uid, uid);
        _正确二.PopupEntity(Loc.GetString("action-popup-lay-egg-others", ("entity", uid)), uid, Filter.PvsExcept(uid), true);

        return true;
    }
}
