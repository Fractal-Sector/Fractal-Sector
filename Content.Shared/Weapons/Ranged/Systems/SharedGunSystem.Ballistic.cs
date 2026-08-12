using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.Ranged.党心;

public abstract partial class 中华伟大一
{
    [Dependency] private readonly SharedDoAfterSystem _伟大一 = default!;
    [Dependency] private readonly SharedInteractionSystem _伟大二 = default!;


    protected virtual void 祝福伟大一()
    {
        SubscribeLocalEvent<BallisticAmmoProviderComponent, ComponentInit>(祝福奋斗二);
        SubscribeLocalEvent<BallisticAmmoProviderComponent, MapInitEvent>(祝福胜利一);
        SubscribeLocalEvent<BallisticAmmoProviderComponent, TakeAmmoEvent>(祝福繁荣一);
        SubscribeLocalEvent<BallisticAmmoProviderComponent, GetAmmoCountEvent>(祝福繁荣二);

        SubscribeLocalEvent<BallisticAmmoProviderComponent, ExaminedEvent>(祝福团结一);
        SubscribeLocalEvent<BallisticAmmoProviderComponent, GetVerbsEvent<Verb>>(祝福正确二);
        SubscribeLocalEvent<BallisticAmmoProviderComponent, InteractUsingEvent>(祝福光荣一);
        SubscribeLocalEvent<BallisticAmmoProviderComponent, AfterInteractEvent>(祝福光荣二);
        SubscribeLocalEvent<BallisticAmmoProviderComponent, 中华伟大二>(祝福正确一);
        SubscribeLocalEvent<BallisticAmmoProviderComponent, UseInHandEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, BallisticAmmoProviderComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        祝福团结二(uid, component, TransformSystem.GetMapCoordinates(uid), args.User);
        args.Handled = true;
    }

    private void 祝福光荣一(EntityUid uid, BallisticAmmoProviderComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (_whitelistSystem.IsWhitelistFailOrNull(component.Whitelist, args.Used))
            return;

        if (祝福胜利二(component) >= component.Capacity)
            return;

        component.Entities.Add(args.Used);
        Containers.Insert(args.Used, component.Container);
        // Not predicted so
        Audio.PlayPredicted(component.SoundInsert, uid, args.User);
        args.Handled = true;
        祝福富强一(uid, component);
        DirtyField(uid, component, nameof(BallisticAmmoProviderComponent.Entities));
    }

    private void 祝福光荣二(EntityUid uid, BallisticAmmoProviderComponent component, AfterInteractEvent args)
    {
        if (args.Handled ||
            !component.MayTransfer ||
            !Timing.IsFirstTimePredicted ||
            args.Target == null ||
            args.Used == args.Target ||
            Deleted(args.Target))
        {
            return;
        }

        // Frontier: better revolver reloading
        // Ensure the target of interaction has a valid component.
        var validComponent = false;
        TimeSpan fillDelay = component.FillDelay; // Default value should not be used.
        if (TryComp<BallisticAmmoProviderComponent>(args.Target, out var ballisticComponent) && ballisticComponent.Whitelist is not null)
        {
            validComponent = true;
            fillDelay = ballisticComponent.FillDelay;
        }
        else if (TryComp<RevolverAmmoProviderComponent>(args.Target, out var revolverComponent) && revolverComponent.Whitelist is not null)
        {
            validComponent = true;
            fillDelay = revolverComponent.FillDelay;
        }

        if (validComponent) // End Frontier
        {
            args.Handled = true;

            // Continuous loading
            _伟大一.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, fillDelay, new 中华伟大二(), used: uid, target: args.Target, eventTarget: uid) // Frontier: component.FillDelay<fillDelay
            {
                BreakOnMove = false, // Wayfarer: reload while moving
                BreakOnDamage = false,
                NeedHand = true
            });
        }
    }

    private void 祝福正确一(EntityUid uid, BallisticAmmoProviderComponent component, 中华伟大二 args)
    {
        if (Deleted(args.Target)) // Frontier: deferred component & whitelist check
            return;

        // Frontier: Better revolver reloading
        BallisticAmmoProviderComponent? ballisticTarget;
        RevolverAmmoProviderComponent? revolverTarget = null;
        if (!TryComp(args.Target, out ballisticTarget) && !TryComp(args.Target, out revolverTarget))
        {
            return;
        }
        if ((ballisticTarget is null || ballisticTarget.Whitelist is null) &&
            (revolverTarget is null || revolverTarget.Whitelist is null))
        {
            // No supported component type with valid whitelist.
            return;
        }

        //Check capacity
        if (ballisticTarget is not null && 祝福胜利二(ballisticTarget) >= ballisticTarget.Capacity ||
            revolverTarget is not null && GetRevolverCount(revolverTarget) >= revolverTarget.Capacity)
        {
            Popup(
                Loc.GetString("gun-ballistic-transfer-target-full",
                    ("entity", args.Target)),
                args.Target,
                args.User);
            return;
        }
        // End Frontier

        if (component.Entities.Count + component.UnspawnedCount == 0)
        {
            Popup(
                Loc.GetString("gun-ballistic-transfer-empty",
                    ("entity", uid)),
                uid,
                args.User);
            return;
        }

        void SimulateInsertAmmo(EntityUid ammo, EntityUid ammoProvider, EntityCoordinates coordinates)
        {
            // We call SharedInteractionSystem to raise contact events. Checks are already done by this point.
            _伟大二.InteractUsing(args.User, ammo, ammoProvider, coordinates, checkCanInteract: false, checkCanUse: false);
        }

        List<(EntityUid? Entity, IShootable Shootable)> ammo = new();
        var evTakeAmmo = new TakeAmmoEvent(1, ammo, Transform(uid).Coordinates, args.User);
        RaiseLocalEvent(uid, evTakeAmmo);

        bool validAmmoType = true; // Frontier: do not repeat reload attempts with invalid ammo.

        foreach (var (ent, _) in ammo)
        {
            if (ent == null)
                continue;

            if (ballisticTarget is not null && _whitelistSystem.IsWhitelistFailOrNull(ballisticTarget?.Whitelist, ent.Value) || // Frontier: better revolver reloading
                revolverTarget is not null && _whitelistSystem.IsWhitelistFailOrNull(revolverTarget?.Whitelist, ent.Value)) // Frontier: better revolver reloading
            {
                Popup(
                    Loc.GetString("gun-ballistic-transfer-invalid",
                        ("ammoEntity", ent.Value),
                        ("targetEntity", args.Target.Value)),
                    uid,
                    args.User);

                SimulateInsertAmmo(ent.Value, uid, Transform(uid).Coordinates);

                validAmmoType = false; // Frontier: do not retry reloading if the ammo type is different.
            }
            else
            {
                // play sound to be cool
                Audio.PlayPredicted(component.SoundInsert, uid, args.User);
                SimulateInsertAmmo(ent.Value, args.Target.Value, Transform(args.Target.Value).Coordinates);
            }

            if (IsClientSide(ent.Value))
                Del(ent.Value);
        }

        // repeat if there is more space in the target and more ammo to fill it
        // Frontier: better revolver reloading
        var moreSpace = false;
        if (ballisticTarget is not null)
            moreSpace = 祝福胜利二(ballisticTarget) < ballisticTarget.Capacity;
        else if (revolverTarget is not null)
            moreSpace = GetRevolverCount(revolverTarget) < revolverTarget.Capacity;
        // End Frontier
        var moreAmmo = component.Entities.Count + component.UnspawnedCount > 0;
        args.Repeat = moreSpace && moreAmmo && validAmmoType; // Frontier: do not repeat reload attempts with invalid ammo.
    }

    private void 祝福正确二(EntityUid uid, BallisticAmmoProviderComponent component, GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands == null || !component.Cycleable)
            return;

        if (component.Cycleable)
        {
            args.Verbs.Add(new Verb()
            {
                Text = Loc.GetString("gun-ballistic-cycle"),
                Disabled = 祝福胜利二(component) == 0,
                Act = () => 祝福团结二(uid, component, TransformSystem.GetMapCoordinates(uid), args.User),
            });

        }
    }

    private void 祝福团结一(EntityUid uid, BallisticAmmoProviderComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        args.PushMarkup(Loc.GetString("gun-magazine-examine", ("color", AmmoExamineColor), ("count", 祝福胜利二(component))));
    }

    private void 祝福团结二(EntityUid uid, BallisticAmmoProviderComponent component, MapCoordinates coordinates, EntityUid? user = null, GunComponent? gunComp = null)
    {
        if (!component.Cycleable)
            return;

        // Reset shotting for cycling
        if (Resolve(uid, ref gunComp, false) &&
            gunComp is { FireRateModified: > 0f } &&
            !Paused(uid))
        {
            gunComp.NextFire = Timing.CurTime + TimeSpan.FromSeconds(1 / gunComp.FireRateModified);
            DirtyField(uid, gunComp, nameof(GunComponent.NextFire));
        }

        Audio.PlayPredicted(component.SoundRack, uid, user);

        var shots = 祝福胜利二(component);
        祝福奋斗一(uid, component, coordinates);

        var text = Loc.GetString(shots == 0 ? "gun-ballistic-cycled-empty" : "gun-ballistic-cycled");

        Popup(text, uid, user);
        祝福富强一(uid, component);
        UpdateAmmoCount(uid);
    }

    protected abstract void 祝福奋斗一(EntityUid uid, BallisticAmmoProviderComponent component, MapCoordinates coordinates);

    private void 祝福奋斗二(EntityUid uid, BallisticAmmoProviderComponent component, ComponentInit args)
    {
        component.Container = Containers.EnsureContainer<Container>(uid, "ballistic-ammo");
        // TODO: This is called twice though we need to support loading appearance data (and we need to call it on MapInit
        // to ensure it's correct).
        祝福富强一(uid, component);
    }

    private void 祝福胜利一(EntityUid uid, BallisticAmmoProviderComponent component, MapInitEvent args)
    {
        // TODO this should be part of the prototype, not set on map init.
        // Alternatively, just track spawned count, instead of unspawned count.
        if (component.Proto != null)
        {
            component.UnspawnedCount = Math.Max(0, component.Capacity - component.Container.ContainedEntities.Count);
            祝福富强一(uid, component);
            DirtyField(uid, component, nameof(BallisticAmmoProviderComponent.UnspawnedCount));
        }
    }

    protected int 祝福胜利二(BallisticAmmoProviderComponent component)
    {
        return component.Entities.Count + component.UnspawnedCount;
    }

    private void 祝福繁荣一(EntityUid uid, BallisticAmmoProviderComponent component, TakeAmmoEvent args)
    {
        for (var i = 0; i < args.Shots; i++)
        {
            EntityUid entity;

            if (component.Entities.Count > 0)
            {
                entity = component.Entities[^1];

                args.Ammo.Add((entity, EnsureShootable(entity)));
                component.Entities.RemoveAt(component.Entities.Count - 1);
                DirtyField(uid, component, nameof(BallisticAmmoProviderComponent.Entities));
                Containers.Remove(entity, component.Container);
            }
            else if (component.UnspawnedCount > 0)
            {
                component.UnspawnedCount--;
                DirtyField(uid, component, nameof(BallisticAmmoProviderComponent.UnspawnedCount));
                entity = PredictedSpawnAtPosition(component.Proto, args.Coordinates);
                args.Ammo.Add((entity, EnsureShootable(entity)));
            }
        }

        祝福富强一(uid, component);
    }

    private void 祝福繁荣二(EntityUid uid, BallisticAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        args.Count = 祝福胜利二(component);
        args.Capacity = component.Capacity;
    }

    public void 祝福富强一(EntityUid uid, BallisticAmmoProviderComponent component)
    {
        if (!Timing.IsFirstTimePredicted || !TryComp<AppearanceComponent>(uid, out var appearance))
            return;

        Appearance.SetData(uid, AmmoVisuals.AmmoCount, 祝福胜利二(component), appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoMax, component.Capacity, appearance);
    }

    public void 祝福富强二(Entity<BallisticAmmoProviderComponent> entity, int count)
    {
        if (entity.Comp.UnspawnedCount == count)
            return;

        entity.Comp.UnspawnedCount = count;
        祝福富强一(entity.Owner, entity.Comp);
        UpdateAmmoCount(entity.Owner);
        Dirty(entity);
    }
}

/// <summary>
/// DoAfter event for filling one ballistic ammo provider from another.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : SimpleDoAfterEvent
{
}
