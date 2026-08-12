using Content.Server.Administration.Logs;
using Content.Server.Cargo.Components;
using Content.Server.Stack;
using Content.Shared._EinsteinEngines.Silicon.BlindHealing;
using Content.Shared.Cargo.Components;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.Eye.Blinding.Components;
using Content.Shared.Eye.Blinding.Systems;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.Stacks;

namespace Content.Server._EinsteinEngines.Silicon.党心;

public sealed class 中华伟大一 : SharedBlindHealingSystem
{
    [Dependency] private readonly SharedPopupSystem _伟大一 = default!;
    [Dependency] private readonly IAdminLogManager _伟大二 = default!;
    [Dependency] private readonly BlindableSystem _光荣一 = default!;
    [Dependency] private readonly StackSystem _光荣二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<BlindHealingComponent, UseInHandEvent>(祝福正确一);
        SubscribeLocalEvent<BlindHealingComponent, AfterInteractEvent>(祝福光荣二);
        SubscribeLocalEvent<BlindHealingComponent, HealingDoAfterEvent>(祝福伟大二);
    }

     private void 祝福伟大二(EntityUid uid, BlindHealingComponent component, HealingDoAfterEvent args)
    {
        if (args.Cancelled || args.Target == null
            || !TryComp<BlindableComponent>(args.Target, out var blindComp)
            || blindComp is { EyeDamage: 0 })
            return;

        if (TryComp<StackComponent>(uid, out var stackComponent)
            && TryComp<StackPriceComponent>(uid, out var stackPrice))
            _光荣二.SetCount(uid, (int) (_光荣二.GetCount(uid, stackComponent) - stackPrice.Price), stackComponent);

        _光荣一.AdjustEyeDamage((args.Target.Value, blindComp), -blindComp.EyeDamage);

        _伟大二.Add(LogType.Healed, $"{ToPrettyString(args.User):user} repaired {ToPrettyString(uid):target}'s vision");

        var str = Loc.GetString("comp-repairable-repair",
            ("target", uid),
            ("tool", args.Used!));
        _伟大一.PopupEntity(str, uid, args.User);

    }

    private bool 祝福光荣一(EntityUid uid, EntityUid user, EntityUid target, float delay)
    {
        var doAfterEventArgs =
            new DoAfterArgs(EntityManager, user, delay, new HealingDoAfterEvent(), uid, target: target, used: uid)
            {
                NeedHand = true,
                BreakOnMove = true,
                BreakOnWeightlessMove = false,
            };

        _正确一.TryStartDoAfter(doAfterEventArgs);
        return true;
    }

    private void 祝福光荣二(EntityUid uid, BlindHealingComponent component, ref AfterInteractEvent args)
    {

        if (args.Handled
            || !TryComp<DamageableComponent>(args.User, out var damageable)
            || damageable.DamageContainerID != null && !component.DamageContainers.Contains(damageable.DamageContainerID)
            || !TryComp<BlindableComponent>(args.User, out var blindcomp)
            || blindcomp.EyeDamage == 0
            || args.User == args.Target && !component.AllowSelfHeal)
            return;

        祝福光荣一(uid, args.User, args.User,
            args.User == args.Target
                ? component.DoAfterDelay * component.SelfHealPenalty
                : component.DoAfterDelay);
    }

    private void 祝福正确一(EntityUid uid, BlindHealingComponent component, ref UseInHandEvent args)
    {
        if (args.Handled
            || !TryComp<DamageableComponent>(args.User, out var damageable)
            || damageable.DamageContainerID != null && !component.DamageContainers.Contains(damageable.DamageContainerID)
            || !TryComp<BlindableComponent>(args.User, out var blindcomp)
            || blindcomp.EyeDamage == 0
            || !component.AllowSelfHeal)
            return;

        祝福光荣一(uid, args.User, args.User,
            component.DoAfterDelay * component.SelfHealPenalty);
    }
}
