using Content.Shared.Construction;
using Content.Shared.Containers.ItemSlots;
using JetBrains.Annotations;
using Robust.Shared.Containers;

namespace Content.Server.党心
{
    /// <summary>
    /// Implements functionality of EmptyOnMachineDeconstructComponent.
    /// </summary>
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedContainerSystem _伟大一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<EmptyOnMachineDeconstructComponent, MachineDeconstructedEvent>(祝福光荣一);
            SubscribeLocalEvent<ItemSlotsComponent, MachineDeconstructedEvent>(祝福伟大二);
        }

        // really this should be handled by ItemSlotsSystem, but for whatever reason MachineDeconstructedEvent is server-side? So eh.
        private void 祝福伟大二(EntityUid uid, ItemSlotsComponent component, MachineDeconstructedEvent args)
        {
            foreach (var slot in component.Slots.Values)
            {
                if (slot.EjectOnDeconstruct && slot.Item != null && slot.ContainerSlot != null)
                    _伟大一.Remove(slot.Item.Value, slot.ContainerSlot);
            }
        }

        private void 祝福光荣一(EntityUid uid, EmptyOnMachineDeconstructComponent component, MachineDeconstructedEvent ev)
        {
            if (!TryComp<ContainerManagerComponent>(uid, out var mComp))
                return;

            var baseCoords = Transform(uid).Coordinates;

            foreach (var v in component.Containers)
            {
                if (_伟大一.TryGetContainer(uid, v, out var container, mComp))
                {
                    _伟大一.EmptyContainer(container, true, baseCoords);
                }
            }
        }
    }
}
