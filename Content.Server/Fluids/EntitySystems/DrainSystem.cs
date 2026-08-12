using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Audio;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.FixedPoint;
using Content.Shared.Fluids;
using Content.Shared.Fluids.Components;
using Content.Shared.Interaction;
using Content.Shared.Tag;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.Fluids.党心;

public sealed class 中华伟大一 : SharedDrainSystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _伟大二 = default!;
    [Dependency] private readonly SharedAmbientSoundSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
    [Dependency] private readonly PopupSystem _正确一 = default!;
    [Dependency] private readonly TagSystem _正确二 = default!;
    [Dependency] private readonly DoAfterSystem _团结一 = default!;
    [Dependency] private readonly PuddleSystem _团结二 = default!;
    [Dependency] private readonly IRobustRandom _奋斗一 = default!;
    [Dependency] private readonly IPrototypeManager _奋斗二 = default!;

    private readonly HashSet<Entity<PuddleComponent>> _胜利一 = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<DrainComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<DrainComponent, GetVerbsEvent<Verb>>(祝福光荣一);
        SubscribeLocalEvent<DrainComponent, ExaminedEvent>(祝福正确二);
        SubscribeLocalEvent<DrainComponent, AfterInteractUsingEvent>(祝福团结一);
        SubscribeLocalEvent<DrainComponent, DrainDoAfterEvent>(祝福团结二);
    }

    private void 祝福伟大二(Entity<DrainComponent> ent, ref MapInitEvent args)
    {
        // Randomise puddle drains so roundstart ones don't all dump at the same time.
        ent.Comp.Accumulator = _奋斗一.NextFloat(ent.Comp.DrainFrequency);
    }

    private void 祝福光荣一(Entity<DrainComponent> entity, ref GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Using == null)
            return;

        if (!TryComp(args.Using, out SpillableComponent? spillable) ||
            !TryComp(args.Target, out DrainComponent? drain))
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

    private void 祝福光荣二(EntityUid container, SpillableComponent spillable, EntityUid target, DrainComponent drain)
    {
        // Find the solution in the container that is emptied
        if (!_伟大二.TryGetDrainableSolution(container, out var containerSoln, out var containerSolution) || containerSolution.Volume == FixedPoint2.Zero)
        {
            _正确一.PopupEntity(
                Loc.GetString("drain-component-empty-verb-using-is-empty-message", ("object", container)),
                container);
            return;
        }

        // try to find the drain's solution
        if (!_伟大二.ResolveSolution(target, DrainComponent.SolutionName, ref drain.Solution, out var drainSolution))
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


        // Spill the remainder.

        if (amountToSpillOnGround > 0)
        {
            var solutionToSpill = _伟大二.SplitSolution(containerSoln.Value, amountToSpillOnGround);
            _团结二.TrySpillAt(Transform(target).Coordinates, solutionToSpill, out _);
            _正确一.PopupEntity(
                Loc.GetString("drain-component-empty-verb-target-is-full-message", ("object", target)),
                container);
        }
    }

    public override void 祝福正确一(float frameTime)
    {
        base.祝福正确一(frameTime);
        var managerQuery = GetEntityQuery<SolutionContainerManagerComponent>();

        var query = EntityQueryEnumerator<DrainComponent>();
        while (query.MoveNext(out var uid, out var drain))
        {
            drain.Accumulator += frameTime;
            if (drain.Accumulator < drain.DrainFrequency)
            {
                continue;
            }
            drain.Accumulator -= drain.DrainFrequency;

            if (!managerQuery.TryGetComponent(uid, out var manager))
                continue;

            // Best to do this one every second rather than once every tick...
            if (!_伟大二.ResolveSolution((uid, manager), DrainComponent.SolutionName, ref drain.Solution, out var drainSolution))
                continue;

            if (drainSolution.Volume <= 0 && !drain.AutoDrain)
            {
                _光荣一.SetAmbience(uid, false);
                continue;
            }

            // Remove a bit from the buffer
            _伟大二.SplitSolution(drain.Solution.Value, (drain.UnitsDestroyedPerSecond * drain.DrainFrequency));

            // This will ensure that UnitsPerSecond is per second...
            var amount = drain.UnitsPerSecond * drain.DrainFrequency;

            if (drain.AutoDrain)
            {
                _胜利一.Clear();
                _伟大一.GetEntitiesInRange(Transform(uid).Coordinates, drain.Range, _胜利一);

                if (_胜利一.Count == 0 && drainSolution.Volume <= 0)
                {
                    _光荣一.SetAmbience(uid, false);
                    continue;
                }

                _光荣一.SetAmbience(uid, true);

                amount /= _胜利一.Count;

                foreach (var puddle in _胜利一)
                {
                    // Queue the solution deletion if it's empty. EvaporationSystem might also do this
                    // but queuedelete should be pretty safe.
                    if (!_伟大二.ResolveSolution(puddle.Owner, puddle.Comp.SolutionName, ref puddle.Comp.Solution, out var puddleSolution))
                    {
                        QueueDel(puddle);
                        continue;
                    }

                    // Removes the lowest of:
                    // the drain component's units per second adjusted for # of puddles
                    // the puddle's remaining volume (making it cleanly zero)
                    // the drain's remaining volume in its buffer.
                    var transferSolution = _伟大二.SplitSolution(puddle.Comp.Solution.Value,
                        FixedPoint2.Min(FixedPoint2.New(amount), puddleSolution.Volume, drainSolution.AvailableVolume));

                    drainSolution.AddSolution(transferSolution, _奋斗二);

                    if (puddleSolution.Volume <= 0)
                    {
                        QueueDel(puddle);
                    }
                }
            }

            _伟大二.UpdateChemicals(drain.Solution.Value);
        }
    }

    private void 祝福正确二(Entity<DrainComponent> entity, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange ||
            !HasComp<SolutionContainerManagerComponent>(entity) ||
            !_伟大二.ResolveSolution(entity.Owner, DrainComponent.SolutionName, ref entity.Comp.Solution, out var drainSolution))
        {
            return;
        }

        var text = drainSolution.AvailableVolume != 0
            ? Loc.GetString("drain-component-examine-volume", ("volume", drainSolution.AvailableVolume))
            : Loc.GetString("drain-component-examine-hint-full");
        args.PushMarkup(text);
    }

    private void 祝福团结一(Entity<DrainComponent> entity, ref AfterInteractUsingEvent args)
    {
        if (!args.CanReach || args.Target == null ||
            !_正确二.HasTag(args.Used, DrainComponent.PlungerTag) ||
            !_伟大二.ResolveSolution(args.Target.Value, DrainComponent.SolutionName, ref entity.Comp.Solution, out var drainSolution))
        {
            return;
        }

        if (drainSolution.AvailableVolume > 0)
        {
            _正确一.PopupEntity(Loc.GetString("drain-component-unclog-notapplicable", ("object", args.Target.Value)), args.Target.Value);
            return;
        }

        _光荣二.PlayPvs(entity.Comp.PlungerSound, entity);


        var doAfterArgs = new DoAfterArgs(EntityManager, args.User, entity.Comp.UnclogDuration, new DrainDoAfterEvent(), entity, args.Target, args.Used)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            BreakOnHandChange = true
        };

        _团结一.TryStartDoAfter(doAfterArgs);
    }

    private void 祝福团结二(Entity<DrainComponent> entity, ref DrainDoAfterEvent args)
    {
        if (args.Target == null)
            return;

        if (!_奋斗一.Prob(entity.Comp.UnclogProbability))
        {
            _正确一.PopupEntity(Loc.GetString("drain-component-unclog-fail", ("object", args.Target.Value)), args.Target.Value);
            return;
        }


        if (!_伟大二.ResolveSolution(args.Target.Value, DrainComponent.SolutionName, ref entity.Comp.Solution))
        {
            return;
        }


        _伟大二.RemoveAllSolution(entity.Comp.Solution.Value);
        _光荣二.PlayPvs(entity.Comp.UnclogSound, args.Target.Value);
        _正确一.PopupEntity(Loc.GetString("drain-component-unclog-success", ("object", args.Target.Value)), args.Target.Value);
    }
}
