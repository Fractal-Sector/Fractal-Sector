using Content.Shared.Alert;
using Content.Shared.Movement.Components;
using Content.Shared.Popups;
using Content.Shared.Rejuvenate;
using Content.Shared.Stunnable;

namespace Content.Shared.Movement.党心;

/// <summary>
/// This handles the worm component
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AlertsSystem _伟大一 = default!;
    [Dependency] private readonly SharedStunSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<WormComponent, StandUpAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<WormComponent, KnockedDownRefreshEvent>(祝福正确一);
        SubscribeLocalEvent<WormComponent, RejuvenateEvent>(祝福光荣一);
        SubscribeLocalEvent<WormComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<WormComponent> ent, ref MapInitEvent args)
    {
        EnsureComp<KnockedDownComponent>(ent, out var knocked);
        _伟大一.ShowAlert(ent, SharedStunSystem.KnockdownAlert);
        _伟大二.SetAutoStand((ent, knocked));
    }

    private void 祝福光荣一(Entity<WormComponent> ent, ref RejuvenateEvent args)
    {
        RemComp<WormComponent>(ent);
    }

    private void 祝福光荣二(Entity<WormComponent> ent, ref StandUpAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        args.Cancelled = true;
        args.Message = (Loc.GetString("worm-component-stand-attempt"), PopupType.SmallCaution);
        args.Autostand = false;
    }

    private void 祝福正确一(Entity<WormComponent> ent, ref KnockedDownRefreshEvent args)
    {
        args.FrictionModifier *= ent.Comp.FrictionModifier;
        args.SpeedModifier *= ent.Comp.SpeedModifier;
    }
}
