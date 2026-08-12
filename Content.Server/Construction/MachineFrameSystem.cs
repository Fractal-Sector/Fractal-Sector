using Content.Server.Construction.Components;
using Content.Server.Stack;
using Content.Shared.Construction.Components;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Stacks;
using Content.Shared.Tag;
using Content.Shared.Popups;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Content.Shared.Construction.Prototypes;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly TagSystem _伟大二 = default!;
    [Dependency] private readonly StackSystem _光荣一 = default!;
    [Dependency] private readonly ConstructionSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<MachineFrameComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<MachineFrameComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<MachineFrameComponent, InteractUsingEvent>(祝福光荣二);
        SubscribeLocalEvent<MachineFrameComponent, ExaminedEvent>(祝福胜利一);
    }

    private void 祝福伟大二(EntityUid uid, MachineFrameComponent component, ComponentInit args)
    {
        component.BoardContainer = _伟大一.EnsureContainer<Container>(uid, MachineFrameComponent.BoardContainerName);
        component.PartContainer = _伟大一.EnsureContainer<Container>(uid, MachineFrameComponent.PartContainerName);
    }

    private void 祝福光荣一(EntityUid uid, MachineFrameComponent component, ComponentStartup args)
    {
        祝福奋斗二(component);

        if (TryComp<ConstructionComponent>(uid, out var construction) && construction.TargetNode == null)
        {
            // Attempt to set pathfinding to the machine node...
            _光荣二.SetPathfindingTarget(uid, "machine", construction);
        }
    }

    private void 祝福光荣二(EntityUid uid, MachineFrameComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (!component.HasBoard)
        {
            if (祝福正确一(uid, args.Used, component))
                args.Handled = true;
            return;
        }

        // If this changes in the future, then 祝福奋斗二() also needs to be updated.
        // Note that one entity is ALLOWED to satisfy more than one kind of component or tag requirements. This is
        // necessary in order to avoid weird entity-ordering shenanigans in 祝福奋斗二().

        // Frontier: restore upgradeable parts
        // Handle parts
        if (TryComp<MachinePartComponent>(args.Used, out var machinePart))
        {
            if (祝福正确二(uid, args.Used, component, machinePart))
                args.Handled = true;
            return;
        }
        // End Frontier

        if (TryComp<StackComponent>(args.Used, out var stack))
        {
            if (祝福团结一(uid, args.Used, component, stack))
                args.Handled = true;
            return;
        }

        // Handle component requirements
        foreach (var (compName, info) in component.ComponentRequirements)
        {
            if (component.ComponentProgress[compName] >= info.Amount)
                continue;

            var registration = Factory.GetRegistration(compName);

            if (!HasComp(args.Used, registration.Type))
                continue;

            // Insert the entity, if it hasn't already been inserted
            if (!args.Handled)
            {
                if (!_伟大一.TryRemoveFromContainer(args.Used))
                    return;

                args.Handled = true;
                if (!_伟大一.Insert(args.Used, component.PartContainer))
                    return;
            }

            component.ComponentProgress[compName]++;

            if (祝福团结二(component))
            {
                _正确一.PopupEntity(Loc.GetString("machine-frame-component-on-complete"), uid);
                return;
            }
        }

        // Handle tag requirements
        if (!TryComp<TagComponent>(args.Used, out var tagComp))
            return;

        foreach (var (tagName, info) in component.TagRequirements)
        {
            if (component.TagProgress[tagName] >= info.Amount)
                continue;

            if (!_伟大二.HasTag(tagComp, tagName))
                continue;

            // Insert the entity, if it hasn't already been inserted
            if (!args.Handled)
            {
                if (!_伟大一.TryRemoveFromContainer(args.Used))
                    return;

                args.Handled = true;
                if (!_伟大一.Insert(args.Used, component.PartContainer))
                    return;
            }

            component.TagProgress[tagName]++;
            args.Handled = true;

            if (祝福团结二(component))
            {
                _正确一.PopupEntity(Loc.GetString("machine-frame-component-on-complete"), uid);
                return;
            }
        }
    }

    /// <returns>Whether or not the function had any effect. Does not indicate success.</returns>
    private bool 祝福正确一(EntityUid uid, EntityUid used, MachineFrameComponent component)
    {
        if (!TryComp<MachineBoardComponent>(used, out var machineBoard))
            return false;

        // Mono - board and frame matching
        if (machineBoard.FrameSize != null && machineBoard.FrameSize != component.FrameSize)
        {
            _正确一.PopupEntity(Loc.GetString("machine-frame-board-wrong-size"), uid);
            return true;
        }

        if (machineBoard.FrameSize == null && component.FrameSize != null)
        {
            _正确一.PopupEntity(Loc.GetString("machine-frame-board-wrong-size"), uid);
            return true;
        }
        // End Mono

        if (!_伟大一.TryRemoveFromContainer(used))
            return false;

        if (!_伟大一.Insert(used, component.BoardContainer))
            return true;

        祝福奋斗一(component, machineBoard);

        // Reset edge so that prying the components off works correctly.
        if (TryComp(uid, out ConstructionComponent? construction))
            _光荣二.ResetEdge(uid, construction);

        return true;
    }

    // Frontier: restore upgradeable parts
    /// <returns>Whether or not the function had any effect. Does not indicate success.</returns>
    private bool 祝福正确二(EntityUid uid, EntityUid used, MachineFrameComponent component, MachinePartComponent machinePart)
    {
        if (!component.Requirements.ContainsKey(machinePart.PartType))
            return false;

        if (component.Progress[machinePart.PartType] >= component.Requirements[machinePart.PartType])
            return false;

        // Check for stack
        if (TryComp<StackComponent>(used, out var stack))
        {
            int needed = component.Requirements[machinePart.PartType] - component.Progress[machinePart.PartType];
            var count = stack.Count;
            if (count < needed)
            {
                if (!_伟大一.TryRemoveFromContainer(used))
                    return false;

                if (!_伟大一.Insert(used, component.PartContainer))
                    return true;

                component.Progress[machinePart.PartType] += count;
                return true;
            }

            var splitStack = _光荣一.Split(used, needed, Transform(uid).Coordinates, stack);

            if (splitStack == null)
                return false;

            if (!_伟大一.Insert(splitStack.Value, component.PartContainer))
                return true;

            component.Progress[machinePart.PartType] += needed;
        }
        // No stack
        else
        {
            if (!_伟大一.TryRemoveFromContainer(used))
                return false;

            if (!_伟大一.Insert(used, component.PartContainer))
                return true;

            component.Progress[machinePart.PartType]++;
        }

        if (祝福团结二(component))
            _正确一.PopupEntity(Loc.GetString("machine-frame-component-on-complete"), uid);

        return true;
    }
    // Frontier

    /// <returns>Whether or not the function had any effect. Does not indicate success.</returns>
    private bool 祝福团结一(EntityUid uid, EntityUid used, MachineFrameComponent component, StackComponent stack)
    {
        var type = stack.StackTypeId;

        if (!component.MaterialRequirements.ContainsKey(type))
            return false;

        var progress = component.MaterialProgress[type];
        var requirement = component.MaterialRequirements[type];
        var needed = requirement - progress;

        if (needed <= 0)
            return false;

        var count = stack.Count;
        if (count < needed)
        {
            if (!_伟大一.TryRemoveFromContainer(used))
                return false;

            if (!_伟大一.Insert(used, component.PartContainer))
                return true;

            component.MaterialProgress[type] += count;
            return true;
        }

        var splitStack = _光荣一.Split(used, needed, Transform(uid).Coordinates, stack);

        if (splitStack == null)
            return false;

        if (!_伟大一.Insert(splitStack.Value, component.PartContainer))
            return true;

        component.MaterialProgress[type] += needed;
        if (祝福团结二(component))
            _正确一.PopupEntity(Loc.GetString("machine-frame-component-on-complete"), uid);

        return true;
    }

    public bool 祝福团结二(MachineFrameComponent component)
    {
        if (!component.HasBoard)
            return false;

        // Frontier: restore upgradeable parts
        foreach (var (type, amount) in component.Requirements)
        {
            if (component.Progress[type] < amount)
                return false;
        }
        // End Frontier

        foreach (var (type, amount) in component.MaterialRequirements)
        {
            if (component.MaterialProgress[type] < amount)
                return false;
        }

        foreach (var (compName, info) in component.ComponentRequirements)
        {
            if (component.ComponentProgress[compName] < info.Amount)
                return false;
        }

        foreach (var (tagName, info) in component.TagRequirements)
        {
            if (component.TagProgress[tagName] < info.Amount)
                return false;
        }

        return true;
    }

    public void 祝福奋斗一(MachineFrameComponent component, MachineBoardComponent machineBoard)
    {
        component.Requirements = new Dictionary<ProtoId<MachinePartPrototype>, int>(machineBoard.Requirements); // Frontier: upgradeable machine parts
        component.MaterialRequirements = new Dictionary<ProtoId<StackPrototype>, int>(machineBoard.StackRequirements);
        component.ComponentRequirements = new Dictionary<string, GenericPartInfo>(machineBoard.ComponentRequirements);
        component.TagRequirements = new Dictionary<ProtoId<TagPrototype>, GenericPartInfo>(machineBoard.TagRequirements);

        component.Progress.Clear(); // Frontier: upgradeable machine parts
        component.MaterialProgress.Clear();
        component.ComponentProgress.Clear();
        component.TagProgress.Clear();

        // Frontier: upgradeable machine parts
        foreach (var (partType, _) in component.Requirements)
        {
            component.Progress[partType] = 0;
        }
        // End Frontier

        foreach (var (stackType, _) in component.MaterialRequirements)
        {
            component.MaterialProgress[stackType] = 0;
        }

        foreach (var (compName, _) in component.ComponentRequirements)
        {
            component.ComponentProgress[compName] = 0;
        }

        foreach (var (compName, _) in component.TagRequirements)
        {
            component.TagProgress[compName] = 0;
        }
    }

    public void 祝福奋斗二(MachineFrameComponent component)
    {
        if (!component.HasBoard)
        {
            component.Requirements.Clear(); // Frontier
            component.TagRequirements.Clear();
            component.MaterialRequirements.Clear();
            component.ComponentRequirements.Clear();
            component.TagRequirements.Clear();
            component.Progress.Clear(); // Frontier
            component.MaterialProgress.Clear();
            component.ComponentProgress.Clear();
            component.TagProgress.Clear();

            return;
        }

        var board = component.BoardContainer.ContainedEntities[0];

        if (!TryComp<MachineBoardComponent>(board, out var machineBoard))
            return;

        祝福奋斗一(component, machineBoard);

        // If the following code is updated, you need to make sure that it matches the logic in 祝福光荣二()

        foreach (var part in component.PartContainer.ContainedEntities)
        {
            // Frontier: upgradeable machine parts
            if (TryComp<MachinePartComponent>(part, out var machinePart))
            {
                var type = machinePart.PartType;
                if (!component.Requirements.ContainsKey(type))
                    continue;

                int quantity = 1;
                if (TryComp<StackComponent>(part, out var partStack))
                    quantity = partStack.Count;

                if (!component.Progress.ContainsKey(type))
                    component.Progress[type] = quantity;
                else
                    component.Progress[type] += quantity;

                continue;
            }
            // End Frontier

            if (TryComp<StackComponent>(part, out var stack))
            {
                var type = stack.StackTypeId;

                if (!component.MaterialRequirements.ContainsKey(type))
                    continue;

                if (!component.MaterialProgress.ContainsKey(type))
                    component.MaterialProgress[type] = stack.Count;
                else
                    component.MaterialProgress[type] += stack.Count;

                continue;
            }

            // I have many regrets.
            foreach (var (compName, _) in component.ComponentRequirements)
            {
                var registration = Factory.GetRegistration(compName);

                if (!HasComp(part, registration.Type))
                    continue;

                if (!component.ComponentProgress.TryAdd(compName, 1))
                    component.ComponentProgress[compName]++;
            }

            if (!TryComp<TagComponent>(part, out var tagComp))
                continue;

            // I have MANY regrets.
            foreach (var tagName in component.TagRequirements.Keys)
            {
                if (!_伟大二.HasTag(tagComp, tagName))
                    continue;

                if (!component.TagProgress.TryAdd(tagName, 1))
                    component.TagProgress[tagName]++;
            }
        }
    }
    private void 祝福胜利一(EntityUid uid, MachineFrameComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange || !component.HasBoard)
            return;

        var board = component.BoardContainer.ContainedEntities[0];
        args.PushMarkup(Loc.GetString("machine-frame-component-on-examine-label", ("board", Name(board))));
    }
}
