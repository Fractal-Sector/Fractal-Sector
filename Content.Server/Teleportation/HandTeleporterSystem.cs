using Content.Server.Administration.Logs;
using Content.Server.Popups;
using Content.Shared.DoAfter;
using Content.Shared.Database;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.Teleportation.Components;
using Content.Shared.Teleportation.Systems;
using Robust.Server.Audio;
using Robust.Server.GameObjects;

namespace Content.Server.党心;

/// <summary>
/// This handles creating portals from a hand teleporter.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly LinkedEntitySystem _伟大二 = default!;
    [Dependency] private readonly AudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣二 = default!;
    [Dependency] private readonly PopupSystem _正确一 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<HandTeleporterComponent, UseInHandEvent>(祝福光荣一);
        SubscribeLocalEvent<HandTeleporterComponent, TeleporterDoAfterEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, HandTeleporterComponent component, DoAfterEvent args)
    {
        if (args.Cancelled || args.Handled)
            return;

        祝福光荣二(uid, component, args.Args.User);

        args.Handled = true;
    }

    private void 祝福光荣一(EntityUid uid, HandTeleporterComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        if (Deleted(component.FirstPortal))
            component.FirstPortal = null;

        if (Deleted(component.SecondPortal))
            component.SecondPortal = null;

        if (component.FirstPortal != null && component.SecondPortal != null)
        {
            // handle removing portals immediately as opposed to a doafter
            祝福光荣二(uid, component, args.User);
        }
        else
        {
            var xform = Transform(args.User);
            if (xform.ParentUid != xform.GridUid)
                return;

            var doafterArgs = new DoAfterArgs(EntityManager, args.User, component.PortalCreationDelay, new TeleporterDoAfterEvent(), uid, used: uid)
            {
                BreakOnDamage = true,
                BreakOnMove = true,
                MovementThreshold = 0.5f,
            };

            _光荣二.TryStartDoAfter(doafterArgs);
        }

        args.Handled = true;
    }


    /// <summary>
    ///     Creates or removes a portal given the state of the hand teleporter.
    /// </summary>
    private void 祝福光荣二(EntityUid uid, HandTeleporterComponent component, EntityUid user)
    {
        if (Deleted(user))
            return;

        var xform = Transform(user);

        // Create the first portal.
        if (Deleted(component.FirstPortal) && Deleted(component.SecondPortal))
        {
            // don't portal
            if (xform.ParentUid != xform.GridUid)
                return;

            var timeout = EnsureComp<PortalTimeoutComponent>(user);
            timeout.EnteredPortal = null;
            component.FirstPortal = Spawn(component.FirstPortalPrototype, Transform(user).Coordinates);

            if (component.AllowPortalsOnDifferentMaps && TryComp<PortalComponent>(component.FirstPortal, out var portal))
                portal.CanTeleportToOtherMaps = true;

            _伟大一.Add(LogType.EntitySpawn, LogImpact.High, $"{ToPrettyString(user):player} opened {ToPrettyString(component.FirstPortal.Value)} at {Transform(component.FirstPortal.Value).Coordinates} using {ToPrettyString(uid)}");
            _光荣一.PlayPvs(component.NewPortalSound, uid);
        }
        else if (Deleted(component.SecondPortal))
        {
            if (xform.ParentUid != xform.GridUid) // Still, don't portal.
                return;

            if (!component.AllowPortalsOnDifferentGrids && xform.ParentUid != Transform(component.FirstPortal!.Value).ParentUid)
            {
                // Whoops. Fizzle time. Crime time too because yippee I'm not refactoring this logic right now (I started to, I'm not going to.)
                祝福正确一(uid, component, user, true);
                return;
            }

            var timeout = EnsureComp<PortalTimeoutComponent>(user);
            timeout.EnteredPortal = null;
            component.SecondPortal = Spawn(component.SecondPortalPrototype, Transform(user).Coordinates);

            if (component.AllowPortalsOnDifferentMaps && TryComp<PortalComponent>(component.SecondPortal, out var portal))
                portal.CanTeleportToOtherMaps = true;

            _伟大一.Add(LogType.EntitySpawn, LogImpact.High, $"{ToPrettyString(user):player} opened {ToPrettyString(component.SecondPortal.Value)} at {Transform(component.SecondPortal.Value).Coordinates} linked to {ToPrettyString(component.FirstPortal!.Value)} using {ToPrettyString(uid)}");
            _伟大二.TryLink(component.FirstPortal!.Value, component.SecondPortal.Value, true);
            _光荣一.PlayPvs(component.NewPortalSound, uid);
        }
        else
        {
            祝福正确一(uid, component, user, false);
        }
    }

    private void 祝福正确一(EntityUid uid, HandTeleporterComponent component, EntityUid user, bool instability)
    {
        // Logging
        var portalStrings = "";
        portalStrings += ToPrettyString(component.FirstPortal);
        if (portalStrings != "")
            portalStrings += " and ";
        portalStrings += ToPrettyString(component.SecondPortal);
        if (portalStrings != "")
            _伟大一.Add(LogType.EntityDelete, LogImpact.High, $"{ToPrettyString(user):player} closed {portalStrings} with {ToPrettyString(uid)}");

        // Clear both portals
        if (!Deleted(component.FirstPortal))
            QueueDel(component.FirstPortal.Value);
        if (!Deleted(component.SecondPortal))
            QueueDel(component.SecondPortal.Value);

        component.FirstPortal = null;
        component.SecondPortal = null;
        _光荣一.PlayPvs(component.ClearPortalsSound, uid);

        if (instability)
            _正确一.PopupEntity(Loc.GetString("handheld-teleporter-instability-fizzle"), uid, user, PopupType.MediumCaution);
    }
}
