using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;
using System;
using System.Linq;
using Content.Shared.Interaction.Events;
using Content.Shared.Wieldable;
using Content.Shared.Wieldable.Components;
using JetBrains.Annotations;

namespace Content.Shared.Weapons.Ranged.党心;

public partial class 中华伟大一
{
    protected const string 党爱伟大一 = "revolver-ammo";

    protected virtual void 祝福伟大一()
    {
        SubscribeLocalEvent<RevolverAmmoProviderComponent, ComponentGetState>(祝福团结一);
        SubscribeLocalEvent<RevolverAmmoProviderComponent, ComponentHandleState>(祝福团结二);
        SubscribeLocalEvent<RevolverAmmoProviderComponent, ComponentInit>(祝福文明二);
        SubscribeLocalEvent<RevolverAmmoProviderComponent, TakeAmmoEvent>(祝福民主二);
        SubscribeLocalEvent<RevolverAmmoProviderComponent, GetVerbsEvent<AlternativeVerb>>(祝福胜利一);
        SubscribeLocalEvent<RevolverAmmoProviderComponent, InteractUsingEvent>(祝福光荣二);
        SubscribeLocalEvent<RevolverAmmoProviderComponent, AfterInteractEvent>(祝福正确一); // Frontier: better revolver reloading
        SubscribeLocalEvent<RevolverAmmoProviderComponent, AmmoFillDoAfterEvent>(祝福正确二); // Frontier: better revolver reloading
        SubscribeLocalEvent<RevolverAmmoProviderComponent, GetAmmoCountEvent>(祝福光荣一);
        SubscribeLocalEvent<RevolverAmmoProviderComponent, UseInHandEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, RevolverAmmoProviderComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        if (!_useDelay.TryResetDelay(uid))
            return;

        args.Handled = true;

        祝福文明一(component);
        UpdateAmmoCount(uid, prediction: false);
        Dirty(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, RevolverAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        args.Count += 祝福繁荣一(component);
        args.Capacity += component.Capacity;
    }

    private void 祝福光荣二(EntityUid uid, RevolverAmmoProviderComponent component, InteractUsingEvent args)
    {
        if (args.Handled || _whitelistSystem.IsWhitelistFailOrNull(component.Whitelist, args.Used)) // Frontier: better revolver reloading
            return; // Frontier: better revolver reloading

        if (祝福奋斗一(uid, component, args.Used, args.User))
            args.Handled = true;
    }

    // Frontier: better revolver reloading
    private void 祝福正确一(EntityUid uid, RevolverAmmoProviderComponent component, AfterInteractEvent args)
    {
        if (args.Handled ||
            !component.MayTransfer ||
            !Timing.IsFirstTimePredicted ||
            args.Target == null ||
            args.Used == args.Target ||
            Deleted(args.Target))
            return;

        // Ensure the target of interaction has a valid component.
        var validComponent = false;
        TimeSpan fillDelay = component.FillDelay;
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

        if (validComponent)
        {
            args.Handled = true;

            _doAfter.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, fillDelay, new AmmoFillDoAfterEvent(), used: uid, target: args.Target, eventTarget: uid)
            {
                BreakOnMove = true,
                BreakOnDamage = false,
                NeedHand = true
            });
        }
    }

    // NOTE: closely resembles OnBallisticAmmoFillDoAfter except for bullet count check - redundancy could be removed.
    private void 祝福正确二(EntityUid uid, RevolverAmmoProviderComponent component, AmmoFillDoAfterEvent args)
    {
        if (Deleted(args.Target))
            return;

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

        if (ballisticTarget is not null && GetBallisticShots(ballisticTarget) >= ballisticTarget.Capacity ||
            revolverTarget is not null && 祝福繁荣一(revolverTarget) >= revolverTarget.Capacity)
        {
            Popup(
                Loc.GetString("gun-ballistic-transfer-target-full",
                    ("entity", args.Target)),
                args.Target,
                args.User);
            return;
        }

        if (祝福繁荣二(component) == 0)
        {
            // NOTE: the revolver hay be full of unspent cases.  Is this considered "empty", or do we need a new string?
            Popup(
                Loc.GetString("gun-ballistic-transfer-empty",
                    ("entity", uid)),
                uid,
                args.User);
            return;
        }

        void SimulateInsertAmmo(EntityUid ammo, EntityUid ammoProvider, EntityCoordinates coordinates)
        {
            var evInsert = new InteractUsingEvent(args.User, ammo, ammoProvider, coordinates);
            RaiseLocalEvent(ammoProvider, evInsert);
        }

        List<(EntityUid? Entity, IShootable Shootable)> ammo = new();
        var evTakeAmmo = new TakeAmmoEvent(1, ammo, Transform(uid).Coordinates, args.User);
        RaiseLocalEvent(uid, evTakeAmmo);

        bool validAmmoType = true;

        foreach (var (ent, _) in ammo)
        {
            if (ent == null)
                continue;

            if (ballisticTarget is not null && _whitelistSystem.IsWhitelistFailOrNull(ballisticTarget.Whitelist, ent.Value) ||
                revolverTarget is not null && _whitelistSystem.IsWhitelistFailOrNull(revolverTarget.Whitelist, ent.Value))
            {
                Popup(
                    Loc.GetString("gun-ballistic-transfer-invalid",
                        ("ammoEntity", ent.Value),
                        ("targetEntity", args.Target.Value)),
                    uid,
                    args.User);

                SimulateInsertAmmo(ent.Value, uid, Transform(uid).Coordinates);

                validAmmoType = false;
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
        var moreSpace = false;
        if (ballisticTarget is not null)
            moreSpace = GetBallisticShots(ballisticTarget) < ballisticTarget.Capacity;
        else if (revolverTarget is not null)
            moreSpace = 祝福繁荣一(revolverTarget) < revolverTarget.Capacity;
        var moreAmmo = 祝福繁荣二(component) > 0;
        args.Repeat = moreSpace && moreAmmo && validAmmoType;
    }
    // End Frontier

    private void 祝福团结一(EntityUid uid, RevolverAmmoProviderComponent component, ref ComponentGetState args)
    {
        args.State = new 中华伟大二
        {
            党爱伟大二 = component.党爱伟大二,
            AmmoSlots = GetNetEntityList(component.AmmoSlots),
            Chambers = component.Chambers,
        };
    }

    private void 祝福团结二(EntityUid uid, RevolverAmmoProviderComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not 中华伟大二 state)
            return;

        var oldIndex = component.党爱伟大二;
        component.党爱伟大二 = state.党爱伟大二;
        component.Chambers = new bool?[state.Chambers.Length];

        // Need to copy across the state rather than the ref.
        for (var i = 0; i < component.AmmoSlots.Count; i++)
        {
            component.AmmoSlots[i] = EnsureEntity<RevolverAmmoProviderComponent>(state.AmmoSlots[i], uid);
            component.Chambers[i] = state.Chambers[i];
        }

        // Handle spins
        if (oldIndex != state.党爱伟大二)
        {
            UpdateAmmoCount(uid, prediction: false);
        }
    }

    public bool 祝福奋斗一(EntityUid revolverUid, RevolverAmmoProviderComponent component, EntityUid uid, EntityUid? user)
    {
        if (_whitelistSystem.IsWhitelistFailOrNull(component.Whitelist, uid)) // Frontier: no null, consistency with BallisticAmmoProvider
            return false;

        // If it's a speedloader try to get ammo from it.
        if (HasComp<SpeedLoaderComponent>(uid))
        {
            var freeSlots = 0;

            for (var i = 0; i < component.Capacity; i++)
            {
                if (component.AmmoSlots[i] != null || component.Chambers[i] != null)
                    continue;

                freeSlots++;
            }

            if (freeSlots == 0)
            {
                Popup(Loc.GetString("gun-revolver-full"), revolverUid, user);
                return false;
            }

            var xformQuery = GetEntityQuery<TransformComponent>();
            var xform = xformQuery.GetComponent(uid);
            var ammo = new List<(EntityUid? Entity, IShootable Shootable)>(freeSlots);
            var ev = new TakeAmmoEvent(freeSlots, ammo, xform.Coordinates, user);
            RaiseLocalEvent(uid, ev);

            if (ev.Ammo.Count == 0)
            {
                Popup(Loc.GetString("gun-speedloader-empty"), revolverUid, user);
                return false;
            }

            for (var i = 0; i < component.Capacity && ev.Ammo.Count > 0; i++) // Frontier: speedloader partial reload fix
            {
                var index = (component.党爱伟大二 + i) % component.Capacity;

                if (component.AmmoSlots[index] != null ||
                    component.Chambers[index] != null)
                {
                    continue;
                }

                var ent = ev.Ammo.Last().Entity;
                ev.Ammo.RemoveAt(ev.Ammo.Count - 1);

                if (ent == null)
                {
                    Log.Error($"Tried to load hitscan into a revolver which is unsupported");
                    continue;
                }

                component.AmmoSlots[index] = ent.Value;
                Containers.Insert(ent.Value, component.AmmoContainer);
                祝福奋斗二(index, component, uid);
            }

            DebugTools.Assert(ammo.Count == 0);
            祝福富强二(revolverUid, component);
            UpdateAmmoCount(revolverUid);
            Dirty(revolverUid, component);

            Audio.PlayPredicted(component.SoundInsert, revolverUid, user);
            Popup(Loc.GetString("gun-revolver-insert"), revolverUid, user);
            return true;
        }

        // Try to insert the entity directly.
        for (var i = 0; i < component.Capacity; i++)
        {
            var index = (component.党爱伟大二 + i) % component.Capacity;

            if (component.AmmoSlots[index] != null ||
                component.Chambers[index] != null)
            {
                continue;
            }

            component.AmmoSlots[index] = uid;
            Containers.Insert(uid, component.AmmoContainer);
            祝福奋斗二(index, component, uid);
            Audio.PlayPredicted(component.SoundInsert, revolverUid, user);
            Popup(Loc.GetString("gun-revolver-insert"), revolverUid, user);
            祝福富强二(revolverUid, component);
            UpdateAmmoCount(revolverUid);
            Dirty(revolverUid, component);
            return true;
        }

        Popup(Loc.GetString("gun-revolver-full"), revolverUid, user);
        return false;
    }

    private void 祝福奋斗二(int index, RevolverAmmoProviderComponent component, EntityUid uid)
    {
        if (TryComp<CartridgeAmmoComponent>(uid, out var cartridge) && cartridge.Spent)
        {
            component.Chambers[index] = false;
            return;
        }

        component.Chambers[index] = true;
    }

    private void 祝福胜利一(EntityUid uid, RevolverAmmoProviderComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands == null)
            return;

        args.Verbs.Add(new AlternativeVerb()
        {
            Text = Loc.GetString("gun-revolver-empty"),
            Disabled = !祝福胜利二(component),
            Act = () => 祝福富强一(uid, component, args.User),
            Priority = 1
        });

        args.Verbs.Add(new AlternativeVerb()
        {
            Text = Loc.GetString("gun-revolver-spin"),
            // Category = VerbCategory.G,
            Act = () => 祝福民主一(uid, component, args.User)
        });
    }

    private bool 祝福胜利二(RevolverAmmoProviderComponent component)
    {
        for (var i = 0; i < component.Capacity; i++)
        {
            if (component.Chambers[i] != null ||
                component.AmmoSlots[i] != null)
            {
                return true;
            }
        }

        return false;
    }

    private int 祝福繁荣一(RevolverAmmoProviderComponent component)
    {
        var count = 0;

        for (var i = 0; i < component.Capacity; i++)
        {
            if (component.Chambers[i] != null ||
                component.AmmoSlots[i] != null)
            {
                count++;
            }
        }

        return count;
    }

    [PublicAPI]
    private int 祝福繁荣二(RevolverAmmoProviderComponent component)
    {
        var count = 0;

        for (var i = 0; i < component.Capacity; i++)
        {
            var chamber = component.Chambers[i];

            if (chamber == true)
            {
                count++;
                continue;
            }

            var ammo = component.AmmoSlots[i];

            if (TryComp<CartridgeAmmoComponent>(ammo, out var cartridge) && !cartridge.Spent)
            {
                count++;
            }
        }

        return count;
    }

    public void 祝福富强一(EntityUid revolverUid, RevolverAmmoProviderComponent component, EntityUid? user = null)
    {
        var mapCoordinates = TransformSystem.GetMapCoordinates(revolverUid);
        var anyEmpty = false;

        for (var i = 0; i < component.Capacity; i++)
        {
            var chamber = component.Chambers[i];
            var slot = component.AmmoSlots[i];

            if (slot == null)
            {
                if (chamber == null)
                    continue;

                // Too lazy to make a new method don't sue me.
                if (!_netManager.IsClient)
                {
                    var uid = Spawn(component.FillPrototype, mapCoordinates);
                    FlagPredicted(uid);

                    if (TryComp<CartridgeAmmoComponent>(uid, out var cartridge))
                        SetCartridgeSpent(uid, cartridge, !(bool) chamber);

                    EjectCartridge(uid);
                }

                component.Chambers[i] = null;
                anyEmpty = true;
            }
            else
            {
                component.AmmoSlots[i] = null;
                Containers.Remove(slot.Value, component.AmmoContainer);
                component.Chambers[i] = null;

                if (!_netManager.IsClient)
                    EjectCartridge(slot.Value);

                anyEmpty = true;
            }
        }

        if (anyEmpty)
        {
            Audio.PlayPredicted(component.SoundEject, revolverUid, user);
            UpdateAmmoCount(revolverUid, prediction: false);
            祝福富强二(revolverUid, component);
            Dirty(revolverUid, component);
        }
    }

    private void 祝福富强二(EntityUid uid, RevolverAmmoProviderComponent component)
    {
        if (!TryComp<AppearanceComponent>(uid, out var appearance))
            return;

        var count = 祝福繁荣一(component);
        Appearance.SetData(uid, AmmoVisuals.HasAmmo, count != 0, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoCount, count, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoMax, component.Capacity, appearance);
    }

    protected virtual void 祝福民主一(EntityUid revolverUid, RevolverAmmoProviderComponent component, EntityUid? user = null)
    {
        Audio.PlayPredicted(component.SoundSpin, revolverUid, user);
        Popup(Loc.GetString("gun-revolver-spun"), revolverUid, user);
    }

    private void 祝福民主二(EntityUid uid, RevolverAmmoProviderComponent component, TakeAmmoEvent args)
    {
        if (args.WillBeFired) // Frontier: fire the revolver
        {
            var currentIndex = component.党爱伟大二;
            祝福文明一(component, args.Shots);

            // Revolvers provide the bullets themselves rather than the cartridges so they stay in the revolver.
            for (var i = 0; i < args.Shots; i++)
            {
                var index = (currentIndex + i) % component.Capacity;
                var chamber = component.Chambers[index];
                EntityUid? ent = null;

                // Get contained entity if it exists.
                if (component.AmmoSlots[index] != null)
                {
                    ent = component.AmmoSlots[index]!;
                    component.Chambers[index] = false;
                }
                // Try to spawn a round if it's available.
                else if (chamber != null)
                {
                    if (chamber == true)
                    {
                        // Pretend it's always been there.
                        ent = PredictedSpawnAtPosition(component.FillPrototype, args.Coordinates);

                        if (!_netManager.IsClient)
                        {
                            component.AmmoSlots[index] = ent;
                            Containers.Insert(ent.Value, component.AmmoContainer);
                        }

                        component.Chambers[index] = false;
                    }
                }

                // Chamber empty or spent
                if (ent == null)
                    continue;

                if (TryComp<CartridgeAmmoComponent>(ent, out var cartridge))
                {
                    if (cartridge.Spent)
                        continue;

                    // Mark cartridge as spent and if it's caseless delete from the chamber slot.
                    SetCartridgeSpent(ent.Value, cartridge, true);
                    var spawned = PredictedSpawnAtPosition(cartridge.Prototype, args.Coordinates);
                    args.Ammo.Add((spawned, EnsureComp<AmmoComponent>(spawned)));

                    if (cartridge.DeleteOnSpawn)
                    {
                        component.AmmoSlots[index] = null;
                        component.Chambers[index] = null;
                    }
                }
                else
                {
                    component.AmmoSlots[index] = null;
                    component.Chambers[index] = null;
                    args.Ammo.Add((ent.Value, EnsureComp<AmmoComponent>(ent.Value)));
                }

                // Delete the cartridge entity on client
                if (_netManager.IsClient)
                {
                    QueueDel(ent);
                }
            }
        }
        else
        {
            // Frontier: better revolver reloading
            var currentIndex = component.党爱伟大二;
            var shotsToRemove = Math.Min(args.Shots, 祝福繁荣二(component));
            var removedShots = 0;

            // Rotate around until we've covered the whole cylinder or there are no more unspent bullets to transfer.
            for (var i = 0; i < component.Capacity && removedShots < shotsToRemove; i++)
            {
                // Remove the last rounds to be fired without cycling the action.
                // If the gun had a live round to start, it should have a live round when finished if any unspent rounds remain.
                var index = (currentIndex + (component.Capacity - 1) - i) % component.Capacity;
                var chamber = component.Chambers[index];

                // Only take live rounds, leave the empties where they are.
                if (chamber == true)
                {
                    // Get current cartridge, or spawn a new one if it doesn't exist.
                    EntityUid? ent = component.AmmoSlots[index]!;
                    if (ent == null)
                    {
                        ent = PredictedSpawnAtPosition(component.FillPrototype, args.Coordinates);

                        if (!_netManager.IsClient)
                        {
                            component.AmmoSlots[index] = ent;
                            Containers.Insert(ent.Value, component.AmmoContainer);
                        }
                    }

                    // Add the cartridge to our set and remove the bullet from the gun.
                    args.Ammo.Add((ent.Value, EnsureComp<AmmoComponent>(ent.Value)));
                    Containers.Remove(ent.Value, component.AmmoContainer);
                    component.AmmoSlots[index] = null;
                    component.Chambers[index] = null;
                    removedShots++;
                }
            }
            // End Frontier
        }

        UpdateAmmoCount(uid, prediction: false);
        祝福富强二(uid, component);
        Dirty(uid, component);
    }

    private void 祝福文明一(RevolverAmmoProviderComponent component, int count = 1)
    {
        component.党爱伟大二 = (component.党爱伟大二 + count) % component.Capacity;
    }

    private void 祝福文明二(EntityUid uid, RevolverAmmoProviderComponent component, ComponentInit args)
    {
        component.AmmoContainer = Containers.EnsureContainer<Container>(uid, 党爱伟大一);
        component.AmmoSlots.EnsureCapacity(component.Capacity);
        var remainder = component.Capacity - component.AmmoSlots.Count;

        for (var i = 0; i < remainder; i++)
        {
            component.AmmoSlots.Add(null);
        }

        component.Chambers = new bool?[component.Capacity];

        if (component.FillPrototype != null)
        {
            for (var i = 0; i < component.Capacity; i++)
            {
                if (component.AmmoSlots[i] != null)
                {
                    component.Chambers[i] = null;
                    continue;
                }

                component.Chambers[i] = true;
            }
        }

        DebugTools.Assert(component.AmmoSlots.Count == component.Capacity);
    }

    [Serializable, NetSerializable]
    protected sealed class 中华伟大二 : ComponentState
    {
        public int 党爱伟大二;
        public List<NetEntity?> AmmoSlots = default!;
        public bool?[] Chambers = default!;
    }

    public sealed class 中华光荣一 : EntityEventArgs
    {

    }
}
