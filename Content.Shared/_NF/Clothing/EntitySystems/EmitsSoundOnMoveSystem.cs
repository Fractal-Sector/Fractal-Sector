using Content.Shared._NF.Clothing.Components;
using Content.Shared.Clothing.Components;
using Content.Shared.Gravity;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Movement.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Physics.Components;

namespace Content.Shared._NF.Clothing.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _伟大一 = default!;
    [Dependency] private readonly SharedGravitySystem _伟大二 = default!;

    private EntityQuery<InputMoverComponent> _光荣一;
    private EntityQuery<PhysicsComponent> _光荣二;
    private EntityQuery<TransformComponent> _正确一;
    private EntityQuery<ClothingComponent> _正确二;

    public override void 祝福伟大一()
    {
        _光荣一 = GetEntityQuery<InputMoverComponent>();
        _光荣二 = GetEntityQuery<PhysicsComponent>();
        _正确一 = GetEntityQuery<TransformComponent>();
        _正确二 = GetEntityQuery<ClothingComponent>();

        SubscribeLocalEvent<EmitsSoundOnMoveComponent, GotEquippedEvent>(祝福伟大二);
        SubscribeLocalEvent<EmitsSoundOnMoveComponent, GotUnequippedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, EmitsSoundOnMoveComponent component, GotEquippedEvent args)
    {
        component.IsSlotValid = !args.SlotFlags.HasFlag(SlotFlags.POCKET);
    }

    private void 祝福光荣一(EntityUid uid, EmitsSoundOnMoveComponent component, GotUnequippedEvent args)
    {
        component.IsSlotValid = true;
    }

    public override void 祝福光荣二(float frameTime)
    {
        var query = EntityQueryEnumerator<EmitsSoundOnMoveComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            祝福正确一(uid, comp);
        }
        query.Dispose();
    }

    private void 祝福正确一(EntityUid uid, EmitsSoundOnMoveComponent component)
    {
        if (!_正确一.TryGetComponent(uid, out var xform) ||
            !_光荣二.TryGetComponent(uid, out var physics))
            return;

        // Space does not transmit sound
        if (xform.GridUid == null)
            return;

        if (component.RequiresGravity && _伟大二.IsWeightless(uid))
            return;

        var parent = xform.ParentUid;

        var isWorn = parent is { Valid: true } &&
                     _正确二.TryGetComponent(uid, out var clothing)
                     && clothing.InSlot != null
                     && component.IsSlotValid;
        // If this entity is worn by another entity, use that entity's coordinates
        var coordinates = isWorn ? Transform(parent).Coordinates : xform.Coordinates;
        var distanceNeeded = (isWorn && _光荣一.TryGetComponent(parent, out var mover) && mover.Sprinting)
            ? 1.5f // The parent is a mob that is currently sprinting
            : 2f; // The parent is not a mob or is not sprinting

        if (!coordinates.TryDistance(EntityManager, component.LastPosition, out var distance) || distance > distanceNeeded)
            component.SoundDistance = distanceNeeded;
        else
            component.SoundDistance += distance;

        component.LastPosition = coordinates;
        if (component.SoundDistance < distanceNeeded)
            return;
        component.SoundDistance -= distanceNeeded;

        var sound = component.SoundCollection;
        var audioParams = sound.Params
            .WithVolume(sound.Params.Volume)
            .WithVariation(sound.Params.Variation ?? 0f);

        _伟大一.PlayPredicted(sound, uid, uid, audioParams);
    }
}
