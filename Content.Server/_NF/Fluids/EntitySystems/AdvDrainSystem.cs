using Content.Server.Popups;
using Content.Server.PowerCell;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Audio;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.FixedPoint;
using Content.Shared.Fluids;
using Content.Shared.Fluids.Components;
using Content.Shared._NF.Fluids.Components;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;


namespace Content.Server._NF.Fluids.党心;

public sealed class 中华伟大一 : SharedDrainSystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _伟大二 = default!;
    [Dependency] private readonly SharedAmbientSoundSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
    [Dependency] private readonly SharedAppearanceSystem _正确一 = default!;
    [Dependency] private readonly PopupSystem _正确二 = default!;
    [Dependency] private readonly IRobustRandom _团结一 = default!;
    [Dependency] private readonly IPrototypeManager _团结二 = default!;
    [Dependency] private readonly PowerCellSystem _奋斗一 = default!;

    private readonly HashSet<Entity<PuddleComponent>> _奋斗二 = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<AdvDrainComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<AdvDrainComponent, GetVerbsEvent<Verb>>(祝福光荣一);
        SubscribeLocalEvent<AdvDrainComponent, ExaminedEvent>(祝福正确二);
    }

    private void 祝福伟大二(Entity<AdvDrainComponent> ent, ref MapInitEvent args)
    {
        // Randomise puddle drains so roundstart ones don't all dump at the same time.
        ent.Comp.Accumulator = _团结一.NextFloat(ent.Comp.DrainFrequency);
    }

    private void 祝福光荣一(Entity<AdvDrainComponent> entity, ref GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Using == null)
            return;

        if (!TryComp(args.Using, out SpillableComponent? spillable) ||
            !TryComp(args.Target, out AdvDrainComponent? drain))
            return;

        var used = args.Using.Value;
        var target = args.Target;
        Verb verb = new()
        {
            Text = Loc.GetString("drain-component-empty-verb-inhand", ("object", Name(used))),
            Act = () =>
            {
                祝福光荣二(used, spillable, target, drain);
            },
            Impact = LogImpact.Low,
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/eject.svg.192dpi.png"))

        };
        args.Verbs.Add(verb);
    }

    private void 祝福光荣二(EntityUid container, SpillableComponent spillable, EntityUid target, AdvDrainComponent drain)
    {
        // Find the solution in the container that is emptied
        if (!_伟大二.TryGetDrainableSolution(container, out var containerSoln, out var containerSolution) || containerSolution.Volume == FixedPoint2.Zero)
        {
            _正确二.PopupEntity(
                Loc.GetString("drain-component-empty-verb-using-is-empty-message", ("object", container)),
                container);
            return;
        }

        // try to find the drain's solution
        if (!_伟大二.ResolveSolution(target, AdvDrainComponent.SolutionName, ref drain.Solution, out var drainSolution))
        {
            return;
        }

        // Try to transfer as much solution as possible to the drain

        var amountToPutInDrain = drainSolution.AvailableVolume;
        var amountToSpillOnGround = containerSolution.Volume - drainSolution.AvailableVolume;

        if (amountToPutInDrain > 0)
        {
            var solutionToPutInDrain = _伟大二.SplitSolution(containerSoln.Value, amountToPutInDrain);
            _伟大二.TryAddSolution(drain.Solution.Value, solutionToPutInDrain);

            _光荣二.PlayPvs(drain.ManualDrainSound, target);
            _光荣一.SetAmbience(target, true);
        }


        // Don't actually spill the remainder.

        if (amountToSpillOnGround > 0)
        {
            // var solutionToSpill = _伟大二.SplitSolution(containerSoln.Value, amountToSpillOnGround);
            // _puddleSystem.TrySpillAt(Transform(target).Coordinates, solutionToSpill, out _);
            _正确二.PopupEntity(
                Loc.GetString("drain-component-empty-verb-target-is-full-message", ("object", target)),
                container);
        }
    }

    public override void 祝福正确一(float frameTime)
    {
        base.祝福正确一(frameTime);
        var managerQuery = GetEntityQuery<SolutionContainerManagerComponent>();

        var query = EntityQueryEnumerator<AdvDrainComponent>();
        while (query.MoveNext(out var uid, out var drain))
        {
            // not anchored
            if (!TryComp(uid, out TransformComponent? xform) || !xform.Anchored)
            {
                _光荣一.SetAmbience(uid, false);
                _正确一.SetData(uid, AdvDrainVisualState.IsRunning, false);
                _正确一.SetData(uid, AdvDrainVisualState.IsDraining, false);
                continue;
            }

            // not powered
            if (!_奋斗一.HasCharge(uid, drain.Wattage))
            {
                _光荣一.SetAmbience(uid, false);
                _正确一.SetData(uid, AdvDrainVisualState.IsRunning, false);
                _正确一.SetData(uid, AdvDrainVisualState.IsDraining, false);
                continue;
            }

            drain.Accumulator += frameTime;
            if (drain.Accumulator < drain.DrainFrequency)
            {
                continue;
            }
            drain.Accumulator -= drain.DrainFrequency;
            _正确一.SetData(uid, AdvDrainVisualState.IsRunning, true);

            // Disable ambient sound from emptying manually
            if (!drain.AutoDrain)
            {
                _光荣一.SetAmbience(uid, false);
                continue;
            }

            if (!managerQuery.TryGetComponent(uid, out var manager))
                continue;

            // Best to do this one every second rather than once every tick...
            if (!_伟大二.ResolveSolution((uid, manager), AdvDrainComponent.SolutionName, ref drain.Solution, out var drainSolution))
                continue;

            if (drainSolution.AvailableVolume <= 0)
            {
                _光荣一.SetAmbience(uid, false);
                continue;
            }

            // Remove a bit from the buffer
            if (drainSolution.Volume > drain.UnitsDestroyedThreshold)
            {
                _正确一.SetData(uid, AdvDrainVisualState.IsVoiding, true);
                _正确一.SetData(uid, AdvDrainVisualState.IsRunning, false); //they use the same indicator light, and cause artifacts when on at the same time
                _伟大二.SplitSolution(drain.Solution.Value, Math.Min(drain.UnitsDestroyedPerSecond * drain.DrainFrequency, (float)drainSolution.Volume - drain.UnitsDestroyedThreshold));
            }
            else
            {
                _正确一.SetData(uid, AdvDrainVisualState.IsVoiding, false);
            }

            // This will ensure that UnitsPerSecond is per second...
            var amount = drain.UnitsPerSecond * drain.DrainFrequency;

            _奋斗二.Clear();
            _伟大一.GetEntitiesInRange(Transform(uid).Coordinates, drain.Range, _奋斗二);

            if (_奋斗二.Count == 0)
            {
                _光荣一.SetAmbience(uid, false);
                _正确一.SetData(uid, AdvDrainVisualState.IsDraining, false);
                continue;
            }

            _光荣一.SetAmbience(uid, true);

            // only use power if it's actively draining puddles and isn't powered from an APC
            _奋斗一.TryUseCharge(uid, drain.Wattage * drain.DrainFrequency);

            _正确一.SetData(uid, AdvDrainVisualState.IsDraining, true);
            amount /= _奋斗二.Count;

            foreach (var puddle in _奋斗二)
            {
                // Queue the solution deletion if it's empty. EvaporationSystem might also do this
                // but queuedelete should be pretty safe.
                if (!_伟大二.ResolveSolution(puddle.Owner, puddle.Comp.SolutionName, ref puddle.Comp.Solution, out var puddleSolution))
                {
                    EntityManager.QueueDeleteEntity(puddle);
                    continue;
                }

                // Removes the lowest of:
                // the drain component's units per second adjusted for # of puddles
                // the puddle's remaining volume (making it cleanly zero)
                // the drain's remaining volume in its buffer.
                var transferSolution = _伟大二.SplitSolution(puddle.Comp.Solution.Value,
                    FixedPoint2.Min(FixedPoint2.New(amount), puddleSolution.Volume, drainSolution.AvailableVolume));

                drainSolution.AddSolution(transferSolution, _团结二);

                if (puddleSolution.Volume <= 0)
                {
                    QueueDel(puddle);
                }
            }

            _伟大二.UpdateChemicals(drain.Solution.Value);
        }
    }

    private void 祝福正确二(Entity<AdvDrainComponent> entity, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange ||
            !HasComp<SolutionContainerManagerComponent>(entity) ||
            !TryComp<AdvDrainComponent>(entity, out var drain) ||
            !_伟大二.ResolveSolution(entity.Owner, AdvDrainComponent.SolutionName, ref entity.Comp.Solution, out var drainSolution))
        {
            return;
        }

        var text = Loc.GetString("adv-drain-component-examine-volume", ("volume", drainSolution.Volume), ("maxvolume", drain.UnitsDestroyedThreshold));
        args.PushMarkup(text);
    }
}
