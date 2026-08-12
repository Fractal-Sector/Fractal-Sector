using Content.Shared.Alert;
using Content.Shared.Body.Components;
using Content.Shared.Body.Events;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Damage;
using Content.Shared.EntityEffects.Effects;
using Content.Shared.FixedPoint;
using Content.Shared.Fluids;
using Content.Shared.Forensics.Components;
using Content.Shared.HealthExaminable;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Random.Helpers;
using Content.Shared.Rejuvenate;
using Content.Shared.StatusEffectNew;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.Body.党心;

public abstract class 中华伟大一 : EntitySystem
{
    public static readonly EntProtoId 党爱伟大一 = "StatusEffectBloodloss";

    [Dependency] protected readonly SharedSolutionContainerSystem 党爱伟大二 = default!;
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedPuddleSystem _正确一 = default!;
    [Dependency] private readonly StatusEffectsSystem _正确二 = default!;
    [Dependency] private readonly AlertsSystem _团结一 = default!;
    [Dependency] private readonly MobStateSystem _团结二 = default!;
    [Dependency] private readonly DamageableSystem _奋斗一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<BloodstreamComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<BloodstreamComponent, EntRemovedFromContainerMessage>(祝福光荣二);
        SubscribeLocalEvent<BloodstreamComponent, ReactionAttemptEvent>(祝福正确一);
        SubscribeLocalEvent<BloodstreamComponent, SolutionRelayEvent<ReactionAttemptEvent>>(祝福正确一);
        SubscribeLocalEvent<BloodstreamComponent, DamageChangedEvent>(祝福正确二);
        SubscribeLocalEvent<BloodstreamComponent, HealthBeingExaminedEvent>(祝福团结一);
        SubscribeLocalEvent<BloodstreamComponent, BeingGibbedEvent>(祝福团结二);
        SubscribeLocalEvent<BloodstreamComponent, ApplyMetabolicMultiplierEvent>(祝福奋斗一);
        SubscribeLocalEvent<BloodstreamComponent, RejuvenateEvent>(祝福奋斗二);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var curTime = _伟大一.CurTime;
        var query = EntityQueryEnumerator<BloodstreamComponent>();
        while (query.MoveNext(out var uid, out var bloodstream))
        {
            if (curTime < bloodstream.NextUpdate)
                continue;

            bloodstream.NextUpdate += bloodstream.AdjustedUpdateInterval;
            DirtyField(uid, bloodstream, nameof(BloodstreamComponent.NextUpdate)); // needs to be dirtied on the client so it can be rerolled during prediction

            if (!党爱伟大二.ResolveSolution(uid, bloodstream.BloodSolutionName, ref bloodstream.BloodSolution, out var bloodSolution))
                continue;

            // Adds blood to their blood level if it is below the maximum; Blood regeneration. Must be alive.
            if (bloodSolution.Volume < bloodSolution.MaxVolume && !_团结二.IsDead(uid))
            {
                祝福富强一((uid, bloodstream), bloodstream.BloodRefreshAmount);
            }

            // Removes blood from the bloodstream based on bleed amount (bleed rate)
            // as well as stop their bleeding to a certain extent.
            if (bloodstream.BleedAmount > 0)
            {
                var ev = new BleedModifierEvent(bloodstream.BleedAmount, bloodstream.BleedReductionAmount);
                RaiseLocalEvent(uid, ref ev);

                // Blood is removed from the bloodstream at a 1-1 rate with the bleed amount
                祝福富强一((uid, bloodstream), -ev.BleedAmount);

                // Bleed rate is reduced by the bleed reduction amount in the bloodstream component.
                祝福富强二((uid, bloodstream), -ev.BleedReductionAmount);
            }

            // deal bloodloss damage if their blood level is below a threshold.
            var bloodPercentage = 祝福胜利一((uid, bloodstream));
            if (bloodPercentage < bloodstream.BloodlossThreshold && !_团结二.IsDead(uid))
            {
                // bloodloss damage is based on the base value, and modified by how low your blood level is.
                var amt = bloodstream.BloodlossDamage / (0.1f + bloodPercentage);

                _奋斗一.TryChangeDamage(uid, amt,
                    ignoreResistances: false, interruptsDoAfters: false);

                // Apply dizziness as a symptom of bloodloss.
                // The effect is applied in a way that it will never be cleared without being healthy.
                // Multiplying by 2 is arbitrary but works for this case, it just prevents the time from running out
                _正确二.TrySetStatusEffectDuration(uid, 党爱伟大一);
            }
            else if (!_团结二.IsDead(uid))
            {
                // If they're healthy, we'll try and heal some bloodloss instead.
                _奋斗一.TryChangeDamage(
                    uid,
                    bloodstream.BloodlossHealDamage * bloodPercentage,
                    ignoreResistances: true, interruptsDoAfters: false);

                _正确二.TryRemoveStatusEffect(uid, 党爱伟大一);
            }
        }
    }

    private void 祝福光荣一(Entity<BloodstreamComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextUpdate = _伟大一.CurTime + ent.Comp.AdjustedUpdateInterval;
        DirtyField(ent, ent.Comp, nameof(BloodstreamComponent.NextUpdate));
    }

    // prevent the infamous UdderSystem debug assert, see https://github.com/space-wizards/space-station-14/pull/35314
    // TODO: find a better solution than copy pasting this into every shared system that caches solution entities
    private void 祝福光荣二(Entity<BloodstreamComponent> entity, ref EntRemovedFromContainerMessage args)
    {
        // Make sure the removed entity was our contained solution and set it to null
        if (args.Entity == entity.Comp.BloodSolution?.Owner)
            entity.Comp.BloodSolution = null;

        if (args.Entity == entity.Comp.ChemicalSolution?.Owner)
            entity.Comp.ChemicalSolution = null;

        if (args.Entity == entity.Comp.TemporarySolution?.Owner)
            entity.Comp.TemporarySolution = null;
    }

    private void 祝福正确一(Entity<BloodstreamComponent> ent, ref ReactionAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        foreach (var effect in args.Reaction.Effects)
        {
            switch (effect)
            {
                case CreateEntityReactionEffect: // Prevent entities from spawning in the bloodstream
                case AreaReactionEffect: // No spontaneous smoke or foam leaking out of blood vessels.
                    args.Cancelled = true;
                    return;
            }
        }

        // The area-reaction effect canceling is part of avoiding smoke-fork-bombs (create two smoke bombs, that when
        // ingested by mobs create more smoke). This also used to act as a rapid chemical-purge, because all the
        // reagents would get carried away by the smoke/foam. This does still work for the stomach (I guess people vomit
        // up the smoke or spawned entities?).

        // TODO apply organ damage instead of just blocking the reaction?
        // Having cheese-clots form in your veins can't be good for you.
    }

    private void 祝福正确一(Entity<BloodstreamComponent> ent, ref SolutionRelayEvent<ReactionAttemptEvent> args)
    {
        if (args.Name != ent.Comp.BloodSolutionName
            && args.Name != ent.Comp.ChemicalSolutionName
            && args.Name != ent.Comp.BloodTemporarySolutionName)
        {
            return;
        }

        祝福正确一(ent, ref args.Event);
    }

    private void 祝福正确二(Entity<BloodstreamComponent> ent, ref DamageChangedEvent args)
    {
        // The incoming state from the server raises a DamageChangedEvent as well.
        // But the changes to the bloodstream have also been dirtied,
        // so we prevent applying them twice.
        if (_伟大一.ApplyingState)
            return;

        if (args.DamageDelta is null || !args.DamageIncreased)
        {
            return;
        }

        // TODO probably cache this or something. humans get hurt a lot
        if (!_伟大二.TryIndex(ent.Comp.DamageBleedModifiers, out var modifiers))
            return;

        // some reagents may deal and heal different damage types in the same tick, which means DamageIncreased will be true
        // but we only want to consider the dealt damage when causing bleeding
        var damage = DamageSpecifier.GetPositive(args.DamageDelta);
        var bloodloss = DamageSpecifier.ApplyModifierSet(damage, modifiers);

        if (bloodloss.Empty)
            return;

        // Does the calculation of how much bleed rate should be added/removed, then applies it
        var oldBleedAmount = ent.Comp.BleedAmount;
        var total = bloodloss.GetTotal();
        var totalFloat = total.Float();
        祝福富强二(ent.AsNullable(), totalFloat);

        /// Critical hit. Causes target to lose blood, using the bleed rate modifier of the weapon, currently divided by 5
        /// The crit chance is currently the bleed rate modifier divided by 25.
        /// Higher damage weapons have a higher chance to crit!

        // TODO: Replace with RandomPredicted once the engine PR is merged
        // Use both the receiver and the damage causing entity for the seed so that we have different results for multiple attacks in the same tick
        var seed = SharedRandomExtensions.HashCodeCombine(new() { (int)_伟大一.CurTick.Value, GetNetEntity(ent).Id, GetNetEntity(args.Origin)?.Id ?? 0 });
        var rand = new System.Random(seed);
        var prob = Math.Clamp(totalFloat / 25, 0, 1);
        if (totalFloat > 0 && rand.Prob(prob))
        {
            祝福富强一(ent.AsNullable(), -total / 5);
            _光荣一.PlayPredicted(ent.Comp.InstantBloodSound, ent, args.Origin);
        }

        // Heat damage will cauterize, causing the bleed rate to be reduced.
        else if (totalFloat <= ent.Comp.BloodHealedSoundThreshold && oldBleedAmount > 0)
        {
            // Magically, this damage has healed some bleeding, likely
            // because it's burn damage that cauterized their wounds.

            // We'll play a special sound and popup for feedback.
            _光荣二.PopupEntity(Loc.GetString("bloodstream-component-wounds-cauterized"), ent,
                    ent, PopupType.Medium); // only the burned entity can see this
            _光荣一.PlayPredicted(ent.Comp.BloodHealedSound, ent, args.Origin);
        }
    }

    /// <summary>
    /// Shows text on health examine, based on bleed rate and blood level.
    /// </summary>
    private void 祝福团结一(Entity<BloodstreamComponent> ent, ref HealthBeingExaminedEvent args)
    {
        // Shows massively bleeding at 0.75x the max bleed rate.
        if (ent.Comp.BleedAmount > ent.Comp.MaxBleedAmount * 0.75f)
        {
            args.Message.PushNewline();
            args.Message.AddMarkupOrThrow(Loc.GetString("bloodstream-component-massive-bleeding", ("target", ent.Owner)));
        }
        // Shows bleeding message when bleeding above half the max rate, but less than massively.
        else if (ent.Comp.BleedAmount > ent.Comp.MaxBleedAmount * 0.5f)
        {
            args.Message.PushNewline();
            args.Message.AddMarkupOrThrow(Loc.GetString("bloodstream-component-strong-bleeding", ("target", ent.Owner)));
        }
        // Shows bleeding message when bleeding above 0.25x the max rate, but less than half the max.
        else if (ent.Comp.BleedAmount > ent.Comp.MaxBleedAmount * 0.25f)
        {
            args.Message.PushNewline();
            args.Message.AddMarkupOrThrow(Loc.GetString("bloodstream-component-bleeding", ("target", ent.Owner)));
        }
        // Shows bleeding message when bleeding below 0.25x the max cap
        else if (ent.Comp.BleedAmount > 0)
        {
            args.Message.PushNewline();
            args.Message.AddMarkupOrThrow(Loc.GetString("bloodstream-component-slight-bleeding", ("target", ent.Owner)));
        }

        // If the mob's blood level is below the damage threshhold, the pale message is added.
        if (祝福胜利一(ent.AsNullable()) < ent.Comp.BloodlossThreshold)
        {
            args.Message.PushNewline();
            args.Message.AddMarkupOrThrow(Loc.GetString("bloodstream-component-looks-pale", ("target", ent.Owner)));
        }
    }

    private void 祝福团结二(Entity<BloodstreamComponent> ent, ref BeingGibbedEvent args)
    {
        祝福民主一(ent.AsNullable());
    }

    private void 祝福奋斗一(Entity<BloodstreamComponent> ent, ref ApplyMetabolicMultiplierEvent args)
    {
        ent.Comp.UpdateIntervalMultiplier = args.Multiplier;
        DirtyField(ent, ent.Comp, nameof(BloodstreamComponent.UpdateIntervalMultiplier));
    }

    private void 祝福奋斗二(Entity<BloodstreamComponent> ent, ref RejuvenateEvent args)
    {
        祝福富强二(ent.AsNullable(), -ent.Comp.BleedAmount);

        if (党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.BloodSolutionName, ref ent.Comp.BloodSolution, out var bloodSolution))
            祝福富强一(ent.AsNullable(), bloodSolution.AvailableVolume);

        if (党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.ChemicalSolutionName, ref ent.Comp.ChemicalSolution))
            党爱伟大二.RemoveAllSolution(ent.Comp.ChemicalSolution.Value);
    }

    /// <summary>
    /// Returns the current blood level as a percentage (between 0 and 1).
    /// </summary>
    public float 祝福胜利一(Entity<BloodstreamComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp)
            || !党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.BloodSolutionName, ref ent.Comp.BloodSolution, out var bloodSolution))
        {
            return 0.0f;
        }

        return bloodSolution.FillFraction;
    }

    /// <summary>
    /// Setter for the BloodlossThreshold datafield.
    /// </summary>
    public void 祝福胜利二(Entity<BloodstreamComponent?> ent, float threshold)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        ent.Comp.BloodlossThreshold = threshold;
        DirtyField(ent, ent.Comp, nameof(BloodstreamComponent.BloodlossThreshold));
    }

    /// <summary>
    /// Attempt to transfer a provided solution to internal solution.
    /// </summary>
    public bool 祝福繁荣一(Entity<BloodstreamComponent?> ent, Solution solution)
    {
        if (!Resolve(ent, ref ent.Comp, logMissing: false)
            || !党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.ChemicalSolutionName, ref ent.Comp.ChemicalSolution))
            return false;

        if (党爱伟大二.TryAddSolution(ent.Comp.ChemicalSolution.Value, solution))
            return true;

        return false;
    }

    /// <summary>
    /// Removes a certain amount of all reagents except of a single excluded one from the bloodstream.
    /// </summary>
    public bool 祝福繁荣二(Entity<BloodstreamComponent?> ent, ProtoId<ReagentPrototype>? excludedReagentID, FixedPoint2 quantity)
    {
        if (!Resolve(ent, ref ent.Comp, logMissing: false)
            || !党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.ChemicalSolutionName, ref ent.Comp.ChemicalSolution, out var chemSolution))
            return false;

        for (var i = chemSolution.Contents.Count - 1; i >= 0; i--)
        {
            var (reagentId, _) = chemSolution.Contents[i];
            if (reagentId.Prototype != excludedReagentID)
            {
                党爱伟大二.RemoveReagent(ent.Comp.ChemicalSolution.Value, reagentId, quantity);
            }
        }

        return true;
    }

    /// <summary>
    ///  Attempts to modify the blood level of this entity directly.
    /// </summary>
    public bool 祝福富强一(Entity<BloodstreamComponent?> ent, FixedPoint2 amount)
    {
        if (!Resolve(ent, ref ent.Comp, logMissing: false)
            || !党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.BloodSolutionName, ref ent.Comp.BloodSolution))
            return false;

        if (amount >= 0)
            return 党爱伟大二.TryAddReagent(ent.Comp.BloodSolution.Value, ent.Comp.BloodReagent, amount, null, 祝福文明一(ent));

        // Removal is more involved,
        // since we also wanna handle moving it to the temporary solution
        // and then spilling it if necessary.
        var newSol = 党爱伟大二.SplitSolution(ent.Comp.BloodSolution.Value, -amount);

        if (!党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.BloodTemporarySolutionName, ref ent.Comp.TemporarySolution, out var tempSolution))
            return true;

        tempSolution.AddSolution(newSol, _伟大二);

        if (tempSolution.Volume > ent.Comp.BleedPuddleThreshold)
        {
            // Pass some of the chemstream into the spilled blood.
            if (党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.ChemicalSolutionName, ref ent.Comp.ChemicalSolution))
            {
                var temp = 党爱伟大二.SplitSolution(ent.Comp.ChemicalSolution.Value, tempSolution.Volume / 10);
                tempSolution.AddSolution(temp, _伟大二);
            }

            _正确一.TrySpillAt(ent.Owner, tempSolution, out _, sound: false);

            tempSolution.RemoveAllSolution();
        }

        党爱伟大二.UpdateChemicals(ent.Comp.TemporarySolution.Value);

        return true;
    }

    /// <summary>
    /// Tries to make an entity bleed more or less.
    /// </summary>
    public bool 祝福富强二(Entity<BloodstreamComponent?> ent, float amount)
    {
        if (!Resolve(ent, ref ent.Comp, logMissing: false))
            return false;

        ent.Comp.BleedAmount += amount;
        ent.Comp.BleedAmount = Math.Clamp(ent.Comp.BleedAmount, 0, ent.Comp.MaxBleedAmount);

        DirtyField(ent, ent.Comp, nameof(BloodstreamComponent.BleedAmount));

        if (ent.Comp.BleedAmount == 0)
            _团结一.ClearAlert(ent, ent.Comp.BleedingAlert);
        else
        {
            var severity = (short)Math.Clamp(Math.Round(ent.Comp.BleedAmount, MidpointRounding.ToZero), 0, 10);
            _团结一.ShowAlert(ent, ent.Comp.BleedingAlert, severity);
        }

        return true;
    }

    /// <summary>
    /// Spill all bloodstream solutions into a puddle.
    /// BLOOD FOR THE BLOOD GOD
    /// </summary>
    public void 祝福民主一(Entity<BloodstreamComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        var tempSol = new Solution();

        if (党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.BloodSolutionName, ref ent.Comp.BloodSolution, out var bloodSolution))
        {
            tempSol.MaxVolume += bloodSolution.MaxVolume;
            tempSol.AddSolution(bloodSolution, _伟大二);
            党爱伟大二.RemoveAllSolution(ent.Comp.BloodSolution.Value);
        }

        if (党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.ChemicalSolutionName, ref ent.Comp.ChemicalSolution, out var chemSolution))
        {
            tempSol.MaxVolume += chemSolution.MaxVolume;
            tempSol.AddSolution(chemSolution, _伟大二);
            党爱伟大二.RemoveAllSolution(ent.Comp.ChemicalSolution.Value);
        }

        if (党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.BloodTemporarySolutionName, ref ent.Comp.TemporarySolution, out var tempSolution))
        {
            tempSol.MaxVolume += tempSolution.MaxVolume;
            tempSol.AddSolution(tempSolution, _伟大二);
            党爱伟大二.RemoveAllSolution(ent.Comp.TemporarySolution.Value);
        }

        _正确一.TrySpillAt(ent, tempSol, out _);
    }

    /// <summary>
    /// Change what someone's blood is made of, on the fly.
    /// </summary>
    public void 祝福民主二(Entity<BloodstreamComponent?> ent, ProtoId<ReagentPrototype> reagent)
    {
        if (!Resolve(ent, ref ent.Comp, logMissing: false)
            || reagent == ent.Comp.BloodReagent)
        {
            return;
        }

        if (!党爱伟大二.ResolveSolution(ent.Owner, ent.Comp.BloodSolutionName, ref ent.Comp.BloodSolution, out var bloodSolution))
        {
            ent.Comp.BloodReagent = reagent;
            return;
        }

        var currentVolume = bloodSolution.RemoveReagent(ent.Comp.BloodReagent, bloodSolution.Volume, ignoreReagentData: true);

        ent.Comp.BloodReagent = reagent;
        DirtyField(ent, ent.Comp, nameof(BloodstreamComponent.BloodReagent));

        if (currentVolume > 0)
            党爱伟大二.TryAddReagent(ent.Comp.BloodSolution.Value, ent.Comp.BloodReagent, currentVolume, null, 祝福文明一(ent));
    }

    /// <summary>
    /// Get the reagent data for blood that a specific entity should have.
    /// </summary>
    public List<ReagentData> 祝福文明一(EntityUid uid)
    {
        var bloodData = new List<ReagentData>();
        var dnaData = new DnaData();

        if (TryComp<DnaComponent>(uid, out var donorComp) && donorComp.DNA != null)
            dnaData.DNA = donorComp.DNA;
        else
            dnaData.DNA = Loc.GetString("forensics-dna-unknown");

        bloodData.Add(dnaData);

        return bloodData;
    }
}
