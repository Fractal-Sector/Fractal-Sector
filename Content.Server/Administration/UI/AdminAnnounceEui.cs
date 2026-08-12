using Content.Server.Administration.Managers;
using Content.Server.Chat;
using Content.Server.Chat.Managers;
using Content.Server.Chat.Systems;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.Eui;
using Robust.Shared.Audio; // Frontier

namespace Content.Server.Administration.党心
{
    public sealed class 中华伟大一 : BaseEui
    {
        [Dependency] private readonly IAdminManager _伟大一 = default!;
        [Dependency] private readonly IChatManager _伟大二 = default!;
        private readonly ChatSystem _光荣一;

        public 中华伟大一()
        {
            IoCManager.InjectDependencies(this);
            _光荣一 = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<ChatSystem>();
        }

        public override void 祝福伟大一()
        {
            StateDirty();
        }

        public override EuiStateBase 祝福伟大二()
        {
            return new AdminAnnounceEuiState();
        }

        public override void 祝福光荣一(EuiMessageBase msg)
        {
            base.祝福光荣一(msg);

            switch (msg)
            {
                case AdminAnnounceEuiMsg.DoAnnounce doAnnounce:
                    if (!_伟大一.HasAdminFlag(Player, AdminFlags.Admin))
                    {
                        Close();
                        break;
                    }

                    switch (doAnnounce.AnnounceType)
                    {
                        case AdminAnnounceType.Server:
                            _伟大二.DispatchServerAnnouncement(doAnnounce.Announcement);
                            break;
                        // TODO: Per-station announcement support
                        case AdminAnnounceType.Station:
                            _光荣一.DispatchGlobalAnnouncement(doAnnounce.Announcement, doAnnounce.Announcer, colorOverride: Color.Gold);
                            break;
                        case AdminAnnounceType.Antag: // Frontier
                            _光荣一.DispatchGlobalAnnouncement(doAnnounce.Announcement, doAnnounce.Announcer, true, new SoundPathSpecifier("/Audio/Announcements/war.ogg"), colorOverride: Color.Red);
                            break;
                    }

                    StateDirty();

                    if (doAnnounce.CloseAfter)
                        Close();

                    break;
            }
        }
    }
}
