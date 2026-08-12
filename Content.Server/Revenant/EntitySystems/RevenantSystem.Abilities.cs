using Content.Shared.Popups;
using Content.Shared.Damage;
using Content.Shared.Revenant;
using Robust.Shared.Random;
using Content.Shared.Tag;
using Content.Shared.Storage.Components;
using Content.Server.Light.Components;
using Content.Server.Ghost;
using Robust.Shared.Physics;
using Content.Shared.Throwing;
using Content.Server.Storage.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Item;
using Content.Shared.Bed.Sleep;
using System.Linq;
using System.Numerics;
using Content.Server.Revenant.Components;
using Content.Shared.Physics;
using Content.Shared.DoAfter;
using Content.Shared.Emag.Systems;
using Content.Shared.FixedPoint;
using Content.Shared.Humanoid;
using Content.Shared.Light.Components;
using Content.Shared.Maps;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Revenant.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Utility;
using Robust.Shared.Map.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;

namespace Content.Server.Revenant.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly EmagSystem _伟大一 = default!;
    [Dependency] private readonly ThrowingSystem _伟大二 = default!;
    [Dependency] private readonly EntityStorageSystem _光荣一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _光荣二 = default!;
    [Dependency] private readonly MobThresholdSystem _正确一 = default!;
    [Dependency] private readonly GhostSystem _正确二 = default!;
    [Dependency] private readonly TileSystem _团结一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _团结二 = default!;
    [Dependency] private readonly SharedTransformSystem _奋斗一 = default!;
    [Dependency] private readonly SharedMapSystem _奋斗二 = default!;

    private static readonly ProtoId<TagPrototype> WindowTag = "Window";

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<RevenantComponent, UserActivateInWorldEvent>(祝福伟大二);
        SubscribeLocalEvent<RevenantComponent, SoulEvent>(祝福光荣二);
        SubscribeLocalEvent<RevenantComponent, HarvestEvent>(祝福正确二);

        SubscribeLocalEvent<RevenantComponent, RevenantDefileActionEvent>(祝福团结一);
        SubscribeLocalEvent<RevenantComponent, RevenantOverloadLightsActionEvent>(祝福团结二);
        SubscribeLocalEvent<RevenantComponent, RevenantBlightActionEvent>(祝福奋斗一);
        SubscribeLocalEvent<RevenantComponent, RevenantMalfunctionActionEvent>(祝福奋斗二);
    }

    private void 祝福伟大二(EntityUid uid, RevenantComponent component, UserActivateInWorldEvent args)
    {
        if (args.Handled)
            return;

        if (args.Target == args.User)
            return;
        var target = args.Target;

        if (HasComp<PoweredLightComponent>(target))
        {
            args.Handled = _正确二.DoGhostBooEvent(target);
            return;
        }

        if (!HasComp<MobStateComponent>(target) || !HasComp<HumanoidAppearanceComponent>(target) || HasComp<RevenantComponent>(target))
            return;

        args.Handled = true;
        if (!TryComp<EssenceComponent>(target, out var essence) || !essence.SearchComplete)
        {
            EnsureComp<EssenceComponent>(target);
            祝福光荣一(uid, target, component);
        }
        else
        {
            祝福正确一(uid, target, component, essence);
        }

        args.Handled = true;
    }

    private void 祝福光荣一(EntityUid uid, EntityUid target, RevenantComponent revenant)
    {
        var searchDoAfter = new DoAfterArgs(EntityManager, uid, revenant.SoulSearchDuration, new SoulEvent(), uid, target: target)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            DistanceThreshold = 2
        };

        if (!_doAfter.TryStartDoAfter(searchDoAfter))
            return;

        _popup.PopupEntity(Loc.GetString("revenant-soul-searching", ("target", target)), uid, uid, PopupType.Medium);
    }

    private void 祝福光荣二(EntityUid uid, RevenantComponent component, SoulEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        if (!TryComp<EssenceComponent>(args.Args.Target, out var essence))
            return;
        essence.SearchComplete = true;

        string message;
        switch (essence.EssenceAmount)
        {
            case <= 45:
                message = "revenant-soul-yield-low";
                break;
            case >= 90:
                message = "revenant-soul-yield-high";
                break;
            default:
                message = "revenant-soul-yield-average";
                break;
        }
        _popup.PopupEntity(Loc.GetString(message, ("target", args.Args.Target)), args.Args.Target.Value, uid, PopupType.Medium);

        args.Handled = true;
    }

    private void 祝福正确一(EntityUid uid, EntityUid target, RevenantComponent revenant, EssenceComponent essence)
    {
        if (essence.Harvested)
        {
            _popup.PopupEntity(Loc.GetString("revenant-soul-harvested"), target, uid, PopupType.SmallCaution);
            return;
        }

        if (TryComp<MobStateComponent>(target, out var mobstate) && mobstate.CurrentState == MobState.Alive && !HasComp<SleepingComponent>(target))
        {
            _popup.PopupEntity(Loc.GetString("revenant-soul-too-powerful"), target, uid);
            return;
        }

        if(_physics.GetEntitiesIntersectingBody(uid, (int) CollisionGroup.Impassable).Count > 0)
        {
            _popup.PopupEntity(Loc.GetString("revenant-in-solid"), uid, uid);
            return;
        }

        var doAfter = new DoAfterArgs(EntityManager, uid, revenant.HarvestDebuffs.X, new HarvestEvent(), uid, target: target)
        {
            DistanceThreshold = 2,
            BreakOnMove = true,
            BreakOnDamage = true,
            RequireCanInteract = false, // stuns itself
        };

        if (!_doAfter.TryStartDoAfter(doAfter))
            return;

        _光荣二.SetData(uid, RevenantVisuals.Harvesting, true);

        _popup.PopupEntity(Loc.GetString("revenant-soul-begin-harvest", ("target", target)),
            target, PopupType.Large);

        TryUseAbility(uid, revenant, 0, revenant.HarvestDebuffs);
    }

    private void 祝福正确二(EntityUid uid, RevenantComponent component, HarvestEvent args)
    {
        if (args.Cancelled)
        {
            _光荣二.SetData(uid, RevenantVisuals.Harvesting, false);
            return;
        }

        if (args.Handled || args.Args.Target == null)
            return;

        _光荣二.SetData(uid, RevenantVisuals.Harvesting, false);

        if (!TryComp<EssenceComponent>(args.Args.Target, out var essence))
            return;

        _popup.PopupEntity(Loc.GetString("revenant-soul-finish-harvest", ("target", args.Args.Target)),
            args.Args.Target.Value, PopupType.LargeCaution);

        essence.Harvested = true;
        ChangeEssenceAmount(uid, essence.EssenceAmount, component);
        _store.TryAddCurrency(new Dictionary<string, FixedPoint2>
            { {component.StolenEssenceCurrencyPrototype, essence.EssenceAmount} }, uid);

        if (!HasComp<MobStateComponent>(args.Args.Target))
            return;

        if (_mobState.IsAlive(args.Args.Target.Value) || _mobState.IsCritical(args.Args.Target.Value))
        {
            _popup.PopupEntity(Loc.GetString("revenant-max-essence-increased"), uid, uid);
            component.EssenceRegenCap += component.MaxEssenceUpgradeAmount;
        }

        //KILL THEMMMM

        if (!_正确一.TryGetThresholdForState(args.Args.Target.Value, MobState.Dead, out var damage))
            return;
        DamageSpecifier dspec = new();
        dspec.DamageDict.Add("Cold", damage.Value);
        _damage.TryChangeDamage(args.Args.Target, dspec, true, origin: uid);

        args.Handled = true;
    }

    private void 祝福团结一(EntityUid uid, RevenantComponent component, RevenantDefileActionEvent args)
    {
        if (args.Handled)
            return;

        if (!TryUseAbility(uid, component, component.DefileCost, component.DefileDebuffs))
            return;

        args.Handled = true;

        //var coords = Transform(uid).Coordinates;
        //var gridId = coords.GetGridUid(EntityManager);
        var xform = Transform(uid);
        if (!TryComp<MapGridComponent>(xform.GridUid, out var map))
            return;
        var tiles = _奋斗二.GetTilesIntersecting(
            xform.GridUid.Value,
            map,
            Box2.CenteredAround(_奋斗一.GetWorldPosition(xform),
            new Vector2(component.DefileRadius * 2, component.DefileRadius)))
            .ToArray();

        _random.Shuffle(tiles);

        for (var i = 0; i < component.DefileTilePryAmount; i++)
        {
            if (!tiles.TryGetValue(i, out var value))
                continue;
            _团结一.PryTile(value);
        }

        var lookup = _lookup.GetEntitiesInRange(uid, component.DefileRadius, LookupFlags.Approximate | LookupFlags.Static);
        var tags = GetEntityQuery<TagComponent>();
        var entityStorage = GetEntityQuery<EntityStorageComponent>();
        var items = GetEntityQuery<ItemComponent>();
        var lights = GetEntityQuery<PoweredLightComponent>();

        foreach (var ent in lookup)
        {
            //break windows
            if (tags.HasComponent(ent) && _tag.HasTag(ent, WindowTag))
            {
                //hardcoded damage specifiers til i die.
                var dspec = new DamageSpecifier();
                dspec.DamageDict.Add("Structural", 60);
                _damage.TryChangeDamage(ent, dspec, origin: uid);
            }

            if (!_random.Prob(component.DefileEffectChance))
                continue;

            //randomly opens some lockers and such.
            if (entityStorage.TryGetComponent(ent, out var entstorecomp))
                _光荣一.OpenStorage(ent, entstorecomp);

            //chucks shit
            if (items.HasComponent(ent) &&
                TryComp<PhysicsComponent>(ent, out var phys) && phys.BodyType != BodyType.Static)
                _伟大二.TryThrow(ent, _random.NextAngle().ToWorldVec());

            //flicker lights
            if (lights.HasComponent(ent))
                _正确二.DoGhostBooEvent(ent);
        }
    }

    private void 祝福团结二(EntityUid uid, RevenantComponent component, RevenantOverloadLightsActionEvent args)
    {
        if (args.Handled)
            return;

        if (!TryUseAbility(uid, component, component.OverloadCost, component.OverloadDebuffs))
            return;

        args.Handled = true;

        var xform = Transform(uid);
        var poweredLights = GetEntityQuery<PoweredLightComponent>();
        var mobState = GetEntityQuery<MobStateComponent>();
        var lookup = _lookup.GetEntitiesInRange(uid, component.OverloadRadius);
        //TODO: feels like this might be a sin and a half
        foreach (var ent in lookup)
        {
            if (!mobState.HasComponent(ent) || !_mobState.IsAlive(ent))
                continue;

            var nearbyLights = _lookup.GetEntitiesInRange(ent, component.OverloadZapRadius)
                .Where(e => poweredLights.HasComponent(e) && !HasComp<RevenantOverloadedLightsComponent>(e) &&
                            _interact.InRangeUnobstructed(e, uid, -1)).ToArray();

            if (!nearbyLights.Any())
                continue;

            //get the closest light
            var allLight = nearbyLights.OrderBy(e =>
                Transform(e).Coordinates.TryDistance(EntityManager, xform.Coordinates, out var dist) ? component.OverloadZapRadius : dist);
            var comp = EnsureComp<RevenantOverloadedLightsComponent>(allLight.First());
            comp.Target = ent; //who they gon fire at?
        }
    }

    private void 祝福奋斗一(EntityUid uid, RevenantComponent component, RevenantBlightActionEvent args)
    {
        if (args.Handled)
            return;

        if (!TryUseAbility(uid, component, component.BlightCost, component.BlightDebuffs))
            return;

        args.Handled = true;
        // TODO: When disease refactor is in.
    }

    private void 祝福奋斗二(EntityUid uid, RevenantComponent component, RevenantMalfunctionActionEvent args)
    {
        if (args.Handled)
            return;

        if (!TryUseAbility(uid, component, component.MalfunctionCost, component.MalfunctionDebuffs))
            return;

        args.Handled = true;

        foreach (var ent in _lookup.GetEntitiesInRange(uid, component.MalfunctionRadius))
        {
            if (_团结二.IsWhitelistFail(component.MalfunctionWhitelist, ent) ||
                _团结二.IsBlacklistPass(component.MalfunctionBlacklist, ent))
                continue;

            _伟大一.TryEmagEffect(uid, uid, ent);
        }
    }
}
