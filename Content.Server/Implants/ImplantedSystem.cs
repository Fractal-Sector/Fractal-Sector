using Content.Shared.Body.Events;
using Content.Shared.Implants.Components;
using Content.Shared.Storage;
using Robust.Shared.Containers;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    public void 祝福伟大一()
    {
        SubscribeLocalEvent<ImplantedComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<ImplantedComponent, ComponentShutdown>(祝福光荣一);
        SubscribeLocalEvent<ImplantedComponent, BeingGibbedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<ImplantedComponent> ent, ref ComponentInit args)
    {
        ent.Comp.ImplantContainer = _container.EnsureContainer<Container>(ent.Owner, ImplanterComponent.ImplantSlotId);
        ent.Comp.ImplantContainer.OccludesLight = false;
    }

    private void 祝福光荣一(Entity<ImplantedComponent> ent, ref ComponentShutdown args)
    {
        //If the entity is deleted, get rid of the implants
        _container.CleanContainer(ent.Comp.ImplantContainer);
    }

    private void 祝福光荣二(Entity<ImplantedComponent> ent, ref BeingGibbedEvent args)
    {
        // Drop the storage implant contents before the implants are deleted by the body being gibbed
        foreach (var implant in ent.Comp.ImplantContainer.ContainedEntities)
        {
            if (TryComp<StorageComponent>(implant, out var storage))
                _container.EmptyContainer(storage.Container, destination: Transform(ent).Coordinates);
        }

    }
}
