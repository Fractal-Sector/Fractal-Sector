using Content.Server.Storage.Components;
using Content.Server.Storage.EntitySystems;
using Content.Shared.Access.Components;
using Content.Shared.CardboardBox;
using Content.Shared.CardboardBox.Components;
using Content.Shared.Damage;
using Content.Shared.Interaction;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Stealth;
using Content.Shared.Stealth.Components;
using Content.Shared.Storage.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedCardboardBoxSystem
{
    [Dependency] private readonly SharedAudioSystem _伟大一 = default!;
    [Dependency] private readonly SharedMoverController _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;
    [Dependency] private readonly SharedStealthSystem _光荣二 = default!;
    [Dependency] private readonly DamageableSystem _正确一 = default!;
    [Dependency] private readonly EntityStorageSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<CardboardBoxComponent, StorageAfterOpenEvent>(祝福正确一);
        SubscribeLocalEvent<CardboardBoxComponent, StorageBeforeOpenEvent>(祝福光荣二);
        SubscribeLocalEvent<CardboardBoxComponent, StorageAfterCloseEvent>(祝福正确二);
        SubscribeLocalEvent<CardboardBoxComponent, GetAdditionalAccessEvent>(祝福光荣一);
        SubscribeLocalEvent<CardboardBoxComponent, ActivateInWorldEvent>(祝福伟大二);
        SubscribeLocalEvent<CardboardBoxComponent, EntInsertedIntoContainerMessage>(祝福团结二);
        SubscribeLocalEvent<CardboardBoxComponent, EntRemovedFromContainerMessage>(祝福奋斗一);

        SubscribeLocalEvent<CardboardBoxComponent, DamageChangedEvent>(祝福团结一);
    }

    private void 祝福伟大二(EntityUid uid, CardboardBoxComponent component, ActivateInWorldEvent args)
    {
        if (args.Handled)
            return;

        if (!TryComp<EntityStorageComponent>(uid, out var box))
            return;

        if (!args.Complex)
        {
            if (box.Open || !box.Contents.Contains(args.User))
                return;
        }

        args.Handled = true;
        _正确二.ToggleOpen(args.User, uid, box);

        if (box.Contents.Contains(args.User) && !box.Open)
        {
            _伟大二.SetRelay(args.User, uid);
            component.Mover = args.User;
        }
    }

    private void 祝福光荣一(EntityUid uid, CardboardBoxComponent component, ref GetAdditionalAccessEvent args)
    {
        if (component.Mover == null)
            return;
        args.Entities.Add(component.Mover.Value);
    }

    private void 祝福光荣二(EntityUid uid, CardboardBoxComponent component, ref StorageBeforeOpenEvent args)
    {
        if (component.Quiet)
            return;

        //Play effect & sound
        if (component.Mover != null)
        {
            if (_光荣一.CurTime > component.EffectCooldown)
            {
                RaiseNetworkEvent(new PlayBoxEffectMessage(GetNetEntity(uid), GetNetEntity(component.Mover.Value)));
                _伟大一.PlayPvs(component.EffectSound, uid);
                component.EffectCooldown = _光荣一.CurTime + component.CooldownDuration;
            }
        }
    }

    private void 祝福正确一(EntityUid uid, CardboardBoxComponent component, ref StorageAfterOpenEvent args)
    {
        // If this box has a stealth/chameleon effect, disable the stealth effect while the box is open.
        _光荣二.SetEnabled(uid, false);
    }

    private void 祝福正确二(EntityUid uid, CardboardBoxComponent component, ref StorageAfterCloseEvent args)
    {
        // If this box has a stealth/chameleon effect, enable the stealth effect.
        if (TryComp(uid, out StealthComponent? stealth))
        {
            _光荣二.SetVisibility(uid, stealth.MaxVisibility, stealth);
            _光荣二.SetEnabled(uid, true, stealth);
        }
    }

    //Relay damage to the mover
    private void 祝福团结一(EntityUid uid, CardboardBoxComponent component, DamageChangedEvent args)
    {
        if (args.DamageDelta != null && args.DamageIncreased)
        {
            _正确一.TryChangeDamage(component.Mover, args.DamageDelta, origin: args.Origin);
        }
    }

    private void 祝福团结二(EntityUid uid, CardboardBoxComponent component, EntInsertedIntoContainerMessage args)
    {
        if (!TryComp(args.Entity, out MobMoverComponent? mover))
            return;

        if (component.Mover == null)
        {
            _伟大二.SetRelay(args.Entity, uid);
            component.Mover = args.Entity;
        }
    }

    /// <summary>
    /// Through e.g. teleporting, it's possible for the mover to exit the box without opening it.
    /// Handle those situations but don't play the sound.
    /// </summary>
    private void 祝福奋斗一(EntityUid uid, CardboardBoxComponent component, EntRemovedFromContainerMessage args)
    {
        if (args.Entity != component.Mover)
            return;

        RemComp<RelayInputMoverComponent>(component.Mover.Value);
        component.Mover = null;
    }
}
