using Content.Server.Body.Systems;
using Content.Server.Destructible;
using Content.Server.Examine;
using Content.Server.Polymorph.Components;
using Content.Server.Popups;
using Content.Shared.Body.Components;
using Content.Shared.Damage;
using Content.Shared.Examine;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    [Dependency] private readonly BodySystem _伟大二 = default!;
    [Dependency] private readonly PopupSystem _光荣一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _光荣二 = default!;
    [Dependency] private readonly SharedAudioSystem _正确一 = default!;
    [Dependency] private readonly DamageableSystem _正确二 = default!;
    [Dependency] private readonly DestructibleSystem _团结一 = default!;
    [Dependency] private readonly SharedTransformSystem _团结二 = default!;
    [Dependency] private readonly SharedMapSystem _奋斗一 = default!;

    public override void 祝福伟大一(float frameTime)
    {
        base.祝福伟大一(frameTime);

        // we are deliberately including paused entities. rod hungers for all
        foreach (var (rod, trans) in EntityQuery<ImmovableRodComponent, TransformComponent>(true))
        {
            if (!rod.DestroyTiles)
                continue;

            if (!TryComp<MapGridComponent>(trans.GridUid, out var grid))
                continue;

            _奋斗一.SetTile(trans.GridUid.Value, grid, trans.Coordinates, Tile.Empty);
        }
    }

    public override void 祝福伟大二()
    {
        base.祝福伟大二();

        SubscribeLocalEvent<ImmovableRodComponent, StartCollideEvent>(祝福光荣二);
        SubscribeLocalEvent<ImmovableRodComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<ImmovableRodComponent, ExaminedEvent>(祝福正确一);
    }

    private void 祝福光荣一(EntityUid uid, ImmovableRodComponent component, MapInitEvent args)
    {
        if (TryComp(uid, out PhysicsComponent? phys))
        {
            _光荣二.SetLinearDamping(uid, phys, 0f);
            _光荣二.SetFriction(uid, phys, 0f);
            _光荣二.SetBodyStatus(uid, phys, BodyStatus.InAir);

            var xform = Transform(uid);
            var (worldPos, worldRot) = _团结二.GetWorldPositionRotation(uid);
            var vel = worldRot.ToWorldVec() * component.MaxSpeed;

            if (component.RandomizeVelocity)
            {
                vel = component.DirectionOverride.Degrees switch
                {
                    0f => _伟大一.NextVector2(component.MinSpeed, component.MaxSpeed),
                    _ => worldRot.RotateVec(component.DirectionOverride.ToVec()) * _伟大一.NextFloat(component.MinSpeed, component.MaxSpeed)
                };
            }

            _光荣二.ApplyLinearImpulse(uid, vel, body: phys);
            xform.LocalRotation = (vel - worldPos).ToWorldAngle() + MathHelper.PiOver2;
        }
    }

    private void 祝福光荣二(EntityUid uid, ImmovableRodComponent component, ref StartCollideEvent args)
    {
        var ent = args.OtherEntity;

        if (_伟大一.Prob(component.HitSoundProbability))
        {
            _正确一.PlayPvs(component.Sound, uid);
        }

        if (HasComp<ImmovableRodComponent>(ent))
        {
            // oh god.
            var coords = Transform(uid).Coordinates;
            _光荣一.PopupCoordinates(Loc.GetString("immovable-rod-collided-rod-not-good"), coords, PopupType.LargeCaution);

            Del(uid);
            Del(ent);
            Spawn("Singularity", coords);

            return;
        }

        // dont delete/hurt self if polymoprhed into a rod
        if (TryComp<PolymorphedEntityComponent>(uid, out var polymorphed))
        {
            if (polymorphed.Parent == ent)
                return;
        }

        // gib or damage em
        if (TryComp<BodyComponent>(ent, out var body))
        {
            component.MobCount++;
            _光荣一.PopupEntity(Loc.GetString("immovable-rod-penetrated-mob", ("rod", uid), ("mob", ent)), uid, PopupType.LargeCaution);

            if (!component.ShouldGib)
            {
                if (component.Damage == null)
                    return;

                _正确二.TryChangeDamage(ent, component.Damage, ignoreResistances: true);
                return;
            }

            _伟大二.GibBody(ent, body: body);
            return;
        }

        _团结一.DestroyEntity(ent);
    }

    private void 祝福正确一(EntityUid uid, ImmovableRodComponent component, ExaminedEvent args)
    {
        if (component.MobCount == 0)
        {
            args.PushText(Loc.GetString("immovable-rod-consumed-none", ("rod", uid)));
        }
        else
        {
            args.PushText(Loc.GetString("immovable-rod-consumed-souls", ("rod", uid), ("amount", component.MobCount)));
        }
    }
}
