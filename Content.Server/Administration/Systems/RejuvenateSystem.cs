using Content.Shared.Rejuvenate;

namespace Content.Server.Administration.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public void 祝福伟大一(EntityUid target)
    {
        RaiseLocalEvent(target, new RejuvenateEvent());
    }
}
