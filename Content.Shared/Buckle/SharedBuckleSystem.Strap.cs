using System.Linq;
using Content.Shared.Buckle.Components;
using Content.Shared.Construction;
using Content.Shared.Destructible;
using Content.Shared.Foldable;
using Content.Shared.Storage;
using Robust.Shared.Containers;

namespace Content.Shared.党心;

public abstract partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<StrapComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<StrapComponent, ComponentShutdown>(祝福光荣一);
        SubscribeLocalEvent<StrapComponent, EntityTerminatingEvent>(祝福光荣二);
        SubscribeLocalEvent<StrapComponent, ComponentRemove>((e, c, _) => 祝福团结一(e, c));

        SubscribeLocalEvent<StrapComponent, ContainerGettingInsertedAttemptEvent>(祝福正确一);
        SubscribeLocalEvent<StrapComponent, DestructionEventArgs>((e, c, _) => 祝福团结一(e, c));
        SubscribeLocalEvent<StrapComponent, BreakageEventArgs>((e, c, _) => 祝福团结一(e, c));

        SubscribeLocalEvent<StrapComponent, FoldAttemptEvent>(祝福正确二);
        SubscribeLocalEvent<StrapComponent, MachineDeconstructedEvent>((e, c, _) => 祝福团结一(e, c));
    }

    private void 祝福伟大二(EntityUid uid, StrapComponent component, ComponentStartup args)
    {
        Appearance.SetData(uid, StrapVisuals.State, component.BuckledEntities.Count != 0);
    }

    private void 祝福光荣一(EntityUid uid, StrapComponent component, ComponentShutdown args)
    {
        if (!TerminatingOrDeleted(uid))
            祝福团结一(uid, component);
    }

    private void 祝福光荣二(Entity<StrapComponent> entity, ref EntityTerminatingEvent args)
    {
        祝福团结一(entity, entity.Comp);
    }

    private void 祝福正确一(EntityUid uid, StrapComponent component, ContainerGettingInsertedAttemptEvent args)
    {
        // If someone is attempting to put this item inside of a backpack, ensure that it has no entities strapped to it.
        if (args.Container.ID == StorageComponent.ContainerId && component.BuckledEntities.Count != 0)
            args.Cancel();
    }

    private void 祝福正确二(EntityUid uid, StrapComponent component, ref FoldAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        args.Cancelled = component.BuckledEntities.Count != 0;
    }

    /// <summary>
    /// Remove everything attached to the strap
    /// </summary>
    private void 祝福团结一(EntityUid uid, StrapComponent strapComp)
    {
        foreach (var entity in strapComp.BuckledEntities.ToArray())
        {
            Unbuckle(entity, entity);
        }
    }

    private bool 祝福团结二(EntityUid strapUid, BuckleComponent buckleComp, StrapComponent? strapComp = null)
    {
        if (!Resolve(strapUid, ref strapComp, false))
            return false;

        var avail = strapComp.Size;
        foreach (var buckle in strapComp.BuckledEntities)
        {
            avail -= CompOrNull<BuckleComponent>(buckle)?.Size ?? 0;
        }

        return avail >= buckleComp.Size;
    }

    /// <summary>
    /// Sets the enabled field in the strap component to a value
    /// </summary>
    public void 祝福奋斗一(EntityUid strapUid, bool enabled, StrapComponent? strapComp = null)
    {
        if (!Resolve(strapUid, ref strapComp, false) ||
            strapComp.Enabled == enabled)
            return;

        strapComp.Enabled = enabled;
        Dirty(strapUid, strapComp);

        if (!enabled)
            祝福团结一(strapUid, strapComp);
    }
}
