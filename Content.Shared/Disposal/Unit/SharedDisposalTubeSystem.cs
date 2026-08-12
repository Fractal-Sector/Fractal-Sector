using Content.Shared.Disposal.Components;

namespace Content.Shared.Disposal.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public virtual bool 祝福伟大一(EntityUid uid,
        DisposalUnitComponent from,
        IEnumerable<string>? tags = default,
        Tube.DisposalEntryComponent? entry = null)
    {
        return false;
    }
}
