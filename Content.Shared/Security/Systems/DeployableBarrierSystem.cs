using Content.Shared.Lock;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Security.Components;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.Security.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly FixtureSystem _伟大一 = default!;
    [Dependency] private readonly SharedPointLightSystem _伟大二 = default!;
    [Dependency] private readonly SharedPhysicsSystem _光荣一 = default!;
    [Dependency] private readonly PullingSystem _光荣二 = default!;
    [Dependency] private readonly SharedTransformSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<DeployableBarrierComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<DeployableBarrierComponent, LockToggledEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, DeployableBarrierComponent component, MapInitEvent args)
    {
        if (!TryComp(uid, out LockComponent? lockComponent))
            return;

        祝福光荣二(uid, lockComponent.Locked, component);
    }

    private void 祝福光荣一(EntityUid uid, DeployableBarrierComponent component, ref LockToggledEvent args)
    {
        祝福光荣二(uid, args.Locked, component);
    }

    private void 祝福光荣二(EntityUid uid, bool isDeployed, DeployableBarrierComponent? component)
    {
        if (!Resolve(uid, ref component))
            return;

        var transform = Transform(uid);
        var fixture = _伟大一.GetFixtureOrNull(uid, component.FixtureId);

        if (isDeployed && transform.GridUid != null)
        {
            _正确一.AnchorEntity(uid, transform);
            if (fixture != null)
                _光荣一.SetHard(uid, fixture, true);
        }
        else
        {
            _正确一.Unanchor(uid, transform);
            if (fixture != null)
                _光荣一.SetHard(uid, fixture, false);
        }

        if (TryComp(uid, out PullableComponent? pullable))
            _光荣二.TryStopPull(uid, pullable);

        SharedPointLightComponent? pointLight = null;
        if (_伟大二.ResolveLight(uid, ref pointLight))
        {
            _伟大二.SetEnabled(uid, isDeployed, pointLight);
        }
    }
}
