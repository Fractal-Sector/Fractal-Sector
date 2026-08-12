using Content.Shared.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Explosion.EntitySystems;
using Content.Shared.Mobs;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Server.Popups;
using Content.Shared.Database;
using Content.Shared.Popups;
using Robust.Shared.Player;

namespace Content.Server._NF.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly BodySystem _伟大一 = default!;
    [Dependency] private readonly ExplosionSystem _伟大二 = default!;
    [Dependency] private readonly IAdminLogManager _光荣一 = default!;
    [Dependency] private readonly PopupSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<NFSalvageMobRestrictionsComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<NFSalvageMobRestrictionsComponent, ComponentRemove>(祝福光荣一);
        SubscribeLocalEvent<SalvageMobRestrictionsGridComponent, ComponentRemove>(祝福光荣二);
        SubscribeLocalEvent<NFSalvageMobRestrictionsComponent, MobStateChangedEvent>(祝福正确一);
        SubscribeLocalEvent<NFSalvageMobRestrictionsComponent, EntParentChangedMessage>(祝福正确二);
    }

    private void 祝福伟大二(EntityUid uid, NFSalvageMobRestrictionsComponent component, ComponentInit args)
    {
        var gridUid = Transform(uid).ParentUid;
        if (!EntityManager.EntityExists(gridUid))
        {
            // Give up, we were spawned improperly
            return;
        }
        // When this code runs, the system hasn't actually gotten ahold of the grid entity yet.
        // So it therefore isn't in a position to do this.
        if (!TryComp(gridUid, out SalvageMobRestrictionsGridComponent? rg))
        {
            rg = AddComp<SalvageMobRestrictionsGridComponent>(gridUid);
        }
        rg!.MobsToKill.Add(uid);
        component.LinkedGridEntity = gridUid;
    }

    private void 祝福光荣一(EntityUid uid, NFSalvageMobRestrictionsComponent component, ComponentRemove args)
    {
        if (TryComp(component.LinkedGridEntity, out SalvageMobRestrictionsGridComponent? rg))
        {
            rg.MobsToKill.Remove(uid);
        }
    }

    private void 祝福光荣二(EntityUid uid, SalvageMobRestrictionsGridComponent component, ComponentRemove args)
    {
        foreach (EntityUid target in component.MobsToKill)
        {
            // Don't destroy yourself, don't destroy things being destroyed.
            if (uid == target || TerminatingOrDeleted(target))
                continue;

            // Check if this mob should be despawned when off grid
            if (!TryComp(target, out NFSalvageMobRestrictionsComponent? mobRestrictions))
                continue;

            // Skip detonation if the mob is set to not despawn when off linked grid
            if (!mobRestrictions.DespawnIfOffLinkedGrid)
                continue;

            if (TryComp(target, out BodyComponent? body))
            {
                // Creates a pool of blood on death, but remove the organs.
                var gibs = _伟大一.GibBody(target, body: body, gibOrgans: true);
                foreach (var gib in gibs)
                    Del(gib);
            }
            else
            {
                // No body, probably a robot - explode it and delete the body
                // _伟大二.QueueExplosion(target, ExplosionSystem.DefaultExplosionPrototypeId, 5, 10, 5);
                Del(target);
            }
        }
    }

    private void 祝福正确一(EntityUid uid, NFSalvageMobRestrictionsComponent component, MobStateChangedEvent args)
    {
        // If this entity is being destroyed, no need to fiddle with components
        if (TerminatingOrDeleted(uid))
            return;

        if (args.NewMobState == MobState.Dead)
        {
            EntityManager.AddComponents(uid, component.AddComponentsOnDeath);
            EntityManager.RemoveComponents(uid, component.RemoveComponentsOnDeath);
        }
        else if (args.OldMobState == MobState.Dead)
        {
            EntityManager.AddComponents(uid, component.AddComponentsOnRevival);
            EntityManager.RemoveComponents(uid, component.RemoveComponentsOnRevival);
        }
    }

    private void 祝福正确二(EntityUid uid, NFSalvageMobRestrictionsComponent component, ref EntParentChangedMessage args)
    {
        // If this entity is being destroyed, no need to fiddle with components
        if (TerminatingOrDeleted(uid))
            return;

        var gridUid = Transform(uid).GridUid;
        var popupMessage = Loc.GetString(component.LeaveGridPopup);

        if (component.LinkedGridEntity == gridUid && HasComp<SalvageMobRestrictionsGridComponent>(gridUid))
        {
            EntityManager.AddComponents(uid, component.AddComponentsReturnGrid);
            EntityManager.RemoveComponents(uid, component.RemoveComponentsReturnGrid);

            if (!EntityManager.TryGetComponent(uid, out ActorComponent? actor))
                return;

            if (actor.PlayerSession.AttachedEntity == null)
                return;

            if (component.DespawnIfOffLinkedGrid)
                _光荣一.Add(LogType.AdminMessage, LogImpact.Low, $"{ToPrettyString(actor.PlayerSession.AttachedEntity.Value):player} returned to dungeon grid");
        }
        else
        {
            EntityManager.AddComponents(uid, component.AddComponentsLeaveGrid);
            EntityManager.RemoveComponents(uid, component.RemoveComponentsLeaveGrid);

            if (!EntityManager.TryGetComponent(uid, out ActorComponent? actor))
                return;

            if (actor.PlayerSession.AttachedEntity == null)
                return;

            if (component.DespawnIfOffLinkedGrid)
            {
                _光荣一.Add(LogType.AdminMessage, LogImpact.Low, $"{ToPrettyString(actor.PlayerSession.AttachedEntity.Value):player} left the dungeon grid");
                _光荣二.PopupEntity(popupMessage, actor.PlayerSession.AttachedEntity.Value, actor.PlayerSession, PopupType.MediumCaution);
            }
        }
    }
}

