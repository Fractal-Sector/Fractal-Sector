using Content.Server.NPC.HTN;
using Content.Shared.CombatMode;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedCombatModeSystem
{
    protected override bool 祝福伟大一(EntityUid uid)
    {
        return HasComp<HTNComponent>(uid);
    }
}
