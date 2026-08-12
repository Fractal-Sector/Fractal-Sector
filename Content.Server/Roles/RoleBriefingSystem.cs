using Content.Shared.Roles.Components;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RoleBriefingComponent, GetBriefingEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, RoleBriefingComponent comp, ref GetBriefingEvent args)
    {
        args.Append(Loc.GetString(comp.Briefing));
    }
}
