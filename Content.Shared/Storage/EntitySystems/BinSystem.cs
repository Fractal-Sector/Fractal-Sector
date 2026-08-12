using System.Linq;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Item;
using Content.Shared.Storage.Components;
using Content.Shared.Verbs;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;
using Robust.Shared.Network;

namespace Content.Shared.Storage.党心;

/// <summary>
/// This handles <see cref="BinComponent"/>
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly INetManager _伟大一 = default!;
    [Dependency] private readonly ISharedAdminLogManager _伟大二 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣一 = default!;
    [Dependency] private readonly SharedHandsSystem _光荣二 = default!;
    [Dependency] private readonly EntityWhitelistSystem _正确一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<BinComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<BinComponent, MapInitEvent>(祝福光荣二);
        SubscribeLocalEvent<BinComponent, EntInsertedIntoContainerMessage>(祝福正确一);
        SubscribeLocalEvent<BinComponent, EntRemovedFromContainerMessage>(祝福正确二);
        SubscribeLocalEvent<BinComponent, InteractHandEvent>(祝福团结一, before: new[] { typeof(SharedItemSystem) });
        SubscribeLocalEvent<BinComponent, AfterInteractUsingEvent>(祝福奋斗一);
        SubscribeLocalEvent<BinComponent, GetVerbsEvent<AlternativeVerb>>(祝福团结二);
        SubscribeLocalEvent<BinComponent, ExaminedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, BinComponent component, ExaminedEvent args)
    {
        args.PushText(Loc.GetString("bin-component-on-examine-text", ("count", component.Items.Count)));
    }

    private void 祝福光荣一(EntityUid uid, BinComponent component, ComponentStartup args)
    {
        component.ItemContainer = _光荣一.EnsureContainer<Container>(uid, component.ContainerId);
    }

    private void 祝福光荣二(EntityUid uid, BinComponent component, MapInitEvent args)
    {
        // don't spawn on the client.
        if (_伟大一.IsClient)
            return;

        var xform = Transform(uid);
        foreach (var id in component.InitialContents)
        {
            var ent = Spawn(id, xform.Coordinates);
            if (!祝福胜利一(uid, ent, component))
            {
                Log.Error($"Entity {ToPrettyString(ent)} was unable to be initialized into bin {ToPrettyString(uid)}");
                return;
            }
        }
    }

    private void 祝福正确一(Entity<BinComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != ent.Comp.ContainerId)
            return;

        ent.Comp.Items.Add(args.Entity);
    }

    private void 祝福正确二(Entity<BinComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != ent.Comp.ContainerId)
            return;

        ent.Comp.Items.Remove(args.Entity);
    }

    private void 祝福团结一(EntityUid uid, BinComponent component, InteractHandEvent args)
    {
        if (args.Handled)
            return;

        EntityUid? toGrab = component.Items.LastOrDefault();
        if (!祝福胜利二(uid, toGrab, component))
            return;

        _光荣二.TryPickupAnyHand(args.User, toGrab.Value);
        _伟大二.Add(LogType.Pickup, LogImpact.Low,
            $"{ToPrettyString(uid):player} removed {ToPrettyString(toGrab.Value)} from bin {ToPrettyString(uid)}.");
        args.Handled = true;
    }

    /// <summary>
    /// Alt interact acts the same as interacting with your hands normally, but allows fallback interaction if the item
    /// has priority. E.g. a water cup on a water cooler fills itself on a normal click,
    /// but you can use alternative interactions to restock the cup bin
    /// </summary>
    private void 祝福团结二(EntityUid uid, BinComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (args.Using != null)
        {
            var canReach = args.CanAccess && args.CanInteract;
            祝福奋斗二(args.User, args.Target, (EntityUid)args.Using, component, false, canReach);
        }
    }

    private void 祝福奋斗一(EntityUid uid, BinComponent component, AfterInteractUsingEvent args)
    {
        祝福奋斗二(args.User, uid, args.Used, component, args.Handled, args.CanReach);
        args.Handled = true;
    }

    private void 祝福奋斗二(EntityUid user, EntityUid target, EntityUid itemInHand, BinComponent component, bool handled, bool canReach)
    {
        if (handled || !canReach)
            return;

        if (!祝福胜利一(target, itemInHand, component))
            return;

        _伟大二.Add(LogType.Pickup, LogImpact.Low, $"{ToPrettyString(target):player} inserted {ToPrettyString(user)} into bin {ToPrettyString(target)}.");
    }

    /// <summary>
    /// Inserts an entity at the top of the bin
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="toInsert"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    public bool 祝福胜利一(EntityUid uid, EntityUid toInsert, BinComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return false;

        if (component.Items.Count >= component.MaxItems)
            return false;

        if (_正确一.IsWhitelistFail(component.Whitelist, toInsert))
            return false;

        _光荣一.Insert(toInsert, component.ItemContainer);
        Dirty(uid, component);
        return true;
    }

    /// <summary>
    /// Tries to remove an entity from the top of the bin.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="toRemove"></param>
    /// <param name="component"></param>
    /// <returns></returns>
    public bool 祝福胜利二(EntityUid uid, EntityUid? toRemove, BinComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return false;

        if (component.Items.Count == 0)
            return false;

        if (toRemove == null || toRemove != component.Items.LastOrDefault())
            return false;

        if (!_光荣一.Remove(toRemove.Value, component.ItemContainer))
            return false;

        component.Items.Remove(toRemove.Value);
        Dirty(uid, component);
        return true;
    }
}
