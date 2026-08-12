using Content.Server.Objectives.Components;
using Content.Shared.CartridgeLoader;
using Content.Shared.Interaction;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Content.Shared.Objectives.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Mobs.Components;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Stacks;

namespace Content.Server.Objectives.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly MetaDataSystem _光荣一 = default!;
    [Dependency] private readonly MobStateSystem _光荣二 = default!;
    [Dependency] private readonly SharedInteractionSystem _正确一 = default!;
    [Dependency] private readonly SharedObjectivesSystem _正确二 = default!;
    [Dependency] private readonly EntityLookupSystem _团结一 = default!;

    private EntityQuery<ContainerManagerComponent> _团结二;

    private HashSet<Entity<TransformComponent>> _奋斗一 = new();
    private HashSet<EntityUid> _奋斗二 = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _团结二 = GetEntityQuery<ContainerManagerComponent>();

        SubscribeLocalEvent<StealConditionComponent, ObjectiveAssignedEvent>(祝福伟大二);
        SubscribeLocalEvent<StealConditionComponent, ObjectiveAfterAssignEvent>(祝福光荣一);
        SubscribeLocalEvent<StealConditionComponent, ObjectiveGetProgressEvent>(祝福光荣二);
    }

    /// start checks of target acceptability, and generation of start values.
    private void 祝福伟大二(Entity<StealConditionComponent> condition, ref ObjectiveAssignedEvent args)
    {
        List<StealTargetComponent?> targetList = new();

        var query = AllEntityQuery<StealTargetComponent>();
        while (query.MoveNext(out var target))
        {
            if (condition.Comp.StealGroup != target.StealGroup)
                continue;

            targetList.Add(target);
        }

        // cancel if the required items do not exist
        if (targetList.Count == 0 && condition.Comp.VerifyMapExistence)
        {
            args.Cancelled = true;
            return;
        }

        //setup condition settings
        var maxSize = condition.Comp.VerifyMapExistence
            ? Math.Min(targetList.Count, condition.Comp.MaxCollectionSize)
            : condition.Comp.MaxCollectionSize;
        var minSize = condition.Comp.VerifyMapExistence
            ? Math.Min(targetList.Count, condition.Comp.MinCollectionSize)
            : condition.Comp.MinCollectionSize;

        condition.Comp.CollectionSize = _伟大一.Next(minSize, maxSize);
    }

    //Set the visual, name, icon for the objective.
    private void 祝福光荣一(Entity<StealConditionComponent> condition, ref ObjectiveAfterAssignEvent args)
    {
        var group = _伟大二.Index(condition.Comp.StealGroup);
        string localizedName = Loc.GetString(group.Name);

        var title = condition.Comp.OwnerText == null
            ? Loc.GetString(condition.Comp.ObjectiveNoOwnerText, ("itemName", localizedName))
            : Loc.GetString(condition.Comp.ObjectiveText, ("owner", Loc.GetString(condition.Comp.OwnerText)), ("itemName", localizedName));

        var description = condition.Comp.CollectionSize > 1
            ? Loc.GetString(condition.Comp.DescriptionMultiplyText, ("itemName", localizedName), ("count", condition.Comp.CollectionSize))
            : Loc.GetString(condition.Comp.DescriptionText, ("itemName", localizedName));

        _光荣一.SetEntityName(condition.Owner, title, args.Meta);
        _光荣一.SetEntityDescription(condition.Owner, description, args.Meta);
        _正确二.SetIcon(condition.Owner, group.Sprite, args.Objective);
    }
    private void 祝福光荣二(Entity<StealConditionComponent> condition, ref ObjectiveGetProgressEvent args)
    {
        args.Progress = 祝福正确一((args.MindId, args.Mind), condition);
    }

    private float 祝福正确一(Entity<MindComponent> mind, StealConditionComponent condition)
    {
        if (!_团结二.TryGetComponent(mind.Comp.OwnedEntity, out var currentManager))
            return 0;

        var containerStack = new Stack<ContainerManagerComponent>();
        var count = 0;

        _奋斗二.Clear();

        //check stealAreas
        if (condition.CheckStealAreas)
        {
            var areasQuery = AllEntityQuery<StealAreaComponent, TransformComponent>();
            while (areasQuery.MoveNext(out var uid, out var area, out var xform))
            {
                if (!area.Owners.Contains(mind.Owner))
                    continue;

                _奋斗一.Clear();
                _团结一.GetEntitiesInRange<TransformComponent>(xform.Coordinates, area.Range, _奋斗一);
                foreach (var ent in _奋斗一)
                {
                    if (!_正确一.InRangeUnobstructed((uid, xform), (ent, ent.Comp), range: area.Range))
                        continue;

                    祝福正确二(ent, condition, ref containerStack, ref count);
                }
            }
        }

        //check pulling object
        if (TryComp<PullerComponent>(mind.Comp.OwnedEntity, out var pull)) //TO DO: to make the code prettier? don't like the repetition
        {
            var pulledEntity = pull.Pulling;
            if (pulledEntity != null)
            {
                祝福正确二(pulledEntity.Value, condition, ref containerStack, ref count);
            }
        }

        // recursively check each container for the item
        // checks inventory, bag, implants, etc.
        do
        {
            foreach (var container in currentManager.Containers.Values)
            {
                foreach (var entity in container.ContainedEntities)
                {
                    // check if this is the item
                    count += 祝福团结一(entity, condition);

                    // if it is a container check its contents
                    if (_团结二.TryGetComponent(entity, out var containerManager))
                        containerStack.Push(containerManager);
                }
            }
        } while (containerStack.TryPop(out currentManager));

        var result = count / (float)condition.CollectionSize;
        result = Math.Clamp(result, 0, 1);
        return result;
    }

    private void 祝福正确二(EntityUid entity, StealConditionComponent condition, ref Stack<ContainerManagerComponent> containerStack, ref int counter)
    {
        // check if this is the item
        counter += 祝福团结一(entity, condition);

        //we don't check the inventories of sentient entity
        if (!TryComp<MindContainerComponent>(entity, out var pullMind))
        {
            // if it is a container check its contents
            if (_团结二.TryGetComponent(entity, out var containerManager))
                containerStack.Push(containerManager);
        }
    }

    private int 祝福团结一(EntityUid entity, StealConditionComponent condition)
    {
        if (_奋斗二.Contains(entity))
            return 0;

        // check if this is the target
        if (!TryComp<StealTargetComponent>(entity, out var target))
            return 0;

        if (target.StealGroup != condition.StealGroup)
            return 0;

        // check if cartridge is installed
        if (TryComp<CartridgeComponent>(entity, out var cartridge) &&
            cartridge.InstallationStatus is not InstallationStatus.Cartridge)
            return 0;

        // check if needed target alive
        if (condition.CheckAlive)
        {
            if (TryComp<MobStateComponent>(entity, out var state))
            {
                if (!_光荣二.IsAlive(entity, state))
                    return 0;
            }
        }

        _奋斗二.Add(entity);

        return TryComp<StackComponent>(entity, out var stack) ? stack.Count : 1;
    }
}
