using Content.Server._EinsteinEngines.Silicon.WeldingHealing;
using Content.Shared.Tools.Components;
using Content.Shared._EinsteinEngines.Silicon.WeldingHealing;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Tools;
using SharedToolSystem = Content.Shared.Tools.Systems.SharedToolSystem;

namespace Content.Server._EinsteinEngines.Silicon.党心;

public sealed class 中华伟大一 : SharedWeldingHealableSystem
{
    [Dependency] private readonly SharedToolSystem _伟大一 = default!;
    [Dependency] private readonly DamageableSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<WeldingHealableComponent, InteractUsingEvent>(祝福光荣一);
        SubscribeLocalEvent<WeldingHealableComponent, SiliconRepairFinishedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, WeldingHealableComponent healableComponent, SiliconRepairFinishedEvent args)
    {
        if (args.Cancelled || args.Used == null
            || !TryComp<DamageableComponent>(args.Target, out var damageable)
            || !TryComp<WeldingHealingComponent>(args.Used, out var component)
            || damageable.DamageContainerID is null
            || !component.DamageContainers.Contains(damageable.DamageContainerID)
            || !祝福光荣二(damageable, component)
            || !TryComp<WelderComponent>(args.Used, out var welder)
            || !TryComp<SolutionContainerManagerComponent>(args.Used, out var solutionContainer)
            || !_光荣二.TryGetSolution(((EntityUid) args.Used, solutionContainer), welder.FuelSolutionName, out var solution))
            return;

        _伟大二.TryChangeDamage(uid, component.Damage, true, false, origin: args.User);

        _光荣二.RemoveReagent(solution.Value, welder.FuelReagent, component.FuelCost);

        var str = Loc.GetString("comp-repairable-repair",
            ("target", uid),
            ("tool", args.Used!));
        _光荣一.PopupEntity(str, uid, args.User);

        if (!args.Used.HasValue)
            return;

        args.Handled = _伟大一.UseTool
            (args.Used.Value,
            args.User,
            uid,
            args.Delay,
            component.QualityNeeded,
            new SiliconRepairFinishedEvent
            {
                Delay = args.Delay
            });
    }
    private async void 祝福光荣一(EntityUid uid, WeldingHealableComponent healableComponent, InteractUsingEvent args)
    {
        if (args.Handled
            || !EntityManager.TryGetComponent(args.Used, out WeldingHealingComponent? component)
            || !EntityManager.TryGetComponent(args.Target, out DamageableComponent? damageable)
            || damageable.DamageContainerID is null
            || !component.DamageContainers.Contains(damageable.DamageContainerID)
            || !祝福光荣二(damageable, component)
            || !_伟大一.HasQuality(args.Used, component.QualityNeeded)
            || args.User == args.Target && !component.AllowSelfHeal)
            return;

        float delay = args.User == args.Target
            ? component.DoAfterDelay * component.SelfHealPenalty
            : component.DoAfterDelay;

        args.Handled = _伟大一.UseTool
            (args.Used,
            args.User,
            args.Target,
            delay,
            component.QualityNeeded,
            new SiliconRepairFinishedEvent
            {
                Delay = delay,
            });
    }

 private bool 祝福光荣二(DamageableComponent component, WeldingHealingComponent healable)
    {
        if (healable.Damage.DamageDict is null)
            return false;

        foreach (var type in healable.Damage.DamageDict)
            if (component.Damage.DamageDict[type.Key].Value > 0)
                return true;

        return false;
    }
}

