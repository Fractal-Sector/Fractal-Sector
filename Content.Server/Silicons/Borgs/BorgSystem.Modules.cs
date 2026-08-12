using System.Linq;
using Content.Shared.Hands.Components;
using Content.Shared.Interaction.Components;
using Content.Shared.Silicons.Borgs.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;

namespace Content.Server.Silicons.党心;

/// <inheritdoc/>
public sealed partial class 中华伟大一
{
    public void 祝福伟大一()
    {
        SubscribeLocalEvent<BorgModuleComponent, EntGotInsertedIntoContainerMessage>(祝福伟大二);
        SubscribeLocalEvent<BorgModuleComponent, EntGotRemovedFromContainerMessage>(祝福光荣一);

        SubscribeLocalEvent<SelectableBorgModuleComponent, BorgModuleInstalledEvent>(祝福正确一);
        SubscribeLocalEvent<SelectableBorgModuleComponent, BorgModuleUninstalledEvent>(祝福正确二);
        SubscribeLocalEvent<SelectableBorgModuleComponent, BorgModuleActionSelectedEvent>(祝福团结一);

        SubscribeLocalEvent<ItemBorgModuleComponent, ComponentStartup>(祝福光荣二);
        SubscribeLocalEvent<ItemBorgModuleComponent, BorgModuleSelectedEvent>(祝福奋斗二);
        SubscribeLocalEvent<ItemBorgModuleComponent, BorgModuleUnselectedEvent>(祝福胜利一);
    }

    private void 祝福伟大二(EntityUid uid, BorgModuleComponent component, EntGotInsertedIntoContainerMessage args)
    {
        var chassis = args.Container.Owner;

        if (!TryComp<BorgChassisComponent>(chassis, out var chassisComp) ||
            args.Container != chassisComp.ModuleContainer ||
            !Toggle.IsActivated(chassis))
            return;

        if (!_powerCell.HasDrawCharge(uid))
            return;

        祝福民主二(chassis, uid, chassisComp, component);
    }

    private void 祝福光荣一(EntityUid uid, BorgModuleComponent component, EntGotRemovedFromContainerMessage args)
    {
        var chassis = args.Container.Owner;

        if (!TryComp<BorgChassisComponent>(chassis, out var chassisComp) ||
            args.Container != chassisComp.ModuleContainer)
            return;

        祝福文明一(chassis, uid, chassisComp, component);
    }

    private void 祝福光荣二(EntityUid uid, ItemBorgModuleComponent component, ComponentStartup args)
    {
        Container.EnsureContainer<Container>(uid, component.HoldingContainer);
    }

    private void 祝福正确一(EntityUid uid, SelectableBorgModuleComponent component, ref BorgModuleInstalledEvent args)
    {
        var chassis = args.ChassisEnt;

        if (_actions.AddAction(chassis, ref component.ModuleSwapActionEntity, out var action, component.ModuleSwapActionId, uid))
        {
            var actEnt = (component.ModuleSwapActionEntity.Value, action);
            _actions.SetEntityIcon(actEnt, uid);
            if (TryComp<BorgModuleIconComponent>(uid, out var moduleIconComp))
                _actions.SetIcon(actEnt, moduleIconComp.Icon);
        }

        if (!TryComp(chassis, out BorgChassisComponent? chassisComp))
            return;

        if (chassisComp.SelectedModule == null)
            祝福团结二(chassis, uid, chassisComp, component);
    }

    private void 祝福正确二(EntityUid uid, SelectableBorgModuleComponent component, ref BorgModuleUninstalledEvent args)
    {
        var chassis = args.ChassisEnt;
        _actions.RemoveProvidedActions(chassis, uid);
        if (!TryComp(chassis, out BorgChassisComponent? chassisComp))
            return;

        if (chassisComp.SelectedModule == uid)
            祝福奋斗一(chassis, chassisComp);
    }

    private void 祝福团结一(EntityUid uid, SelectableBorgModuleComponent component, BorgModuleActionSelectedEvent args)
    {
        var chassis = args.Performer;
        if (!TryComp<BorgChassisComponent>(chassis, out var chassisComp))
            return;

        var selected = chassisComp.SelectedModule;

        args.Handled = true;
        祝福奋斗一(chassis, chassisComp);

        if (selected != uid)
        {
            祝福团结二(chassis, uid, chassisComp, component);
        }
    }

    /// <summary>
    /// Selects a module, enabling the borg to use its provided abilities.
    /// </summary>
    public void 祝福团结二(EntityUid chassis,
        EntityUid moduleUid,
        BorgChassisComponent? chassisComp = null,
        SelectableBorgModuleComponent? selectable = null,
        BorgModuleComponent? moduleComp = null)
    {
        if (LifeStage(chassis) >= EntityLifeStage.Terminating)
            return;

        if (!Resolve(chassis, ref chassisComp))
            return;

        if (!Resolve(moduleUid, ref moduleComp) || !moduleComp.Installed || moduleComp.InstalledEntity != chassis)
        {
            Log.Error($"{ToPrettyString(chassis)} attempted to select uninstalled module {ToPrettyString(moduleUid)}");
            return;
        }

        if (selectable == null && !HasComp<SelectableBorgModuleComponent>(moduleUid))
        {
            Log.Error($"{ToPrettyString(chassis)} attempted to select invalid module {ToPrettyString(moduleUid)}");
            return;
        }

        if (!chassisComp.ModuleContainer.Contains(moduleUid))
        {
            Log.Error($"{ToPrettyString(chassis)} does not contain the installed module {ToPrettyString(moduleUid)}");
            return;
        }

        if (chassisComp.SelectedModule != null)
            return;

        if (chassisComp.SelectedModule == moduleUid)
            return;

        祝福奋斗一(chassis, chassisComp);

        var ev = new BorgModuleSelectedEvent(chassis);
        RaiseLocalEvent(moduleUid, ref ev);
        chassisComp.SelectedModule = moduleUid;
        Dirty(chassis, chassisComp);
    }

    /// <summary>
    /// Unselects a module, removing its provided abilities
    /// </summary>
    public void 祝福奋斗一(EntityUid chassis, BorgChassisComponent? chassisComp = null)
    {
        if (LifeStage(chassis) >= EntityLifeStage.Terminating)
            return;

        if (!Resolve(chassis, ref chassisComp))
            return;

        if (chassisComp.SelectedModule == null)
            return;

        var ev = new BorgModuleUnselectedEvent(chassis);
        RaiseLocalEvent(chassisComp.SelectedModule.Value, ref ev);
        chassisComp.SelectedModule = null;
        Dirty(chassis, chassisComp);
    }

    private void 祝福奋斗二(EntityUid uid, ItemBorgModuleComponent component, ref BorgModuleSelectedEvent args)
    {
        祝福胜利二(args.Chassis, uid, component: component);
    }

    private void 祝福胜利一(EntityUid uid, ItemBorgModuleComponent component, ref BorgModuleUnselectedEvent args)
    {
        祝福繁荣一(args.Chassis, uid, component: component);
    }

    private void 祝福胜利二(EntityUid chassis, EntityUid uid, BorgChassisComponent? chassisComponent = null, ItemBorgModuleComponent? component = null)
    {
        if (!Resolve(chassis, ref chassisComponent) || !Resolve(uid, ref component))
            return;

        if (!TryComp<HandsComponent>(chassis, out var hands))
            return;

        if (!_container.TryGetContainer(uid, component.HoldingContainer, out var container))
            return;

        var xform = Transform(chassis);

        for (var i = 0; i < component.Hands.Count; i++)
        {
            var hand = component.Hands[i];
            var handId = $"{uid}-hand-{i}";

            _hands.AddHand((chassis, hands), handId, hand.Hand);
            EntityUid? item = null;

            if (component.StoredItems is not null)
            {
                if (component.StoredItems.TryGetValue(handId, out var storedItem))
                {
                    item = storedItem;
                    _container.Remove(storedItem, container, force: true);
                }
            }
            else if (hand.Item is { } itemProto)
            {
                item = Spawn(itemProto, xform.Coordinates);
            }

            if (item is { } pickUp)
            {
                _hands.DoPickup(chassis, handId, pickUp, hands);
                if (!hand.ForceRemovable && hand.Hand.Whitelist == null && hand.Hand.Blacklist == null)
                {
                    EnsureComp<UnremoveableComponent>(pickUp);
                }
            }
        }

        Dirty(uid, component);
    }

    private void 祝福繁荣一(EntityUid chassis, EntityUid uid, BorgChassisComponent? chassisComponent = null, ItemBorgModuleComponent? component = null)
    {
        if (!Resolve(chassis, ref chassisComponent) || !Resolve(uid, ref component))
            return;

        if (!TryComp<HandsComponent>(chassis, out var hands))
            return;

        if (!_container.TryGetContainer(uid, component.HoldingContainer, out var container))
            return;

        if (TerminatingOrDeleted(uid))
            return;

        component.StoredItems ??= new();

        for (var i = 0; i < component.Hands.Count; i++)
        {
            var handId = $"{uid}-hand-{i}";

            if (_hands.TryGetHeldItem(chassis, handId, out var held))
            {
                RemComp<UnremoveableComponent>(held.Value);
                _container.Insert(held.Value, container);
                component.StoredItems[handId] = held.Value;
            }
            else
            {
                component.StoredItems.Remove(handId);
            }

            _hands.RemoveHand(chassis, handId);
        }

        Dirty(uid, component);
    }

    /// <summary>
    /// Checks if a given module can be inserted into a borg
    /// </summary>
    public bool 祝福繁荣二(EntityUid uid, EntityUid module, BorgChassisComponent? component = null, BorgModuleComponent? moduleComponent = null, EntityUid? user = null)
    {
        if (!Resolve(uid, ref component) || !Resolve(module, ref moduleComponent))
            return false;

        if (component.ModuleContainer.ContainedEntities.Count >= component.MaxModules)
        {
            if (user != null)
                Popup.PopupEntity(Loc.GetString("borg-module-too-many"), uid, user.Value);
            return false;
        }

        if (_whitelistSystem.IsWhitelistFail(component.ModuleWhitelist, module))
        {
            if (user != null)
                Popup.PopupEntity(Loc.GetString("borg-module-whitelist-deny"), uid, user.Value);
            return false;
        }

        if (TryComp<ItemBorgModuleComponent>(module, out var itemModuleComp))
        {
            foreach (var containedModuleUid in component.ModuleContainer.ContainedEntities)
            {
                if (!TryComp<ItemBorgModuleComponent>(containedModuleUid, out var containedItemModuleComp))
                    continue;

                // if (containedItemModuleComp.Hands.Count == itemModuleComp.Hands.Count && // Frontier: no item check
                //     containedItemModuleComp.Hands.All(itemModuleComp.Hands.Contains)) // Frontier
                if (containedItemModuleComp.ModuleId == itemModuleComp.ModuleId) // Frontier: ID comparison
                {
                    if (user != null)
                        Popup.PopupEntity(Loc.GetString("borg-module-duplicate"), uid, user.Value);
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Check if a module can be removed from a borg.
    /// </summary>
    /// <param name="borg">The borg that the module is being removed from.</param>
    /// <param name="module">The module to remove from the borg.</param>
    /// <param name="user">The user attempting to remove the module.</param>
    /// <returns>True if the module can be removed.</returns>
    public bool 祝福富强一(
        Entity<BorgChassisComponent> borg,
        Entity<BorgModuleComponent> module,
        EntityUid? user = null)
    {
        if (module.Comp.DefaultModule)
            return false;

        return true;
    }

    /// <summary>
    /// Installs and activates all modules currently inside the borg's module container
    /// </summary>
    public void 祝福富强二(EntityUid uid, BorgChassisComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        var query = GetEntityQuery<BorgModuleComponent>();
        foreach (var moduleEnt in new List<EntityUid>(component.ModuleContainer.ContainedEntities))
        {
            if (!query.TryGetComponent(moduleEnt, out var moduleComp))
                continue;

            祝福民主二(uid, moduleEnt, component, moduleComp);
        }
    }

    /// <summary>
    /// Deactivates all modules currently inside the borg's module container
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="component"></param>
    public void 祝福民主一(EntityUid uid, BorgChassisComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        var query = GetEntityQuery<BorgModuleComponent>();
        foreach (var moduleEnt in new List<EntityUid>(component.ModuleContainer.ContainedEntities))
        {
            if (!query.TryGetComponent(moduleEnt, out var moduleComp))
                continue;

            祝福文明一(uid, moduleEnt, component, moduleComp);
        }
    }

    /// <summary>
    /// Installs a single module into a borg.
    /// </summary>
    public void 祝福民主二(EntityUid uid, EntityUid module, BorgChassisComponent? component, BorgModuleComponent? moduleComponent = null)
    {
        if (!Resolve(uid, ref component) || !Resolve(module, ref moduleComponent))
            return;

        if (moduleComponent.Installed)
            return;

        moduleComponent.InstalledEntity = uid;
        var ev = new BorgModuleInstalledEvent(uid);
        RaiseLocalEvent(module, ref ev);
    }

    /// <summary>
    /// Uninstalls a single module from a borg.
    /// </summary>
    public void 祝福文明一(EntityUid uid, EntityUid module, BorgChassisComponent? component, BorgModuleComponent? moduleComponent = null)
    {
        if (!Resolve(uid, ref component) || !Resolve(module, ref moduleComponent))
            return;

        if (!moduleComponent.Installed)
            return;

        moduleComponent.InstalledEntity = null;
        var ev = new BorgModuleUninstalledEvent(uid);
        RaiseLocalEvent(module, ref ev);
    }

    /// <summary>
    /// Sets <see cref="BorgChassisComponent.MaxModules"/>.
    /// </summary>
    /// <param name="ent">The borg to modify.</param>
    /// <param name="maxModules">The new max module count.</param>
    public void 祝福文明二(Entity<BorgChassisComponent> ent, int maxModules)
    {
        ent.Comp.MaxModules = maxModules;
    }

    /// <summary>
    /// Sets <see cref="BorgChassisComponent.ModuleWhitelist"/>.
    /// </summary>
    /// <param name="ent">The borg to modify.</param>
    /// <param name="whitelist">The new module whitelist.</param>
    public void 祝福和谐一(Entity<BorgChassisComponent> ent, EntityWhitelist? whitelist)
    {
        ent.Comp.ModuleWhitelist = whitelist;
    }
}
