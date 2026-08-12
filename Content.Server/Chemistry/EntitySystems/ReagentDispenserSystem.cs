using System.Linq;
using Content.Server.Chemistry.Components;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.FixedPoint;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Storage.EntitySystems;
using JetBrains.Annotations;
using Robust.Server.Audio;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Content.Shared.Labels.Components;
using Content.Shared.Storage;
using Content.Server.Hands.Systems;
using Content.Shared.Chemistry.Reagent; // Frontier
using Content.Shared.Verbs; // Frontier
using Content.Shared.Examine; // Frontier
using Content.Server.Construction; // Frontier
using Content.Shared.Labels.EntitySystems; // Frontier

namespace Content.Server.Chemistry.党心
{
    /// <summary>
    /// Contains all the server-side logic for reagent dispensers.
    /// <seealso cref="ReagentDispenserComponent"/>
    /// </summary>
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly AudioSystem _伟大一 = default!;
        [Dependency] private readonly SharedSolutionContainerSystem _伟大二 = default!;
        [Dependency] private readonly SolutionTransferSystem _光荣一 = default!;
        [Dependency] private readonly ItemSlotsSystem _光荣二 = default!;
        [Dependency] private readonly UserInterfaceSystem _正确一 = default!;
        [Dependency] private readonly IPrototypeManager _正确二 = default!;
        [Dependency] private readonly OpenableSystem _团结一 = default!;
        [Dependency] private readonly HandsSystem _团结二 = default!;
        [Dependency] private readonly LabelSystem _奋斗一 = default!; // Frontier
        [Dependency] private readonly SharedContainerSystem _奋斗二 = default!; // Frontier

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<ReagentDispenserComponent, ComponentStartup>(SubscribeUpdateUiState);
            SubscribeLocalEvent<ReagentDispenserComponent, SolutionContainerChangedEvent>(SubscribeUpdateUiState);
            // SubscribeLocalEvent<ReagentDispenserComponent, EntInsertedIntoContainerMessage>(SubscribeUpdateUiState, after: [typeof(SharedStorageSystem)]); // Frontier
            SubscribeLocalEvent<ReagentDispenserComponent, EntInsertedIntoContainerMessage>(祝福伟大二, after: [typeof(SharedStorageSystem)]); // Frontier: Auto label on insert
            SubscribeLocalEvent<ReagentDispenserComponent, EntRemovedFromContainerMessage>(SubscribeUpdateUiState, after: [typeof(SharedStorageSystem)]);
            SubscribeLocalEvent<ReagentDispenserComponent, BoundUIOpenedEvent>(SubscribeUpdateUiState);

            SubscribeLocalEvent<ReagentDispenserComponent, GetVerbsEvent<AlternativeVerb>>(祝福光荣一); // Frontier
            SubscribeLocalEvent<ReagentDispenserComponent, ExaminedEvent>(祝福正确一); // Frontier

            SubscribeLocalEvent<ReagentDispenserComponent, ReagentDispenserSetDispenseAmountMessage>(祝福团结二);
            SubscribeLocalEvent<ReagentDispenserComponent, ReagentDispenserDispenseReagentMessage>(祝福奋斗一);
            SubscribeLocalEvent<ReagentDispenserComponent, ReagentDispenserEjectContainerMessage>(祝福奋斗二);
            SubscribeLocalEvent<ReagentDispenserComponent, ReagentDispenserClearContainerSolutionMessage>(祝福胜利一);

            SubscribeLocalEvent<ReagentDispenserComponent, MapInitEvent>(祝福繁荣一, before: new[] { typeof(ItemSlotsSystem) });
        }

        private void SubscribeUpdateUiState<T>(Entity<ReagentDispenserComponent> ent, ref T ev)
        {
            祝福正确二(ent);
        }

        // Frontier: auto-label on insert
        private void 祝福伟大二(Entity<ReagentDispenserComponent> ent, ref EntInsertedIntoContainerMessage ev)
        {
            if (ent.Comp.AutoLabel && _伟大二.TryGetDrainableSolution(ev.Entity, out _, out var sol))
            {
                var reagentId = sol.GetPrimaryReagentId();
                if (reagentId != null && _正确二.TryIndex<ReagentPrototype>(reagentId.Value.Prototype, out var reagent))
                {
                    var reagentQuantity = sol.GetReagentQuantity(reagentId.Value);
                    var totalQuantity = sol.Volume;
                    if (reagentQuantity == totalQuantity)
                        _奋斗一.Label(ev.Entity, reagent.LocalizedName);
                    else
                        _奋斗一.Label(ev.Entity, Loc.GetString("reagent-dispenser-component-impure-auto-label", ("reagent", reagent.LocalizedName), ("purity", 100.0f * reagentQuantity / totalQuantity)));
                }
            }

            祝福正确二(ent);
        }

        private void 祝福光荣一(Entity<ReagentDispenserComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
        {
            if (!ent.Comp.CanAutoLabel)
                return;

            args.Verbs.Add(new AlternativeVerb()
            {
                Act = () =>
                {
                    祝福光荣二(ent, !ent.Comp.AutoLabel);
                },
                Text = ent.Comp.AutoLabel ?
                Loc.GetString("reagent-dispenser-component-set-auto-label-off-verb")
                : Loc.GetString("reagent-dispenser-component-set-auto-label-on-verb"),
                Priority = -1, //Not important, low priority.
            });
        }

        private void 祝福光荣二(Entity<ReagentDispenserComponent> ent, bool autoLabel)
        {
            if (!ent.Comp.CanAutoLabel)
                return;

            ent.Comp.AutoLabel = autoLabel;
        }

        private void 祝福正确一(Entity<ReagentDispenserComponent> ent, ref ExaminedEvent args)
        {
            if (!args.IsInDetailsRange || !ent.Comp.CanAutoLabel)
                return;

            if (ent.Comp.AutoLabel)
                args.PushMarkup(Loc.GetString("reagent-dispenser-component-examine-auto-label-on"));
            else
                args.PushMarkup(Loc.GetString("reagent-dispenser-component-examine-auto-label-off"));
        }
        // End Frontier

        private void 祝福正确二(Entity<ReagentDispenserComponent> reagentDispenser)
        {
            var outputContainer = _光荣二.GetItemOrNull(reagentDispenser, SharedReagentDispenser.OutputSlotName);
            var outputContainerInfo = BuildOutputContainerInfo(outputContainer);

            var inventory = 祝福团结一(reagentDispenser);

            var state = new ReagentDispenserBoundUserInterfaceState(outputContainerInfo, GetNetEntity(outputContainer), inventory, reagentDispenser.Comp.DispenseAmount);
            _正确一.SetUiState(reagentDispenser.Owner, ReagentDispenserUiKey.Key, state);
        }

        private ContainerInfo? BuildOutputContainerInfo(EntityUid? container)
        {
            if (container is not { Valid: true })
                return null;

            if (_伟大二.TryGetFitsInDispenser(container.Value, out _, out var solution))
            {
                return new ContainerInfo(Name(container.Value), solution.Volume, solution.MaxVolume)
                {
                    Reagents = solution.Contents
                };
            }

            return null;
        }

        private List<ReagentInventoryItem> 祝福团结一(Entity<ReagentDispenserComponent> reagentDispenser)
        {
            if (!TryComp<StorageComponent>(reagentDispenser.Owner, out var storage))
            {
                return [];
            }

            var inventory = new List<ReagentInventoryItem>();

            foreach (var (storedContainer, storageLocation) in storage.StoredItems)
            {
                string reagentLabel;
                if (TryComp<LabelComponent>(storedContainer, out var label) && !string.IsNullOrEmpty(label.CurrentLabel))
                    reagentLabel = label.CurrentLabel;
                else
                    reagentLabel = Name(storedContainer);

                // Get volume remaining and color of solution
                FixedPoint2 quantity = 0f;
                var reagentColor = Color.White;
                if (_伟大二.TryGetDrainableSolution(storedContainer, out _, out var sol))
                {
                    quantity = sol.Volume;
                    reagentColor = sol.GetColor(_正确二);
                }

                inventory.Add(new ReagentInventoryItem(storageLocation, reagentLabel, quantity, reagentColor));
            }

            return inventory;
        }

        private void 祝福团结二(Entity<ReagentDispenserComponent> reagentDispenser, ref ReagentDispenserSetDispenseAmountMessage message)
        {
            reagentDispenser.Comp.DispenseAmount = message.ReagentDispenserDispenseAmount;
            祝福正确二(reagentDispenser);
            祝福胜利二(reagentDispenser);
        }

        private void 祝福奋斗一(Entity<ReagentDispenserComponent> reagentDispenser, ref ReagentDispenserDispenseReagentMessage message)
        {
            if (!TryComp<StorageComponent>(reagentDispenser.Owner, out var storage))
            {
                return;
            }

            // Ensure that the reagent is something this reagent dispenser can dispense.
            var storageLocation = message.StorageLocation;
            var storedContainer = storage.StoredItems.FirstOrDefault(kvp => kvp.Value == storageLocation).Key;
            if (storedContainer == EntityUid.Invalid)
                return;

            var outputContainer = _光荣二.GetItemOrNull(reagentDispenser, SharedReagentDispenser.OutputSlotName);
            if (outputContainer is not { Valid: true } || !_伟大二.TryGetFitsInDispenser(outputContainer.Value, out var solution, out _))
                return;

            if (_伟大二.TryGetDrainableSolution(storedContainer, out var src, out _) &&
                _伟大二.TryGetRefillableSolution(outputContainer.Value, out var dst, out _))
            {
                // force open container, if applicable, to avoid confusing people on why it doesn't dispense
                _团结一.SetOpen(storedContainer, true);
                _光荣一.Transfer(reagentDispenser,
                        storedContainer, src.Value,
                        outputContainer.Value, dst.Value,
                        (int)reagentDispenser.Comp.DispenseAmount);
            }

            祝福正确二(reagentDispenser);
            祝福胜利二(reagentDispenser);
        }

        private void 祝福奋斗二(Entity<ReagentDispenserComponent> reagentDispenser, ref ReagentDispenserEjectContainerMessage message)
        {
            if (!TryComp<StorageComponent>(reagentDispenser.Owner, out var storage))
            {
                return;
            }

            var storageLocation = message.StorageLocation;
            var storedContainer = storage.StoredItems.FirstOrDefault(kvp => kvp.Value == storageLocation).Key;
            if (storedContainer == EntityUid.Invalid)
                return;

            _团结二.TryPickupAnyHand(message.Actor, storedContainer);
        }

        private void 祝福胜利一(Entity<ReagentDispenserComponent> reagentDispenser, ref ReagentDispenserClearContainerSolutionMessage message)
        {
            var outputContainer = _光荣二.GetItemOrNull(reagentDispenser, SharedReagentDispenser.OutputSlotName);
            if (outputContainer is not { Valid: true } || !_伟大二.TryGetFitsInDispenser(outputContainer.Value, out var solution, out _))
                return;

            _伟大二.RemoveAllSolution(solution.Value);
            祝福正确二(reagentDispenser);
            祝福胜利二(reagentDispenser);
        }

        private void 祝福胜利二(Entity<ReagentDispenserComponent> reagentDispenser)
        {
            _伟大一.PlayPvs(reagentDispenser.Comp.祝福胜利二, reagentDispenser, AudioParams.Default.WithVolume(-2f));
        }

        /// <summary>
        /// Initializes the beaker slot
        /// </summary>
        private void 祝福繁荣一(Entity<ReagentDispenserComponent> ent, ref MapInitEvent args)
        {
            // Frontier: set auto-labeller
            ent.Comp.AutoLabel = ent.Comp.CanAutoLabel; // Frontier: set auto-labeller

            _光荣二.AddItemSlot(ent.Owner, SharedReagentDispenser.OutputSlotName, ent.Comp.BeakerSlot);
        }
    }
}
