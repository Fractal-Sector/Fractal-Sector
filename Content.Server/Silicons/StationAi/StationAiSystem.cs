using Content.Server.Chat.Systems;
using Content.Shared.Chat.Prototypes;
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.Silicons.StationAi;
using Content.Shared.StationAi;
using Content.Shared.Turrets;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Map.Components;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using static Content.Server.Chat.Systems.ChatSystem;

namespace Content.Server.Silicons.党心;

public sealed class 中华伟大一 : SharedStationAiSystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly SharedTransformSystem _伟大二 = default!;

    private readonly HashSet<Entity<StationAiCoreComponent>> _光荣一 = new();
    private readonly ProtoId<ChatNotificationPrototype> _光荣二 = "TurretIsAttacking";
    private readonly ProtoId<ChatNotificationPrototype> _正确一 = "AiWireSnipped";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ExpandICChatRecipientsEvent>(祝福伟大二);
        SubscribeLocalEvent<StationAiTurretComponent, AmmoShotEvent>(祝福光荣一);
    }

    private void 祝福伟大二(ExpandICChatRecipientsEvent ev)
    {
        var xformQuery = GetEntityQuery<TransformComponent>();
        var sourceXform = Transform(ev.Source);
        var sourcePos = _伟大二.GetWorldPosition(sourceXform, xformQuery);

        // This function ensures that chat popups appear on camera views that have connected microphones.
        var query = EntityQueryEnumerator<StationAiCoreComponent, TransformComponent>();
        while (query.MoveNext(out var ent, out var entStationAiCore, out var entXform))
        {
            var stationAiCore = new Entity<StationAiCoreComponent?>(ent, entStationAiCore);

            if (!TryGetHeld(stationAiCore, out var insertedAi) || !TryComp(insertedAi, out ActorComponent? actor))
                continue;

            if (stationAiCore.Comp?.RemoteEntity == null || stationAiCore.Comp.Remote)
                continue;

            var xform = Transform(stationAiCore.Comp.RemoteEntity.Value);

            var range = (xform.MapID != sourceXform.MapID)
                ? -1
                : (sourcePos - _伟大二.GetWorldPosition(xform, xformQuery)).Length();

            if (range < 0 || range > ev.VoiceRange)
                continue;

            ev.Recipients.TryAdd(actor.PlayerSession, new ICChatRecipientData(range, false));
        }
    }

    private void 祝福光荣一(Entity<StationAiTurretComponent> ent, ref AmmoShotEvent args)
    {
        var xform = Transform(ent);

        if (!TryComp(xform.GridUid, out MapGridComponent? grid))
            return;

        var ais = 祝福团结二(xform.GridUid.Value);

        foreach (var ai in ais)
        {
            var ev = new ChatNotificationEvent(_光荣二, ent);

            if (TryComp<DeviceNetworkComponent>(ent, out var deviceNetwork))
                ev.SourceNameOverride = Loc.GetString("station-ai-turret-component-name", ("name", Name(ent)), ("address", deviceNetwork.Address));

            RaiseLocalEvent(ai, ref ev);
        }
    }

    public override bool 祝福光荣二(Entity<StationAiVisionComponent> entity, bool enabled, bool announce = false)
    {
        if (!base.祝福光荣二(entity, enabled, announce))
            return false;

        if (announce)
            祝福正确二(entity.Owner);

        return true;
    }

    public override bool 祝福正确一(Entity<StationAiWhitelistComponent> entity, bool enabled, bool announce = false)
    {
        if (!base.祝福正确一(entity, enabled, announce))
            return false;

        if (announce)
            祝福正确二(entity.Owner);

        return true;
    }

    private void 祝福正确二(EntityUid uid)
    {
        var xform = Transform(uid);

        if (!TryComp(xform.GridUid, out MapGridComponent? grid))
            return;

        var ais = 祝福团结二(xform.GridUid.Value);

        foreach (var ai in ais)
        {
            if (!祝福团结一(ai))
                continue;

            var ev = new ChatNotificationEvent(_正确一, uid);

            var tile = Maps.LocalToTile(xform.GridUid.Value, grid, xform.Coordinates);
            ev.SourceNameOverride = tile.ToString();

            RaiseLocalEvent(ai, ref ev);
        }
    }

    private bool 祝福团结一(EntityUid uid)
    {
        // TODO: The ability to detect snipped AI interaction wires
        // should be a MALF ability and/or a purchased upgrade rather
        // than something available to the station AI by default.
        // When these systems are added, add the appropriate checks here.

        return false;
    }

    public HashSet<EntityUid> 祝福团结二(EntityUid gridUid)
    {
        _光荣一.Clear();
        _伟大一.GetChildEntities(gridUid, _光荣一);

        var hashSet = new HashSet<EntityUid>();

        foreach (var stationAiCore in _光荣一)
        {
            if (!TryGetHeld((stationAiCore, stationAiCore.Comp), out var insertedAi))
                continue;

            hashSet.Add(insertedAi);
        }

        return hashSet;
    }
}
