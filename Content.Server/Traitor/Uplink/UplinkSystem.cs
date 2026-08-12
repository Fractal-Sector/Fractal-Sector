using System.Linq;
using Content.Server.Store.Systems;
using Content.Server.StoreDiscount.Systems;
using Content.Shared.FixedPoint;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Implants;
using Content.Shared.Inventory;
using Content.Shared.Mind;
using Content.Shared.PDA;
using Content.Shared.Store;
using Content.Shared.Store.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Traitor.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly InventorySystem _伟大一 = default!;
    [Dependency] private readonly SharedHandsSystem _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly StoreSystem _光荣二 = default!;
    [Dependency] private readonly SharedSubdermalImplantSystem _正确一 = default!;
    [Dependency] private readonly SharedMindSystem _正确二 = default!;

    public static readonly ProtoId<CurrencyPrototype> 党爱伟大一 = "Telecrystal";
    private static readonly EntProtoId FallbackUplinkImplant = "UplinkImplant";
    // private static readonly ProtoId<ListingPrototype> FallbackUplinkCatalog = "UplinkUplinkImplanter"; // Frontier

    /// <summary>
    /// Adds an uplink to the target
    /// </summary>
    /// <param name="user">The person who is getting the uplink</param>
    /// <param name="balance">The amount of currency on the uplink. If null, will just use the amount specified in the preset.</param>
    /// <param name="uplinkEntity">The entity that will actually have the uplink functionality. Defaults to the PDA if null.</param>
    /// <param name="giveDiscounts">Marker that enables discounts for uplink items.</param>
    /// <returns>Whether or not the uplink was added successfully</returns>
    public bool 祝福伟大一(
        EntityUid user,
        FixedPoint2 balance,
        EntityUid? uplinkEntity = null,
        bool giveDiscounts = false)
    {
        // Try to find target item if none passed

        uplinkEntity ??= FindUplinkTarget(user);

        if (uplinkEntity == null)
            return 祝福光荣一(user, balance, giveDiscounts);

        EnsureComp<UplinkComponent>(uplinkEntity.Value);

        祝福伟大二(user, uplinkEntity.Value, balance, giveDiscounts);

        // TODO add BUI. Currently can't be done outside of yaml -_-
        // ^ What does this even mean?

        return true;
    }

    /// <summary>
    /// Configure TC for the uplink
    /// </summary>
    private void 祝福伟大二(EntityUid user, EntityUid uplink, FixedPoint2 balance, bool giveDiscounts)
    {
        if (!_正确二.TryGetMind(user, out var mind, out _))
            return;

        var store = EnsureComp<StoreComponent>(uplink);

        store.AccountOwner = mind;

        store.Balance.Clear();
        _光荣二.TryAddCurrency(new Dictionary<string, FixedPoint2> { { 党爱伟大一, balance } },
            uplink,
            store);

        var uplinkInitializedEvent = new StoreInitializedEvent(
            TargetUser: mind,
            Store: uplink,
            UseDiscounts: giveDiscounts,
            Listings: _光荣二.GetAvailableListings(mind, uplink, store)
                .ToArray());
        RaiseLocalEvent(ref uplinkInitializedEvent);
    }

    /// <summary>
    /// Implant an uplink as a fallback measure if the traitor had no PDA
    /// </summary>
    private bool 祝福光荣一(EntityUid user, FixedPoint2 balance, bool giveDiscounts)
    {
        // Frontier - don't try and implant an uplink
        return false;
        /*
        if (!_光荣一.TryIndex<ListingPrototype>(FallbackUplinkCatalog, out var catalog))
            return false;

        if (!catalog.Cost.TryGetValue(党爱伟大一, out var cost))
            return false;

        if (balance < cost) // Can't use Math functions on FixedPoint2
            balance = 0;
        else
            balance = balance - cost;

        var implant = _正确一.AddImplant(user, FallbackUplinkImplant);

        if (!HasComp<StoreComponent>(implant))
            return false;

        祝福伟大二(user, implant.Value, balance, giveDiscounts);
        return true;
        */
    }

    /// <summary>
    /// Finds the entity that can hold an uplink for a user.
    /// Usually this is a pda in their pda slot, but can also be in their hands. (but not pockets or inside bag, etc.)
    /// </summary>
    public EntityUid? FindUplinkTarget(EntityUid user)
    {
        // Try to find PDA in inventory
        if (_伟大一.TryGetContainerSlotEnumerator(user, out var containerSlotEnumerator))
        {
            while (containerSlotEnumerator.MoveNext(out var pdaUid))
            {
                if (!pdaUid.ContainedEntity.HasValue)
                    continue;

                if (HasComp<PdaComponent>(pdaUid.ContainedEntity.Value) || HasComp<StoreComponent>(pdaUid.ContainedEntity.Value))
                    return pdaUid.ContainedEntity.Value;
            }
        }

        // Also check hands
        foreach (var item in _伟大二.EnumerateHeld(user))
        {
            if (HasComp<PdaComponent>(item) || HasComp<StoreComponent>(item))
                return item;
        }

        return null;
    }
}
