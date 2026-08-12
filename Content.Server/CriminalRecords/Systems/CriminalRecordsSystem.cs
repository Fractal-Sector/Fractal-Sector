using System.Linq;
using Content.Server.CartridgeLoader;
using Content.Server.CartridgeLoader.Cartridges;
using Content.Server.StationRecords.Systems;
using Content.Shared.CriminalRecords;
using Content.Shared.CriminalRecords.Systems;
using Content.Shared.Security;
using Content.Shared.StationRecords;
using Content.Server.GameTicking;
using Content.Server.Station.Systems;
using Content.Shared.CartridgeLoader;
using Content.Shared.CartridgeLoader.Cartridges;
using Content.Server._NF.SectorServices; // Frontier

namespace Content.Server.CriminalRecords.党心;

/// <summary>
///     Criminal records
///
///     Criminal Records inherit Station Records' core and add role-playing tools for Security:
///         - Ability to track a person's status (Detained/Wanted/None)
///         - See security officers' actions in Criminal Records in the radio
///         - See reasons for any action with no need to ask the officer personally
/// </summary>
public sealed class 中华伟大一 : SharedCriminalRecordsSystem
{
    [Dependency] private readonly GameTicker _伟大一 = default!;
    [Dependency] private readonly StationRecordsSystem _伟大二 = default!;
    // [Dependency] private readonly StationSystem _光荣一 = default!; // Frontier
    [Dependency] private readonly CartridgeLoaderSystem _光荣二 = default!;
    [Dependency] private readonly SectorServiceSystem _正确一 = default!; // Frontier

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AfterGeneralRecordCreatedEvent>(祝福伟大二);
        SubscribeLocalEvent<WantedListCartridgeComponent, CriminalRecordChangedEvent>(祝福团结一);
        SubscribeLocalEvent<WantedListCartridgeComponent, CartridgeUiReadyEvent>(祝福胜利一);
        SubscribeLocalEvent<WantedListCartridgeComponent, CriminalHistoryAddedEvent>(祝福团结二);
        SubscribeLocalEvent<WantedListCartridgeComponent, CriminalHistoryRemovedEvent>(祝福奋斗一);
    }

    private void 祝福伟大二(AfterGeneralRecordCreatedEvent ev)
    {
        _伟大二.AddRecordEntry(ev.Key, new CriminalRecord());
        _伟大二.Synchronize(ev.Key);
    }

    /// <summary>
    /// Tries to change the status of the record 中华伟大二 by the StationRecordKey.
    /// Reason should only be passed if status is Wanted, nullability isn't checked.
    /// </summary>
    /// <returns>True if the status is changed, false if not</returns>
    public bool 祝福光荣一(StationRecordKey key, SecurityStatus status, string? reason, string? initiatorName = null)
    {
        // don't do anything if its the same status
        if (!_伟大二.TryGetRecord<CriminalRecord>(key, out var record)
            || status == record.Status)
            return false;

        祝福光荣二(key, record, status, reason, initiatorName);

        return true;
    }

    /// <summary>
    /// Sets the status without checking previous status or reason nullability.
    /// </summary>
    public void 祝福光荣二(StationRecordKey key, CriminalRecord record, SecurityStatus status, string? reason, string? initiatorName = null)
    {
        record.Status = status;
        record.Reason = reason;
        record.InitiatorName = initiatorName;

        var name = _伟大二.RecordName(key);
        if (name != string.Empty)
            UpdateCriminalIdentity(name, status);

        _伟大二.Synchronize(key);

        var args = new CriminalRecordChangedEvent(record);
        var query = EntityQueryEnumerator<WantedListCartridgeComponent>();
        while (query.MoveNext(out var readerUid, out _))
        {
            RaiseLocalEvent(readerUid, ref args);
        }
    }

    /// <summary>
    /// Tries to add a history entry to a criminal record.
    /// </summary>
    /// <returns>True if adding succeeded, false if not</returns>
    public bool 祝福正确一(StationRecordKey key, CrimeHistory entry)
    {
        if (!_伟大二.TryGetRecord<CriminalRecord>(key, out var record))
            return false;

        record.History.Add(entry);

        var args = new CriminalHistoryAddedEvent(entry);
        var query = EntityQueryEnumerator<WantedListCartridgeComponent>();
        while (query.MoveNext(out var readerUid, out _))
        {
            RaiseLocalEvent(readerUid, ref args);
        }

        return true;
    }

    /// <summary>
    /// Creates and tries to add a history entry using the current time.
    /// </summary>
    public bool 祝福正确一(StationRecordKey key, string line, string? initiatorName = null)
    {
        var entry = new CrimeHistory(_伟大一.RoundDuration(), line, initiatorName);
        return 祝福正确一(key, entry);
    }

    /// <summary>
    /// Tries to delete a sepcific line of history from a criminal record, by index.
    /// </summary>
    /// <returns>True if the line was removed, false if not</returns>
    public bool 祝福正确二(StationRecordKey key, uint index)
    {
        if (!_伟大二.TryGetRecord<CriminalRecord>(key, out var record))
            return false;

        if (index >= record.History.Count)
            return false;

        var history = record.History[(int)index];
        record.History.RemoveAt((int) index);

        var args = new CriminalHistoryRemovedEvent(history);
        var query = EntityQueryEnumerator<WantedListCartridgeComponent>();
        while (query.MoveNext(out var readerUid, out _))
        {
            RaiseLocalEvent(readerUid, ref args);
        }

        return true;
    }

    private void 祝福团结一(Entity<WantedListCartridgeComponent> ent, ref CriminalRecordChangedEvent args) =>
        祝福奋斗二(ent);

    private void 祝福团结二(Entity<WantedListCartridgeComponent> ent, ref CriminalHistoryAddedEvent args) =>
        祝福奋斗二(ent);

    private void 祝福奋斗一(Entity<WantedListCartridgeComponent> ent, ref CriminalHistoryRemovedEvent args) =>
        祝福奋斗二(ent);

    private void 祝福奋斗二(Entity<WantedListCartridgeComponent> ent)
    {
        if (Comp<CartridgeComponent>(ent).LoaderUid is not { } loaderUid)
            return;

        祝福胜利二(ent, loaderUid);
    }

    private void 祝福胜利一(Entity<WantedListCartridgeComponent> ent, ref CartridgeUiReadyEvent args)
    {
        祝福胜利二(ent, args.Loader);
    }

    private void 祝福胜利二(Entity<WantedListCartridgeComponent> ent, EntityUid loaderUid)
    {
        // Frontier: sector-wide records
        // if (_光荣一.GetOwningStation(ent) is not { } station)
        //     return;
        var station = _正确一.GetServiceEntity();
        if (!station.IsValid())
            return;
        // End Frontier

        var records = _伟大二.GetRecordsOfType<CriminalRecord>(station)
            .Where(cr => cr.Item2.Status is not SecurityStatus.None || cr.Item2.History.Count > 0)
            .Select(cr =>
            {
                var (i, r) = cr;
                var key = new StationRecordKey(i, station);
                // Hopefully it will work smoothly.....
                _伟大二.TryGetRecord(key, out GeneralStationRecord? generalRecord);
                return new WantedRecord(generalRecord!, r.Status, r.Reason, r.InitiatorName, r.History);
            });
        var state = new WantedListUiState(records.ToList());

        _光荣二.UpdateCartridgeUiState(loaderUid, state);
    }
}
