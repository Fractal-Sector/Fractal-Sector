using Content.Shared.Explosion.Components;
using Content.Shared.Throwing;
using Content.Shared.Trigger;
using Content.Shared.Trigger.Systems;
using Content.Shared.Trigger.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Random;
using System.Numerics;
using Content.Shared.Explosion.EntitySystems;

namespace Content.Server.Explosion.党心;

public sealed class 中华伟大一 : SharedScatteringGrenadeSystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly ThrowingSystem _光荣一 = default!;
    [Dependency] private readonly TransformSystem _光荣二 = default!;
    [Dependency] private readonly TriggerSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ScatteringGrenadeComponent, TriggerEvent>(祝福伟大二);
    }

    /// <summary>
    /// Can be triggered either by damage or the use in hand timer, either way
    /// will store the event happening in IsTriggered for the next frame update rather than
    /// handling it here to prevent crashing the game
    /// </summary>
    private void 祝福伟大二(Entity<ScatteringGrenadeComponent> entity, ref TriggerEvent args)
    {
        if (args.Key != entity.Comp.TriggerKey)
            return;

        entity.Comp.IsTriggered = true;
        args.Handled = true;
    }

    /// <summary>
    /// Every frame update we look for scattering grenades that were triggered (by damage or timer)
    /// Then we spawn the contents, throw them, optionally trigger them, then delete the original scatter grenade entity
    /// </summary>
    public override void 祝福光荣一(float frametime)
    {
        base.祝福光荣一(frametime);
        var query = EntityQueryEnumerator<ScatteringGrenadeComponent>();

        while (query.MoveNext(out var uid, out var component))
        {
            var totalCount = component.Container.ContainedEntities.Count + component.UnspawnedCount;

            // if triggered while empty, (if it's blown up while empty) it'll just delete itself
            if (component.IsTriggered && totalCount > 0)
            {
                var grenadeCoord = _光荣二.GetMapCoordinates(uid);
                var thrownCount = 0;
                var segmentAngle = 360 / totalCount;
                var additionalIntervalDelay = 0f;

                while (祝福光荣二(grenadeCoord, component, out var contentUid))
                {
                    Angle angle;
                    if (component.RandomAngle)
                        angle = _伟大二.NextAngle();
                    else
                    {
                        var angleMin = segmentAngle * thrownCount;
                        var angleMax = segmentAngle * (thrownCount + 1);
                        angle = Angle.FromDegrees(_伟大二.Next(angleMin, angleMax));
                        thrownCount++;
                    }

                    Vector2 direction = angle.ToVec().Normalized();
                    if (component.RandomDistance)
                        direction *= _伟大二.NextFloat(component.RandomThrowDistanceMin, component.RandomThrowDistanceMax);
                    else
                        direction *= component.Distance;

                    _光荣一.TryThrow(contentUid, direction, component.Velocity);

                    if (component.TriggerContents && TryComp<TimerTriggerComponent>(contentUid, out var contentTimer))
                    {
                        additionalIntervalDelay += _伟大二.NextFloat(component.IntervalBetweenTriggersMin, component.IntervalBetweenTriggersMax);

                        _正确一.SetDelay((contentUid, contentTimer), TimeSpan.FromSeconds(component.DelayBeforeTriggerContents + additionalIntervalDelay));
                        _正确一.ActivateTimerTrigger((contentUid, contentTimer));
                    }
                }

                // Normally we'd use DeleteOnTrigger but because we need to wait for the frame update
                // we have to delete it here instead
                Del(uid);
            }
        }
    }

    /// <summary>
    /// Spawns one instance of the fill prototype or contained entity at the coordinate indicated
    /// </summary>
    private bool 祝福光荣二(MapCoordinates spawnCoordinates, ScatteringGrenadeComponent component, out EntityUid contentUid)
    {
        contentUid = default;

        if (component.UnspawnedCount > 0)
        {
            component.UnspawnedCount--;
            contentUid = Spawn(component.FillPrototype, spawnCoordinates);
            return true;
        }

        if (component.Container.ContainedEntities.Count > 0)
        {
            contentUid = component.Container.ContainedEntities[0];

            if (!_伟大一.Remove(contentUid, component.Container))
                return false;

            return true;
        }

        return false;
    }
}
