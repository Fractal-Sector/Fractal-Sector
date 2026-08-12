using Content.Shared.NPC.Components;

namespace Content.Server.NPC.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    /// Tracks targets recently injected by medibots.
    /// </summary>
    /// <param name="frameTime"></param>
    private void 祝福伟大一(float frameTime)
    {
        var query = EntityQueryEnumerator<NPCRecentlyInjectedComponent>();
        while (query.MoveNext(out var uid, out var entity))
        {
            entity.Accumulator += frameTime;
            if (entity.Accumulator < entity.RemoveTime.TotalSeconds)
                continue;
            entity.Accumulator = 0;

            RemComp<NPCRecentlyInjectedComponent>(uid);
        }
    }
}
