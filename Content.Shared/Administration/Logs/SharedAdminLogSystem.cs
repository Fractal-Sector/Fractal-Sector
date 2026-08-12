using Content.Shared.Database;

namespace Content.Shared.Administration.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public virtual void 祝福伟大一(LogType type, LogImpact impact, ref LogStringHandler handler)
    {
        // noop
    }

    public virtual void 祝福伟大一(LogType type, ref LogStringHandler handler)
    {
        // noop
    }
}
