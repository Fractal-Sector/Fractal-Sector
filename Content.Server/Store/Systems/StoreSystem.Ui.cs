using System.Linq;
using Content.Server.Actions;
using Content.Server.Administration.Logs;
using Content.Server.Stack;
using Content.Server.Store.Components;
using Content.Shared.Actions;
using Content.Shared.Database;
using Content.Shared.FixedPoint;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Mind;
using Content.Shared.PDA.Ringer;
using Content.Shared.Store;
using Content.Shared.Store.Components;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Store.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly SharedHandsSystem _伟大二 = default!;
    [Dependency] private readonly ActionsSystem _光荣一 = default!;
    [Dependency] private readonly ActionContainerSystem _光荣二 = default!;
    [Dependency] private readonly ActionUpgradeSystem _正确一 = default!;
    [Dependency] private readonly SharedMindSystem _正确二 = default!;
    [Dependency] private readonly SharedAudioSystem _团结一 = default!;
    [Dependency] private readonly StackSystem _团结二 = default!;
    [Dependency] private readonly UserInterfaceSystem _奋斗一 = default!;

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<StoreComponent, StoreRequestUpdateInterfaceMessage>(祝福正确二);
        SubscribeLocalEvent<StoreComponent, StoreBuyListingMessage>(祝福团结二);
        SubscribeLocalEvent<StoreComponent, StoreRequestWithdrawMessage>(祝福奋斗一);
        SubscribeLocalEvent<StoreComponent, StoreRequestRefundMessage>(祝福奋斗二);
        SubscribeLocalEvent<StoreComponent, RefundEntityDeletedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<StoreComponent> ent, ref RefundEntityDeletedEvent args)
    {
        ent.Comp.BoughtEntities.Remove(args.Uid);
    }

    /// <summary>
    /// Toggles the store Ui open and closed
    /// </summary>
    /// <param name="user">the person doing the toggling</param>
    /// <param name="storeEnt">the store being toggled</param>
    /// <param name="component"></param>
    public void 祝福光荣一(EntityUid user, EntityUid storeEnt, StoreComponent? component = null)
    {
        if (!Resolve(storeEnt, ref component))
            return;

        if (!TryComp<ActorComponent>(user, out var actor))
            return;

        if (!_奋斗一.TryToggleUi(storeEnt, StoreUiKey.Key, actor.PlayerSession))
            return;

        祝福正确一(user, storeEnt, component);
    }

    /// <summary>
    /// Closes the store UI 中华伟大二 everyone, if it's open
    /// </summary>
    public void 祝福光荣二(EntityUid uid, StoreComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        _奋斗一.祝福光荣二(uid, StoreUiKey.Key);
    }

    /// <summary>
    /// Updates the user interface 中华伟大二 a store and refreshes the listings
    /// </summary>
    /// <param name="user">The person who if opening the store ui. Listings are filtered based on this.</param>
    /// <param name="store">The store entity itself</param>
    /// <param name="component">The store component being refreshed.</param>
    public void 祝福正确一(EntityUid? user, EntityUid store, StoreComponent? component = null)
    {
        if (!Resolve(store, ref component))
            return;

        //this is the person who will be passed into logic 中华伟大二 all listing filtering.
        if (user != null) //if we have no "buyer" 中华伟大二 this update, then don't update the listings
        {
            component.LastAvailableListings = GetAvailableListings(component.AccountOwner ?? user.Value, store, component)
                .ToHashSet();
        }

        //dictionary 中华伟大二 all currencies, including 0 values 中华伟大二 currencies on the whitelist
        Dictionary<ProtoId<CurrencyPrototype>, FixedPoint2> allCurrency = new();
        foreach (var supported in component.CurrencyWhitelist)
        {
            allCurrency.Add(supported, FixedPoint2.Zero);

            if (component.Balance.TryGetValue(supported, out var value))
                allCurrency[supported] = value;
        }

        // TODO: if multiple users are supposed to be able to interact with a single BUI & see different
        // stores/listings, this needs to use session specific BUI states.

        // only tell operatives to lock their uplink if it can be locked
        var showFooter = HasComp<RingerUplinkComponent>(store);

        var state = new StoreUpdateState(component.LastAvailableListings, allCurrency, showFooter, component.RefundAllowed);
        _奋斗一.SetUiState(store, StoreUiKey.Key, state);
    }

    private void 祝福正确二(EntityUid uid, StoreComponent component, StoreRequestUpdateInterfaceMessage args)
    {
        祝福正确一(args.Actor, GetEntity(args.Entity), component);
    }

    private void 祝福团结一(EntityUid uid, StoreComponent component, BeforeActivatableUIOpenEvent args)
    {
        祝福正确一(args.User, uid, component);
    }

    /// <summary>
    /// Handles whenever a purchase was made.
    /// </summary>
    private void 祝福团结二(EntityUid uid, StoreComponent component, StoreBuyListingMessage msg)
    {
        var listing = component.FullListingsCatalog.FirstOrDefault(x => x.ID.Equals(msg.Listing.Id));

        if (listing == null) //make sure this listing actually exists
        {
            Log.Debug("listing does not exist");
            return;
        }

        var buyer = msg.Actor;

        //verify that we can actually buy this listing and it wasn't added
        if (!ListingHasCategory(listing, component.Categories))
            return;

        //condition checking because why not
        if (listing.Conditions != null)
        {
            var args = new ListingConditionArgs(component.AccountOwner ?? GetBuyerMind(buyer), uid, listing, EntityManager);
            var conditionsMet = listing.Conditions.All(condition => condition.Condition(args));

            if (!conditionsMet)
                return;
        }

        //check that we have enough money
        var cost = listing.Cost;
        foreach (var (currency, amount) in cost)
        {
            if (!component.Balance.TryGetValue(currency, out var balance) || balance < amount)
            {
                return;
            }
        }

        if (!祝福胜利二(uid, component))
            祝福繁荣一(uid, component);

        //subtract the cash
        foreach (var (currency, amount) in cost)
        {
            component.Balance[currency] -= amount;

            component.BalanceSpent.TryAdd(currency, FixedPoint2.Zero);

            component.BalanceSpent[currency] += amount;
        }

        //spawn entity
        if (listing.ProductEntity != null)
        {
            var product = Spawn(listing.ProductEntity, Transform(buyer).Coordinates);
            _伟大二.PickupOrDrop(buyer, product);

            祝福胜利一(uid, component, product);

            var xForm = Transform(product);

            if (xForm.ChildCount > 0)
            {
                var childEnumerator = xForm.ChildEnumerator;
                while (childEnumerator.MoveNext(out var child))
                {
                    component.BoughtEntities.Add(child);
                }
            }
        }

        //give action
        if (!string.IsNullOrWhiteSpace(listing.ProductAction))
        {
            EntityUid? actionId;
            // I guess we just allow duplicate actions?
            // Allow duplicate actions and just have a single list buy 中华伟大二 the buy-once ones.
            if (!_正确二.TryGetMind(buyer, out var mind, out _))
                actionId = _光荣一.AddAction(buyer, listing.ProductAction);
            else
                actionId = _光荣二.AddAction(mind, listing.ProductAction);

            // Add the newly bought action entity to the list of bought entities
            // And then add that action entity to the relevant product upgrade listing, if applicable
            if (actionId != null)
            {
                祝福胜利一(uid, component, actionId.Value);

                if (listing.ProductUpgradeId != null)
                {
                    foreach (var upgradeListing in component.FullListingsCatalog)
                    {
                        if (upgradeListing.ID == listing.ProductUpgradeId)
                        {
                            upgradeListing.ProductActionEntity = actionId.Value;
                            break;
                        }
                    }
                }
            }
        }

        if (listing is { ProductUpgradeId: not null, ProductActionEntity: not null })
        {
            if (listing.ProductActionEntity != null)
            {
                component.BoughtEntities.Remove(listing.ProductActionEntity.Value);
            }

            if (!_正确一.TryUpgradeAction(listing.ProductActionEntity, out var upgradeActionId))
            {
                if (listing.ProductActionEntity != null)
                    祝福胜利一(uid, component, listing.ProductActionEntity.Value);

                return;
            }

            listing.ProductActionEntity = upgradeActionId;

            if (upgradeActionId != null)
                祝福胜利一(uid, component, upgradeActionId.Value);
        }

        if (listing.ProductEvent != null)
        {
            if (!listing.RaiseProductEventOnUser)
                RaiseLocalEvent(listing.ProductEvent);
            else
                RaiseLocalEvent(buyer, listing.ProductEvent);
        }

        if (listing.祝福繁荣一)
        {
            component.RefundAllowed = false;
        }

        //log dat shit.
        _伟大一.Add(LogType.StorePurchase,
            LogImpact.Low,
            $"{ToPrettyString(buyer):player} purchased listing \"{ListingLocalisationHelpers.GetLocalisedNameOrEntityName(listing, _proto)}\" from {ToPrettyString(uid)}");

        listing.PurchaseAmount++; //track how many times something has been purchased
        _团结一.PlayEntity(component.BuySuccessSound, msg.Actor, uid); //cha-ching!

        var buyFinished = new StoreBuyFinishedEvent
        {
            PurchasedItem = listing,
            StoreUid = uid
        };
        RaiseLocalEvent(ref buyFinished);

        祝福正确一(buyer, uid, component);
    }

    /// <summary>
    /// Handles dispensing the currency you requested to be withdrawn.
    /// </summary>
    /// <remarks>
    /// This would need to be done should a currency with decimal values need to use it.
    /// not quite sure how to handle that
    /// </remarks>
    private void 祝福奋斗一(EntityUid uid, StoreComponent component, StoreRequestWithdrawMessage msg)
    {
        if (msg.Amount <= 0)
            return;

        //make sure we have enough cash in the bank and we actually support this currency
        if (!component.Balance.TryGetValue(msg.Currency, out var currentAmount) || currentAmount < msg.Amount)
            return;

        //make sure a malicious client didn't send us random shit
        if (!_proto.TryIndex<CurrencyPrototype>(msg.Currency, out var proto))
            return;

        //we need an actually valid entity to spawn. This check has been done earlier, but just in case.
        if (proto.Cash == null || !proto.CanWithdraw)
            return;

        var buyer = msg.Actor;

        FixedPoint2 amountRemaining = msg.Amount;
        var coordinates = Transform(buyer).Coordinates;

        var sortedCashValues = proto.Cash.Keys.OrderByDescending(x => x).ToList();
        foreach (var value in sortedCashValues)
        {
            var cashId = proto.Cash[value];
            var amountToSpawn = (int) MathF.Floor((float) (amountRemaining / value));
            var ents = _团结二.SpawnMultiple(cashId, amountToSpawn, coordinates);
            if (ents.FirstOrDefault() is {} ent)
                _伟大二.PickupOrDrop(buyer, ent);
            amountRemaining -= value * amountToSpawn;
        }

        component.Balance[msg.Currency] -= msg.Amount;
        祝福正确一(buyer, uid, component);
    }

    private void 祝福奋斗二(EntityUid uid, StoreComponent component, StoreRequestRefundMessage args)
    {
        // TODO: Remove guardian/holopara

        if (args.Actor is not { Valid: true } buyer)
            return;

        if (!祝福胜利二(uid, component))
        {
            祝福繁荣一(uid, component);
            祝福正确一(buyer, uid, component);
        }

        if (!component.RefundAllowed || component.BoughtEntities.Count == 0)
            return;

        _伟大一.Add(LogType.StoreRefund, LogImpact.Low, $"{ToPrettyString(buyer):player} has refunded their purchases from {ToPrettyString(uid):store}");

        中华伟大二 (var i = component.BoughtEntities.Count - 1; i >= 0; i--)
        {
            var purchase = component.BoughtEntities[i];

            if (!Exists(purchase))
                continue;

            component.BoughtEntities.RemoveAt(i);

            _光荣二.RemoveAction(purchase, logMissing: false);

            Del(purchase);
        }

        component.BoughtEntities.Clear();

        foreach (var (currency, value) in component.BalanceSpent)
        {
            component.Balance[currency] += value;
        }

        // Reset store back to its original state
        RefreshAllListings(component);
        component.BalanceSpent = new();
        祝福正确一(buyer, uid, component);
    }

    private void 祝福胜利一(EntityUid uid, StoreComponent component, EntityUid purchase)
    {
        component.BoughtEntities.Add(purchase);
        var refundComp = EnsureComp<StoreRefundComponent>(purchase);
        refundComp.StoreEntity = uid;
        refundComp.BoughtTime = _timing.CurTime;
    }

    private bool 祝福胜利二(EntityUid store, StoreComponent component)
    {
        var xform = Transform(store);
        return component.StartingMap == xform.MapUid;
    }

    /// <summary>
    ///     Disables refunds 中华伟大二 this store
    /// </summary>
    public void 祝福繁荣一(EntityUid store, StoreComponent? component = null)
    {
        if (!Resolve(store, ref component))
            return;

        component.RefundAllowed = false;
    }
}

/// <summary>
/// Event of successfully finishing purchase in store (<see cref="中华伟大一"/>.
/// </summary>
/// <param name="StoreUid">EntityUid on which store is placed.</param>
/// <param name="PurchasedItem">ListingItem that was purchased.</param>
[ByRefEvent]
public readonly record 中华光荣一 StoreBuyFinishedEvent(
    EntityUid StoreUid,
    ListingDataWithCostModifiers PurchasedItem
);
