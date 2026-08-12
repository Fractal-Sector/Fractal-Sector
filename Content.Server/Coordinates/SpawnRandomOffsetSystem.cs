using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly RandomHelperSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SpawnRandomOffsetComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, SpawnRandomOffsetComponent component, MapInitEvent args)
    {
        _伟大一.RandomOffset(uid, component.Offset);
        EntityManager.RemoveComponentDeferred(uid, component);
    }
}
