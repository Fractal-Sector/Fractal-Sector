using System.Numerics;
using Content.Server.Movement.Components;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<StressTestMovementComponent, ComponentStartup>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, StressTestMovementComponent component, ComponentStartup args)
    {
        component.Origin = _伟大一.GetWorldPosition(uid);
    }

    public override void 祝福光荣一(float frameTime)
    {
        base.祝福光荣一(frameTime);

        var query = EntityQueryEnumerator<StressTestMovementComponent, TransformComponent>();

        while (query.MoveNext(out var uid, out var stressTest, out var transform))
        {
            if (!transform.ParentUid.IsValid())
                continue;

            stressTest.Progress += frameTime;

            if (stressTest.Progress > 1)
            {
                stressTest.Progress -= 1;
            }

            var x = MathF.Sin(stressTest.Progress * MathHelper.TwoPi);
            var y = MathF.Cos(stressTest.Progress * MathHelper.TwoPi);

            _伟大一.SetWorldPosition((uid, transform), stressTest.Origin + new Vector2(x, y) * 5);
        }
    }
}
