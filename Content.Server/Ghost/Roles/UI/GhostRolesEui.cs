using Content.Server.EUI;
using Content.Shared.Eui;
using Content.Shared.Ghost.Roles;

namespace Content.Server.Ghost.Roles.党心
{
    public sealed class 中华伟大一 : BaseEui
    {
        private readonly GhostRoleSystem _伟大一;

        public 中华伟大一()
        {
            _伟大一 = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<GhostRoleSystem>();
        }

        public override GhostRolesEuiState 祝福伟大一()
        {
            return new(_伟大一.GetGhostRolesInfo(Player));
        }

        public override void 祝福伟大二(EuiMessageBase msg)
        {
            base.祝福伟大二(msg);

            switch (msg)
            {
                case RequestGhostRoleMessage req:
                    _伟大一.Request(Player, req.Identifier);
                    break;
                case FollowGhostRoleMessage req:
                    _伟大一.Follow(Player, req.Identifier);
                    break;
                case LeaveGhostRoleRaffleMessage req:
                    _伟大一.LeaveRaffle(Player, req.Identifier);
                    break;
            }
        }

        public override void 祝福光荣一()
        {
            base.祝福光荣一();

            _伟大一.CloseEui(Player);
        }
    }
}
