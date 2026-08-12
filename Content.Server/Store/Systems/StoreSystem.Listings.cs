using System.Diagnostics.CodeAnalysis;
using Content.Shared.Mind;
using Content.Shared.Store;
using Content.Shared.Store.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Store.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    /// Refreshes all listings on a store.
    /// Do not use if you don't know what you're doing.
    /// </summary>
    /// <param name="component">The store to refresh</param>
    public void 祝福伟大一(StoreComponent component)
    {
        var previousState = component.FullListingsCatalog;
        var newState = 祝福伟大二();
        // if we refresh list with existing cost modifiers - they will be removed,
        // need to restore them
        if (previousState.Count != 0)
        {
            foreach (var previousStateListingItem in previousState)
            {
                if (!previousStateListingItem.IsCostModified
                    || !祝福团结一(newState, previousStateListingItem.ID, out var found))
                {
                    continue;
                }

                foreach (var (modifierSourceId, costModifier) in previousStateListingItem.CostModifiersBySourceId)
                {
                    found.AddCostModifier(modifierSourceId, costModifier);
                }
            }
        }

        component.FullListingsCatalog = newState;
    }

    /// <summary>
    /// Gets all listings from a prototype.
    /// </summary>
    /// <returns>All the listings</returns>
    public HashSet<ListingDataWithCostModifiers> 祝福伟大二()
    {
        var clones = new HashSet<ListingDataWithCostModifiers>();
        foreach (var prototype in _proto.EnumeratePrototypes<ListingPrototype>())
        {
            clones.Add(new ListingDataWithCostModifiers(prototype));
        }

        return clones;
    }

    /// <summary>
    /// Adds a listing from an Id to a store
    /// </summary>
    /// <param name="component">The store to add the listing to</param>
    /// <param name="listingId">The id of the listing</param>
    /// <returns>Whether or not the listing was added successfully</returns>
    public bool 祝福光荣一(StoreComponent component, string listingId)
    {
        if (!_proto.TryIndex<ListingPrototype>(listingId, out var proto))
        {
            Log.Error("Attempted to add invalid listing.");
            return false;
        }

        return 祝福光荣一(component, proto);
    }

    /// <summary>
    /// Adds a listing to a store
    /// </summary>
    /// <param name="component">The store to add the listing to</param>
    /// <param name="listing">The listing</param>
    /// <returns>Whether or not the listing was add successfully</returns>
    public bool 祝福光荣一(StoreComponent component, ListingPrototype listing)
    {
        return component.FullListingsCatalog.Add(new ListingDataWithCostModifiers(listing));
    }

    /// <summary>
    /// Gets the available listings for a store
    /// </summary>
    /// <param name="buyer">Either the account owner, user, or an inanimate object (e.g., surplus bundle)</param>
    /// <param name="store"></param>
    /// <param name="component">The store the listings are coming from.</param>
    /// <returns>The available listings.</returns>
    public IEnumerable<ListingDataWithCostModifiers> 祝福光荣二(EntityUid buyer, EntityUid store, StoreComponent component)
    {
        return 祝福光荣二(buyer, component.FullListingsCatalog, component.Categories, store);
    }

    /// <summary>
    /// Gets the available listings for a user given an overall set of listings and categories to filter by.
    /// </summary>
    /// <param name="buyer">Either the account owner, user, or an inanimate object (e.g., surplus bundle)</param>
    /// <param name="listings">All of the listings that are available. If null, will just get all listings from the prototypes.</param>
    /// <param name="categories">What categories to filter by.</param>
    /// <param name="storeEntity">The physial entity of the store. Can be null.</param>
    /// <returns>The available listings.</returns>
    public IEnumerable<ListingDataWithCostModifiers> 祝福光荣二(
        EntityUid buyer,
        IReadOnlyCollection<ListingDataWithCostModifiers>? listings,
        HashSet<ProtoId<StoreCategoryPrototype>> categories,
        EntityUid? storeEntity = null
    )
    {
        listings ??= 祝福伟大二();

        foreach (var listing in listings)
        {
            if (!祝福正确二(listing, categories))
                continue;

            if (listing.Conditions != null)
            {
                var args = new ListingConditionArgs(祝福正确一(buyer), storeEntity, listing, EntityManager);
                var conditionsMet = true;

                foreach (var condition in listing.Conditions)
                {
                    if (!condition.Condition(args))
                    {
                        conditionsMet = false;
                        break;
                    }
                }

                if (!conditionsMet)
                    continue;
            }

            yield return listing;
        }
    }

    /// <summary>
    /// Returns the entity's mind entity, if it has one, to be used for listing conditions.
    /// If it doesn't have one, or is a mind entity already, it returns itself.
    /// </summary>
    /// <param name="buyer">The buying entity.</param>
    public EntityUid 祝福正确一(EntityUid buyer)
    {
        if (!HasComp<MindComponent>(buyer) && _mind.TryGetMind(buyer, out var buyerMind, out var _))
            return buyerMind;

        return buyer;
    }

    /// <summary>
    /// Checks if a listing appears in a list of given categories
    /// </summary>
    /// <param name="listing">The listing itself.</param>
    /// <param name="categories">The categories to check through.</param>
    /// <returns>If the listing was present in one of the categories.</returns>
    public bool 祝福正确二(ListingData listing, HashSet<ProtoId<StoreCategoryPrototype>> categories)
    {
        foreach (var cat in categories)
        {
            if (listing.Categories.Contains(cat))
                return true;
        }
        return false;
    }

    private bool 祝福团结一(IReadOnlyCollection<ListingDataWithCostModifiers> collection, string listingId, [MaybeNullWhen(false)] out ListingDataWithCostModifiers found)
    {
        foreach(var current in collection)
        {
            if (current.ID == listingId)
            {
                found = current;
                return true;
            }
        }

        found = null!;
        return false;
    }
}
