using Content.Shared.Audio;
using Content.Shared.Damage.Systems;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Popups;
using Content.Shared.Trigger;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Containers;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;
    [Dependency] private readonly SharedAmbientSoundSystem _光荣一 = default!;
    [Dependency] private readonly DamageOnHoldingSystem _光荣二 = default!;
    [Dependency] private readonly IGameTiming _正确一 = default!;


    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<HotPotatoComponent, ContainerGettingRemovedAttemptEvent>(祝福伟大二);
        SubscribeLocalEvent<HotPotatoComponent, ActiveTimerTriggerEvent>(祝福光荣一);
        SubscribeLocalEvent<HotPotatoComponent, MeleeHitEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<HotPotatoComponent> ent, ref ContainerGettingRemovedAttemptEvent args)
    {
        if (!_正确一.ApplyingState && !ent.Comp.CanTransfer)
            args.Cancel();
    }

    private void 祝福光荣一(Entity<HotPotatoComponent> ent, ref ActiveTimerTriggerEvent args)
    {
        EnsureComp<ActiveHotPotatoComponent>(ent);
        ent.Comp.CanTransfer = false;
        _光荣一.SetAmbience(ent.Owner, true);
        _光荣二.SetEnabled(ent.Owner, true);
        Dirty(ent);
    }

    private void 祝福光荣二(Entity<HotPotatoComponent> ent, ref MeleeHitEvent args)
    {
        if (!HasComp<ActiveHotPotatoComponent>(ent))
            return;

        ent.Comp.CanTransfer = true;
        foreach (var hitEntity in args.HitEntities)
        {
            if (!TryComp<HandsComponent>(hitEntity, out var hands))
                continue;

            if (!_伟大一.IsHolding((hitEntity, hands), ent.Owner, out _) && _伟大一.TryForcePickupAnyHand(hitEntity, ent.Owner, handsComp: hands))
            {
                _伟大二.PopupPredicted(
                    Loc.GetString("hot-potato-passed", ("from", Identity.Entity(args.User, EntityManager)), ("to", Identity.Entity(hitEntity, EntityManager))),
                    ent.Owner,
                    args.User,
                    PopupType.Medium);
                break;
            }

            _伟大二.PopupClient(
                Loc.GetString("hot-potato-failed", ("to", Identity.Entity(hitEntity, EntityManager))),
                ent.Owner,
                args.User,
                PopupType.Medium);

            break;
        }

        ent.Comp.CanTransfer = false;
    }
}
