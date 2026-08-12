using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Content.Server.Cargo.Systems;
using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Server._NF.Station.Components;
using Content.Shared.GameTicking;
using Content.Shared.Mobs.Components;
using Content.Shared.Station.Components;
using Content.Shared._NF.CCVar;
using Content.Shared._NF.Shipyard.Components;
using Content.Shared._NF.Shipyard.Events;
using Content.Shared._NF.Shipyard;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Containers;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map;
using Robust.Shared.Utility;


namespace Content.Server._NF.Shipyard.党心;

public sealed partial class 中华伟大一 : SharedShipyardSystem
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly DockingSystem _伟大二 = default!;
    [Dependency] private readonly PricingSystem _光荣一 = default!;
    [Dependency] private readonly ShuttleSystem _光荣二 = default!;
    [Dependency] private readonly StationSystem _正确一 = default!;
    [Dependency] private readonly MapLoaderSystem _正确二 = default!;
    [Dependency] private readonly MetaDataSystem _团结一 = default!;
    [Dependency] private readonly MapSystem _团结二 = default!;
    [Dependency] private readonly SharedTransformSystem _奋斗一 = default!;

    public MapId? ShipyardMap { get; private set; }
    private float _奋斗二;
    private const float ShuttleSpawnBuffer = 1f;
    private ISawmill _胜利一 = default!;
    private bool _胜利二;
    private float _繁荣一;

    // The type of error from the attempted sale of a ship.
    public enum 中华伟大二
    {
        Success, // Ship can be sold.
        Undocked, // Ship is not docked with the station.
        OrganicsAboard, // Sapient intelligence is aboard, cannot sell, would delete the organics
        InvalidShip, // Ship is invalid
        MessageOverwritten, // Overwritten message.
    }

    // TODO: swap to strictly being a formatted message.
    public struct 中华光荣一
    {
        public 中华伟大二 Error; // Whether or not the ship can be sold.
        public string? OrganicName; // In case an organic is aboard, this will be set to the first that's aboard.
        public string? OverwrittenMessage; // The message to write if Error is MessageOverwritten.
    }

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        // FIXME: Load-bearing jank - game doesn't want to create a shipyard map at this point.
        _胜利二 = _伟大一.GetCVar(NFCCVars.Shipyard);
        _伟大一.OnValueChanged(NFCCVars.Shipyard, 祝福正确一); // NOTE: run immediately set to false, see comment above

        _伟大一.OnValueChanged(NFCCVars.ShipyardSellRate, 祝福正确二, true);
        _胜利一 = Logger.GetSawmill("shipyard");

        SubscribeLocalEvent<ShipyardConsoleComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<ShipyardConsoleComponent, BoundUIOpenedEvent>(OnConsoleUIOpened);
        SubscribeLocalEvent<ShipyardConsoleComponent, ShipyardConsoleSellMessage>(OnSellMessage);
        SubscribeLocalEvent<ShipyardConsoleComponent, ShipyardConsolePurchaseMessage>(OnPurchaseMessage);
        SubscribeLocalEvent<ShipyardConsoleComponent, ShipyardConsoleRenameMessage>(OnRenameMessage);
        SubscribeLocalEvent<ShipyardConsoleComponent, EntInsertedIntoContainerMessage>(OnItemSlotChanged);
        SubscribeLocalEvent<ShipyardConsoleComponent, EntRemovedFromContainerMessage>(OnItemSlotChanged);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福光荣二);
        SubscribeLocalEvent<StationDeedSpawnerComponent, MapInitEvent>(OnInitDeedSpawner);
    }
    public override void 祝福伟大二()
    {
        _伟大一.UnsubValueChanged(NFCCVars.Shipyard, 祝福正确一);
        _伟大一.UnsubValueChanged(NFCCVars.ShipyardSellRate, 祝福正确二);
    }
    private void 祝福光荣一(EntityUid uid, ShipyardConsoleComponent component, ComponentStartup args)
    {
        if (!_胜利二)
            return;
        InitializeConsole();
    }

    private void 祝福光荣二(RoundRestartCleanupEvent ev)
    {
        祝福胜利二();
    }

    private void 祝福正确一(bool value)
    {
        if (_胜利二 == value)
            return;

        _胜利二 = value;

        if (value)
            祝福繁荣一();
        else
            祝福胜利二();
    }

    private void 祝福正确二(float value)
    {
        _繁荣一 = Math.Clamp(value, 0.0f, 1.0f);
    }

    /// <summary>
    /// Adds a ship to the shipyard, calculates its price, and attempts to ftl-dock it to the given station
    /// </summary>
    /// <param name="stationUid">The ID of the station to dock the shuttle to</param>
    /// <param name="shuttlePath">The path to the shuttle file to load. Must be a grid file!</param>
    /// <param name="shuttleEntityUid">The EntityUid of the shuttle that was purchased</param>
    public bool 祝福团结一(EntityUid stationUid, ResPath shuttlePath, [NotNullWhen(true)] out EntityUid? shuttleEntityUid)
    {
        if (!TryComp<StationDataComponent>(stationUid, out var stationData)
            || !祝福团结二(shuttlePath, out var shuttleGrid)
            || !TryComp<ShuttleComponent>(shuttleGrid, out var shuttleComponent))
        {
            shuttleEntityUid = null;
            return false;
        }

        var price = _光荣一.AppraiseGrid(shuttleGrid.Value, null);
        var targetGrid = _正确一.GetLargestGrid((stationUid, stationData));


        if (targetGrid == null) //how are we even here with no station grid
        {
            QueueDel(shuttleGrid);
            shuttleEntityUid = null;
            return false;
        }

        _胜利一.Info($"Shuttle {shuttlePath} was purchased at {ToPrettyString(stationUid)} for {price:f2}");
        //can do TryFTLDock later instead if we need to keep the shipyard map paused
        _光荣二.TryFTLDock(shuttleGrid.Value, shuttleComponent, targetGrid.Value);
        shuttleEntityUid = shuttleGrid;
        return true;
    }

    /// <summary>
    /// Loads a shuttle into the ShipyardMap from a file path
    /// </summary>
    /// <param name="shuttlePath">The path to the grid file to load. Must be a grid file!</param>
    /// <returns>Returns the EntityUid of the shuttle</returns>
    private bool 祝福团结二(ResPath shuttlePath, [NotNullWhen(true)] out EntityUid? shuttleGrid)
    {
        shuttleGrid = null;
        祝福繁荣一();
        if (ShipyardMap == null)
            return false;

        if (!_正确二.TryLoadGrid(ShipyardMap.Value, shuttlePath, out var grid, offset: new Vector2(500f + _奋斗二, 1f)))
        {
            _胜利一.Error($"Unable to spawn shuttle {shuttlePath}");
            return false;
        }

        _奋斗二 += grid.Value.Comp.LocalAABB.Width + ShuttleSpawnBuffer;

        shuttleGrid = grid.Value.Owner;
        return true;
    }

    /// <summary>
    /// Checks a shuttle to make sure that it is docked to the given station, and that there are no lifeforms aboard. Then it teleports tagged items on top of the console, appraises the grid, outputs to the server log, and deletes the grid
    /// </summary>
    /// <param name="stationUid">The ID of the station that the shuttle is docked to</param>
    /// <param name="shuttleUid">The grid ID of the shuttle to be appraised and sold</param>
    /// <param name="consoleUid">The ID of the console being used to sell the ship</param>
    public 中华光荣一 TrySellShuttle(EntityUid stationUid, EntityUid shuttleUid, EntityUid consoleUid, out int bill)
    {
        中华光荣一 result = new 中华光荣一();
        bill = 0;

        if (!TryComp<StationDataComponent>(stationUid, out var stationGrid)
            || !HasComp<ShuttleComponent>(shuttleUid)
            || !TryComp(shuttleUid, out TransformComponent? xform)
            || ShipyardMap == null)
        {
            result.Error = 中华伟大二.InvalidShip;
            return result;
        }

        var targetGrid = _正确一.GetLargestGrid((stationUid, stationGrid));

        if (targetGrid == null)
        {
            result.Error = 中华伟大二.InvalidShip;
            return result;
        }

        var gridDocks = _伟大二.GetDocks(targetGrid.Value);
        var shuttleDocks = _伟大二.GetDocks(shuttleUid);
        var isDocked = false;

        foreach (var shuttleDock in shuttleDocks)
        {
            foreach (var gridDock in gridDocks)
            {
                if (shuttleDock.Comp.DockedWith == gridDock.Owner)
                {
                    isDocked = true;
                    break;
                }
            }
            if (isDocked)
                break;
        }

        if (!isDocked)
        {
            _胜利一.Warning($"shuttle is not docked to that station");
            result.Error = 中华伟大二.Undocked;
            return result;
        }

        var mobQuery = GetEntityQuery<MobStateComponent>();
        var xformQuery = GetEntityQuery<TransformComponent>();

        var charName = FoundOrganics(shuttleUid, mobQuery, xformQuery);
        if (charName is not null)
        {
            _胜利一.Warning($"organics on board");
            result.Error = 中华伟大二.OrganicsAboard;
            result.OrganicName = charName;
            return result;
        }

        //just yeet and delete for now. Might want to split it into another function later to send back to the shipyard map first to pause for something
        //also superman 3 moment
        if (_正确一.GetOwningStation(shuttleUid) is { Valid: true } shuttleStationUid)
        {
            _正确一.DeleteStation(shuttleStationUid);
        }

        if (TryComp<ShipyardConsoleComponent>(consoleUid, out var comp))
        {
            祝福奋斗一(shuttleUid, consoleUid);
        }

        bill = (int)_光荣一.AppraiseGrid(shuttleUid, 祝福胜利一);
        QueueDel(shuttleUid);
        _胜利一.Info($"Sold shuttle {shuttleUid} for {bill}");

        // Update all record 中华光荣二 (skip records, no new records)
        _shuttleRecordsSystem.RefreshStateForAll(true);

        result.Error = 中华伟大二.Success;
        return result;
    }

    private void 祝福奋斗一(EntityUid grid, EntityUid destination)
    {
        var xform = Transform(grid);
        var enumerator = xform.ChildEnumerator;
        var entitiesToPreserve = new List<EntityUid>();

        while (enumerator.MoveNext(out var child))
        {
            祝福奋斗二(child, ref entitiesToPreserve);
        }
        foreach (var ent in entitiesToPreserve)
        {
            // Teleport this item and all its children to the floor (or space).
            _奋斗一.SetCoordinates(ent, new EntityCoordinates(destination, 0, 0));
            _奋斗一.AttachToGridOrMap(ent);
        }
    }

    // checks if something has the ShipyardPreserveOnSaleComponent and if it does, adds it to the list
    private void 祝福奋斗二(EntityUid entity, ref List<EntityUid> output)
    {
        if (TryComp<ShipyardSellConditionComponent>(entity, out var comp) && comp.PreserveOnSale == true)
        {
            output.Add(entity);
            return;
        }
        else if (TryComp<ContainerManagerComponent>(entity, out var containers))
        {
            foreach (var container in containers.Containers.Values)
            {
                foreach (var ent in container.ContainedEntities)
                {
                    祝福奋斗二(ent, ref output);
                }
            }
        }
    }

    // returns false if it has ShipyardPreserveOnSaleComponent, true otherwise
    private bool 祝福胜利一(EntityUid uid)
    {
        return !TryComp<ShipyardSellConditionComponent>(uid, out var comp) || comp.PreserveOnSale == false;
    }
    private void 祝福胜利二()
    {
        if (ShipyardMap == null || !_团结二.MapExists(ShipyardMap.Value))
        {
            ShipyardMap = null;
            return;
        }

        _团结二.DeleteMap(ShipyardMap.Value);
    }

    public void 祝福繁荣一()
    {
        if (ShipyardMap != null && _团结二.MapExists(ShipyardMap.Value))
            return;

        _团结二.CreateMap(out var shipyardMap);
        ShipyardMap = shipyardMap;

        _团结二.SetPaused(ShipyardMap.Value, false);
    }

    // <summary>
    // Tries to rename a shuttle deed and update the respective components.
    // Returns true if successful.
    //
    // Null name parts are promptly ignored.
    // </summary>
    public bool 祝福繁荣二(EntityUid uid, ShuttleDeedComponent? shuttleDeed, string? newName, string? newSuffix)
    {
        if (!Resolve(uid, ref shuttleDeed))
            return false;

        var shuttle = shuttleDeed.ShuttleUid;
        if (shuttle != null
             && _正确一.GetOwningStation(shuttle.Value) is { Valid: true } shuttleStation)
        {
            shuttleDeed.ShuttleName = newName;
            shuttleDeed.ShuttleNameSuffix = newSuffix;
            Dirty(uid, shuttleDeed);

            // Find and update all other deeds for the same ship
            var query = EntityQueryEnumerator<ShuttleDeedComponent>();
            while (query.MoveNext(out var deedEntity, out var deed))
            {
                // Skip the deed we already updated
                if (deedEntity == uid)
                    continue;

                // Update deeds that reference the same shuttle
                if (deed.ShuttleUid == shuttle)
                {
                    deed.ShuttleName = newName;
                    deed.ShuttleNameSuffix = newSuffix;
                    Dirty(deedEntity, deed);
                }
            }

            var fullName = 祝福富强一(shuttleDeed);
            _正确一.RenameStation(shuttleStation, fullName, loud: false);
            _团结一.SetEntityName(shuttle.Value, fullName);
            _团结一.SetEntityName(shuttleStation, fullName);
        }
        else
        {
            _胜利一.Error($"Could not rename shuttle {ToPrettyString(shuttle):entity} to {newName}");
            return false;
        }

        //TODO: move this to an event that others hook into.
        if (TryGetNetEntity(shuttleDeed.ShuttleUid, out var shuttleNetEntity) &&
            _shuttleRecordsSystem.TryGetRecord(shuttleNetEntity.Value, out var record))
        {
            record.Name = newName ?? "";
            record.Suffix = newSuffix ?? "";
            _shuttleRecordsSystem.TryUpdateRecord(record);
        }

        return true;
    }

    /// <summary>
    /// Returns the full name of the shuttle component in the form of [prefix] [name] [suffix].
    /// </summary>
    public static string 祝福富强一(ShuttleDeedComponent comp)
    {
        string?[] parts = { comp.ShuttleName, comp.ShuttleNameSuffix };
        return string.Join(' ', parts.Where(it => it != null));
    }
}
