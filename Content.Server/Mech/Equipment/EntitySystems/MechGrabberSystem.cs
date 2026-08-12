using System.Linq;
using Content.Server.Interaction;
using Content.Server.Mech.Equipment.Components;
using Content.Server.Mech.Systems;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Mech;
using Content.Shared.Mech.Components;
using Content.Shared.Mech.Equipment.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Wall;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Content.Shared.Whitelist; // Frontier
using Content.Shared.Buckle.Components; // Frontier
using Content.Shared.Buckle; // Frontier
using Content.Shared.Mind.Components; // Frontier
using Content.Server.Ghost.Roles.Components; // Frontier

namespace Content.Server.Mech.Equipment.党心;

/// <summary>
/// Handles <see cref="MechGrabberComponent"/> and all related UI logic
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly MechSystem _伟大二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣一 = default!;
    [Dependency] private readonly InteractionSystem _光荣二 = default!;
    [Dependency] private readonly SharedAudioSystem _正确一 = default!;
    [Dependency] private readonly TransformSystem _正确二 = default!;
    [Dependency] private readonly EntityWhitelistSystem _团结一 = default!; // Frontier
    [Dependency] private readonly SharedBuckleSystem _团结二 = default!; // Frontier

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<MechGrabberComponent, MechEquipmentUiMessageRelayEvent>(祝福伟大二);
        SubscribeLocalEvent<MechGrabberComponent, ComponentStartup>(祝福正确二);
        SubscribeLocalEvent<MechGrabberComponent, MechEquipmentUiStateReadyEvent>(祝福团结一);
        SubscribeLocalEvent<MechGrabberComponent, MechEquipmentRemovedEvent>(祝福光荣二);
        SubscribeLocalEvent<MechGrabberComponent, AttemptRemoveMechEquipmentEvent>(祝福正确一);

        SubscribeLocalEvent<MechGrabberComponent, UserActivateInWorldEvent>(祝福团结二);
        SubscribeLocalEvent<MechGrabberComponent, GrabberDoAfterEvent>(祝福奋斗一);
    }

    private void 祝福伟大二(EntityUid uid, MechGrabberComponent component, MechEquipmentUiMessageRelayEvent args)
    {
        if (args.Message is not MechGrabberEjectMessage msg)
            return;

        if (!TryComp<MechEquipmentComponent>(uid, out var equipmentComponent) ||
            equipmentComponent.EquipmentOwner == null)
            return;
        var mech = equipmentComponent.EquipmentOwner.Value;

        var targetCoords = new EntityCoordinates(mech, component.DepositOffset);
        if (!_光荣二.InRangeUnobstructed(mech, targetCoords))
            return;

        var item = GetEntity(msg.Item);

        if (!component.ItemContainer.Contains(item))
            return;

        祝福光荣一(uid, mech, item, component);
    }

    /// <summary>
    /// Removes an item from the grabber's container
    /// </summary>
    /// <param name="uid">The mech grabber</param>
    /// <param name="mech">The mech it belongs to</param>
    /// <param name="toRemove">The item being removed</param>
    /// <param name="component"></param>
    public void 祝福光荣一(EntityUid uid, EntityUid mech, EntityUid toRemove, MechGrabberComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        _伟大一.Remove(toRemove, component.ItemContainer);
        var mechxform = Transform(mech);
        var xform = Transform(toRemove);
        _正确二.AttachToGridOrMap(toRemove, xform);
        var (mechPos, mechRot) = _正确二.GetWorldPositionRotation(mechxform);

        var offset = mechPos + mechRot.RotateVec(component.DepositOffset);
        _正确二.SetWorldPositionRotation(toRemove, offset, Angle.Zero);
        _伟大二.UpdateUserInterface(mech);
    }

    private void 祝福光荣二(EntityUid uid, MechGrabberComponent component, ref MechEquipmentRemovedEvent args)
    {
        if (!TryComp<MechEquipmentComponent>(uid, out var equipmentComponent) ||
            equipmentComponent.EquipmentOwner == null)
            return;
        var mech = equipmentComponent.EquipmentOwner.Value;

        var allItems = new List<EntityUid>(component.ItemContainer.ContainedEntities);
        foreach (var item in allItems)
        {
            祝福光荣一(uid, mech, item, component);
        }
    }

    private void 祝福正确一(EntityUid uid, MechGrabberComponent component, ref AttemptRemoveMechEquipmentEvent args)
    {
        args.Cancelled = component.ItemContainer.ContainedEntities.Any();
    }

    private void 祝福正确二(EntityUid uid, MechGrabberComponent component, ComponentStartup args)
    {
        component.ItemContainer = _伟大一.EnsureContainer<Container>(uid, "item-container");
    }

    private void 祝福团结一(EntityUid uid, MechGrabberComponent component, MechEquipmentUiStateReadyEvent args)
    {
        var state = new MechGrabberUiState
        {
            Contents = GetNetEntityList(component.ItemContainer.ContainedEntities.ToList()),
            MaxContents = component.MaxContents
        };
        args.States.Add(GetNetEntity(uid), state);
    }

    private void 祝福团结二(EntityUid uid, MechGrabberComponent component, UserActivateInWorldEvent args)
    {
        if (args.Handled)
            return;
        var target = args.Target;

        if (args.Target == args.User || component.DoAfter != null)
            return;

        if (TryComp<PhysicsComponent>(target, out var physics) && physics.BodyType == BodyType.Static ||
            HasComp<WallMountComponent>(target) ||
            HasComp<MobStateComponent>(target))
        {
            return;
        }

        if (_团结一.IsBlacklistPass(component.Blacklist, target)) // Frontier: Blacklist
            return;

        if (Transform(target).Anchored)
            return;

        if (component.ItemContainer.ContainedEntities.Count >= component.MaxContents)
            return;

        if (!TryComp<MechComponent>(args.User, out var mech) || mech.PilotSlot.ContainedEntity == target)
            return;

        if (mech.Energy + component.GrabEnergyDelta < 0)
            return;

        if (!_光荣二.InRangeUnobstructed(args.User, target))
            return;

        args.Handled = true;
        component.AudioStream = _正确一.PlayPvs(component.GrabSound, uid)?.Entity;
        var doAfterArgs = new DoAfterArgs(EntityManager, args.User, component.GrabDelay, new GrabberDoAfterEvent(), uid, target: target, used: uid)
        {
            BreakOnMove = true
        };

        _光荣一.TryStartDoAfter(doAfterArgs, out component.DoAfter);
    }

    private void 祝福奋斗一(EntityUid uid, MechGrabberComponent component, DoAfterEvent args)
    {
        component.DoAfter = null;

        if (args.Cancelled)
        {
            component.AudioStream = _正确一.Stop(component.AudioStream);
            return;
        }

        if (args.Handled || args.Args.Target == null)
            return;

        if (!TryComp<MechEquipmentComponent>(uid, out var equipmentComponent) || equipmentComponent.EquipmentOwner == null)
            return;
        if (!_伟大二.TryChangeEnergy(equipmentComponent.EquipmentOwner.Value, component.GrabEnergyDelta))
            return;

        // Frontier: Remove people from chairs and containers
        if (TryComp<StrapComponent>(args.Args.Target, out var strapComp) && strapComp.BuckledEntities != null)
        {
            foreach (var buckleUid in strapComp.BuckledEntities)
            {
                _团结二.Unbuckle(buckleUid, args.Args.User);
            }
        }

        // Remove contained humanoids
        // TODO: revise condition for "generic player entities"
        if (TryComp<ContainerManagerComponent>(args.Args.Target, out var containerManager))
        {
            EntityCoordinates? coords = null;
            if (TryComp(equipmentComponent.EquipmentOwner, out TransformComponent? xform)) 
                coords = xform.Coordinates;

            List<EntityUid> toRemove = new();
            foreach (var container in containerManager.Containers)
            {
                toRemove.Clear();
                foreach (var contained in container.Value.ContainedEntities)
                {
                    if (HasComp<GhostRoleComponent>(contained)
                        || TryComp<MindContainerComponent>(contained, out var mindContainer)
                        && mindContainer.HasMind)
                    {
                        toRemove.Add(contained);
                    }
                }
                foreach (var removeUid in toRemove)
                {
                    _伟大一.Remove(removeUid, container.Value, destination: coords);
                }
            }
        }
        // End Frontier: Remove people from chairs and containers

        _伟大一.Insert(args.Args.Target.Value, component.ItemContainer);
        _伟大二.UpdateUserInterface(equipmentComponent.EquipmentOwner.Value);

        args.Handled = true;
    }
}
