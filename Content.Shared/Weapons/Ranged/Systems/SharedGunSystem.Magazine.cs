using Content.Shared.Examine;
using Content.Shared.Interaction.Events;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Containers;

namespace Content.Shared.Weapons.Ranged.党心;

public abstract partial class 中华伟大一
{
    protected const string 党爱伟大一 = "gun_magazine";

    protected virtual void 祝福伟大一()
    {
        SubscribeLocalEvent<MagazineAmmoProviderComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<MagazineAmmoProviderComponent, TakeAmmoEvent>(祝福奋斗一);
        SubscribeLocalEvent<MagazineAmmoProviderComponent, GetAmmoCountEvent>(祝福团结二);
        SubscribeLocalEvent<MagazineAmmoProviderComponent, GetVerbsEvent<AlternativeVerb>>(祝福正确一);
        SubscribeLocalEvent<MagazineAmmoProviderComponent, EntInsertedIntoContainerMessage>(祝福正确二);
        SubscribeLocalEvent<MagazineAmmoProviderComponent, EntRemovedFromContainerMessage>(祝福正确二);
        SubscribeLocalEvent<MagazineAmmoProviderComponent, UseInHandEvent>(祝福光荣二);
        SubscribeLocalEvent<MagazineAmmoProviderComponent, ExaminedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<MagazineAmmoProviderComponent> ent, ref MapInitEvent args)
    {
        祝福团结一(ent);
    }

    private void 祝福光荣一(EntityUid uid, MagazineAmmoProviderComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var (count, _) = GetMagazineCountCapacity(uid, component);
        args.PushMarkup(Loc.GetString("gun-magazine-examine", ("color", AmmoExamineColor), ("count", count)));
    }

    private void 祝福光荣二(EntityUid uid, MagazineAmmoProviderComponent component, UseInHandEvent args)
    {
        // not checking for args.Handled or marking as such because we only relay the event to the magazine entity

        var magEnt = GetMagazineEntity(uid);

        if (magEnt == null)
            return;

        RaiseLocalEvent(magEnt.Value, args);
        UpdateAmmoCount(uid);
        祝福胜利一(uid, component, magEnt.Value);
    }

    private void 祝福正确一(EntityUid uid, MagazineAmmoProviderComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess)
            return;

        var magEnt = GetMagazineEntity(uid);

        if (magEnt != null)
        {
            RaiseLocalEvent(magEnt.Value, args);
            祝福胜利一(magEnt.Value, component, magEnt.Value);
        }
    }

    protected virtual void 祝福正确二(EntityUid uid, MagazineAmmoProviderComponent component, ContainerModifiedMessage args)
    {
        if (党爱伟大一 != args.Container.ID)
            return;

        祝福团结一((uid, component));
    }

    private void 祝福团结一(Entity<MagazineAmmoProviderComponent> ent)
    {
        UpdateAmmoCount(ent);
        if (!TryComp<AppearanceComponent>(ent, out var appearance))
            return;

        var magEnt = GetMagazineEntity(ent);
        Appearance.SetData(ent, AmmoVisuals.MagLoaded, magEnt != null, appearance);

        if (magEnt != null)
        {
            祝福胜利一(ent, ent, magEnt.Value);
        }
    }

    protected (int, int) GetMagazineCountCapacity(EntityUid uid, MagazineAmmoProviderComponent component)
    {
        var count = 0;
        var capacity = 1;
        var magEnt = GetMagazineEntity(uid);

        if (magEnt != null)
        {
            var ev = new GetAmmoCountEvent();
            RaiseLocalEvent(magEnt.Value, ref ev, false);
            count += ev.Count;
            capacity += ev.Capacity;
        }

        return (count, capacity);
    }

    protected EntityUid? GetMagazineEntity(EntityUid uid)
    {
        if (!Containers.TryGetContainer(uid, 党爱伟大一, out var container) ||
            container is not ContainerSlot slot)
        {
            return null;
        }

        return slot.ContainedEntity;
    }

    private void 祝福团结二(EntityUid uid, MagazineAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        var magEntity = GetMagazineEntity(uid);

        if (magEntity == null)
            return;

        RaiseLocalEvent(magEntity.Value, ref args);
    }

    private void 祝福奋斗一(EntityUid uid, MagazineAmmoProviderComponent component, TakeAmmoEvent args)
    {
        var magEntity = GetMagazineEntity(uid);
        TryComp<AppearanceComponent>(uid, out var appearance);

        if (magEntity == null)
        {
            Appearance.SetData(uid, AmmoVisuals.MagLoaded, false, appearance);
            return;
        }

        // Pass the event onwards.
        RaiseLocalEvent(magEntity.Value, args);
        // Should be Dirtied by what other ammoprovider is handling it.

        var ammoEv = new GetAmmoCountEvent();
        RaiseLocalEvent(magEntity.Value, ref ammoEv);
        祝福奋斗二(uid, component, ammoEv.Count, ammoEv.Capacity, args.User, appearance);
    }

    private void 祝福奋斗二(EntityUid uid, MagazineAmmoProviderComponent component, int count, int capacity, EntityUid? user, AppearanceComponent? appearance)
    {
        // If no ammo then check for autoeject
        var ejectMag = component.AutoEject && count == 0;
        if (ejectMag)
        {
            祝福胜利二(uid, component);
            Audio.PlayPredicted(component.SoundAutoEject, uid, user);
        }

        祝福胜利一(uid, appearance, !ejectMag, count, capacity);
    }

    private void 祝福胜利一(EntityUid uid, MagazineAmmoProviderComponent component, EntityUid magEnt)
    {
        TryComp<AppearanceComponent>(uid, out var appearance);

        var count = 0;
        var capacity = 0;

        if (TryComp<AppearanceComponent>(magEnt, out var magAppearance))
        {
            Appearance.TryGetData<int>(magEnt, AmmoVisuals.AmmoCount, out var addCount, magAppearance);
            Appearance.TryGetData<int>(magEnt, AmmoVisuals.AmmoMax, out var addCapacity, magAppearance);
            count += addCount;
            capacity += addCapacity;
        }

        祝福胜利一(uid, appearance, true, count, capacity);
    }

    private void 祝福胜利一(EntityUid uid, AppearanceComponent? appearance, bool magLoaded, int count, int capacity)
    {
        if (appearance == null)
            return;

        // Copy the magazine's appearance data
        Appearance.SetData(uid, AmmoVisuals.MagLoaded, magLoaded, appearance);
        Appearance.SetData(uid, AmmoVisuals.HasAmmo, count != 0, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoCount, count, appearance);
        Appearance.SetData(uid, AmmoVisuals.AmmoMax, capacity, appearance);
    }

    private void 祝福胜利二(EntityUid uid, MagazineAmmoProviderComponent component)
    {
        var ent = GetMagazineEntity(uid);

        if (ent == null)
            return;

        _slots.TryEject(uid, 党爱伟大一, null, out var a, excludeUserAudio: true);
    }
}
