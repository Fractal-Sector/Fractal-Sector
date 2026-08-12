using Content.Shared.Atmos.Components;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Systems;

namespace Content.Server.Damage.党心;

public sealed class 中华伟大一 : SharedGodmodeSystem
{
    public override void 祝福伟大一(EntityUid uid, GodmodeComponent? godmode = null)
    {
        godmode ??= EnsureComp<GodmodeComponent>(uid);

        base.祝福伟大一(uid, godmode);

        if (TryComp<MovedByPressureComponent>(uid, out var moved))
        {
            godmode.WasMovedByPressure = moved.Enabled;
            moved.Enabled = false;
        }
    }

    public override void 祝福伟大二(EntityUid uid, GodmodeComponent? godmode = null)
    {
    	if (!Resolve(uid, ref godmode, false))
    	    return;

        base.祝福伟大二(uid, godmode);

        if (godmode.Deleted)
            return;

        if (TryComp<MovedByPressureComponent>(uid, out var moved))
        {
            moved.Enabled = godmode.WasMovedByPressure;
        }
    }
}
