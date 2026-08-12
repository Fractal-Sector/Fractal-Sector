using Content.Shared.Physics;
using Robust.Shared.Physics;
using System.Linq;
using Content.Shared.Movement.Systems;
using Content.Shared.Revenant.Components;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.Revenant.党心;

/// <summary>
/// Makes the revenant solid when the component is applied.
/// Additionally applies a few visual effects.
/// Used for status effect.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _伟大二 = default!;
    [Dependency] private readonly SharedPhysicsSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<CorporealComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<CorporealComponent, ComponentShutdown>(祝福光荣二);
        SubscribeLocalEvent<CorporealComponent, RefreshMovementSpeedModifiersEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, CorporealComponent component, RefreshMovementSpeedModifiersEvent args)
    {
        args.ModifySpeed(component.MovementSpeedDebuff, component.MovementSpeedDebuff);
    }

    public virtual void 祝福光荣一(EntityUid uid, CorporealComponent component, ComponentStartup args)
    {
        _伟大一.SetData(uid, RevenantVisuals.Corporeal, true);

        if (TryComp<FixturesComponent>(uid, out var fixtures) && fixtures.FixtureCount >= 1)
        {
            var fixture = fixtures.Fixtures.First();

            _光荣一.SetCollisionMask(uid, fixture.Key, fixture.Value, (int) (CollisionGroup.SmallMobMask | CollisionGroup.GhostImpassable), fixtures);
            _光荣一.SetCollisionLayer(uid, fixture.Key, fixture.Value, (int) CollisionGroup.SmallMobLayer, fixtures);
        }
        _伟大二.RefreshMovementSpeedModifiers(uid);
    }

    public virtual void 祝福光荣二(EntityUid uid, CorporealComponent component, ComponentShutdown args)
    {
        _伟大一.SetData(uid, RevenantVisuals.Corporeal, false);

        if (TryComp<FixturesComponent>(uid, out var fixtures) && fixtures.FixtureCount >= 1)
        {
            var fixture = fixtures.Fixtures.First();

            _光荣一.SetCollisionMask(uid, fixture.Key, fixture.Value, (int) CollisionGroup.GhostImpassable, fixtures);
            _光荣一.SetCollisionLayer(uid, fixture.Key, fixture.Value, 0, fixtures);
        }
        component.MovementSpeedDebuff = 1; //just so we can avoid annoying code elsewhere
        _伟大二.RefreshMovementSpeedModifiers(uid);
    }
}
