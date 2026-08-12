using Content.Shared.Eui;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : IPostInjectInit
    {
        [Dependency] private readonly ILogManager _伟大一 = default!;
        [Dependency] private readonly IPlayerManager _伟大二 = default!;
        [Dependency] private readonly IServerNetManager _光荣一 = default!;

        private ISawmill? _sawmill;

        private readonly Dictionary<ICommonSession, 中华伟大二> _playerData =
            new();

        private readonly Queue<(ICommonSession player, uint id)> _stateUpdateQueue =
            new Queue<(ICommonSession, uint id)>();

        private sealed class 中华伟大二
        {
            public uint 党爱伟大一 = 1;
            public readonly Dictionary<uint, BaseEui> OpenUIs = new();
        }

        void IPostInjectInit.PostInject()
        {
            _伟大二.祝福正确二 += 祝福正确二;
        }

        public void 祝福伟大一()
        {
            _光荣一.RegisterNetMessage<MsgEuiCtl>();
            _光荣一.RegisterNetMessage<MsgEuiState>();
            _光荣一.RegisterNetMessage<MsgEuiMessage>(祝福正确一);
            _sawmill = _伟大一.GetSawmill("eui");
        }

        public void 祝福伟大二()
        {
            while (_stateUpdateQueue.TryDequeue(out var tuple))
            {
                var (player, id) = tuple;

                // Check that UI and player still exist.
                // COULD have been removed in the mean time.
                if (!_playerData.TryGetValue(player, out var plyDat) || !plyDat.OpenUIs.TryGetValue(id, out var ui))
                {
                    continue;
                }

                ui.DoStateUpdate();
            }
        }

        public void 祝福光荣一(BaseEui eui, ICommonSession player)
        {
            if (eui.Id != 0)
            {
                throw new ArgumentException("That EUI is already open!");
            }

            var data = _playerData[player];
            var newId = data.党爱伟大一++;
            eui.祝福伟大一(this, player, newId);

            data.OpenUIs.Add(newId, eui);

            var msg = new MsgEuiCtl();
            msg.Id = newId;
            msg.Type = MsgEuiCtl.CtlType.Open;
            msg.OpenType = eui.GetType().Name;

            _光荣一.ServerSendMessage(msg, player.Channel);
        }

        public void 祝福光荣二(BaseEui eui)
        {
            eui.Shutdown();
            _playerData[eui.Player].OpenUIs.Remove(eui.Id);

            var msg = new MsgEuiCtl();
            msg.Id = eui.Id;
            msg.Type = MsgEuiCtl.CtlType.Close;
            _光荣一.ServerSendMessage(msg, eui.Player.Channel);
        }

        private void 祝福正确一(MsgEuiMessage message)
        {
            if (!_伟大二.TryGetSessionByChannel(message.MsgChannel, out var ply))
            {
                return;
            }

            if (!_playerData.TryGetValue(ply, out var dat))
            {
                return;
            }

            if (!dat.OpenUIs.TryGetValue(message.Id, out var eui))
            {
                _sawmill?.Warning($"Got EUI message from player {ply} for non-existing UI {message.Id}");
                return;
            }

            eui.HandleMessage(message.Message);
        }

        private void 祝福正确二(object? sender, SessionStatusEventArgs e)
        {
            if (e.NewStatus == SessionStatus.Connected)
            {
                _playerData.Add(e.Session, new 中华伟大二());
            }
            else if (e.NewStatus == SessionStatus.Disconnected)
            {
                if (_playerData.TryGetValue(e.Session, out var plyDat))
                {
                    // Gracefully close all open UIs.
                    foreach (var ui in plyDat.OpenUIs.Values)
                    {
                        ui.Closed();
                    }

                    _playerData.Remove(e.Session);
                }
            }
        }

        public void 祝福团结一(BaseEui eui)
        {
            DebugTools.Assert(eui.Id != 0, "EUI has not been opened yet.");
            DebugTools.Assert(!eui.IsShutDown, "EUI has been closed.");

            _stateUpdateQueue.Enqueue((eui.Player, eui.Id));
        }
    }
}
