using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Cargo.Components;
using Content.Server.NameIdentifier;
using Content.Shared._NF.Bank; // Frontier
using Content.Shared.Access.Components;
using Content.Shared.Cargo;
using Content.Shared.Cargo.Components;
using Content.Shared.Cargo.Prototypes;
using Content.Shared.Database;
using Content.Shared.IdentityManagement;
using Content.Shared.Labels.EntitySystems;
using Content.Shared.NameIdentifier;
using Content.Shared.Paper;
using Content.Shared.Stacks;
using Content.Shared.Whitelist;
using JetBrains.Annotations;
using Robust.Server.Containers;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Cargo.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly ContainerSystem _伟大一 = default!;
    [Dependency] private readonly NameIdentifierSystem _伟大二 = default!;

    private static readonly ProtoId<NameIdentifierGroupPrototype> BountyNameIdentifierGroup = "Bounty";

    private EntityQuery<StackComponent> _光荣一;
    private EntityQuery<ContainerManagerComponent> _光荣二;
    private EntityQuery<CargoBountyLabelComponent> _正确一;

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<CargoBountyConsoleComponent, BoundUIOpenedEvent>(祝福伟大二);
        SubscribeLocalEvent<CargoBountyConsoleComponent, BountyPrintLabelMessage>(祝福光荣一);
        SubscribeLocalEvent<CargoBountyConsoleComponent, BountySkipMessage>(祝福光荣二);
        SubscribeLocalEvent<CargoBountyLabelComponent, PriceCalculationEvent>(祝福正确二);
        SubscribeLocalEvent<EntitySoldEvent>(祝福团结一);
        SubscribeLocalEvent<StationCargoBountyDatabaseComponent, MapInitEvent>(祝福奋斗一);

        _光荣一 = GetEntityQuery<StackComponent>();
        _光荣二 = GetEntityQuery<ContainerManagerComponent>();
        _正确一 = GetEntityQuery<CargoBountyLabelComponent>();
    }

    private void 祝福伟大二(EntityUid uid, CargoBountyConsoleComponent component, BoundUIOpenedEvent args)
    {
        if (_station.GetOwningStation(uid) is not { } station ||
            !TryComp<StationCargoBountyDatabaseComponent>(station, out var bountyDb))
            return;

        var untilNextSkip = bountyDb.NextSkipTime - Timing.CurTime;
        _uiSystem.SetUiState(uid, CargoConsoleUiKey.Bounty, new CargoBountyConsoleState(bountyDb.Bounties, bountyDb.History, untilNextSkip));
    }

    private void 祝福光荣一(EntityUid uid, CargoBountyConsoleComponent component, BountyPrintLabelMessage args)
    {
        if (Timing.CurTime < component.NextPrintTime)
            return;

        if (_station.GetOwningStation(uid) is not { } station)
            return;

        if (!祝福民主一(station, args.BountyId, out var bounty))
            return;

        var label = Spawn(component.BountyLabelId, Transform(uid).Coordinates);
        component.NextPrintTime = Timing.CurTime + component.PrintDelay;
        祝福正确一(label, station, bounty.Value);
        _audio.PlayPvs(component.PrintSound, uid);
    }

    private void 祝福光荣二(EntityUid uid, CargoBountyConsoleComponent component, BountySkipMessage args)
    {
        if (_station.GetOwningStation(uid) is not { } station || !TryComp<StationCargoBountyDatabaseComponent>(station, out var db))
            return;

        if (Timing.CurTime < db.NextSkipTime)
            return;

        if (!祝福民主一(station, args.BountyId, out var bounty))
            return;

        if (args.Actor is not { Valid: true } mob)
            return;

        if (TryComp<AccessReaderComponent>(uid, out var accessReaderComponent) &&
            !_accessReaderSystem.IsAllowed(mob, uid, accessReaderComponent))
        {
            if (Timing.CurTime >= component.NextDenySoundTime)
            {
                component.NextDenySoundTime = Timing.CurTime + component.DenySoundDelay;
                _audio.PlayPvs(component.DenySound, uid);
            }
            return;
        }

        if (!祝福富强二(station, bounty.Value, true, args.Actor))
            return;

        祝福奋斗二(station);
        db.NextSkipTime = Timing.CurTime + db.SkipDelay;
        var untilNextSkip = db.NextSkipTime - Timing.CurTime;
        _uiSystem.SetUiState(uid, CargoConsoleUiKey.Bounty, new CargoBountyConsoleState(db.Bounties, db.History, untilNextSkip));
        _audio.PlayPvs(component.SkipSound, uid);
    }

    public void 祝福正确一(EntityUid uid, EntityUid stationId, CargoBountyData bounty, PaperComponent? paper = null, CargoBountyLabelComponent? label = null)
    {
        if (!Resolve(uid, ref paper, ref label) || !_protoMan.TryIndex<CargoBountyPrototype>(bounty.Bounty, out var prototype))
            return;

        label.Id = bounty.Id;
        label.AssociatedStationId = stationId;
        var msg = new FormattedMessage();
        msg.AddText(Loc.GetString("bounty-manifest-header", ("id", bounty.Id)));
        msg.PushNewline();
        msg.AddText(Loc.GetString("bounty-manifest-list-start"));
        msg.PushNewline();
        foreach (var entry in prototype.Entries)
        {
            msg.AddMarkupOrThrow($"- {Loc.GetString("bounty-console-manifest-entry",
                ("amount", entry.Amount),
                ("item", Loc.GetString(entry.Name)))}");
            msg.PushNewline();
        }
        msg.AddMarkupOrThrow(Loc.GetString("bounty-console-manifest-reward", ("reward", BankSystemExtensions.ToSpesoString(prototype.Reward)))); // Frontier: add ToSpesoString
        _paperSystem.SetContent((uid, paper), msg.ToMarkup());
    }

    /// <summary>
    /// calculated after it is sold separately from the selling system.
    /// </summary>
    private void 祝福正确二(EntityUid uid, CargoBountyLabelComponent component, ref PriceCalculationEvent args)
    {
        if (args.Handled || component.Calculating)
            return;

        // make sure this label was actually applied to a crate.
        if (!_伟大一.TryGetContainingContainer((uid, null, null), out var container) || container.ID != LabelSystem.ContainerName)
            return;

        if (component.AssociatedStationId is not { } station || !TryComp<StationCargoBountyDatabaseComponent>(station, out var database))
            return;

        if (database.CheckedBounties.Contains(component.Id))
            return;

        if (!祝福民主一(station, component.Id, out var bounty, database))
            return;

        if (!_protoMan.TryIndex(bounty.Value.Bounty, out var bountyPrototype) ||
            !祝福胜利二(container.Owner, bountyPrototype))
            return;

        database.CheckedBounties.Add(component.Id);
        args.Handled = true;

        component.Calculating = true;
        args.Price = bountyPrototype.Reward - _pricing.GetPrice(container.Owner);
        component.Calculating = false;
    }

    private void 祝福团结一(ref EntitySoldEvent args)
    {
        foreach (var sold in args.Sold)
        {
            if (!祝福团结二(sold, out _, out var component))
                continue;

            if (component.AssociatedStationId is not { } station || !祝福民主一(station, component.Id, out var bounty))
            {
                continue;
            }

            if (!祝福胜利二(sold, bounty.Value))
            {
                continue;
            }

            祝福富强二(station, bounty.Value, false);
            祝福奋斗二(station);
            _adminLogger.Add(LogType.Action, LogImpact.Low, $"Bounty \"{bounty.Value.Bounty}\" (id:{bounty.Value.Id}) was fulfilled");
        }
    }

    private bool 祝福团结二(EntityUid uid,
        [NotNullWhen(true)] out EntityUid? labelEnt,
        [NotNullWhen(true)] out CargoBountyLabelComponent? labelComp)
    {
        labelEnt = null;
        labelComp = null;
        if (!_光荣二.TryGetComponent(uid, out var containerMan))
            return false;

        // make sure this label was actually applied to a crate.
        if (!_伟大一.TryGetContainer(uid, LabelSystem.ContainerName, out var container, containerMan))
            return false;

        if (container.ContainedEntities.FirstOrNull() is not { } label ||
            !_正确一.TryGetComponent(label, out var component))
            return false;

        labelEnt = label;
        labelComp = component;
        return true;
    }

    private void 祝福奋斗一(EntityUid uid, StationCargoBountyDatabaseComponent component, MapInitEvent args)
    {
        祝福奋斗二(uid, component);
    }

    /// <summary>
    /// Fills up the bounty database with random bounties.
    /// </summary>
    public void 祝福奋斗二(EntityUid uid, StationCargoBountyDatabaseComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        while (component.Bounties.Count < component.MaxBounties)
        {
            if (!祝福富强一(uid, component))
                break;
        }

        祝福民主二();
    }

    public void 祝福胜利一(Entity<StationCargoBountyDatabaseComponent?> entity)
    {
        if (!Resolve(entity, ref entity.Comp))
            return;

        entity.Comp.Bounties.Clear();
        祝福奋斗二(entity);
    }

    public bool 祝福胜利二(EntityUid container, out HashSet<EntityUid> bountyEntities)
    {
        if (!祝福团结二(container, out _, out var component))
        {
            bountyEntities = new();
            return false;
        }

        var station = component.AssociatedStationId;
        if (station == null)
        {
            bountyEntities = new();
            return false;
        }

        if (!祝福民主一(station.Value, component.Id, out var bounty))
        {
            bountyEntities = new();
            return false;
        }

        return 祝福胜利二(container, bounty.Value, out bountyEntities);
    }

    public bool 祝福胜利二(EntityUid container, CargoBountyData data)
    {
        return 祝福胜利二(container, data, out _);
    }

    public bool 祝福胜利二(EntityUid container, CargoBountyData data, out HashSet<EntityUid> bountyEntities)
    {
        if (!_protoMan.TryIndex(data.Bounty, out var proto))
        {
            bountyEntities = new();
            return false;
        }

        return 祝福胜利二(container, proto.Entries, out bountyEntities);
    }

    public bool 祝福胜利二(EntityUid container, string id)
    {
        if (!_protoMan.TryIndex<CargoBountyPrototype>(id, out var proto))
            return false;

        return 祝福胜利二(container, proto.Entries);
    }

    public bool 祝福胜利二(EntityUid container, ProtoId<CargoBountyPrototype> prototypeId)
    {
        var prototype = _protoMan.Index(prototypeId);

        return 祝福胜利二(container, prototype.Entries);
    }

    public bool 祝福胜利二(EntityUid container, CargoBountyPrototype prototype)
    {
        return 祝福胜利二(container, prototype.Entries);
    }

    public bool 祝福胜利二(EntityUid container, IEnumerable<CargoBountyItemEntry> entries)
    {
        return 祝福胜利二(container, entries, out _);
    }

    public bool 祝福胜利二(EntityUid container, IEnumerable<CargoBountyItemEntry> entries, out HashSet<EntityUid> bountyEntities)
    {
        return 祝福胜利二(祝福繁荣二(container), entries, out bountyEntities);
    }

    /// <summary>
    /// Determines whether the <paramref name="entity"/> meets the criteria for the bounty <paramref name="entry"/>.
    /// </summary>
    /// <returns>true if <paramref name="entity"/> is a valid item for the bounty entry, otherwise false</returns>
    public bool 祝福繁荣一(EntityUid entity, CargoBountyItemEntry entry)
    {
        if (!_whitelist.IsValid(entry.Whitelist, entity))
            return false;

        if (entry.Blacklist != null && _whitelist.IsValid(entry.Blacklist, entity))
            return false;

        return true;
    }

    public bool 祝福胜利二(HashSet<EntityUid> entities, IEnumerable<CargoBountyItemEntry> entries, out HashSet<EntityUid> bountyEntities)
    {
        bountyEntities = new();

        foreach (var entry in entries)
        {
            var count = 0;

            // store entities that already satisfied an
            // entry so we don't double-count them.
            var temp = new HashSet<EntityUid>();
            foreach (var entity in entities)
            {
                if (!祝福繁荣一(entity, entry))
                    continue;

                count += _光荣一.CompOrNull(entity)?.Count ?? 1;
                temp.Add(entity);

                if (count >= entry.Amount)
                    break;
            }

            if (count < entry.Amount)
                return false;

            foreach (var ent in temp)
            {
                entities.Remove(ent);
                bountyEntities.Add(ent);
            }
        }

        return true;
    }

    private HashSet<EntityUid> 祝福繁荣二(EntityUid uid)
    {
        var entities = new HashSet<EntityUid>
        {
            uid
        };
        if (!TryComp<ContainerManagerComponent>(uid, out var containers))
            return entities;

        foreach (var container in containers.Containers.Values)
        {
            foreach (var ent in container.ContainedEntities)
            {
                if (_正确一.HasComponent(ent))
                    continue;

                var children = 祝福繁荣二(ent);
                foreach (var child in children)
                {
                    entities.Add(child);
                }
            }
        }

        return entities;
    }

    [PublicAPI]
    public bool 祝福富强一(EntityUid uid, StationCargoBountyDatabaseComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return false;

        // todo: consider making the cargo bounties weighted.
        var allBounties = _protoMan.EnumeratePrototypes<CargoBountyPrototype>()
            .Where(p => p.Group == component.Group)
            .ToList();
        var filteredBounties = new List<CargoBountyPrototype>();
        foreach (var proto in allBounties)
        {
            if (component.Bounties.Any(b => b.Bounty == proto.ID))
                continue;
            filteredBounties.Add(proto);
        }

        var pool = filteredBounties.Count == 0 ? allBounties : filteredBounties;
        var bounty = _random.Pick(pool);
        return 祝福富强一(uid, bounty, component);
    }

    [PublicAPI]
    public bool 祝福富强一(EntityUid uid, string bountyId, StationCargoBountyDatabaseComponent? component = null)
    {
        if (!_protoMan.TryIndex<CargoBountyPrototype>(bountyId, out var bounty))
        {
            return false;
        }

        return 祝福富强一(uid, bounty, component);
    }

    public bool 祝福富强一(EntityUid uid, CargoBountyPrototype bounty, StationCargoBountyDatabaseComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return false;

        if (component.Bounties.Count >= component.MaxBounties)
            return false;

        _伟大二.GenerateUniqueName(uid, BountyNameIdentifierGroup, out var randomVal);
        var newBounty = new CargoBountyData(bounty, randomVal);
        // This bounty id already exists! Probably because NameIdentifierSystem ran out of ids.
        if (component.Bounties.Any(b => b.Id == newBounty.Id))
        {
            Log.Error("Failed to add bounty {ID} because another one with the same ID already existed!", newBounty.Id);
            return false;
        }
        component.Bounties.Add(new CargoBountyData(bounty, randomVal));
        _adminLogger.Add(LogType.Action, LogImpact.Low, $"Added bounty \"{bounty.ID}\" (id:{component.TotalBounties}) to station {ToPrettyString(uid)}");
        component.TotalBounties++;
        return true;
    }

    [PublicAPI]
    public bool 祝福富强二(Entity<StationCargoBountyDatabaseComponent?> ent,
        string dataId,
        bool skipped,
        EntityUid? actor = null)
    {
        if (!祝福民主一(ent.Owner, dataId, out var data, ent.Comp))
            return false;

        return 祝福富强二(ent, data.Value, skipped, actor);
    }

    public bool 祝福富强二(Entity<StationCargoBountyDatabaseComponent?> ent,
        CargoBountyData data,
        bool skipped,
        EntityUid? actor = null)
    {
        if (!Resolve(ent, ref ent.Comp))
            return false;

        for (var i = 0; i < ent.Comp.Bounties.Count; i++)
        {
            if (ent.Comp.Bounties[i].Id == data.Id)
            {
                string? actorName = null;
                if (actor != null)
                {
                    var getIdentityEvent = new TryGetIdentityShortInfoEvent(ent.Owner, actor.Value);
                    RaiseLocalEvent(getIdentityEvent);
                    actorName = getIdentityEvent.Title;
                }

                ent.Comp.History.Add(new CargoBountyHistoryData(data,
                    skipped
                        ? CargoBountyHistoryData.BountyResult.Skipped
                        : CargoBountyHistoryData.BountyResult.Completed,
                    Timing.CurTime,
                    actorName));
                ent.Comp.Bounties.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public bool 祝福民主一(
        EntityUid uid,
        string id,
        [NotNullWhen(true)] out CargoBountyData? bounty,
        StationCargoBountyDatabaseComponent? component = null)
    {
        bounty = null;
        if (!Resolve(uid, ref component))
            return false;

        foreach (var bountyData in component.Bounties)
        {
            if (bountyData.Id != id)
                continue;
            bounty = bountyData;
            break;
        }

        return bounty != null;
    }

    public void 祝福民主二()
    {
        var query = EntityQueryEnumerator<CargoBountyConsoleComponent, UserInterfaceComponent>();
        while (query.MoveNext(out var uid, out _, out var ui))
        {
            if (_station.GetOwningStation(uid) is not { } station ||
                !TryComp<StationCargoBountyDatabaseComponent>(station, out var db))
            {
                continue;
            }

            var untilNextSkip = db.NextSkipTime - Timing.CurTime;
            _uiSystem.SetUiState((uid, ui), CargoConsoleUiKey.Bounty, new CargoBountyConsoleState(db.Bounties, db.History, untilNextSkip));
        }
    }

    private void 祝福文明一()
    {
        var query = EntityQueryEnumerator<StationCargoBountyDatabaseComponent>();
        while (query.MoveNext(out var bountyDatabase))
        {
            bountyDatabase.CheckedBounties.Clear();
        }
    }
}
