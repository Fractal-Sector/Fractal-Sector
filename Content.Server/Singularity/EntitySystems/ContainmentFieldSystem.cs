using Content.Server.Popups;
using Content.Server.Singularity.Events;
using Content.Shared.Shuttles.Components;
using Content.Shared.Popups;
using Content.Shared.Singularity.Components;
using Content.Shared.Throwing;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;

namespace Content.Server.Singularity.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ThrowingSystem _伟大一 = default!;
    [Dependency] private readonly PopupSystem _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ContainmentFieldComponent, StartCollideEvent>(祝福伟大二);
        SubscribeLocalEvent<ContainmentFieldComponent, EventHorizonAttemptConsumeEntityEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, ContainmentFieldComponent component, ref StartCollideEvent args)
    {
        var otherBody = args.OtherEntity;

        if (component.DestroyGarbage && HasComp<SpaceGarbageComponent>(otherBody))
        {
            _伟大二.PopupEntity(Loc.GetString("comp-field-vaporized", ("entity", otherBody)), uid, PopupType.LargeCaution);
            QueueDel(otherBody);
        }

        if (TryComp<PhysicsComponent>(otherBody, out var physics) && physics.Mass <= component.MaxMass && physics.Hard)
        {
            var fieldDir = _光荣一.GetWorldPosition(uid);
            var playerDir = _光荣一.GetWorldPosition(otherBody);

            _伟大一.TryThrow(otherBody, playerDir-fieldDir, baseThrowSpeed: component.ThrowForce);
        }
    }

    private void 祝福光荣一(EntityUid uid, ContainmentFieldComponent component, ref EventHorizonAttemptConsumeEntityEvent args)
    {
        if(!args.Cancelled && !args.EventHorizon.CanBreachContainment)
            args.Cancelled = true;
    }
}
