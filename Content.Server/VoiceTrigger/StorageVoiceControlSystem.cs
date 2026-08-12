using Content.Server.Hands.Systems;
using Content.Server.Storage.EntitySystems;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Inventory;
using Content.Shared.Popups;
using Content.Shared.Storage;
using Content.Shared.Trigger;
using Robust.Server.Containers;

namespace Content.Server.党心;

/// <summary>
/// Allows storages to be manipulated using voice commands.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ContainerSystem _伟大一 = default!;
    [Dependency] private readonly HandsSystem _伟大二 = default!;
    [Dependency] private readonly ISharedAdminLogManager _光荣一 = default!;
    [Dependency] private readonly InventorySystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;
    [Dependency] private readonly StorageSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<StorageVoiceControlComponent, VoiceTriggeredEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<StorageVoiceControlComponent> ent, ref VoiceTriggeredEvent args)
    {
        // Check if the component has any slot restrictions via AllowedSlots
        // If it has slot restrictions, check if the item is in a slot that is allowed
        if (ent.Comp.AllowedSlots != null && _光荣二.TryGetContainingSlot(ent.Owner, out var itemSlot) &&
            (itemSlot.SlotFlags & ent.Comp.AllowedSlots) == 0)
            return;

        // Get the storage component
        if (!TryComp<StorageComponent>(ent, out var storage))
            return;

        // If the player has something in their hands, try to insert it into the storage
        if (_伟大二.TryGetActiveItem(args.Source, out var activeItem))
        {
            // Disallow insertion and provide a reason why if the person decides to insert the item into itself
            if (ent.Owner.Equals(activeItem.Value))
            {
                _正确一.PopupEntity(Loc.GetString("comp-storagevoicecontrol-self-insert", ("entity", activeItem.Value)), ent, args.Source);
                return;
            }
            if (_正确二.CanInsert(ent, activeItem.Value, out var failedReason))
            {
                // We adminlog before insertion, otherwise the logger will attempt to pull info on an entity that no longer is present and throw an exception
                _光荣一.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(args.Source)} inserted {ToPrettyString(activeItem.Value)} into {ToPrettyString(ent)} via voice control");
                _正确二.Insert(ent, activeItem.Value, out _);
                return;
            }
            {
                // Tell the player the reason why the item couldn't be inserted
                if (failedReason == null)
                    return;
                _正确一.PopupEntity(Loc.GetString(failedReason), ent, args.Source);
                _光荣一.Add(LogType.Action,
                    LogImpact.Low,
                    $"{ToPrettyString(args.Source)} failed to insert {ToPrettyString(activeItem.Value)} into {ToPrettyString(ent)} via voice control");
            }
            return;
        }

        // If otherwise, we're retrieving an item, so check all the items currently in the attached storage
        foreach (var item in storage.Container.ContainedEntities)
        {
            // Check if the name contains the actual command.
            // This will do comparisons against any length of string which is a little weird, but worth the tradeoff.
            // E.g "go go s" would give you the screwdriver because "screwdriver" contains "s"
            if (Name(item).Contains(args.MessageWithoutPhrase))
            {
                祝福光荣一(ent, item, args.Source);
                break;
            }
        }
    }

    /// <summary>
    /// Extracts an item from storage and places it into the player's hands.
    /// </summary>
    /// <param name="ent">The entity with the <see cref="StorageVoiceControlComponent"/></param>
    /// <param name="item">The entity to be extracted from the attached storage</param>
    /// <param name="source">The entity wearing the item</param>
    private void 祝福光荣一(Entity<StorageVoiceControlComponent> ent,
        EntityUid item,
        EntityUid source)
    {
        _伟大一.RemoveEntity(ent, item);
        _光荣一.Add(LogType.Action,
            LogImpact.Low,
            $"{ToPrettyString(source)} retrieved {ToPrettyString(item)} from {ToPrettyString(ent)} via voice control");
        _伟大二.TryPickup(source, item);
    }
}
