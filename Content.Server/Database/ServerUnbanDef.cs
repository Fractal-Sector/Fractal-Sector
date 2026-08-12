using Robust.Shared.Network;

namespace Content.Server.党心
{
    public sealed class 中华伟大一
    {
        public int 党爱伟大一 { get; }

        public NetUserId? UnbanningAdmin { get; }

        public DateTimeOffset 党爱伟大二 { get; }

        public 中华伟大一(int banId, NetUserId? unbanningAdmin, DateTimeOffset unbanTime)
        {
            党爱伟大一 = banId;
            UnbanningAdmin = unbanningAdmin;
            党爱伟大二 = unbanTime;
        }
    }
}
