using Content.Server.GameTicking.Rules.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.Storage;

namespace Content.Server.GameTicking.党心;

public sealed class 中华伟大一 : GameRuleSystem<SubGamemodesComponent>
{
    protected override void 祝福伟大一(EntityUid uid, SubGamemodesComponent comp, GameRuleComponent rule, GameRuleAddedEvent args)
    {
        var picked = EntitySpawnCollection.GetSpawns(comp.Rules, RobustRandom);
        foreach (var id in picked)
        {
            Log.Info($"Starting gamerule {id} as a subgamemode of {ToPrettyString(uid):rule}");
            GameTicker.AddGameRule(id);
        }
    }
}
