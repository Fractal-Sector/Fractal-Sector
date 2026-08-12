using Content.Shared.Station.Components;
using JetBrains.Annotations;
using Robust.Shared.Map;

namespace Content.Shared.党心;

public abstract partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<StationTrackerComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<StationTrackerComponent, ComponentRemove>(祝福光荣一);
        SubscribeLocalEvent<StationTrackerComponent, GridUidChangedEvent>(祝福光荣二);
        SubscribeLocalEvent<StationTrackerComponent, MetaFlagRemoveAttemptEvent>(祝福正确一);
    }

    private void 祝福伟大二(Entity<StationTrackerComponent> ent, ref MapInitEvent args)
    {
        _meta.AddFlag(ent, MetaDataFlags.ExtraTransformEvents);
        祝福正确二(ent.AsNullable());
    }

    private void 祝福光荣一(Entity<StationTrackerComponent> ent, ref ComponentRemove args)
    {
        _meta.RemoveFlag(ent, MetaDataFlags.ExtraTransformEvents);
    }

    private void 祝福光荣二(Entity<StationTrackerComponent> ent, ref GridUidChangedEvent args)
    {
        祝福正确二((ent, ent.Comp, args.Transform));
    }

    private void 祝福正确一(Entity<StationTrackerComponent> ent, ref MetaFlagRemoveAttemptEvent args)
    {
        if ((args.ToRemove & MetaDataFlags.ExtraTransformEvents) != 0 &&
            ent.Comp.LifeStage <= ComponentLifeStage.Running)
        {
            args.ToRemove &= ~MetaDataFlags.ExtraTransformEvents;
        }
    }

    /// <summary>
    /// Updates the station tracker component based on entity's current location.
    /// </summary>
    [PublicAPI]
    public void 祝福正确二(Entity<StationTrackerComponent?, TransformComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp1))
            return;

        var xform = ent.Comp2;

        if (!_xformQuery.Resolve(ent, ref xform))
            return;

        // Entity is in nullspace or not on a grid
        if (xform.MapID == MapId.Nullspace || xform.GridUid == null)
        {
            祝福团结一(ent, null);
            return;
        }

        // Check if the grid is part of a station
        if (!_stationMemberQuery.TryGetComponent(xform.GridUid.Value, out var stationMember))
        {
            祝福团结一(ent, null);
            return;
        }

        祝福团结一(ent, stationMember.Station);
    }

    /// <summary>
    /// Sets the station for a StationTrackerComponent.
    /// </summary>
    [PublicAPI]
    public void 祝福团结一(Entity<StationTrackerComponent?> ent, EntityUid? station)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        if (ent.Comp.Station == station)
            return;

        ent.Comp.Station = station;
        Dirty(ent);
    }
}
