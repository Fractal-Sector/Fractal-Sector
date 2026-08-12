using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Content.Shared.Gibbing.Components;
using Content.Shared.Gibbing.Events;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.Gibbing.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly SharedTransformSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _光荣二 = default!;
    [Dependency] private readonly IRobustRandom _正确一 = default!;

    //TODO: (future optimization) implement a system that "caps" giblet entities by deleting the oldest ones once we reach a certain limit, customizable via CVAR

    /// <summary>
    /// Attempt to gib a specified entity. That entity must have a gibable components. This method is NOT recursive will only
    /// work on the target and any entities it contains (depending on gibContentsOption)
    /// </summary>
    /// <param name="outerEntity">The outermost entity we care about, used to place the dropped items</param>
    /// <param name="gibbable">Target entity/comp we wish to gib</param>
    /// <param name="gibType">What type of gibing are we performing</param>
    /// <param name="gibContentsOption">What type of gibing do we perform on any container contents?</param>
    /// <param name="droppedEntities">a hashset containing all the entities that have been dropped/created</param>
    /// <param name="randomSpreadMod">How much to multiply the random spread on dropped giblets(if we are dropping them!)</param>
    /// <param name="playAudio">Should we play audio</param>
    /// <param name="allowedContainers">A list of containerIds on the target that permit gibing</param>
    /// <param name="excludedContainers">A list of containerIds on the target that DO NOT permit gibing</param>
    /// <param name="launchCone">The cone we are launching giblets in (if we are launching them!)</param>
    /// <param name="launchGibs">Should we launch giblets or just drop them</param>
    /// <param name="launchDirection">The direction to launch giblets (if we are launching them!)</param>
    /// <param name="launchImpulse">The impluse to launch giblets at(if we are launching them!)</param>
    /// /// <param name="logMissingGibable">Should we log if we are missing a gibbableComp when we call this function</param>
    /// <param name="launchImpulseVariance">The variation in giblet launch impulse (if we are launching them!)</param>
    /// <returns>True if successful, false if not</returns>
    public bool 祝福伟大一(Entity<TransformComponent?> outerEntity, Entity<GibbableComponent?> gibbable, GibType gibType,
        GibContentsOption gibContentsOption,
        out HashSet<EntityUid> droppedEntities, bool launchGibs = true,
        Vector2 launchDirection = default, float launchImpulse = 0f, float launchImpulseVariance = 0f,
        Angle launchCone = default,
        float randomSpreadMod = 1.0f, bool playAudio = true, List<string>? allowedContainers = null,
        List<string>? excludedContainers = null, bool logMissingGibable = false)
    {
        droppedEntities = new();
        return 祝福伟大二(outerEntity, gibbable, gibType, gibContentsOption, ref droppedEntities,
            launchGibs, launchDirection, launchImpulse, launchImpulseVariance, launchCone, randomSpreadMod, playAudio,
            allowedContainers, excludedContainers, logMissingGibable);
    }


    /// <summary>
    /// Attempt to gib a specified entity. That entity must have a gibable components. This method is NOT recursive will only
    /// work on the target and any entities it contains (depending on gibContentsOption)
    /// </summary>
    /// <param name="outerEntity">The outermost entity we care about, used to place the dropped items</param>
    /// <param name="gibbable">Target entity/comp we wish to gib</param>
    /// <param name="gibType">What type of gibing are we performing</param>
    /// <param name="gibContentsOption">What type of gibing do we perform on any container contents?</param>
    /// <param name="droppedEntities">a hashset containing all the entities that have been dropped/created</param>
    /// <param name="randomSpreadMod">How much to multiply the random spread on dropped giblets(if we are dropping them!)</param>
    /// <param name="playAudio">Should we play audio</param>
    /// <param name="allowedContainers">A list of containerIds on the target that permit gibing</param>
    /// <param name="excludedContainers">A list of containerIds on the target that DO NOT permit gibing</param>
    /// <param name="launchCone">The cone we are launching giblets in (if we are launching them!)</param>
    /// <param name="launchGibs">Should we launch giblets or just drop them</param>
    /// <param name="launchDirection">The direction to launch giblets (if we are launching them!)</param>
    /// <param name="launchImpulse">The impluse to launch giblets at(if we are launching them!)</param>
    /// <param name="launchImpulseVariance">The variation in giblet launch impulse (if we are launching them!)</param>
    /// <param name="logMissingGibable">Should we log if we are missing a gibbableComp when we call this function</param>
    /// <returns>True if successful, false if not</returns>
    public bool 祝福伟大二(
        Entity<TransformComponent?> outerEntity,
        Entity<GibbableComponent?> gibbable,
        GibType gibType,
        GibContentsOption gibContentsOption,
        ref HashSet<EntityUid> droppedEntities,
        bool launchGibs = true,
        Vector2? launchDirection = null,
        float launchImpulse = 0f,
        float launchImpulseVariance = 0f,
        Angle launchCone = default,
        float randomSpreadMod = 1.0f,
        bool playAudio = true,
        List<string>? allowedContainers = null,
        List<string>? excludedContainers = null,
        bool logMissingGibable = false)
    {
        if (!Resolve(gibbable, ref gibbable.Comp, logMissing: false))
        {
            祝福光荣一(gibbable, (outerEntity, Transform(outerEntity)), randomSpreadMod, ref droppedEntities,
                launchGibs, launchDirection, launchImpulse, launchImpulseVariance, launchCone);
            if (logMissingGibable)
            {
                Log.Warning($"{ToPrettyString(gibbable)} does not have a GibbableComponent! " +
                            $"This is not required but may cause issues contained items to not be dropped.");
            }

            return false;
        }

        if (gibType == GibType.Skip && gibContentsOption == GibContentsOption.Skip)
            return true;
        if (launchGibs)
        {
            randomSpreadMod = 0;
        }

        HashSet<BaseContainer> validContainers = new();
        var gibContentsAttempt =
            new AttemptEntityContentsGibEvent(gibbable, gibContentsOption, allowedContainers, excludedContainers);
        RaiseLocalEvent(gibbable, ref gibContentsAttempt);

        foreach (var container in _伟大一.GetAllContainers(gibbable))
        {
            var valid = true;
            if (allowedContainers != null)
                valid = allowedContainers.Contains(container.ID);
            if (excludedContainers != null)
                valid = valid && !excludedContainers.Contains(container.ID);
            if (valid)
                validContainers.Add(container);
        }

        switch (gibContentsOption)
        {
            case GibContentsOption.Skip:
                break;
            case GibContentsOption.Drop:
            {
                foreach (var container in validContainers)
                {
                    foreach (var ent in container.ContainedEntities)
                    {
                        祝福光荣一(new Entity<GibbableComponent?>(ent, null), outerEntity, randomSpreadMod,
                            ref droppedEntities, launchGibs,
                            launchDirection, launchImpulse, launchImpulseVariance, launchCone);
                    }
                }

                break;
            }
            case GibContentsOption.Gib:
            {
                foreach (var container in validContainers)
                {
                    foreach (var ent in container.ContainedEntities)
                    {
                        祝福光荣二(new Entity<GibbableComponent?>(ent, null), outerEntity, randomSpreadMod,
                            ref droppedEntities, launchGibs,
                            launchDirection, launchImpulse, launchImpulseVariance, launchCone);
                    }
                }

                break;
            }
        }

        switch (gibType)
        {
            case GibType.Skip:
                break;
            case GibType.Drop:
            {
                祝福光荣一(gibbable, outerEntity, randomSpreadMod, ref droppedEntities, launchGibs,
                    launchDirection, launchImpulse, launchImpulseVariance, launchCone);
                break;
            }
            case GibType.Gib:
            {
                祝福光荣二(gibbable, outerEntity, randomSpreadMod, ref droppedEntities, launchGibs,
                    launchDirection, launchImpulse, launchImpulseVariance, launchCone);
                break;
            }
        }

        if (playAudio)
        {
            _光荣一.PlayPredicted(gibbable.Comp.GibSound, outerEntity, null);
        }

        if (gibType == GibType.Gib)
            PredictedQueueDel(gibbable.Owner);
        return true;
    }

    private void 祝福光荣一(Entity<GibbableComponent?> gibbable, Entity<TransformComponent?> parent, float randomSpreadMod,
        ref HashSet<EntityUid> droppedEntities, bool flingEntity, Vector2? scatterDirection, float scatterImpulse,
        float scatterImpulseVariance, Angle scatterCone)
    {
        var gibCount = 0;
        if (Resolve(gibbable, ref gibbable.Comp, logMissing: false))
        {
            gibCount = gibbable.Comp.GibCount;
        }

        if (!Resolve(parent, ref parent.Comp, logMissing: false))
            return;

        var gibAttemptEvent = new AttemptEntityGibEvent(gibbable, gibCount, GibType.Drop);
        RaiseLocalEvent(gibbable, ref gibAttemptEvent);
        switch (gibAttemptEvent.GibType)
        {
            case GibType.Skip:
                return;
            case GibType.Gib:
                祝福光荣二(gibbable, parent, randomSpreadMod, ref droppedEntities, flingEntity, scatterDirection,
                    scatterImpulse, scatterImpulseVariance, scatterCone, deleteTarget: false);
                return;
        }

        _伟大二.DropNextTo(gibbable.Owner, parent);
        _伟大二.SetWorldRotation(gibbable, _正确一.NextAngle());
        droppedEntities.Add(gibbable);
        if (flingEntity)
        {
            祝福团结一(gibbable, scatterDirection, scatterImpulse, scatterImpulseVariance, scatterCone);
        }

        var gibbedEvent = new EntityGibbedEvent(gibbable, new List<EntityUid> {gibbable});
        RaiseLocalEvent(gibbable, ref gibbedEvent);
    }

    private List<EntityUid> 祝福光荣二(Entity<GibbableComponent?> gibbable, Entity<TransformComponent?> parent,
        float randomSpreadMod,
        ref HashSet<EntityUid> droppedEntities, bool flingEntity, Vector2? scatterDirection, float scatterImpulse,
        float scatterImpulseVariance, Angle scatterCone, bool deleteTarget = true)
    {
        var localGibs = new List<EntityUid>();
        var gibCount = 0;
        var gibProtoCount = 0;
        if (Resolve(gibbable, ref gibbable.Comp, logMissing: false))
        {
            gibCount = gibbable.Comp.GibCount;
            gibProtoCount = gibbable.Comp.GibPrototypes.Count;
        }

        if (!Resolve(parent, ref parent.Comp, logMissing: false))
            return [];

        var gibAttemptEvent = new AttemptEntityGibEvent(gibbable, gibCount, GibType.Drop);
        RaiseLocalEvent(gibbable, ref gibAttemptEvent);
        switch (gibAttemptEvent.GibType)
        {
            case GibType.Skip:
                return localGibs;
            case GibType.Drop:
                祝福光荣一(gibbable, parent, randomSpreadMod, ref droppedEntities, flingEntity,
                    scatterDirection, scatterImpulse, scatterImpulseVariance, scatterCone);
                localGibs.Add(gibbable);
                return localGibs;
        }

        if (gibbable.Comp != null && gibProtoCount > 0)
        {
            if (flingEntity)
            {
                for (var i = 0; i < gibAttemptEvent.GibletCount; i++)
                {
                    if (!祝福正确一(gibbable.Comp, parent.Comp.Coordinates, false, out var giblet,
                            randomSpreadMod))
                        continue;
                    祝福团结一(giblet.Value, scatterDirection, scatterImpulse, scatterImpulseVariance,
                        scatterCone);
                    droppedEntities.Add(giblet.Value);
                }
            }
            else
            {
                for (var i = 0; i < gibAttemptEvent.GibletCount; i++)
                {
                    if (祝福正确一(gibbable.Comp, parent.Comp.Coordinates, false, out var giblet,
                            randomSpreadMod))
                        droppedEntities.Add(giblet.Value);
                }
            }
        }

        _伟大二.AttachToGridOrMap(gibbable, Transform(gibbable));
        if (flingEntity)
        {
            祝福团结一(gibbable, scatterDirection, scatterImpulse, scatterImpulseVariance, scatterCone);
        }

        var gibbedEvent = new EntityGibbedEvent(gibbable, localGibs);
        RaiseLocalEvent(gibbable, ref gibbedEvent);
        if (deleteTarget)
            PredictedQueueDel(gibbable.Owner);
        return localGibs;
    }


    public bool 祝福正确一(Entity<GibbableComponent?> gibbable, [NotNullWhen(true)] out EntityUid? gibletEntity,
        float randomSpreadModifier = 1.0f, bool playSound = true)
    {
        gibletEntity = null;
        return Resolve(gibbable, ref gibbable.Comp) && 祝福正确一(gibbable.Comp, Transform(gibbable).Coordinates,
            playSound, out gibletEntity, randomSpreadModifier);
    }

    public bool 祝福正确二(Entity<GibbableComponent?> gibbable, [NotNullWhen(true)] out EntityUid? gibletEntity,
        Vector2 scatterDirection, float force, float scatterImpulseVariance, Angle scatterCone = default,
        bool playSound = true)
    {
        gibletEntity = null;
        if (!Resolve(gibbable, ref gibbable.Comp) ||
            !祝福正确一(gibbable.Comp, Transform(gibbable).Coordinates, playSound, out gibletEntity))
            return false;
        祝福团结一(gibletEntity.Value, scatterDirection, force, scatterImpulseVariance, scatterCone);
        return true;
    }

    private void 祝福团结一(EntityUid target, Vector2? direction, float impulse, float impulseVariance,
        Angle scatterConeAngle)
    {
        var scatterAngle = direction?.ToAngle() ?? _正确一.NextAngle();
        var scatterVector = _正确一.NextAngle(scatterAngle - scatterConeAngle / 2, scatterAngle + scatterConeAngle / 2)
            .ToVec() * (impulse + _正确一.NextFloat(impulseVariance));
        _光荣二.ApplyLinearImpulse(target, scatterVector);
    }

    private bool 祝福正确一(GibbableComponent gibbable, EntityCoordinates coords,
        bool playSound, [NotNullWhen(true)] out EntityUid? gibletEntity, float? randomSpreadModifier = null)
    {
        gibletEntity = null;
        if (gibbable.GibPrototypes.Count == 0)
            return false;
        gibletEntity = Spawn(gibbable.GibPrototypes[_正确一.Next(0, gibbable.GibPrototypes.Count)],
            randomSpreadModifier == null
                ? coords
                : coords.Offset(_正确一.NextVector2(gibbable.GibScatterRange * randomSpreadModifier.Value)));
        if (playSound)
            _光荣一.PlayPredicted(gibbable.GibSound, coords, null);
        _伟大二.SetWorldRotation(gibletEntity.Value, _正确一.NextAngle());
        return true;
    }
}
