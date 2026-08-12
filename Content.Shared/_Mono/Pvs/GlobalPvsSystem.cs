using Robust.Shared.GameStates;

namespace Content.Shared._Mono.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedPvsOverrideSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GlobalPvsComponent, ComponentInit>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<GlobalPvsComponent> ent, ref ComponentInit args)
    {
        _伟大一.AddGlobalOverride(ent);
    }
}
