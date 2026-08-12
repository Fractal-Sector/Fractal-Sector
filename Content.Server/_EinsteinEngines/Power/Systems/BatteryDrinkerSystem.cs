using System.Linq;
using Content.Server.Power.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.DoAfter;
using Content.Shared.PowerCell.Components;
using Content.Shared._EinsteinEngines.Silicon;
using Content.Shared.Verbs;
using Robust.Shared.Utility;
using Content.Server._EinsteinEngines.Silicon.Charge;
using Content.Server.Power.EntitySystems;
using Content.Server.Popups;
using Content.Server.PowerCell;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Content.Server._EinsteinEngines.Power.Components;
using Content.Server._EinsteinEngines.Silicon;

namespace Content.Server._EinsteinEngines.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ItemSlotsSystem _伟大一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly BatterySystem _光荣二 = default!;
    [Dependency] private readonly SiliconChargeSystem _正确一 = default!;
    [Dependency] private readonly PopupSystem _正确二 = default!;
    [Dependency] private readonly PowerCellSystem _团结一 = default!;
    [Dependency] private readonly SharedContainerSystem _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<BatteryComponent, GetVerbsEvent<AlternativeVerb>>(祝福伟大二);

        SubscribeLocalEvent<BatteryDrinkerComponent, BatteryDrinkerDoAfterEvent>(祝福正确一);
    }

    private void 祝福伟大二(EntityUid uid, BatteryComponent batteryComponent, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        if (!TryComp<BatteryDrinkerComponent>(args.User, out var drinkerComp) ||
            !祝福光荣一(uid, drinkerComp) ||
            !_正确一.TryGetSiliconBattery(args.User, out var drinkerBattery))
            return;

        AlternativeVerb verb = new()
        {
            Act = () => 祝福光荣二(uid, args.User, drinkerComp),
            Text = Loc.GetString("battery-drinker-verb-drink"),
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/smite.svg.192dpi.png")),
        };

        args.Verbs.Add(verb);
    }

    private bool 祝福光荣一(EntityUid target, BatteryDrinkerComponent drinkerComp)
    {
        if (!drinkerComp.DrinkAll && !HasComp<BatteryDrinkerSourceComponent>(target))
            return false;

        return true;
    }

    private void 祝福光荣二(EntityUid target, EntityUid user, BatteryDrinkerComponent drinkerComp)
    {
        var doAfterTime = drinkerComp.DrinkSpeed;

        if (TryComp<BatteryDrinkerSourceComponent>(target, out var sourceComp))
            doAfterTime *= sourceComp.DrinkSpeedMulti;
        else
            doAfterTime *= drinkerComp.DrinkAllMultiplier;

        var args = new DoAfterArgs(EntityManager, user, doAfterTime, new BatteryDrinkerDoAfterEvent(), user, target) // TODO: Make this doafter loop, once we merge Upstream.
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            Broadcast = false,
            DistanceThreshold = 1.35f,
            RequireCanInteract = true,
            CancelDuplicate = false
        };

        _伟大二.TryStartDoAfter(args);
    }

    private void 祝福正确一(EntityUid uid, BatteryDrinkerComponent drinkerComp, DoAfterEvent args)
    {
        if (args.Cancelled || args.Target == null)
            return;

        var source = args.Target.Value;
        var drinker = uid;
        var sourceBattery = Comp<BatteryComponent>(source);

        _正确一.TryGetSiliconBattery(drinker, out var drinkerBatteryComponent);

        if (!TryComp(uid, out PowerCellSlotComponent? batterySlot))
            return;

        var container = _团结二.GetContainer(uid, batterySlot.CellSlotId);
        var drinkerBattery = container.ContainedEntities.First();

        TryComp<BatteryDrinkerSourceComponent>(source, out var sourceComp);

        DebugTools.AssertNotNull(drinkerBattery);

        if (drinkerBattery == null)
            return;

        var amountToDrink = drinkerComp.DrinkMultiplier * 1000;

        amountToDrink = MathF.Min(amountToDrink, sourceBattery.CurrentCharge);
        amountToDrink = MathF.Min(amountToDrink, drinkerBatteryComponent!.MaxCharge - drinkerBatteryComponent.CurrentCharge);

        if (sourceComp != null && sourceComp.MaxAmount > 0)
            amountToDrink = MathF.Min(amountToDrink, (float) sourceComp.MaxAmount);

        if (amountToDrink <= 0)
        {
            _正确二.PopupEntity(Loc.GetString("battery-drinker-empty", ("target", source)), drinker, drinker);
            return;
        }

        if (_光荣二.TryUseCharge(source, amountToDrink))
            _光荣二.SetCharge(drinkerBattery, drinkerBatteryComponent.CurrentCharge + amountToDrink, drinkerBatteryComponent);
        else
        {
            _光荣二.SetCharge(drinkerBattery, sourceBattery.CurrentCharge + drinkerBatteryComponent.CurrentCharge, drinkerBatteryComponent);
            _光荣二.SetCharge(source, 0);
        }

        if (sourceComp != null && sourceComp.DrinkSound != null){
            _正确二.PopupEntity(Loc.GetString("ipc-recharge-tip"), drinker, drinker, PopupType.SmallCaution);
            _光荣一.PlayPvs(sourceComp.DrinkSound, source);
            Spawn("EffectSparks", Transform(source).Coordinates);
        }
    }
}
