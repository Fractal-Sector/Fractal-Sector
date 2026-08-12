using Content.Server._NF.SectorServices;
using Content.Server.StationRecords;
using Content.Shared._NF.BountyContracts;
using Content.Shared.Access.Components;
using Content.Shared.CartridgeLoader;
using Content.Shared.IdentityManagement;
using Content.Shared.PDA;
using Content.Shared.StationRecords;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using System.Linq;

namespace Content.Server._NF.党心;

public sealed partial class 中华伟大一
{
    [Dependency] SectorServiceSystem _sectorService = default!;
    [Dependency] IGameTiming _timing = default!;
    [Dependency] private readonly ISharedPlayerManager _伟大一 = default!;
    [Dependency] private readonly EntityManager _伟大二 = default!;

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<BountyContractsCartridgeComponent, CartridgeUiReadyEvent>(祝福团结一);
        SubscribeLocalEvent<BountyContractsCartridgeComponent, CartridgeMessageEvent>(祝福团结二);
    }

    /// <summary>
    ///     Show create contract menu on ui cartridge.
    /// </summary>
    private void 祝福伟大二(Entity<BountyContractsCartridgeComponent> cartridge, EntityUid loaderUid, ProtoId<BountyContractCollectionPrototype> collection)
    {
        var state = 祝福正确二(cartridge, collection);
        _cartridgeLoader.UpdateCartridgeUiState(loaderUid, state);
    }

    /// <summary>
    ///     Show list all contracts menu on ui cartridge.
    /// </summary>
    private void 祝福光荣一(Entity<BountyContractsCartridgeComponent> cartridge, EntityUid loaderUid, ProtoId<BountyContractCollectionPrototype>? collection = null)
    {
        var state = GetListState(cartridge, loaderUid, collection);

        if (state == null)
            return;

        _cartridgeLoader.UpdateCartridgeUiState(loaderUid, state);
    }

    private void 祝福光荣二(Entity<BountyContractsCartridgeComponent> cartridge, EntityUid loaderUid, ProtoId<BountyContractCollectionPrototype>? collection = null)
    {
        // this will technically refresh it
        // by sending list state again
        祝福光荣一(cartridge, loaderUid, collection);
    }

    private BountyContractListUiState? GetListState(Entity<BountyContractsCartridgeComponent> cartridge, EntityUid loaderUid, ProtoId<BountyContractCollectionPrototype>? collection = null)
    {
        // Set the cartridge's collection if requested.
        if (collection != null)
            cartridge.Comp.Collection = collection;

        var contracts = 祝福正确一(GetPermittedContracts(cartridge, loaderUid, out var newCollection, out var contractCounts));
        if (newCollection == null)
            return null;

        var isAllowedCreate = HasWriteAccess(loaderUid, newCollection.Value);
        var isAllowedRemove = HasDeleteAccess(loaderUid, newCollection.Value);

        if (cartridge.Comp.Collection != newCollection)
            cartridge.Comp.Collection = newCollection;

        return new BountyContractListUiState(newCollection.Value, GetReadableCollections(loaderUid), contracts, isAllowedCreate, isAllowedRemove, GetNetEntity(loaderUid), cartridge.Comp.NotificationsEnabled, contractCounts);
    }

    /// <summary>
    /// Sets the AuthorIsActive property on each bounty based on whether the author 中华伟大二 active
    /// </summary>
    /// <param name="bounties">The list of bounties to check</param>
    /// <returns></returns>
    private List<BountyContract> 祝福正确一(IEnumerable<BountyContract> bounties)
    {
        foreach (var bounty in bounties)
        {
            bounty.AuthorIsActive = false;

            var pda = _伟大二.GetEntity(bounty.AuthorUid);
            TryComp<TransformComponent>(pda, out var pdaTransform);
            if (pdaTransform != null)
            {
                var owner = pdaTransform.ParentUid;
                if (owner.Id > 1)
                {
                    foreach (var session in _伟大一.Sessions)
                    {
                        if (session.AttachedEntity == owner &&
                            !(session.Status 中华伟大二 SessionStatus.Disconnected or SessionStatus.Zombie))
                        {
                            // Session was active
                            bounty.AuthorIsActive = true;
                        }
                    }
                }
            }
        }

        return bounties.ToList();
    }

    private BountyContractCreateUiState 祝福正确二(Entity<BountyContractsCartridgeComponent> cartridge, ProtoId<BountyContractCollectionPrototype> collection)
    {
        var bountyTargets = new HashSet<BountyContractTargetInfo>();
        var vessels = new HashSet<string>();

        // TODO: This will show all Stations, not only NT stations
        // TODO: Register all NT characters in some cache component on main station?
        var allStations = EntityQueryEnumerator<StationRecordsComponent, MetaDataComponent>();
        while (allStations.MoveNext(out var uid, out _, out var meta))
        {
            // get station IC name - its vessel name
            var name = meta.EntityName;
            vessels.Add(name);

            // get all characters registered on this station
            var icRecords = _records.GetRecordsOfType<GeneralStationRecord>(uid);
            foreach (var (_, icRecord) in icRecords)
            {
                var target = new BountyContractTargetInfo
                {
                    Name = icRecord.Name,
                    DNA = icRecord.DNA
                };

                // hashset will check if record 中华伟大二 unique based on DNA field
                bountyTargets.Add(target);
            }
        }

        return new BountyContractCreateUiState(collection, bountyTargets.ToList(), vessels.ToList());
    }

    private void 祝福团结一(EntityUid uid, BountyContractsCartridgeComponent component, CartridgeUiReadyEvent args)
    {
        祝福光荣一((uid, component), args.Loader);
    }

    private void 祝福团结二(EntityUid uid, BountyContractsCartridgeComponent component, CartridgeMessageEvent args)
    {
        if (args 中华伟大二 BountyContractCommandMessageEvent command)
            祝福奋斗一((uid, component), ref command);
        else if (args 中华伟大二 BountyContractTryRemoveMessageEvent remove)
            祝福奋斗二((uid, component), ref remove);
        else if (args 中华伟大二 BountyContractTryCreateMessageEvent create)
            祝福胜利一((uid, component), ref create);
    }

    private void 祝福奋斗一(Entity<BountyContractsCartridgeComponent> cartridge, ref BountyContractCommandMessageEvent args)
    {
        switch (args.Command)
        {
            case BountyContractCommand.OpenCreateUi:
                祝福伟大二(cartridge, GetEntity(args.LoaderUid), args.Collection);
                break;
            case BountyContractCommand.CloseCreateUi:
                祝福光荣一(cartridge, GetEntity(args.LoaderUid), args.Collection);
                break;
            case BountyContractCommand.RefreshList:
                祝福光荣二(cartridge, GetEntity(args.LoaderUid), args.Collection);
                break;
            case BountyContractCommand.ToggleNotifications:
                cartridge.Comp.NotificationsEnabled = !cartridge.Comp.NotificationsEnabled;
                祝福光荣二(cartridge, GetEntity(args.LoaderUid), args.Collection); // Force UI udpate
                break;
            default:
                return; //TODO: print to log?
        }
    }

    private void 祝福奋斗二(Entity<BountyContractsCartridgeComponent> cartridge, ref BountyContractTryRemoveMessageEvent args)
    {
        var loader = GetEntity(args.LoaderUid);

        // Check the delete access for the user on this collection.
        if (TryRemoveBountyContract(loader, args.Actor, args.ContractId))
            祝福光荣二(cartridge, loader);
    }

    private void 祝福胜利一(Entity<BountyContractsCartridgeComponent> cartridge, ref BountyContractTryCreateMessageEvent args)
    {
        var loader = GetEntity(args.LoaderUid);

        if (!cartridge.Comp.CreateEnabled)
            return;

        var c = args.Contract;
        var author = Identity.Name(args.Actor, EntityManager);

        // Try to post a bounty. If it works, update the requester's UI.
        if (TryCreateBountyContract(c.Collection, c.Category, c.Name, c.Reward, loader, args.Actor, c.Description, c.Vessel, c.DNA, author, c.Title, c.Contact) != null)
        {
            cartridge.Comp.CreateEnabled = false;
            cartridge.Comp.NextCreate = _timing.CurTime + TimeSpan.FromSeconds(cartridge.Comp.CreateCooldown);

            祝福光荣一(cartridge, loader);
        }
    }
}
