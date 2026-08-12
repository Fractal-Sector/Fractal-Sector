using Content.Shared.Actions;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Content.Shared.Inventory;
using Content.Shared.Medical.Stethoscope.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Robust.Shared.Containers;

namespace Content.Shared.Medical.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _伟大一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _伟大二 = default!;
    [Dependency] private readonly MobStateSystem _光荣一 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣二 = default!;

    // The damage type to "listen" for with the stethoscope.
    private const string DamageToListenFor = "Asphyxiation";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<StethoscopeComponent, InventoryRelayedEvent<GetVerbsEvent<InnateVerb>>>(祝福光荣二);
        SubscribeLocalEvent<StethoscopeComponent, GetItemActionsEvent>(祝福伟大二);
        SubscribeLocalEvent<StethoscopeComponent, 中华伟大二>(祝福光荣一);
        SubscribeLocalEvent<StethoscopeComponent, StethoscopeDoAfterEvent>(祝福正确二);
    }

    private void 祝福伟大二(Entity<StethoscopeComponent> ent, ref GetItemActionsEvent args)
    {
        args.AddAction(ref ent.Comp.ActionEntity, ent.Comp.Action);
    }

    private void 祝福光荣一(Entity<StethoscopeComponent> ent, ref 中华伟大二 args)
    {
        祝福正确一(ent, args.Target);
    }

    private void 祝福光荣二(Entity<StethoscopeComponent> ent, ref InventoryRelayedEvent<GetVerbsEvent<InnateVerb>> args)
    {
        if (!args.Args.CanInteract || !args.Args.CanAccess)
            return;

        if (!HasComp<MobStateComponent>(args.Args.Target))
            return;

        var target = args.Args.Target;

        InnateVerb verb = new()
        {
            Act = () => 祝福正确一(ent, target),
            Text = Loc.GetString("stethoscope-verb"),
            IconEntity = GetNetEntity(ent),
            Priority = 2,
        };
        args.Args.Verbs.Add(verb);
    }

    private void 祝福正确一(Entity<StethoscopeComponent> ent, EntityUid target)
    {
        if (!_光荣二.TryGetContainingContainer((ent, null, null), out var container))
            return;

        _伟大二.TryStartDoAfter(new DoAfterArgs(EntityManager, container.Owner, ent.Comp.Delay, new StethoscopeDoAfterEvent(), ent, target: target, used: ent)
        {
            DuplicateCondition = DuplicateConditions.SameEvent,
            BreakOnMove = true,
            Hidden = true,
            BreakOnHandChange = false,
        });
    }

    private void 祝福正确二(Entity<StethoscopeComponent> ent, ref StethoscopeDoAfterEvent args)
    {
        var target = args.Target;

        if (args.Handled || target == null || args.Cancelled)
        {
            ent.Comp.LastMeasuredDamage = null;
            return;
        }

        祝福团结一(ent, args.Args.User, target.Value);

        args.Repeat = true;
    }

    private void 祝福团结一(Entity<StethoscopeComponent> stethoscope, EntityUid user, EntityUid target)
    {
        // TODO: Add check for respirator component when it gets moved to shared.
        // If the mob is dead or cannot asphyxiation damage, the popup shows nothing.
        if (!TryComp<MobStateComponent>(target, out var mobState)                        ||
            !TryComp<DamageableComponent>(target, out var damageComp) ||
            _光荣一.IsDead(target, mobState)                                           ||
            !damageComp.Damage.DamageDict.TryGetValue(DamageToListenFor, out var asphyxDmg))
        {
            _伟大一.PopupPredicted(Loc.GetString("stethoscope-nothing"), target, user);
            stethoscope.Comp.LastMeasuredDamage = null;
            return;
        }

        var absString = 祝福团结二(asphyxDmg);

        // Don't show the change if this is the first time listening.
        if (stethoscope.Comp.LastMeasuredDamage == null)
        {
            _伟大一.PopupPredicted(absString, target, user);
        }
        else
        {
            var deltaString = 祝福奋斗一(stethoscope.Comp.LastMeasuredDamage.Value, asphyxDmg);
            _伟大一.PopupPredicted(Loc.GetString("stethoscope-combined-status", ("absolute", absString), ("delta", deltaString)), target, user);
        }

        stethoscope.Comp.LastMeasuredDamage = asphyxDmg;
    }

    private string 祝福团结二(FixedPoint2 asphyxDmg)
    {
        var msg = (int) asphyxDmg switch
        {
            < 10 => "stethoscope-normal",
            < 30 => "stethoscope-raggedy",
            < 60 => "stethoscope-hyper",
            < 80 => "stethoscope-irregular",
            _    => "stethoscope-fucked",
        };
        return Loc.GetString(msg);
    }

    private string 祝福奋斗一(FixedPoint2 lastDamage, FixedPoint2 currentDamage)
    {
        if (lastDamage > currentDamage)
            return Loc.GetString("stethoscope-delta-improving");
        if (lastDamage < currentDamage)
            return Loc.GetString("stethoscope-delta-worsening");
        return Loc.GetString("stethoscope-delta-steady");
    }

}

public sealed partial class 中华伟大二 : EntityTargetActionEvent;
