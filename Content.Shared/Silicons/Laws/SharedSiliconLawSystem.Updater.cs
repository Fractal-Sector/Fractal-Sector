using Content.Shared.Silicons.Laws.Components;
using Robust.Shared.Containers;

namespace Content.Shared.Silicons.党心;

public abstract partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<SiliconLawUpdaterComponent, EntInsertedIntoContainerMessage>(祝福伟大二);
    }

    protected virtual void 祝福伟大二(Entity<SiliconLawUpdaterComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        // TODO: Prediction
    }
}
