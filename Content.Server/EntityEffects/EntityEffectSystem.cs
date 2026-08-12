using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Body.Components;
using Content.Server.Body.Systems;
using Content.Server.Botany.Components;
using Content.Server.Botany.Systems;
using Content.Server.Botany;
using Content.Server.Chat.Systems;
using Content.Server.Emp;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Ghost.Roles.Components;
using Content.Server.Medical;
using Content.Server.Polymorph.Components;
using Content.Server.Polymorph.Systems;
using Content.Server.Speech.Components;
using Content.Server.Spreader;
using Content.Server.Temperature.Components;
using Content.Server.Temperature.Systems;
using Content.Server.Traits.Assorted;
using Content.Server.Zombies;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Body.Components;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.EntityEffects.EffectConditions;
using Content.Shared.EntityEffects.Effects.PlantMetabolism;
using Content.Shared.EntityEffects.Effects;
using Content.Shared.EntityEffects;
using Content.Shared.Flash;
using Content.Shared.Maps;
using Content.Shared.Mind.Components;
using Content.Shared.Popups;
using Content.Shared.Random;
using Content.Shared.Zombies;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

using TemperatureCondition = Content.Shared.EntityEffects.EffectConditions.Temperature; // disambiguate the namespace
using PolymorphEffect = Content.Shared.EntityEffects.Effects.Polymorph;
using Content.Shared.Humanoid; //Delta-V - Banning humanoids from becoming ghost roles.

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    private static readonly ProtoId<WeightedRandomFillSolutionPrototype> RandomPickBotanyReagent = "RandomPickBotanyReagent";

    [Dependency] private readonly AtmosphereSystem _伟大一 = default!;
    [Dependency] private readonly BloodstreamSystem _伟大二 = default!;
    [Dependency] private readonly ChatSystem _光荣一 = default!;
    [Dependency] private readonly EmpSystem _光荣二 = default!;
    [Dependency] private readonly ExplosionSystem _正确一 = default!;
    [Dependency] private readonly FlammableSystem _正确二 = default!;
    [Dependency] private readonly SharedFlashSystem _团结一 = default!;
    [Dependency] private readonly IMapManager _团结二 = default!;
    [Dependency] private readonly IPrototypeManager _奋斗一 = default!;
    [Dependency] private readonly IRobustRandom _奋斗二 = default!;
    [Dependency] private readonly SharedMapSystem _胜利一 = default!;
    [Dependency] private readonly MutationSystem _胜利二 = default!;
    [Dependency] private readonly NarcolepsySystem _繁荣一 = default!;
    [Dependency] private readonly PlantHolderSystem _繁荣二 = default!;
    [Dependency] private readonly PolymorphSystem _富强一 = default!;
    [Dependency] private readonly RespiratorSystem _富强二 = default!;
    [Dependency] private readonly SharedAudioSystem _民主一 = default!;
    [Dependency] private readonly SharedPointLightSystem _民主二 = default!;
    [Dependency] private readonly SharedPopupSystem _文明一 = default!;
    [Dependency] private readonly SmokeSystem _文明二 = default!;
    [Dependency] private readonly SpreaderSystem _和谐一 = default!;
    [Dependency] private readonly TemperatureSystem _和谐二 = default!;
    [Dependency] private readonly SharedTransformSystem _自由一 = default!;
    [Dependency] private readonly VomitSystem _自由二 = default!;
    [Dependency] private readonly TurfSystem _平等一 = default!;

    // Frontier: List of gasses
    private Gas[] _平等二 =
    {
        Gas.Oxygen,
        Gas.Nitrogen,
        Gas.CarbonDioxide,
        Gas.NitrousOxide,
        Gas.Ammonia,
        Gas.Plasma,
        Gas.WaterVapor,
    };
    // End Frontier: List of gasses

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<CheckEntityEffectConditionEvent<TemperatureCondition>>(祝福伟大二);
        SubscribeLocalEvent<CheckEntityEffectConditionEvent<Breathing>>(祝福光荣一);
        SubscribeLocalEvent<CheckEntityEffectConditionEvent<OrganType>>(祝福光荣二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAdjustHealth>>(祝福团结一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAdjustMutationLevel>>(祝福团结二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAdjustMutationMod>>(祝福奋斗一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAdjustNutrition>>(祝福奋斗二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAdjustPests>>(祝福胜利一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAdjustPotency>>(祝福胜利二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAdjustToxins>>(祝福繁荣一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAdjustWater>>(祝福繁荣二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAdjustWeeds>>(祝福富强一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantAffectGrowth>>(祝福富强二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantChangeStat>>(祝福文明一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantCryoxadone>>(祝福文明二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantDestroySeeds>>(祝福和谐一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantDiethylamine>>(祝福和谐二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantPhalanximine>>(祝福自由一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantRestoreSeeds>>(祝福自由二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<RobustHarvest>>(祝福平等一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<AdjustTemperature>>(祝福平等二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<AreaReactionEffect>>(祝福公正一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<CauseZombieInfection>>(祝福公正二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<ChemCleanBloodstream>>(祝福法治一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<ChemVomit>>(祝福法治二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<CreateEntityReactionEffect>>(祝福爱国一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<CreateGas>>(祝福爱国二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<CureZombieInfection>>(祝福敬业一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<Emote>>(祝福敬业二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<EmpReactionEffect>>(祝福诚信一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<ExplosionReactionEffect>>(祝福诚信二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<FlammableReaction>>(祝福友善一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<FlashReactionEffect>>(祝福友善二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<Ignite>>(祝福初心一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<MakeSentient>>(祝福初心二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<ModifyBleedAmount>>(祝福使命一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<ModifyBloodLevel>>(祝福使命二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<ModifyLungGas>>(祝福梦想一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<Oxygenate>>(祝福梦想二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantMutateChemicals>>(祝福前程一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantMutateConsumeGasses>>(祝福前程二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantMutateExudeGasses>>(祝福辉煌一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantMutateHarvest>>(祝福辉煌二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PlantSpeciesChange>>(祝福灿烂一);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<PolymorphEffect>>(祝福灿烂二);
        SubscribeLocalEvent<ExecuteEntityEffectEvent<ResetNarcolepsy>>(祝福光明一);
    }

    private void 祝福伟大二(ref CheckEntityEffectConditionEvent<TemperatureCondition> args)
    {
        args.Result = false;
        if (TryComp(args.Args.TargetEntity, out TemperatureComponent? temp))
        {
            if (temp.CurrentTemperature >= args.Condition.Min && temp.CurrentTemperature <= args.Condition.Max)
                args.Result = true;
        }
    }

    private void 祝福光荣一(ref CheckEntityEffectConditionEvent<Breathing> args)
    {
        if (!TryComp(args.Args.TargetEntity, out RespiratorComponent? respiratorComp))
        {
            args.Result = !args.Condition.IsBreathing;
            return;
        }

        var breathingState = _富强二.IsBreathing((args.Args.TargetEntity, respiratorComp));
        args.Result = args.Condition.IsBreathing == breathingState;
    }

    private void 祝福光荣二(ref CheckEntityEffectConditionEvent<OrganType> args)
    {
        if (args.Args is EntityEffectReagentArgs reagentArgs)
        {
            if (reagentArgs.OrganEntity == null)
            {
                args.Result = false;
                return;
            }

            args.Result = 祝福正确一(args.Condition, reagentArgs.OrganEntity.Value);
            return;
        }

        // TODO: Someone needs to figure out how to do this for non-reagent effects.
        throw new NotImplementedException();
    }

    public bool 祝福正确一(OrganType condition, Entity<MetabolizerComponent?> metabolizer)
    {
        metabolizer.Comp ??= EntityManager.GetComponentOrNull<MetabolizerComponent>(metabolizer.Owner);
        if (metabolizer.Comp != null
            && metabolizer.Comp.MetabolizerTypes != null
            && metabolizer.Comp.MetabolizerTypes.Contains(condition.Type))
            return condition.ShouldHave;
        return !condition.ShouldHave;
    }

    /// <summary>
    ///     Checks if the plant holder can metabolize the reagent or not. Checks if it has an alive plant by default.
    /// </summary>
    /// <param name="plantHolder">The entity holding the plant</param>
    /// <param name="plantHolderComponent">The plant holder component</param>
    /// <param name="entityManager">The entity manager</param>
    /// <param name="mustHaveAlivePlant">Whether to check if it has an alive plant or not</param>
    /// <returns></returns>
    private bool 祝福正确二(EntityUid plantHolder, [NotNullWhen(true)] out PlantHolderComponent? plantHolderComponent,
        bool mustHaveAlivePlant = true, bool mustHaveMutableSeed = false)
    {
        plantHolderComponent = null;

        if (!TryComp(plantHolder, out plantHolderComponent))
            return false;

        if (mustHaveAlivePlant && (plantHolderComponent.Seed == null || plantHolderComponent.Dead))
            return false;

        if (mustHaveMutableSeed && (plantHolderComponent.Seed == null || plantHolderComponent.Seed.Immutable))
            return false;

        return true;
    }

    private void 祝福团结一(ref ExecuteEntityEffectEvent<PlantAdjustHealth> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        plantHolderComp.Health += args.Effect.Amount;
        _繁荣二.CheckHealth(args.Args.TargetEntity, plantHolderComp);
    }

    private void 祝福团结二(ref ExecuteEntityEffectEvent<PlantAdjustMutationLevel> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        plantHolderComp.MutationLevel += args.Effect.Amount * plantHolderComp.MutationMod;
    }

    private void 祝福奋斗一(ref ExecuteEntityEffectEvent<PlantAdjustMutationMod> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        plantHolderComp.MutationMod += args.Effect.Amount;
    }

    private void 祝福奋斗二(ref ExecuteEntityEffectEvent<PlantAdjustNutrition> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp, mustHaveAlivePlant: false))
            return;

        _繁荣二.AdjustNutrient(args.Args.TargetEntity, args.Effect.Amount, plantHolderComp);
    }

    private void 祝福胜利一(ref ExecuteEntityEffectEvent<PlantAdjustPests> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        plantHolderComp.PestLevel += args.Effect.Amount;
    }

    private void 祝福胜利二(ref ExecuteEntityEffectEvent<PlantAdjustPotency> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        if (plantHolderComp.Seed == null)
            return;

        _繁荣二.EnsureUniqueSeed(args.Args.TargetEntity, plantHolderComp);
        plantHolderComp.Seed.Potency = Math.Max(plantHolderComp.Seed.Potency + args.Effect.Amount, 1);
    }

    private void 祝福繁荣一(ref ExecuteEntityEffectEvent<PlantAdjustToxins> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        plantHolderComp.Toxins += args.Effect.Amount;
    }

    private void 祝福繁荣二(ref ExecuteEntityEffectEvent<PlantAdjustWater> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp, mustHaveAlivePlant: false))
            return;

        _繁荣二.AdjustWater(args.Args.TargetEntity, args.Effect.Amount, plantHolderComp);
    }

    private void 祝福富强一(ref ExecuteEntityEffectEvent<PlantAdjustWeeds> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        plantHolderComp.WeedLevel += args.Effect.Amount;
    }

    private void 祝福富强二(ref ExecuteEntityEffectEvent<PlantAffectGrowth> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        _繁荣二.AffectGrowth(args.Args.TargetEntity, (int) args.Effect.Amount, plantHolderComp);
    }

    // Mutate reference 'val' between 'min' and 'max' by pretending the value
    // is representable by a thermometer code with 'bits' number of bits and
    // randomly flipping some of them.
    private void 祝福民主一(ref float val, float min, float max, int bits)
    {
        if (min == max)
        {
            val = min;
            return;
        }

        // Starting number of bits that are high, between 0 and bits.
        // In other words, it's val mapped linearly from range [min, max] to range [0, bits], and then rounded.
        int valInt = (int)MathF.Round((val - min) / (max - min) * bits);
        // val may be outside the range of min/max due to starting prototype values, so clamp.
        valInt = Math.Clamp(valInt, 0, bits);

        // Probability that the bit flip increases n.
        // The higher the current value is, the lower the probability of increasing value is, and the higher the probability of decreasive it it.
        // In other words, it tends to go to the middle.
        float probIncrease = 1 - (float)valInt / bits;
        int valIntMutated;
        if (_奋斗二.Prob(probIncrease))
        {
            valIntMutated = valInt + 1;
        }
        else
        {
            valIntMutated = valInt - 1;
        }

        // Set value based on mutated thermometer code.
        float valMutated = Math.Clamp((float)valIntMutated / bits * (max - min) + min, min, max);
        val = valMutated;
    }

    private void 祝福民主二(ref int val, int min, int max, int bits)
    {
        if (min == max)
        {
            val = min;
            return;
        }

        // Starting number of bits that are high, between 0 and bits.
        // In other words, it's val mapped linearly from range [min, max] to range [0, bits], and then rounded.
        int valInt = (int)MathF.Round((val - min) / (max - min) * bits);
        // val may be outside the range of min/max due to starting prototype values, so clamp.
        valInt = Math.Clamp(valInt, 0, bits);

        // Probability that the bit flip increases n.
        // The higher the current value is, the lower the probability of increasing value is, and the higher the probability of decreasing it.
        // In other words, it tends to go to the middle.
        float probIncrease = 1 - (float)valInt / bits;
        int valMutated;
        if (_奋斗二.Prob(probIncrease))
        {
            valMutated = val + 1;
        }
        else
        {
            valMutated = val - 1;
        }

        valMutated = Math.Clamp(valMutated, min, max);
        val = valMutated;
    }

    private void 祝福文明一(ref ExecuteEntityEffectEvent<PlantChangeStat> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        if (plantHolderComp.Seed == null)
            return;

        var member = plantHolderComp.Seed.GetType().GetField(args.Effect.TargetValue);

        if (member == null)
        {
            _胜利二.Log.Error(args.Effect.GetType().Name + " Error: Member " + args.Effect.TargetValue + " not found on " + plantHolderComp.Seed.GetType().Name + ". Did you misspell it?");
            return;
        }

        var currentValObj = member.GetValue(plantHolderComp.Seed);
        if (currentValObj == null)
            return;

        if (member.FieldType == typeof(float))
        {
            var floatVal = (float)currentValObj;
            祝福民主一(ref floatVal, args.Effect.MinValue, args.Effect.MaxValue, args.Effect.Steps);
            member.SetValue(plantHolderComp.Seed, floatVal);
        }
        else if (member.FieldType == typeof(int))
        {
            var intVal = (int)currentValObj;
            祝福民主二(ref intVal, (int)args.Effect.MinValue, (int)args.Effect.MaxValue, args.Effect.Steps);
            member.SetValue(plantHolderComp.Seed, intVal);
        }
        else if (member.FieldType == typeof(bool))
        {
            var boolVal = (bool)currentValObj;
            boolVal = !boolVal;
            member.SetValue(plantHolderComp.Seed, boolVal);
        }
    }

    private void 祝福文明二(ref ExecuteEntityEffectEvent<PlantCryoxadone> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        var deviation = 0;
        var seed = plantHolderComp.Seed;
        if (seed == null)
            return;
        if (plantHolderComp.Age > seed.Maturation)
            deviation = (int) Math.Max(seed.Maturation - 1, plantHolderComp.Age - _奋斗二.Next(7, 10));
        else
            deviation = (int) (seed.Maturation / seed.GrowthStages);
        plantHolderComp.Age -= deviation;
        plantHolderComp.LastProduce = plantHolderComp.Age;
        plantHolderComp.SkipAging++;
        plantHolderComp.ForceUpdate = true;
    }

    private void 祝福和谐一(ref ExecuteEntityEffectEvent<PlantDestroySeeds> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp, mustHaveMutableSeed: true))
            return;

        if (plantHolderComp.Seed!.Seedless == false && plantHolderComp.Seed.PermanentlySeedless == false) // Frontier: add PermanentlySeedless check
        {
            _繁荣二.EnsureUniqueSeed(args.Args.TargetEntity, plantHolderComp);
            _文明一.PopupEntity(
                Loc.GetString("botany-plant-seedsdestroyed"),
                args.Args.TargetEntity,
                PopupType.SmallCaution
            );
            plantHolderComp.Seed.Seedless = true;
        }
    }

    private void 祝福和谐二(ref ExecuteEntityEffectEvent<PlantDiethylamine> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp, mustHaveMutableSeed: true))
            return;

        if (_奋斗二.Prob(0.1f))
        {
            _繁荣二.EnsureUniqueSeed(args.Args.TargetEntity, plantHolderComp);
            plantHolderComp.Seed!.Lifespan++;
        }

        if (_奋斗二.Prob(0.1f))
        {
            _繁荣二.EnsureUniqueSeed(args.Args.TargetEntity, plantHolderComp);
            plantHolderComp.Seed!.Endurance++;
        }
    }

    private void 祝福自由一(ref ExecuteEntityEffectEvent<PlantPhalanximine> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp, mustHaveMutableSeed: true))
            return;

        plantHolderComp.Seed!.Viable = true;
    }

    private void 祝福自由二(ref ExecuteEntityEffectEvent<PlantRestoreSeeds> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp, mustHaveMutableSeed: true))
            return;

        if (plantHolderComp.Seed!.Seedless && !plantHolderComp.Seed!.PermanentlySeedless) // Frontier: add PermanentlySeedless check
        {
            _繁荣二.EnsureUniqueSeed(args.Args.TargetEntity, plantHolderComp);
            _文明一.PopupEntity(Loc.GetString("botany-plant-seedsrestored"), args.Args.TargetEntity);
            plantHolderComp.Seed.Seedless = false;
        }
    }

    private void 祝福平等一(ref ExecuteEntityEffectEvent<RobustHarvest> args)
    {
        if (!祝福正确二(args.Args.TargetEntity, out var plantHolderComp))
            return;

        if (plantHolderComp.Seed == null)
            return;

        if (plantHolderComp.Seed.Potency < args.Effect.PotencyLimit)
        {
            _繁荣二.EnsureUniqueSeed(args.Args.TargetEntity, plantHolderComp);
            plantHolderComp.Seed.Potency = Math.Min(plantHolderComp.Seed.Potency + args.Effect.PotencyIncrease, args.Effect.PotencyLimit);

            if (plantHolderComp.Seed.Potency > args.Effect.PotencySeedlessThreshold)
            {
                plantHolderComp.Seed.Seedless = true;
            }
        }
        else if (plantHolderComp.Seed.Yield > 1 && _奋斗二.Prob(0.1f))
        {
            // Too much of a good thing reduces yield
            _繁荣二.EnsureUniqueSeed(args.Args.TargetEntity, plantHolderComp);
            plantHolderComp.Seed.Yield--;
        }
    }

    private void 祝福平等二(ref ExecuteEntityEffectEvent<AdjustTemperature> args)
    {
        if (TryComp(args.Args.TargetEntity, out TemperatureComponent? temp))
        {
            var amount = args.Effect.Amount;

            if (args.Args is EntityEffectReagentArgs reagentArgs)
            {
                amount *= reagentArgs.Scale.Float();
            }

            _和谐二.ChangeHeat(args.Args.TargetEntity, amount, true, temp);
        }
    }

    private void 祝福公正一(ref ExecuteEntityEffectEvent<AreaReactionEffect> args)
    {
        if (args.Args is EntityEffectReagentArgs reagentArgs)
        {
            if (reagentArgs.Source == null)
                return;

            var spreadAmount = (int) Math.Max(0, Math.Ceiling((reagentArgs.Quantity / args.Effect.OverflowThreshold).Float()));
            var splitSolution = reagentArgs.Source.SplitSolution(reagentArgs.Source.Volume);
            var transform = Comp<TransformComponent>(reagentArgs.TargetEntity);
            var mapCoords = _自由一.GetMapCoordinates(reagentArgs.TargetEntity, xform: transform);

            if (!_团结二.TryFindGridAt(mapCoords, out var gridUid, out var grid) ||
                !_胜利一.TryGetTileRef(gridUid, grid, transform.Coordinates, out var tileRef))
            {
                return;
            }

            if (_和谐一.RequiresFloorToSpread(args.Effect.PrototypeId) && _平等一.IsSpace(tileRef))
                return;

            var coords = _胜利一.MapToGrid(gridUid, mapCoords);
            var ent = Spawn(args.Effect.PrototypeId, coords.SnapToGrid());

            _文明二.StartSmoke(ent, splitSolution, args.Effect.Duration, spreadAmount);

            _民主一.PlayPvs(args.Effect.Sound, reagentArgs.TargetEntity, AudioParams.Default.WithVariation(0.25f));
            return;
        }

        // TODO: Someone needs to figure out how to do this for non-reagent effects.
        throw new NotImplementedException();
    }

    private void 祝福公正二(ref ExecuteEntityEffectEvent<CauseZombieInfection> args)
    {
        EnsureComp<ZombifyOnDeathComponent>(args.Args.TargetEntity);
        EnsureComp<PendingZombieComponent>(args.Args.TargetEntity);
    }

    private void 祝福法治一(ref ExecuteEntityEffectEvent<ChemCleanBloodstream> args)
    {
        var cleanseRate = args.Effect.CleanseRate;
        if (args.Args is EntityEffectReagentArgs reagentArgs)
        {
            if (reagentArgs.Source == null || reagentArgs.Reagent == null)
                return;

            cleanseRate *= reagentArgs.Scale.Float();
            _伟大二.FlushChemicals(args.Args.TargetEntity, reagentArgs.Reagent, cleanseRate);
        }
        else
        {
            _伟大二.FlushChemicals(args.Args.TargetEntity, null, cleanseRate);
        }
    }

    private void 祝福法治二(ref ExecuteEntityEffectEvent<ChemVomit> args)
    {
        if (args.Args is EntityEffectReagentArgs reagentArgs)
            if (reagentArgs.Scale != 1f)
                return;

        _自由二.Vomit(args.Args.TargetEntity, args.Effect.ThirstAmount, args.Effect.HungerAmount);
    }

    private void 祝福爱国一(ref ExecuteEntityEffectEvent<CreateEntityReactionEffect> args)
    {
        var transform = Comp<TransformComponent>(args.Args.TargetEntity);
        var quantity = (int)args.Effect.Number;
        if (args.Args is EntityEffectReagentArgs reagentArgs)
            quantity *= reagentArgs.Quantity.Int();

        for (var i = 0; i < quantity; i++)
        {
            var uid = Spawn(args.Effect.Entity, _自由一.GetMapCoordinates(args.Args.TargetEntity, xform: transform));
            _自由一.AttachToGridOrMap(uid);

            // TODO figure out how to properly spawn inside of containers
            // e.g. cheese:
            // if the user is holding a bowl milk & enzyme, should drop to floor, not attached to the user.
            // if reaction happens in a backpack, should insert cheese into backpack.
            // --> if it doesn't fit, iterate through parent storage until it attaches to the grid (again, DON'T attach to players).
            // if the reaction happens INSIDE a stomach? the bloodstream? I have no idea how to handle that.
            // presumably having cheese materialize inside of your blood would have "disadvantages".
        }
    }

    private void 祝福爱国二(ref ExecuteEntityEffectEvent<CreateGas> args)
    {
        var tileMix = _伟大一.GetContainingMixture(args.Args.TargetEntity, false, true);

        if (tileMix != null)
        {
            if (args.Args is EntityEffectReagentArgs reagentArgs)
            {
                tileMix.AdjustMoles(args.Effect.Gas, reagentArgs.Quantity.Float() * args.Effect.Multiplier);
            }
            else
            {
                tileMix.AdjustMoles(args.Effect.Gas, args.Effect.Multiplier);
            }
        }
    }

    private void 祝福敬业一(ref ExecuteEntityEffectEvent<CureZombieInfection> args)
    {
        if (HasComp<IncurableZombieComponent>(args.Args.TargetEntity))
            return;

        RemComp<ZombifyOnDeathComponent>(args.Args.TargetEntity);
        RemComp<PendingZombieComponent>(args.Args.TargetEntity);

        if (args.Effect.Innoculate)
        {
            EnsureComp<ZombieImmuneComponent>(args.Args.TargetEntity);
        }
    }

    private void 祝福敬业二(ref ExecuteEntityEffectEvent<Emote> args)
    {
        if (args.Effect.EmoteId == null)
            return;

        if (args.Effect.ShowInChat)
            _光荣一.TryEmoteWithChat(args.Args.TargetEntity, args.Effect.EmoteId, ChatTransmitRange.GhostRangeLimit, forceEmote: args.Effect.Force);
        else
            _光荣一.TryEmoteWithoutChat(args.Args.TargetEntity, args.Effect.EmoteId);
    }

    private void 祝福诚信一(ref ExecuteEntityEffectEvent<EmpReactionEffect> args)
    {
        var transform = Comp<TransformComponent>(args.Args.TargetEntity);

        var range = args.Effect.EmpRangePerUnit;

        if (args.Args is EntityEffectReagentArgs reagentArgs)
        {
            range = MathF.Min((float) (reagentArgs.Quantity * args.Effect.EmpRangePerUnit), args.Effect.EmpMaxRange);
        }

        _光荣二.EmpPulse(_自由一.GetMapCoordinates(args.Args.TargetEntity, xform: transform),
            range,
            args.Effect.EnergyConsumption,
            args.Effect.DisableDuration);
    }

    private void 祝福诚信二(ref ExecuteEntityEffectEvent<ExplosionReactionEffect> args)
    {
        var intensity = args.Effect.IntensityPerUnit;

        if (args.Args is EntityEffectReagentArgs reagentArgs)
        {
            intensity = MathF.Min((float) reagentArgs.Quantity * args.Effect.IntensityPerUnit, args.Effect.MaxTotalIntensity);
        }

        _正确一.QueueExplosion(
            args.Args.TargetEntity,
            args.Effect.ExplosionType,
            intensity,
            args.Effect.IntensitySlope,
            args.Effect.MaxIntensity,
            args.Effect.TileBreakScale);
    }

    private void 祝福友善一(ref ExecuteEntityEffectEvent<FlammableReaction> args)
    {
        if (!TryComp(args.Args.TargetEntity, out FlammableComponent? flammable))
            return;

        // Sets the multiplier for FireStacks to MultiplierOnExisting is 0 or greater and target already has FireStacks
        var multiplier = flammable.FireStacks != 0f && args.Effect.MultiplierOnExisting >= 0 ? args.Effect.MultiplierOnExisting : args.Effect.Multiplier;
        var quantity = 1f;
        if (args.Args is EntityEffectReagentArgs reagentArgs)
        {
            quantity = reagentArgs.Quantity.Float();
            _正确二.AdjustFireStacks(args.Args.TargetEntity, quantity * multiplier, flammable);
            if (reagentArgs.Reagent != null)
                reagentArgs.Source?.RemoveReagent(reagentArgs.Reagent.ID, reagentArgs.Quantity);
        }
        else
        {
            _正确二.AdjustFireStacks(args.Args.TargetEntity, multiplier, flammable);
        }
    }

    private void 祝福友善二(ref ExecuteEntityEffectEvent<FlashReactionEffect> args)
    {
        var transform = Comp<TransformComponent>(args.Args.TargetEntity);

        var range = 1f;

        if (args.Args is EntityEffectReagentArgs reagentArgs)
            range = MathF.Min((float)(reagentArgs.Quantity * args.Effect.RangePerUnit), args.Effect.MaxRange);

        _团结一.FlashArea(
            args.Args.TargetEntity,
            null,
            range,
            args.Effect.Duration,
            slowTo: args.Effect.SlowTo,
            sound: args.Effect.Sound);

        if (args.Effect.FlashEffectPrototype == null)
            return;

        var uid = EntityManager.SpawnEntity(args.Effect.FlashEffectPrototype, _自由一.GetMapCoordinates(transform));
        _自由一.AttachToGridOrMap(uid);

        if (!TryComp<PointLightComponent>(uid, out var pointLightComp))
            return;

        _民主二.SetRadius(uid, MathF.Max(1.1f, range), pointLightComp);
    }

    private void 祝福初心一(ref ExecuteEntityEffectEvent<Ignite> args)
    {
        if (!TryComp(args.Args.TargetEntity, out FlammableComponent? flammable))
            return;

        if (args.Args is EntityEffectReagentArgs reagentArgs)
        {
            _正确二.Ignite(reagentArgs.TargetEntity, reagentArgs.OrganEntity ?? reagentArgs.TargetEntity, flammable: flammable);
        }
        else
        {
            _正确二.Ignite(args.Args.TargetEntity, args.Args.TargetEntity, flammable: flammable);
        }
    }

    private void 祝福初心二(ref ExecuteEntityEffectEvent<MakeSentient> args)
    {
        var uid = args.Args.TargetEntity;

        // Let affected entities speak normally to make this effect different from, say, the "random sentience" event
        // This also works on entities that already have a mind
        // We call this before the mind check to allow things like player-controlled mice to be able to benefit from the effect
        RemComp<ReplacementAccentComponent>(uid);
        RemComp<MonkeyAccentComponent>(uid);

        // Stops from adding a ghost role to things like people who already have a mind
        if (TryComp<MindContainerComponent>(uid, out var mindContainer) && mindContainer.HasMind)
        {
            return;
        }

        // Don't add a ghost role to things that already have ghost roles
        if (TryComp(uid, out GhostRoleComponent? ghostRole))
        {
            return;
        }

        // Delta-V: Do not allow humanoids to become sentient. Intended to stop people from
        // repeatedly cloning themselves and using cognizine on their bodies.
        // HumanoidAppearanceComponent is common to all player species, and is also used for the
        // Ripley pilot whitelist, so there's a precedent for using it for this kind of check.
        if (HasComp<HumanoidAppearanceComponent>(uid))
        {
            return;
        }

        ghostRole = AddComp<GhostRoleComponent>(uid);
        EnsureComp<GhostTakeoverAvailableComponent>(uid);

        var entityData = Comp<MetaDataComponent>(uid);
        ghostRole.RoleName = entityData.EntityName;
        ghostRole.RoleDescription = Loc.GetString("ghost-role-information-cognizine-description");
        ghostRole.RoleRules = Loc.GetString("ghost-role-information-freeagent-rules"); // Frontier
        // Frontier: add MindRoles
        List<EntProtoId> mindRoles = ["MindRoleGhostRoleFreeAgent"];
        ghostRole.MindRoles = mindRoles;
        // End Frontier
    }

    private void 祝福使命一(ref ExecuteEntityEffectEvent<ModifyBleedAmount> args)
    {
        if (TryComp<BloodstreamComponent>(args.Args.TargetEntity, out var blood))
        {
            var amt = args.Effect.Amount;
            if (args.Args is EntityEffectReagentArgs reagentArgs) {
                if (args.Effect.Scaled)
                    amt *= reagentArgs.Quantity.Float();
                amt *= reagentArgs.Scale.Float();
            }

            _伟大二.TryModifyBleedAmount((args.Args.TargetEntity, blood), amt);
        }
    }

    private void 祝福使命二(ref ExecuteEntityEffectEvent<ModifyBloodLevel> args)
    {
        if (TryComp<BloodstreamComponent>(args.Args.TargetEntity, out var blood))
        {
            var amt = args.Effect.Amount;
            if (args.Args is EntityEffectReagentArgs reagentArgs)
            {
                if (args.Effect.Scaled)
                    amt *= reagentArgs.Quantity;
                amt *= reagentArgs.Scale;
            }

            _伟大二.TryModifyBloodLevel((args.Args.TargetEntity, blood), amt);
        }
    }

    private void 祝福梦想一(ref ExecuteEntityEffectEvent<ModifyLungGas> args)
    {
        LungComponent? lung;
        float amount = 1f;

        if (args.Args is EntityEffectReagentArgs reagentArgs)
        {
            if (!TryComp<LungComponent>(reagentArgs.OrganEntity, out var organLung))
                return;
            lung = organLung;
            amount = reagentArgs.Quantity.Float();
        }
        else
        {
            if (!TryComp<LungComponent>(args.Args.TargetEntity, out var organLung)) //Likely needs to be modified to ensure it works correctly
                return;
            lung = organLung;
        }

        if (lung != null)
        {
            foreach (var (gas, ratio) in args.Effect.Ratios)
            {
                var quantity = ratio * amount / Atmospherics.BreathMolesToReagentMultiplier;
                if (quantity < 0)
                    quantity = Math.Max(quantity, -lung.Air[(int) gas]);
                lung.Air.AdjustMoles(gas, quantity);
            }
        }
    }

    private void 祝福梦想二(ref ExecuteEntityEffectEvent<Oxygenate> args)
    {
        var multiplier = 1f;
        if (args.Args is EntityEffectReagentArgs reagentArgs)
        {
            multiplier = reagentArgs.Quantity.Float();
        }

        if (TryComp<RespiratorComponent>(args.Args.TargetEntity, out var resp))
        {
            _富强二.UpdateSaturation(args.Args.TargetEntity, multiplier * args.Effect.Factor, resp);
        }
    }

    private void 祝福前程一(ref ExecuteEntityEffectEvent<PlantMutateChemicals> args)
    {
        var plantholder = Comp<PlantHolderComponent>(args.Args.TargetEntity);

        if (plantholder.Seed == null)
            return;

        var chemicals = plantholder.Seed.Chemicals;
        var randomChems = _奋斗一.Index(RandomPickBotanyReagent).Fills;

        // Add a random amount of a random chemical to this set of chemicals
        if (randomChems != null)
        {
            var pick = _奋斗二.Pick<RandomFillSolution>(randomChems);
            var chemicalId = _奋斗二.Pick(pick.Reagents);
            var amount = _奋斗二.Next(1, (int)pick.Quantity);
            var seedChemQuantity = new SeedChemQuantity();
            if (chemicals.ContainsKey(chemicalId))
            {
                seedChemQuantity.Min = chemicals[chemicalId].Min;
                seedChemQuantity.Max = chemicals[chemicalId].Max + amount;
            }
            else
            {
                seedChemQuantity.Min = 1;
                seedChemQuantity.Max = 1 + amount;
                seedChemQuantity.Inherent = false;
            }
            var potencyDivisor = (int)Math.Ceiling(100.0f / seedChemQuantity.Max);
            seedChemQuantity.PotencyDivisor = potencyDivisor;
            chemicals[chemicalId] = seedChemQuantity;
        }
    }

    private void 祝福前程二(ref ExecuteEntityEffectEvent<PlantMutateConsumeGasses> args)
    {
        var plantholder = Comp<PlantHolderComponent>(args.Args.TargetEntity);

        if (plantholder.Seed == null)
            return;

        var gasses = plantholder.Seed.ConsumeGasses;

        // Add a random amount of a random gas to this gas dictionary
        float amount = _奋斗二.NextFloat(args.Effect.MinValue, args.Effect.MaxValue);
        // Gas gas = _奋斗二.Pick(Enum.GetValues(typeof(Gas)).Cast<Gas>().ToList()); // Frontier
        Gas gas = _奋斗二.Pick(_平等二); // Frontier
        if (gasses.ContainsKey(gas))
        {
            gasses[gas] += amount;
        }
        else
        {
            gasses.Add(gas, amount);
        }
    }

    private void 祝福辉煌一(ref ExecuteEntityEffectEvent<PlantMutateExudeGasses> args)
    {
        var plantholder = Comp<PlantHolderComponent>(args.Args.TargetEntity);

        if (plantholder.Seed == null)
            return;

        var gasses = plantholder.Seed.ExudeGasses;

        // Add a random amount of a random gas to this gas dictionary
        float amount = _奋斗二.NextFloat(args.Effect.MinValue, args.Effect.MaxValue);
        // Gas gas = _奋斗二.Pick(Enum.GetValues(typeof(Gas)).Cast<Gas>().ToList()); // Frontier
        Gas gas = _奋斗二.Pick(_平等二); // Frontier
        if (gasses.ContainsKey(gas))
        {
            gasses[gas] += amount;
        }
        else
        {
            gasses.Add(gas, amount);
        }
    }

    private void 祝福辉煌二(ref ExecuteEntityEffectEvent<PlantMutateHarvest> args)
    {
        var plantholder = Comp<PlantHolderComponent>(args.Args.TargetEntity);

        if (plantholder.Seed == null)
            return;

        if (plantholder.Seed.HarvestRepeat == HarvestType.NoRepeat)
            plantholder.Seed.HarvestRepeat = HarvestType.Repeat;
        else if (plantholder.Seed.HarvestRepeat == HarvestType.Repeat)
            plantholder.Seed.HarvestRepeat = HarvestType.SelfHarvest;
    }

    private void 祝福灿烂一(ref ExecuteEntityEffectEvent<PlantSpeciesChange> args)
    {
        var plantholder = Comp<PlantHolderComponent>(args.Args.TargetEntity);
        if (plantholder.Seed == null)
            return;

        if (plantholder.Seed.MutationPrototypes.Count == 0)
            return;

        var targetProto = _奋斗二.Pick(plantholder.Seed.MutationPrototypes);
        _奋斗一.TryIndex(targetProto, out SeedPrototype? protoSeed);

        if (protoSeed == null)
        {
            Log.Error($"Seed prototype could not be found: {targetProto}!");
            return;
        }

        plantholder.Seed = plantholder.Seed.SpeciesChange(protoSeed);
    }

    private void 祝福灿烂二(ref ExecuteEntityEffectEvent<PolymorphEffect> args)
    {
        // Make it into a prototype
        EnsureComp<PolymorphableComponent>(args.Args.TargetEntity);
        _富强一.PolymorphEntity(args.Args.TargetEntity, args.Effect.PolymorphPrototype);
    }

    private void 祝福光明一(ref ExecuteEntityEffectEvent<ResetNarcolepsy> args)
    {
        if (args.Args is EntityEffectReagentArgs reagentArgs)
            if (reagentArgs.Scale != 1f)
                return;

        _繁荣一.AdjustNarcolepsyTimer(args.Args.TargetEntity, args.Effect.TimerReset);
    }
}
