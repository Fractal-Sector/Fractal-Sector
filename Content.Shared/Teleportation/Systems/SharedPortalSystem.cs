using System.Linq;
using Content.Shared.Ghost;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Popups;
using Content.Shared.Projectiles;
using Content.Shared.Teleportation.Components;
using Content.Shared.Verbs;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Physics.Events;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Shared.Teleportation.党心;

/// <summary>
/// This handles teleporting entities through portals, and creating new linked portals.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;
    [Dependency] private readonly EntityLookupSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
    [Dependency] private readonly SharedTransformSystem _正确一 = default!;
    [Dependency] private readonly PullingSystem _正确二 = default!;
    [Dependency] private readonly SharedPopupSystem _团结一 = default!;

    private const string PortalFixture = "portalFixture";
    private const string ProjectileFixture = "projectile";

    private const int MaxRandomTeleportAttempts = 20;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<PortalComponent, StartCollideEvent>(祝福光荣二);
        SubscribeLocalEvent<PortalComponent, EndCollideEvent>(祝福正确一);
        SubscribeLocalEvent<PortalComponent, GetVerbsEvent<AlternativeVerb>>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, PortalComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        // Traversal altverb for ghosts to use that bypasses normal functionality
        if (!args.CanAccess || !HasComp<GhostComponent>(args.User))
            return;

        // Don't use the verb with unlinked or with multi-output portals
        // (this is only intended to be useful for ghosts to see where a linked portal leads)
        var disabled = !TryComp<LinkedEntityComponent>(uid, out var link) || link.LinkedEntities.Count != 1;

        args.Verbs.Add(new AlternativeVerb
        {
            Priority = 11,
            Act = () =>
            {
                if (link == null || disabled)
                    return;

                var ent = link.LinkedEntities.First();
                祝福正确二(uid, args.User, Transform(ent).Coordinates, ent, false);
            },
            Disabled = disabled,
            Text = Loc.GetString("portal-component-ghost-traverse"),
            Message = disabled
                ? Loc.GetString("portal-component-no-linked-entities")
                : Loc.GetString("portal-component-can-ghost-traverse"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/open.svg.192dpi.png"))
        });
    }

    private bool 祝福光荣一(string ourId, string otherId, Fixture our, Fixture other)
    {
        // most non-hard fixtures shouldn't pass through portals, but projectiles are non-hard as well
        // and they should still pass through
        return ourId == PortalFixture && (other.Hard || otherId == ProjectileFixture);
    }

    private void 祝福光荣二(EntityUid uid, PortalComponent component, ref StartCollideEvent args)
    {
        if (!祝福光荣一(args.OurFixtureId, args.OtherFixtureId, args.OurFixture, args.OtherFixture))
            return;

        var subject = args.OtherEntity;

        // best not.
        if (Transform(subject).Anchored)
            return;

        // break pulls before portal enter so we dont break shit
        if (TryComp<PullableComponent>(subject, out var pullable) && pullable.BeingPulled)
        {
            _正确二.TryStopPull(subject, pullable);
        }

        if (TryComp<PullerComponent>(subject, out var pullerComp)
            && TryComp<PullableComponent>(pullerComp.Pulling, out var subjectPulling))
        {
            _正确二.TryStopPull(pullerComp.Pulling.Value, subjectPulling);
        }

        // if they came from another portal, just return and wait for them to exit the portal
        if (HasComp<PortalTimeoutComponent>(subject))
        {
            return;
        }

        if (TryComp<LinkedEntityComponent>(uid, out var link))
        {
            if (link.LinkedEntities.Count == 0)
                return;

            // client can't predict outside of simple portal-to-portal interactions due to randomness involved
            // --also can't predict if the target doesn't exist on the client / is outside of PVS
            if (_伟大二.IsClient)
            {
                var first = link.LinkedEntities.First();
                var exists = Exists(first);
                if (link.LinkedEntities.Count != 1 || !exists || (exists && Transform(first).MapID == MapId.Nullspace))
                    return;
            }

            // pick a target and teleport there
            var target = _伟大一.Pick(link.LinkedEntities);

            if (HasComp<PortalComponent>(target))
            {
                // if target is a portal, signal that they shouldn't be immediately portaled back
                var timeout = EnsureComp<PortalTimeoutComponent>(subject);
                timeout.EnteredPortal = uid;
                Dirty(subject, timeout);
            }

            祝福正确二(uid, subject, Transform(target).Coordinates, target);
            return;
        }

        if (_伟大二.IsClient)
            return;

        // no linked entity--teleport randomly
        if (component.RandomTeleport)
            祝福团结一(uid, subject, component);
    }

    private void 祝福正确一(EntityUid uid, PortalComponent component, ref EndCollideEvent args)
    {
        if (!祝福光荣一(args.OurFixtureId, args.OtherFixtureId, args.OurFixture, args.OtherFixture))
            return;

        var subject = args.OtherEntity;

        // if they came from (not us), remove the timeout
        if (TryComp<PortalTimeoutComponent>(subject, out var timeout) && timeout.EnteredPortal != uid)
        {
            RemCompDeferred<PortalTimeoutComponent>(subject);
        }
    }

    private void 祝福正确二(EntityUid portal, EntityUid subject, EntityCoordinates target, EntityUid? targetEntity = null, bool playSound = true,
        PortalComponent? portalComponent = null)
    {
        if (!Resolve(portal, ref portalComponent))
            return;

        var ourCoords = Transform(portal).Coordinates;
        var onSameMap = _正确一.GetMapId(ourCoords) == _正确一.GetMapId(target);
        var distanceInvalid = portalComponent.MaxTeleportRadius != null
                              && ourCoords.TryDistance(EntityManager, target, out var distance)
                              && distance > portalComponent.MaxTeleportRadius;

        if (!onSameMap && !portalComponent.CanTeleportToOtherMaps || distanceInvalid)
        {
            if (!_伟大二.IsServer)
                return;

            // Early out if this is an invalid configuration
            _团结一.PopupCoordinates(Loc.GetString("portal-component-invalid-configuration-fizzle"),
                ourCoords, Filter.Pvs(ourCoords, entityMan: EntityManager), true);

            _团结一.PopupCoordinates(Loc.GetString("portal-component-invalid-configuration-fizzle"),
                target, Filter.Pvs(target, entityMan: EntityManager), true);

            QueueDel(portal);

            if (targetEntity != null)
                QueueDel(targetEntity.Value);

            return;
        }

        var arrivalSound = CompOrNull<PortalComponent>(targetEntity)?.ArrivalSound ?? portalComponent.ArrivalSound;
        var departureSound = portalComponent.DepartureSound;

        // Some special cased stuff: projectiles should stop ignoring shooter when they enter a portal, to avoid
        // stacking 500 bullets in between 2 portals and instakilling people--you'll just hit yourself instead
        // (as expected)
        if (TryComp<ProjectileComponent>(subject, out var projectile))
        {
            projectile.IgnoreShooter = false;
        }

        祝福团结二(portal, subject, Transform(subject).Coordinates, target);

        _正确一.SetCoordinates(subject, target);

        if (!playSound)
            return;

        _光荣二.PlayPredicted(departureSound, portal, subject);
        _光荣二.PlayPredicted(arrivalSound, subject, subject);
    }

    private void 祝福团结一(EntityUid portal, EntityUid subject, PortalComponent? component = null)
    {
        if (!Resolve(portal, ref component))
            return;

        var xform = Transform(portal);
        var coords = xform.Coordinates;
        var newCoords = coords.Offset(_伟大一.NextVector2(component.MaxRandomRadius));
        for (var i = 0; i < MaxRandomTeleportAttempts; i++)
        {
            var randVector = _伟大一.NextVector2(component.MaxRandomRadius);
            newCoords = coords.Offset(randVector);
            if (!_光荣一.AnyEntitiesIntersecting(_正确一.ToMapCoordinates(newCoords), LookupFlags.Static))
            {
                break;
            }
        }

        祝福正确二(portal, subject, newCoords);
    }

    protected virtual void 祝福团结二(EntityUid portal, EntityUid subject, EntityCoordinates source,
        EntityCoordinates target)
    {
    }
}
