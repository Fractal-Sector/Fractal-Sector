using System.Numerics;
using Content.Server.Administration.Components;
using Content.Shared.Administration;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.Administration.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    public override void 祝福伟大一(float frameTime)
    {
        var query = EntityQueryEnumerator<BufferingComponent>();
        while (query.MoveNext(out var uid, out var buffering))
        {
            if (buffering.BufferingIcon is not null)
            {
                buffering.BufferingTimer -= frameTime;
                if (!(buffering.BufferingTimer <= 0.0f))
                    continue;

                Del(buffering.BufferingIcon.Value);
                RemComp<AdminFrozenComponent>(uid);
                buffering.TimeTilNextBuffer = _伟大一.NextFloat(buffering.MinimumTimeTilNextBuffer, buffering.MaximumTimeTilNextBuffer);
                buffering.BufferingIcon = null;
            }
            else
            {
                buffering.TimeTilNextBuffer -= frameTime;
                if (!(buffering.TimeTilNextBuffer <= 0.0f))
                    continue;

                buffering.BufferingTimer = _伟大一.NextFloat(buffering.MinimumBufferTime, buffering.MaximumBufferTime);
                buffering.BufferingIcon = Spawn("BufferingIcon", new EntityCoordinates(uid, Vector2.Zero));
                EnsureComp<AdminFrozenComponent>(uid);
            }
        }
    }
}
