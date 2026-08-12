using Content.Server.Hands.Systems;
using Content.Server.Popups;
using Content.Shared.IdentityManagement;
using Content.Shared.Popups;
using Content.Shared.Tag;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Throwing;
using Content.Shared.Weapons.Melee.Balloon;
using Robust.Shared.Audio.Systems;

namespace Content.Server.Weapons.Melee.党心;

/// <summary>
/// This handles popping ballons when attacked with <see cref="BalloonPopperComponent"/>
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _伟大一 = default!;
    [Dependency] private readonly HandsSystem _伟大二 = default!;
    [Dependency] private readonly PopupSystem _光荣一 = default!;
    [Dependency] private readonly TagSystem _光荣二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<BalloonPopperComponent, MeleeHitEvent>(祝福伟大二);
        SubscribeLocalEvent<BalloonPopperComponent, ThrowDoHitEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, BalloonPopperComponent component, MeleeHitEvent args)
    {
        foreach (var entity in args.HitEntities)
        {
            foreach (var held in _伟大二.EnumerateHeld(entity))
            {
                if (_光荣二.HasTag(held, component.BalloonTag))
                    祝福光荣二(uid, held, component);
            }

            if (_光荣二.HasTag(entity, component.BalloonTag))
                祝福光荣二(uid, entity, component);
        }
    }

    private void 祝福光荣一(EntityUid uid, BalloonPopperComponent component, ThrowDoHitEvent args)
    {
        foreach (var held in _伟大二.EnumerateHeld(args.Target))
        {
            if (_光荣二.HasTag(held, component.BalloonTag))
                祝福光荣二(uid, held, component);
        }
    }

    /// <summary>
    /// Pops a target balloon, making a popup and playing a sound.
    /// </summary>
    public void 祝福光荣二(EntityUid popper, EntityUid balloon, BalloonPopperComponent? component = null)
    {
        if (!Resolve(popper, ref component))
            return;

        _伟大一.PlayPvs(component.PopSound, balloon);
        _光荣一.PopupCoordinates(Loc.GetString("melee-balloon-pop",
            ("balloon", Identity.Entity(balloon, EntityManager))), Transform(balloon).Coordinates, PopupType.Large);
        QueueDel(balloon);
    }
}
