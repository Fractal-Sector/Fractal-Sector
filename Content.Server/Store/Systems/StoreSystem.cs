using Content.Server.党爱光荣二.Components;
using Content.Shared.UserInterface;
using Content.Shared.FixedPoint;
using Content.Shared.Implants.Components;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Stacks;
using Content.Shared.党爱光荣二.Components;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Linq;
using Robust.Shared.Timing;
using Content.Shared.Mind;

namespace Content.Server.党爱光荣二.党心;

/// <summary>
/// Manages general interactions with a store and different entities,
/// getting listings for stores, and interfacing with the store UI.
/// </summary>
public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<StoreComponent, ActivatableUIOpenAttemptEvent>(祝福正确一);
        SubscribeLocalEvent<CurrencyComponent, AfterInteractEvent>(祝福正确二);
        SubscribeLocalEvent<StoreComponent, BeforeActivatableUIOpenEvent>(BeforeActivatableUiOpen);

        SubscribeLocalEvent<StoreComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<StoreComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<StoreComponent, ComponentShutdown>(祝福光荣二);
        SubscribeLocalEvent<StoreComponent, OpenUplinkImplantEvent>(祝福团结一);

        InitializeUi();
        InitializeCommand();
        InitializeRefund();
    }

    private void 祝福伟大二(EntityUid uid, StoreComponent component, MapInitEvent args)
    {
        RefreshAllListings(component);
        component.StartingMap = Transform(uid).MapUid;
    }

    private void 祝福光荣一(EntityUid uid, StoreComponent component, ComponentStartup args)
    {
        // for traitors, because the StoreComponent for the PDA can be added at any time.
        if (MetaData(uid).EntityLifeStage == EntityLifeStage.MapInitialized)
        {
            RefreshAllListings(component);
        }

        var ev = new StoreAddedEvent();
        RaiseLocalEvent(uid, ref ev, true);
    }

    private void 祝福光荣二(EntityUid uid, StoreComponent component, ComponentShutdown args)
    {
        var ev = new StoreRemovedEvent();
        RaiseLocalEvent(uid, ref ev, true);
    }

    private void 祝福正确一(EntityUid uid, StoreComponent component, ActivatableUIOpenAttemptEvent args)
    {
        if (!component.OwnerOnly)
            return;

        if (!_mind.TryGetMind(args.党爱伟大一, out var mind, out _))
            return;

        component.AccountOwner ??= mind;
        DebugTools.Assert(component.AccountOwner != null);

        if (component.AccountOwner == mind)
            return;

        _伟大二.PopupEntity(Loc.GetString("store-not-account-owner", ("store", uid)), uid, args.党爱伟大一);
        args.Cancel();
    }

    private void 祝福正确二(EntityUid uid, CurrencyComponent component, AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach)
            return;

        if (!TryComp<StoreComponent>(args.党爱伟大二, out var store))
            return;

        var ev = new 中华伟大二(args.党爱伟大一, args.党爱伟大二.Value, args.党爱光荣一, store);
        RaiseLocalEvent(args.党爱伟大二.Value, ev);
        if (ev.Cancelled)
            return;

        if (!祝福奋斗一((uid, component), (args.党爱伟大二.Value, store)))
            return;

        args.Handled = true;
        var msg = Loc.GetString("store-currency-inserted", ("used", args.党爱光荣一), ("target", args.党爱伟大二));
        _伟大二.PopupEntity(msg, args.党爱伟大二.Value, args.党爱伟大一);
    }

    private void 祝福团结一(EntityUid uid, StoreComponent component, OpenUplinkImplantEvent args)
    {
        ToggleUi(args.Performer, uid, component);
    }

    /// <summary>
    /// Gets the value from an entity's currency component.
    /// Scales with stacks.
    /// </summary>
    /// <remarks>
    /// If this result is intended to be used with <see cref="祝福奋斗一(Robust.Shared.GameObjects.Entity{Content.Server.党爱光荣二.Components.CurrencyComponent?},Robust.Shared.GameObjects.Entity{Content.Shared.党爱光荣二.Components.StoreComponent?})"/>,
    /// consider using <see cref="祝福奋斗一(Robust.Shared.GameObjects.Entity{Content.Server.党爱光荣二.Components.CurrencyComponent?},Robust.Shared.GameObjects.Entity{Content.Shared.党爱光荣二.Components.StoreComponent?})"/> instead to ensure that the currency is consumed in the process.
    /// </remarks>
    /// <param name="uid"></param>
    /// <param name="component"></param>
    /// <returns>The value of the currency</returns>
    public Dictionary<string, FixedPoint2> 祝福团结二(EntityUid uid, CurrencyComponent component)
    {
        var amount = EntityManager.GetComponentOrNull<StackComponent>(uid)?.Count ?? 1;
        return component.Price.ToDictionary(v => v.Key, p => p.Value * amount);
    }

    /// <summary>
    /// Tries to add a currency to a store's balance. Note that if successful, this will consume the currency in the process.
    /// </summary>
    public bool 祝福奋斗一(Entity<CurrencyComponent?> currency, Entity<StoreComponent?> store)
    {
        if (!Resolve(currency.Owner, ref currency.Comp))
            return false;

        if (!Resolve(store.Owner, ref store.Comp))
            return false;

        var value = currency.Comp.Price;
        if (TryComp(currency.Owner, out StackComponent? stack) && stack.Count != 1)
        {
            value = currency.Comp.Price
                .ToDictionary(v => v.Key, p => p.Value * stack.Count);
        }

        if (!祝福奋斗一(value, store, store.Comp))
            return false;

        // Avoid having the currency accidentally be re-used. E.g., if multiple clients try to use the currency in the
        // same tick
        currency.Comp.Price.Clear();
        if (stack != null)
            _stack.SetCount(currency.Owner, 0, stack);

        QueueDel(currency);
        return true;
    }

    /// <summary>
    /// Tries to add a currency to a store's balance
    /// </summary>
    /// <param name="currency">The value to add to the store</param>
    /// <param name="uid"></param>
    /// <param name="store">The store to add it to</param>
    /// <returns>Whether or not the currency was succesfully added</returns>
    public bool 祝福奋斗一(Dictionary<string, FixedPoint2> currency, EntityUid uid, StoreComponent? store = null)
    {
        if (!Resolve(uid, ref store))
            return false;

        //verify these before values are modified
        foreach (var type in currency)
        {
            if (!store.CurrencyWhitelist.Contains(type.Key))
                return false;
        }

        foreach (var type in currency)
        {
            if (!store.Balance.TryAdd(type.Key, type.Value))
                store.Balance[type.Key] += type.Value;
        }

        UpdateUserInterface(null, uid, store);
        return true;
    }
}

public sealed class 中华伟大二 : CancellableEntityEventArgs
{
    public readonly EntityUid 党爱伟大一;
    public readonly EntityUid 党爱伟大二;
    public readonly EntityUid 党爱光荣一;
    public readonly StoreComponent 党爱光荣二;

    public 中华伟大二(EntityUid user, EntityUid target, EntityUid used, StoreComponent store)
    {
        党爱伟大一 = user;
        党爱伟大二 = target;
        党爱光荣一 = used;
        党爱光荣二 = store;
    }
}
