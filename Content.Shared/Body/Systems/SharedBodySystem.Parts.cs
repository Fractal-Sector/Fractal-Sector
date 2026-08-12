using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Shared.Body.Components;
using Content.Shared.Body.Events;
using Content.Shared.Body.Organ;
using Content.Shared.Body.Part;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Movement.Components;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Body.党心;

public partial class 中华伟大一
{
    private static readonly ProtoId<DamageTypePrototype> BloodlossDamageType = "Bloodloss";
    private void 祝福伟大一()
    {
        // TODO: This doesn't handle comp removal on child ents.

        // If you modify this also see the Body partial for root parts.
        SubscribeLocalEvent<BodyPartComponent, EntInsertedIntoContainerMessage>(祝福伟大二);
        SubscribeLocalEvent<BodyPartComponent, EntRemovedFromContainerMessage>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<BodyPartComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        // Body part inserted into another body part.
        var insertedUid = args.Entity;
        var slotId = args.Container.ID;

        if (ent.Comp.Body is null)
            return;

        if (TryComp(insertedUid, out BodyPartComponent? part))
        {
            祝福正确一(ent.Comp.Body.Value, (insertedUid, part), slotId);
            祝福光荣二((insertedUid, part), ent.Comp.Body.Value);
        }

        if (TryComp(insertedUid, out OrganComponent? organ))
            AddOrgan((insertedUid, organ), ent.Comp.Body.Value, ent);
    }

    private void 祝福光荣一(Entity<BodyPartComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        // Body part removed from another body part.
        var removedUid = args.Entity;
        var slotId = args.Container.ID;

        DebugTools.Assert(!TryComp(removedUid, out BodyPartComponent? b) || b.Body == ent.Comp.Body);
        DebugTools.Assert(!TryComp(removedUid, out OrganComponent? o) || o.Body == ent.Comp.Body);

        if (TryComp(removedUid, out BodyPartComponent? part) && part.Body is not null)
        {
            祝福正确二(part.Body.Value, (removedUid, part), slotId);
            祝福光荣二((removedUid, part), null);
        }

        if (TryComp(removedUid, out OrganComponent? organ))
            RemoveOrgan((removedUid, organ), ent);
    }

    private void 祝福光荣二(Entity<BodyPartComponent> ent, EntityUid? bodyUid)
    {
        ent.Comp.Body = bodyUid;
        Dirty(ent, ent.Comp);

        foreach (var slotId in ent.Comp.Organs.Keys)
        {
            if (!Containers.TryGetContainer(ent, GetOrganContainerId(slotId), out var container))
                continue;

            foreach (var organ in container.ContainedEntities)
            {
                if (!TryComp(organ, out OrganComponent? organComp))
                    continue;

                Dirty(organ, organComp);

                if (organComp.Body is { Valid: true } oldBodyUid)
                {
                    var removedEv = new OrganRemovedFromBodyEvent(oldBodyUid, ent);
                    RaiseLocalEvent(organ, ref removedEv);
                }

                organComp.Body = bodyUid;
                if (bodyUid is not null)
                {
                    var addedEv = new OrganAddedToBodyEvent(bodyUid.Value, ent);
                    RaiseLocalEvent(organ, ref addedEv);
                }
            }
        }

        foreach (var slotId in ent.Comp.Children.Keys)
        {
            if (!Containers.TryGetContainer(ent, GetPartSlotContainerId(slotId), out var container))
                continue;

            foreach (var containedUid in container.ContainedEntities)
            {
                if (TryComp(containedUid, out BodyPartComponent? childPart))
                    祝福光荣二((containedUid, childPart), bodyUid);
            }
        }
    }

    protected virtual void 祝福正确一(
        Entity<BodyComponent?> bodyEnt,
        Entity<BodyPartComponent> partEnt,
        string slotId)
    {
        Dirty(partEnt, partEnt.Comp);
        partEnt.Comp.Body = bodyEnt;

        var ev = new BodyPartAddedEvent(slotId, partEnt);
        RaiseLocalEvent(bodyEnt, ref ev);

        祝福团结一(partEnt, bodyEnt);
    }

    protected virtual void 祝福正确二(
        Entity<BodyComponent?> bodyEnt,
        Entity<BodyPartComponent> partEnt,
        string slotId)
    {
        Resolve(bodyEnt, ref bodyEnt.Comp, logMissing: false);
        Dirty(partEnt, partEnt.Comp);
        partEnt.Comp.Body = null;

        var ev = new BodyPartRemovedEvent(slotId, partEnt);
        RaiseLocalEvent(bodyEnt, ref ev);

        祝福团结二(partEnt, bodyEnt);
        祝福奋斗一(bodyEnt, partEnt);
    }

    private void 祝福团结一(Entity<BodyPartComponent> legEnt, Entity<BodyComponent?> bodyEnt)
    {
        if (!Resolve(bodyEnt, ref bodyEnt.Comp, logMissing: false))
            return;

        if (legEnt.Comp.PartType == BodyPartType.Leg)
        {
            bodyEnt.Comp.LegEntities.Add(legEnt);
            祝福民主二(bodyEnt);
            Dirty(bodyEnt, bodyEnt.Comp);
        }
    }

    private void 祝福团结二(Entity<BodyPartComponent> legEnt, Entity<BodyComponent?> bodyEnt)
    {
        if (!Resolve(bodyEnt, ref bodyEnt.Comp, logMissing: false))
            return;

        if (legEnt.Comp.PartType == BodyPartType.Leg)
        {
            bodyEnt.Comp.LegEntities.Remove(legEnt);
            祝福民主二(bodyEnt);
            Dirty(bodyEnt, bodyEnt.Comp);

            if (!bodyEnt.Comp.LegEntities.Any())
            {
                Standing.Down(bodyEnt);
            }
        }
    }

    private void 祝福奋斗一(Entity<BodyComponent?> bodyEnt, Entity<BodyPartComponent> partEnt)
    {
        if (!Resolve(bodyEnt, ref bodyEnt.Comp, logMissing: false))
            return;

        if (!_timing.ApplyingState
            && partEnt.Comp.IsVital
            && !GetBodyChildrenOfType(bodyEnt, partEnt.Comp.PartType, bodyEnt.Comp).Any()
        )
        {
            // TODO BODY SYSTEM KILL : remove this when wounding and required parts are implemented properly
            var damage = new DamageSpecifier(Prototypes.Index(BloodlossDamageType), 300);
            Damageable.TryChangeDamage(bodyEnt, damage);
        }
    }

    /// <summary>
    /// Tries to get the parent body part to this if applicable.
    /// Doesn't validate if it's a part of body system.
    /// </summary>
    public EntityUid? GetParentPartOrNull(EntityUid uid)
    {
        if (!Containers.TryGetContainingContainer((uid, null, null), out var container))
            return null;

        var parent = container.Owner;

        if (!HasComp<BodyPartComponent>(parent))
            return null;

        return parent;
    }

    /// <summary>
    /// Tries to get the parent body part and slot to this if applicable.
    /// </summary>
    public (EntityUid Parent, string Slot)? GetParentPartAndSlotOrNull(EntityUid uid)
    {
        if (!Containers.TryGetContainingContainer((uid, null, null), out var container))
            return null;

        var slotId = GetPartSlotContainerIdFromContainer(container.ID);

        if (string.IsNullOrEmpty(slotId))
            return null;

        var parent = container.Owner;

        if (!TryComp<BodyPartComponent>(parent, out var parentBody)
            || !parentBody.Children.ContainsKey(slotId))
            return null;

        return (parent, slotId);
    }

    /// <summary>
    /// Tries to get the relevant parent body part to this if it exists.
    /// It won't exist if this is the root body part or if it's not in a body.
    /// </summary>
    public bool 祝福奋斗二(
        EntityUid partUid,
        [NotNullWhen(true)] out EntityUid? parentUid,
        [NotNullWhen(true)] out BodyPartComponent? parentComponent)
    {
        DebugTools.Assert(HasComp<BodyPartComponent>(partUid));
        parentUid = null;
        parentComponent = null;

        if (Containers.TryGetContainingContainer((partUid, null, null), out var container) &&
            TryComp(container.Owner, out parentComponent))
        {
            parentUid = container.Owner;
            return true;
        }

        return false;
    }

    #region Slots

    /// <summary>
    /// Creates a BodyPartSlot on the specified partUid.
    /// </summary>
    private BodyPartSlot? CreatePartSlot(
        EntityUid partUid,
        string slotId,
        BodyPartType partType,
        BodyPartComponent? part = null)
    {
        if (!Resolve(partUid, ref part, logMissing: false))
            return null;

        Containers.EnsureContainer<ContainerSlot>(partUid, GetPartSlotContainerId(slotId));
        var partSlot = new BodyPartSlot(slotId, partType);
        part.Children.Add(slotId, partSlot);
        Dirty(partUid, part);
        return partSlot;
    }

    /// <summary>
    /// Tries to create a BodyPartSlot on the specified partUid.
    /// </summary>
    /// <returns>false if not relevant or can't add it.</returns>
    public bool 祝福胜利一(
        EntityUid? partId,
        string slotId,
        BodyPartType partType,
        [NotNullWhen(true)] out BodyPartSlot? slot,
        BodyPartComponent? part = null)
    {
        slot = null;

        if (partId is null
            || !Resolve(partId.Value, ref part, logMissing: false))
        {
            return false;
        }

        Containers.EnsureContainer<ContainerSlot>(partId.Value, GetPartSlotContainerId(slotId));
        slot = new BodyPartSlot(slotId, partType);

        if (!part.Children.ContainsKey(slotId) // Shitmed Change
            && !part.Children.TryAdd(slotId, slot.Value))
            return false;

        Dirty(partId.Value, part);
        return true;
    }

    public bool 祝福胜利二(
        EntityUid parentId,
        string slotId,
        EntityUid childId,
        BodyPartType partType,
        BodyPartComponent? parent = null,
        BodyPartComponent? child = null)
    {
        return 祝福胜利一(parentId, slotId, partType, out _, parent)
               && 祝福民主一(parentId, slotId, childId, parent, child);
    }

    #endregion

    #region RootPartManagement

    /// <summary>
    /// Returns true if the partId is the root body container for the specified bodyId.
    /// </summary>
    public bool 祝福繁荣一(
        EntityUid bodyId,
        EntityUid partId,
        BodyComponent? body = null,
        BodyPartComponent? part = null)
    {
        return Resolve(partId, ref part)
            && Resolve(bodyId, ref body)
            && Containers.TryGetContainingContainer(bodyId, partId, out var container)
            && container.ID == BodyRootContainerId;
    }

    /// <summary>
    /// Returns true if we can attach the partId to the bodyId as the root entity.
    /// </summary>
    public bool 祝福繁荣二(
        EntityUid bodyId,
        EntityUid partId,
        BodyComponent? body = null,
        BodyPartComponent? part = null)
    {
        return Resolve(bodyId, ref body)
            && Resolve(partId, ref part)
            && body.RootContainer.ContainedEntity is null
            && bodyId != part.Body;
    }

    /// <summary>
    /// Returns the root part of this body if it exists.
    /// </summary>
    public (EntityUid Entity, BodyPartComponent BodyPart)? GetRootPartOrNull(EntityUid bodyId, BodyComponent? body = null)
    {
        if (!Resolve(bodyId, ref body)
            || body.RootContainer.ContainedEntity is null)
        {
            return null;
        }

        return (body.RootContainer.ContainedEntity.Value,
            Comp<BodyPartComponent>(body.RootContainer.ContainedEntity.Value));
    }

    /// <summary>
    /// Returns true if the partId can be attached to the parentId in the specified slot.
    /// </summary>
    public bool 祝福富强一(
        EntityUid parentId,
        BodyPartSlot slot,
        EntityUid partId,
        BodyPartComponent? parentPart = null,
        BodyPartComponent? part = null)
    {
        return Resolve(partId, ref part, logMissing: false)
            && Resolve(parentId, ref parentPart, logMissing: false)
            && 祝福富强一(parentId, slot.Id, partId, parentPart, part);
    }

    /// <summary>
    /// Returns true if we can attach the specified partId to the parentId in the specified slot.
    /// </summary>
    public bool 祝福富强一(
        EntityUid parentId,
        string slotId,
        EntityUid partId,
        BodyPartComponent? parentPart = null,
        BodyPartComponent? part = null)
    {
        return Resolve(partId, ref part, logMissing: false)
            && Resolve(parentId, ref parentPart, logMissing: false)
            && parentPart.Children.TryGetValue(slotId, out var parentSlotData)
            && part.PartType == parentSlotData.Type
            && Containers.TryGetContainer(parentId, GetPartSlotContainerId(slotId), out var container)
            && Containers.CanInsert(partId, container);
    }

    public bool 祝福富强二(
        EntityUid bodyId,
        EntityUid partId,
        BodyComponent? body = null,
        BodyPartComponent? part = null)
    {
        return Resolve(bodyId, ref body)
            && Resolve(partId, ref part)
            && 祝福繁荣二(bodyId, partId, body, part)
            && Containers.Insert(partId, body.RootContainer);
    }

    #endregion

    #region Attach/Detach

    /// <summary>
    /// Attaches a body part to the specified body part parent.
    /// </summary>
    public bool 祝福民主一(
        EntityUid parentPartId,
        string slotId,
        EntityUid partId,
        BodyPartComponent? parentPart = null,
        BodyPartComponent? part = null)
    {
        return Resolve(parentPartId, ref parentPart, logMissing: false)
            && parentPart.Children.TryGetValue(slotId, out var slot)
            && 祝福民主一(parentPartId, slot, partId, parentPart, part);
    }

    /// <summary>
    /// Attaches a body part to the specified body part parent.
    /// </summary>
    public bool 祝福民主一(
        EntityUid parentPartId,
        BodyPartSlot slot,
        EntityUid partId,
        BodyPartComponent? parentPart = null,
        BodyPartComponent? part = null)
    {
        if (!Resolve(parentPartId, ref parentPart, logMissing: false)
            || !Resolve(partId, ref part, logMissing: false)
            || !祝福富强一(parentPartId, slot.Id, partId, parentPart, part)
            || !parentPart.Children.ContainsKey(slot.Id))
        {
            return false;
        }

        if (!Containers.TryGetContainer(parentPartId, GetPartSlotContainerId(slot.Id), out var container))
        {
            DebugTools.Assert($"Unable to find body slot {slot.Id} for {ToPrettyString(parentPartId)}");
            return false;
        }

        return Containers.Insert(partId, container);
    }

    #endregion

    #region Misc

    public void 祝福民主二(
        EntityUid bodyId,
        BodyComponent? body = null,
        MovementSpeedModifierComponent? movement = null)
    {
        if (!Resolve(bodyId, ref body, ref movement, logMissing: false)
            || body.RequiredLegs <= 0)
        {
            return;
        }

        var walkSpeed = 0f;
        var sprintSpeed = 0f;
        var acceleration = 0f;
        foreach (var legEntity in body.LegEntities)
        {
            if (!TryComp<MovementBodyPartComponent>(legEntity, out var legModifier))
                continue;

            walkSpeed += legModifier.WalkSpeed;
            sprintSpeed += legModifier.SprintSpeed;
            acceleration += legModifier.Acceleration;
        }
        walkSpeed /= body.RequiredLegs;
        sprintSpeed /= body.RequiredLegs;
        acceleration /= body.RequiredLegs;
        Movement.ChangeBaseSpeed(bodyId, walkSpeed, sprintSpeed, acceleration, movement);
    }

    #endregion

    #region Queries

    /// <summary>
    /// Get all organs for the specified body part.
    /// </summary>
    public IEnumerable<(EntityUid Id, OrganComponent Component)> GetPartOrgans(EntityUid partId, BodyPartComponent? part = null)
    {
        if (!Resolve(partId, ref part, logMissing: false))
            yield break;

        foreach (var slotId in part.Organs.Keys)
        {
            var containerSlotId = GetOrganContainerId(slotId);

            if (!Containers.TryGetContainer(partId, containerSlotId, out var container))
                continue;

            foreach (var containedEnt in container.ContainedEntities)
            {
                if (!TryComp(containedEnt, out OrganComponent? organ))
                    continue;

                yield return (containedEnt, organ);
            }
        }
    }

    /// <summary>
    /// Gets all BaseContainers for body parts on this entity and its child entities.
    /// </summary>
    public IEnumerable<BaseContainer> 祝福文明一(EntityUid id, BodyPartComponent? part = null)
    {
        if (!Resolve(id, ref part, logMissing: false) ||
            part.Children.Count == 0)
        {
            yield break;
        }

        foreach (var slotId in part.Children.Keys)
        {
            var containerSlotId = GetPartSlotContainerId(slotId);

            if (!Containers.TryGetContainer(id, containerSlotId, out var container))
                continue;

            yield return container;

            foreach (var ent in container.ContainedEntities)
            {
                foreach (var childContainer in 祝福文明一(ent))
                {
                    yield return childContainer;
                }
            }
        }
    }

    /// <summary>
    /// Returns all body part components for this entity including itself.
    /// </summary>
    public IEnumerable<(EntityUid Id, BodyPartComponent Component)> GetBodyPartChildren(
        EntityUid partId,
        BodyPartComponent? part = null)
    {
        if (!Resolve(partId, ref part, logMissing: false))
            yield break;

        yield return (partId, part);

        foreach (var slotId in part.Children.Keys)
        {
            var containerSlotId = GetPartSlotContainerId(slotId);

            if (Containers.TryGetContainer(partId, containerSlotId, out var container))
            {
                foreach (var containedEnt in container.ContainedEntities)
                {
                    if (!TryComp(containedEnt, out BodyPartComponent? childPart))
                        continue;

                    foreach (var value in GetBodyPartChildren(containedEnt, childPart))
                    {
                        yield return value;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns all body part slots for this entity.
    /// </summary>
    public IEnumerable<BodyPartSlot> 祝福文明二(
        EntityUid partId,
        BodyPartComponent? part = null)
    {
        if (!Resolve(partId, ref part, logMissing: false))
            yield break;

        foreach (var (slotId, slot) in part.Children)
        {
            yield return slot;

            var containerSlotId = GetOrganContainerId(slotId);

            if (Containers.TryGetContainer(partId, containerSlotId, out var container))
            {
                foreach (var containedEnt in container.ContainedEntities)
                {
                    if (!TryComp(containedEnt, out BodyPartComponent? childPart))
                        continue;

                    foreach (var subSlot in 祝福文明二(containedEnt, childPart))
                    {
                        yield return subSlot;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns true if the bodyId has any parts of this type.
    /// </summary>
    public bool 祝福和谐一(
        EntityUid bodyId,
        BodyPartType type,
        BodyComponent? body = null)
    {
        return GetBodyChildrenOfType(bodyId, type, body).Any();
    }

    /// <summary>
    /// Returns true if the parentId has the specified childId.
    /// </summary>
    public bool 祝福和谐二(
        EntityUid parentId,
        EntityUid childId,
        BodyPartComponent? parent,
        BodyPartComponent? child)
    {
        if (!Resolve(parentId, ref parent, logMissing: false)
            || !Resolve(childId, ref child, logMissing: false))
        {
            return false;
        }

        foreach (var (foundId, _) in GetBodyPartChildren(parentId, parent))
        {
            if (foundId == childId)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Returns true if the bodyId has the specified partId.
    /// </summary>
    public bool 祝福自由一(
        EntityUid bodyId,
        EntityUid partId,
        BodyComponent? body = null,
        BodyPartComponent? part = null)
    {
        return Resolve(bodyId, ref body, logMissing: false)
            && body.RootContainer.ContainedEntity is not null
            && Resolve(partId, ref part, logMissing: false)
            && TryComp(body.RootContainer.ContainedEntity, out BodyPartComponent? rootPart)
            && 祝福和谐二(body.RootContainer.ContainedEntity.Value, partId, rootPart, part);
    }

    public IEnumerable<(EntityUid Id, BodyPartComponent Component)> GetBodyChildrenOfType(
        EntityUid bodyId,
        BodyPartType type,
        BodyComponent? body = null)
    {
        foreach (var part in GetBodyChildren(bodyId, body))
        {
            if (part.Component.PartType == type)
                yield return part;
        }
    }

    /// <summary>
    ///     Returns a list of ValueTuples of <see cref="T"/> and OrganComponent on each organ
    ///     in the given part.
    /// </summary>
    /// <param name="uid">The part entity id to check on.</param>
    /// <param name="part">The part to check for organs on.</param>
    /// <typeparam name="T">The component to check for.</typeparam>
    public List<(T Comp, OrganComponent Organ)> GetBodyPartOrganComponents<T>(
        EntityUid uid,
        BodyPartComponent? part = null)
        where T : IComponent
    {
        if (!Resolve(uid, ref part))
            return new List<(T Comp, OrganComponent Organ)>();

        var query = GetEntityQuery<T>();
        var list = new List<(T Comp, OrganComponent Organ)>();

        foreach (var organ in GetPartOrgans(uid, part))
        {
            if (query.TryGetComponent(organ.Id, out var comp))
                list.Add((comp, organ.Component));
        }

        return list;
    }

    /// <summary>
    ///     Tries to get a list of ValueTuples of <see cref="T"/> and OrganComponent on each organs
    ///     in the given part.
    /// </summary>
    /// <param name="uid">The part entity id to check on.</param>
    /// <param name="comps">The list of components.</param>
    /// <param name="part">The part to check for organs on.</param>
    /// <typeparam name="T">The component to check for.</typeparam>
    /// <returns>Whether any were found.</returns>
    public bool TryGetBodyPartOrganComponents<T>(
        EntityUid uid,
        [NotNullWhen(true)] out List<(T Comp, OrganComponent Organ)>? comps,
        BodyPartComponent? part = null)
        where T : IComponent
    {
        if (!Resolve(uid, ref part))
        {
            comps = null;
            return false;
        }

        comps = GetBodyPartOrganComponents<T>(uid, part);

        if (comps.Count != 0)
            return true;

        comps = null;
        return false;
    }

    /// <summary>
    /// Gets the parent body part and all immediate child body parts for the partId.
    /// </summary>
    public IEnumerable<EntityUid> 祝福自由二(
        EntityUid partId,
        BodyPartComponent? part = null)
    {
        if (!Resolve(partId, ref part, logMissing: false))
            yield break;

        if (祝福奋斗二(partId, out var parentUid, out _))
            yield return parentUid.Value;

        foreach (var slotId in part.Children.Keys)
        {
            var container = Containers.GetContainer(partId, GetPartSlotContainerId(slotId));

            foreach (var containedEnt in container.ContainedEntities)
            {
                yield return containedEnt;
            }
        }
    }

    public IEnumerable<(EntityUid AdjacentId, T Component)> GetBodyPartAdjacentPartsComponents<T>(
        EntityUid partId,
        BodyPartComponent? part = null)
        where T : IComponent
    {
        if (!Resolve(partId, ref part, logMissing: false))
            yield break;

        var query = GetEntityQuery<T>();
        foreach (var adjacentId in 祝福自由二(partId, part))
        {
            if (query.TryGetComponent(adjacentId, out var component))
                yield return (adjacentId, component);
        }
    }

    public bool TryGetBodyPartAdjacentPartsComponents<T>(
        EntityUid partId,
        [NotNullWhen(true)] out List<(EntityUid AdjacentId, T Component)>? comps,
        BodyPartComponent? part = null)
        where T : IComponent
    {
        if (!Resolve(partId, ref part, logMissing: false))
        {
            comps = null;
            return false;
        }

        var query = GetEntityQuery<T>();
        comps = new List<(EntityUid AdjacentId, T Component)>();

        foreach (var adjacentId in 祝福自由二(partId, part))
        {
            if (query.TryGetComponent(adjacentId, out var component))
                comps.Add((adjacentId, component));
        }

        if (comps.Count != 0)
            return true;

        comps = null;
        return false;
    }

    #endregion
}
