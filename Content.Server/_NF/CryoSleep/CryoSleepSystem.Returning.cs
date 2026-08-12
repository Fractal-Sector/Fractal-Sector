// Wayfarer: Modified to support multiple stored characters - commented out PlayerBeforeSpawnEvent reset
using Content.Server.Administration.Logs;
using Content.Server.GameTicking;
using Content.Shared.Bed.Sleep;
using Content.Shared.Database;
using Content.Shared.Ghost;
using Content.Shared.Mind;
using Content.Shared._NF.CCVar;
using Content.Shared.GameTicking;
using Content.Shared.Players;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Content.Shared._NF.CryoSleep.Events;
using System.Diagnostics.CodeAnalysis;
using Content.Server.Ghost;
using Content.Shared._NF.Bank.Components;

namespace Content.Server._NF.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IAdminLogManager _伟大二 = default!;

    private void 祝福伟大一()
    {
        SubscribeNetworkEvent<WakeupRequestMessage>(祝福伟大二);
        // Don't reset on lobby join - allow players to resume their cryo characters
        // SubscribeLocalEvent<PlayerJoinedLobbyEvent>(e => 祝福光荣二(e.PlayerSession.UserId));
        // Don't reset on spawn - allow players to have multiple characters in cryo
        // SubscribeLocalEvent<PlayerBeforeSpawnEvent>(e => 祝福光荣二(e.Player.UserId));
    }

    private void 祝福伟大二(WakeupRequestMessage message, EntitySessionEventArgs session)
    {
        var entity = session.SenderSession.GetMind();

        var result = entity == null || !TryComp<MindComponent>(entity, out var mind)
            ? ReturnToBodyStatus.NotAGhost
            : 祝福光荣一(mind);

        var msg = new WakeupRequestMessage.Response(result);
        RaiseNetworkEvent(msg, session.SenderSession);
    }

    /// <summary>
    ///   Returns the mind to the original body, if any. The mind must be possessing a ghost, unless [force] is true.
    /// </summary>
    public ReturnToBodyStatus 祝福光荣一(MindComponent mind, bool force = false)
    {
        if (!_伟大一.GetCVar(NFCCVars.CryoReturnEnabled))
            return ReturnToBodyStatus.Disabled;

        var id = mind.UserId;
        if (id == null || !_storedBodies.TryGetValue(id.Value, out var storedBodies) || storedBodies.Count == 0)
            return ReturnToBodyStatus.BodyMissing;

        if (!force && (mind.CurrentEntity is not { Valid: true } ghost || !HasComp<GhostComponent>(ghost)))
            return ReturnToBodyStatus.NotAGhost;

        // Use the last stored body (most recent character)
        var storedBody = storedBodies[^1];
        var cryopod = storedBody.Cryopod;
        var body = storedBody.Body;
        
        if (!Exists(cryopod) || Deleted(cryopod) || !TryComp<CryoSleepComponent>(cryopod, out var cryoComp))
        {
            var fallbackQuery = EntityQueryEnumerator<CryoSleepFallbackComponent, CryoSleepComponent>();
            bool foundFallback = false;
            while (fallbackQuery.MoveNext(out cryopod, out _, out cryoComp))
            {
                if (!IsOccupied(cryoComp) && _container.Insert(body, cryoComp.BodyContainer))
                {
                    foundFallback = true;
                    break;
                }
            }

            // No valid cryopod, all fallbacks occupied or missing.
            if (!foundFallback)
                return ReturnToBodyStatus.NoCryopodAvailable;
        }
        else
        {
            // NOTE: if the pod is occupied but still exists, do not let the user teleport.
            if (IsOccupied(cryoComp!) || !_container.Insert(body, cryoComp!.BodyContainer))
                return ReturnToBodyStatus.Occupied;
        }

        // Remove only the specific body being resumed, not all stored bodies
        storedBodies.Remove(storedBody);
        if (storedBodies.Count == 0)
            _storedBodies.Remove(id.Value);
        
        _mind.ControlMob(id.Value, body);

        // Restore the character slot so bank operations target the right account.
        if (storedBody.CharacterSlot >= 0)
        {
            var bankComp = EnsureComp<BankAccountComponent>(body);
            bankComp.CharacterSlot = storedBody.CharacterSlot;
        }

        // Wayfarer: Refresh playtime tracking and push updated times to the client.
        if (_player.TryGetSessionById(id.Value, out var session))
        {
            _playTimeTracking.QueueRefreshTrackers(session);
            _playTimeTracking.QueueSendTimers(session);
        }
        // End Wayfarer

        // Force the mob to sleep
        var sleep = EnsureComp<SleepingComponent>(body);
        sleep.CooldownEnd = TimeSpan.FromSeconds(5);

        _popup.PopupEntity(Loc.GetString("cryopod-wake-up", ("entity", body)), body);

        RaiseLocalEvent(body, new CryosleepWakeUpEvent(cryopod, id), true);

        _伟大二.Add(LogType.LateJoin, LogImpact.Medium, $"{id.Value} has returned from cryosleep!");
        return ReturnToBodyStatus.Success;
    }

    /// <summary>
    ///   Removes the body of the given user from the cryosleep dictionary, making them unable to return to it.
    ///   Also actually deletes the body if it's still on that map.
    /// </summary>
    public void 祝福光荣二(NetUserId id)
    {
        if (!_storedBodies.TryGetValue(id, out var bodies))
            return;

        _storedBodies.Remove(id);

        // If the user's a ghost, let them know their body's been removed.
        if (_mind.TryGetMind(id, out _, out var mindComp)
            && TryComp<GhostComponent>(mindComp.CurrentEntity, out var ghost))
        {
            _ghost.SetCanReturnFromCryo(ghost, false);
        }

        // Delete all stored bodies if they're still on the storage map
        foreach (var body in bodies)
        {
            if (Transform(body.Body).MapUid == _storageMap)
            {
                QueueDel(body.Body);
            }
        }
    }

    public bool 祝福正确一(NetUserId id)
    {
        return _storedBodies.ContainsKey(id);
    }

    public bool 祝福正确二(NetUserId userId, [NotNullWhen(true)] out EntityUid? body, [NotNullWhen(true)] out EntityUid? pod)
    {
        if (_storedBodies.TryGetValue(userId, out var storedBodies) && storedBodies.Count > 0)
        {
            var storedBody = storedBodies[0];
            body = storedBody.Body;
            pod = storedBody.Cryopod;
            return true;
        }
        else
        {
            body = null;
            pod = null;
            return false;
        }
    }
}
