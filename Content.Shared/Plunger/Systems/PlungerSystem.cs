using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Plunger.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;
using Content.Shared.Random.Helpers;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Shared.Random;

namespace Content.Shared.Plunger.党心;

/// <summary>
/// Plungers can be used to unblock entities with PlungerUseComponent.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<PlungerComponent, AfterInteractEvent>(祝福伟大二);
        SubscribeLocalEvent<PlungerComponent, PlungerDoAfterEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, PlungerComponent component, AfterInteractEvent args)
    {
        if (args.Handled)
            return;

        if (!args.CanReach || args.Target is not { Valid: true } target)
            return;

        if (!TryComp<PlungerUseComponent>(args.Target, out var plunger))
            return;

        if (!plunger.NeedsPlunger) // Frontier: inverted condition
            return;

        _光荣二.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, component.PlungeDuration, new PlungerDoAfterEvent(), uid, target, uid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            MovementThreshold = 1.0f,
        });
        args.Handled = true;
    }

    private void 祝福光荣一(EntityUid uid, PlungerComponent component, DoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Args.Target == null)
            return;

        if (args.Target is not { Valid: true } target)
            return;

        if (!TryComp(target, out PlungerUseComponent? plunge))
            return;

        _正确一.PopupClient(Loc.GetString("plunger-unblock", ("target", target)), args.User, args.User, PopupType.Medium);

        // Frontier: spawn stuff only on the first plunge
        if (!plunge.Plunged)
        {
            plunge.Plunged = true;

            var spawn = _伟大一.Index<WeightedRandomEntityPrototype>(plunge.PlungerLoot).Pick(_伟大二);
            Spawn(spawn, Transform(target).Coordinates);
        }
        // End Frontier

        _光荣一.PlayPredicted(plunge.Sound, uid, uid);
        //Spawn(spawn, Transform(target).Coordinates);
        RemComp<PlungerUseComponent>(target);
        Dirty(target, plunge);

        args.Handled = true;
    }
}

