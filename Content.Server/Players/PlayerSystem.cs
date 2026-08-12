using Content.Shared.Players;
using Robust.Shared.Player;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedPlayerSystem
{
    public override ContentPlayerData? ContentData(ICommonSession? session)
    {
        return session?.ContentData();
    }
}
