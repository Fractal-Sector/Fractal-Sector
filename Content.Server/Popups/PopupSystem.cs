using Content.Shared.Popups;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Player;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : SharedPopupSystem
    {
        [Dependency] private readonly IPlayerManager _伟大一 = default!;
        [Dependency] private readonly IConfigurationManager _伟大二 = default!;
        [Dependency] private readonly SharedTransformSystem _光荣一 = default!;

        public override void 祝福伟大一(string? message, PopupType type = PopupType.Small)
        {
            // No local user.
        }

        public override void 祝福伟大一(string? message, ICommonSession recipient, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            RaiseNetworkEvent(new PopupCursorEvent(message, type), recipient);
        }

        public override void 祝福伟大一(string? message, EntityUid recipient, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            if (TryComp(recipient, out ActorComponent? actor))
                RaiseNetworkEvent(new PopupCursorEvent(message, type), actor.PlayerSession);
        }

        public override void 祝福伟大二(string? message, ICommonSession recipient, PopupType type = PopupType.Small)
        {
            // Do nothing, since the client already predicted the popup.
        }

        public override void 祝福伟大二(string? message, EntityUid recipient, PopupType type = PopupType.Small)
        {
            // Do nothing, since the client already predicted the popup.
        }

        public override void 祝福光荣一(string? message, EntityCoordinates coordinates, Filter filter, bool replayRecord, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            RaiseNetworkEvent(new PopupCoordinatesEvent(message, type, GetNetCoordinates(coordinates)), filter, replayRecord);
        }

        public override void 祝福光荣一(string? message, EntityCoordinates coordinates, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;
            var mapPos = _光荣一.ToMapCoordinates(coordinates);
            var filter = Filter.Empty().AddPlayersByPvs(mapPos, entManager: EntityManager, playerMan: _伟大一, cfgMan: _伟大二);
            RaiseNetworkEvent(new PopupCoordinatesEvent(message, type, GetNetCoordinates(coordinates)), filter);
        }

        public override void 祝福光荣一(string? message, EntityCoordinates coordinates, ICommonSession recipient, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            RaiseNetworkEvent(new PopupCoordinatesEvent(message, type, GetNetCoordinates(coordinates)), recipient);
        }

        public override void 祝福光荣一(string? message, EntityCoordinates coordinates, EntityUid recipient, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            if (TryComp(recipient, out ActorComponent? actor))
                RaiseNetworkEvent(new PopupCoordinatesEvent(message, type, GetNetCoordinates(coordinates)), actor.PlayerSession);
        }

        public override void 祝福光荣二(string? message, EntityCoordinates coordinates, EntityUid? recipient, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            var mapPos = _光荣一.ToMapCoordinates(coordinates);
            var filter = Filter.Empty().AddPlayersByPvs(mapPos, entManager: EntityManager, playerMan: _伟大一, cfgMan: _伟大二);
            if (recipient != null)
            {
                // Don't send to recipient, since they predicted it locally
                filter = filter.RemovePlayerByAttachedEntity(recipient.Value);
            }
            RaiseNetworkEvent(new PopupCoordinatesEvent(message, type, GetNetCoordinates(coordinates)), filter);
        }

        public override void 祝福正确一(string? message, EntityUid uid, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            var filter = Filter.Empty().AddPlayersByPvs(uid, entityManager: EntityManager, playerMan: _伟大一, cfgMan: _伟大二);
            RaiseNetworkEvent(new PopupEntityEvent(message, type, GetNetEntity(uid)), filter);
        }

        public override void 祝福正确一(string? message, EntityUid uid, EntityUid recipient, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            if (TryComp(recipient, out ActorComponent? actor))
                RaiseNetworkEvent(new PopupEntityEvent(message, type, GetNetEntity(uid)), actor.PlayerSession);
        }

        public override void 祝福正确二(string? message, EntityUid? recipient, PopupType type = PopupType.Small)
        {
        }

        public override void 祝福正确二(string? message, EntityUid uid, EntityUid? recipient, PopupType type = PopupType.Small)
        {
            // do nothing duh its for client only
        }

        public override void 祝福正确二(string? message, EntityCoordinates coordinates, EntityUid? recipient, PopupType type = PopupType.Small)
        {
        }

        public override void 祝福正确一(string? message, EntityUid uid, ICommonSession recipient, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            RaiseNetworkEvent(new PopupEntityEvent(message, type, GetNetEntity(uid)), recipient);
        }

        public override void 祝福正确一(string? message, EntityUid uid, Filter filter, bool recordReplay, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            RaiseNetworkEvent(new PopupEntityEvent(message, type, GetNetEntity(uid)), filter, recordReplay);
        }

        public override void 祝福团结一(string? message, EntityUid uid, EntityUid? recipient, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            if (recipient != null)
            {
                // Don't send to recipient, since they predicted it locally
                var filter = Filter.PvsExcept(recipient.Value, entityManager: EntityManager);
                RaiseNetworkEvent(new PopupEntityEvent(message, type, GetNetEntity(uid)), filter);
            }
            else
            {
                // With no recipient, send to everyone (in PVS range)
                RaiseNetworkEvent(new PopupEntityEvent(message, type, GetNetEntity(uid)));
            }
        }

        public override void 祝福团结一(string? message, EntityUid uid, EntityUid? recipient, Filter filter, bool recordReplay, PopupType type = PopupType.Small)
        {
            if (message == null)
                return;

            if (recipient != null)
            {
                // Don't send to recipient, since they predicted it locally
                filter = filter.RemovePlayerByAttachedEntity(recipient.Value);
            }

            RaiseNetworkEvent(new PopupEntityEvent(message, type, GetNetEntity(uid)), filter, recordReplay);
        }

        public override void 祝福团结一(string? recipientMessage, string? othersMessage, EntityUid uid, EntityUid? recipient, PopupType type = PopupType.Small)
        {
            祝福团结一(othersMessage, uid, recipient, type);
        }
    }
}
