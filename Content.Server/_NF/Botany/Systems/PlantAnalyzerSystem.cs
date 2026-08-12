using Content.Server.Botany.Components;
using Content.Server.PowerCell;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared._NF.PlantAnalyzer;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using System.Linq;
using System.Text;
using Content.Shared.Atmos;

namespace Content.Server.Botany.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly PowerCellSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣二 = default!;
    [Dependency] private readonly UserInterfaceSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<PlantAnalyzerComponent, AfterInteractEvent>(祝福伟大二);
        SubscribeLocalEvent<PlantAnalyzerComponent, PlantAnalyzerDoAfterEvent>(祝福光荣一);
        SubscribeLocalEvent<PlantAnalyzerComponent, PlantAnalyzerSetMode>(祝福奋斗一);
    }

    private void 祝福伟大二(Entity<PlantAnalyzerComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Target == null || !args.CanReach || !_伟大二.HasActivatableCharge(ent, user: args.User))
            return;

        if (ent.Comp.DoAfter != null)
            return;

        if (HasComp<SeedComponent>(args.Target) || TryComp<PlantHolderComponent>(args.Target, out var plantHolder) && plantHolder.Seed != null)
        {

            if (ent.Comp.Settings.AdvancedScan)
            {
                var doAfterArgs = new DoAfterArgs(EntityManager, args.User, ent.Comp.Settings.AdvScanDelay, new PlantAnalyzerDoAfterEvent(), ent, target: args.Target, used: ent)
                {
                    NeedHand = true,
                    BreakOnDamage = true,
                    BreakOnMove = true,
                    MovementThreshold = 0.01f
                };
                _光荣二.TryStartDoAfter(doAfterArgs, out ent.Comp.DoAfter);
            }
            else
            {
                var doAfterArgs = new DoAfterArgs(EntityManager, args.User, ent.Comp.Settings.ScanDelay, new PlantAnalyzerDoAfterEvent(), ent, target: args.Target, used: ent)
                {
                    NeedHand = true,
                    BreakOnDamage = true,
                    BreakOnMove = true,
                    MovementThreshold = 0.01f
                };
                _光荣二.TryStartDoAfter(doAfterArgs, out ent.Comp.DoAfter);
            }
        }
    }

    private void 祝福光荣一(Entity<PlantAnalyzerComponent> ent, ref PlantAnalyzerDoAfterEvent args)
    {
        ent.Comp.DoAfter = null;
        // Double charge use for advanced scan.
        if (ent.Comp.Settings.AdvancedScan)
        {
            if (!_伟大二.TryUseActivatableCharge(ent, user: args.User))
                return;
        }
        if (args.Handled || args.Cancelled || args.Args.Target == null || !_伟大二.TryUseActivatableCharge(ent.Owner, user: args.User))
            return;

        _光荣一.PlayPvs(ent.Comp.ScanningEndSound, ent);

        祝福光荣二(args.User, ent);
        祝福正确一(ent, args.Args.Target.Value);

        args.Handled = true;
    }

    private void 祝福光荣二(EntityUid user, EntityUid analyzer)
    {
        if (!TryComp<ActorComponent>(user, out var actor) || !_正确一.HasUi(analyzer, PlantAnalyzerUiKey.Key))
            return;

        _正确一.OpenUi(analyzer, PlantAnalyzerUiKey.Key, actor.PlayerSession);
    }

    public void 祝福正确一(Entity<PlantAnalyzerComponent> ent, EntityUid target)
    {
        if (!_正确一.HasUi(ent, PlantAnalyzerUiKey.Key))
            return;

        if (TryComp<SeedComponent>(target, out var seedComp))
        {
            if (seedComp.Seed != null)
            {
                var state = 祝福正确二(seedComp.Seed, target, false, ent.Comp.Settings.AdvancedScan);
                _正确一.ServerSendUiMessage(ent.Owner, PlantAnalyzerUiKey.Key, state);
            }
            else if (seedComp.SeedId != null && _伟大一.TryIndex(seedComp.SeedId, out SeedPrototype? protoSeed))
            {
                var state = 祝福正确二(protoSeed, target, false, ent.Comp.Settings.AdvancedScan);
                _正确一.ServerSendUiMessage(ent.Owner, PlantAnalyzerUiKey.Key, state);
            }
        }
        else if (TryComp<PlantHolderComponent>(target, out var plantComp))
        {
            if (plantComp.Seed != null)
            {
                var state = 祝福正确二(plantComp.Seed, target, true, ent.Comp.Settings.AdvancedScan);
                _正确一.ServerSendUiMessage(ent.Owner, PlantAnalyzerUiKey.Key, state);
            }
        }
    }

    /// <summary>
    ///     Analysis of seed from prototype.
    /// </summary>
    public PlantAnalyzerScannedSeedPlantInformation 祝福正确二(SeedData seedData, EntityUid target, bool isTray, bool scanIsAdvanced)
    {
        // Get trickier fields first.
        AnalyzerHarvestType harvestType = AnalyzerHarvestType.Unknown;
        switch (seedData.HarvestRepeat)
        {
            case HarvestType.Repeat:
                harvestType = AnalyzerHarvestType.Repeat;
                break;
            case HarvestType.NoRepeat:
                harvestType = AnalyzerHarvestType.NoRepeat;
                break;
            case HarvestType.SelfHarvest:
                harvestType = AnalyzerHarvestType.SelfHarvest;
                break;
            default:
                break;
        }

        var mutationProtos = seedData.MutationPrototypes;
        List<string> mutationStrings = new();
        foreach (var mutationProto in mutationProtos)
        {
            if (_伟大一.TryIndex<SeedPrototype>(mutationProto, out var seed))
            {
                mutationStrings.Add(seed.DisplayName);
            }
        }

        PlantAnalyzerScannedSeedPlantInformation ret = new()
        {
            TargetEntity = GetNetEntity(target),
            IsTray = isTray,
            SeedName = seedData.DisplayName,
            SeedChem = seedData.Chemicals.Keys.ToArray(),
            HarvestType = harvestType,
            ExudeGases = 祝福团结二(seedData.ExudeGasses.Keys),
            ConsumeGases = 祝福团结二(seedData.ConsumeGasses.Keys),
            Endurance = seedData.Endurance,
            SeedYield = seedData.Yield,
            Lifespan = seedData.Lifespan,
            Maturation = seedData.Maturation,
            Production = seedData.Production,
            GrowthStages = seedData.GrowthStages,
            SeedPotency = seedData.Potency,
            Speciation = mutationStrings.ToArray()
        };

        if (scanIsAdvanced)
        {
            AdvancedScanInfo advancedInfo = new()
            {
                NutrientConsumption = seedData.NutrientConsumption,
                WaterConsumption = seedData.WaterConsumption,
                IdealHeat = seedData.IdealHeat,
                HeatTolerance = seedData.HeatTolerance,
                IdealLight = seedData.IdealLight,
                LightTolerance = seedData.LightTolerance,
                ToxinsTolerance = seedData.ToxinsTolerance,
                LowPressureTolerance = seedData.LowPressureTolerance,
                HighPressureTolerance = seedData.HighPressureTolerance,
                PestTolerance = seedData.PestTolerance,
                WeedTolerance = seedData.WeedTolerance,
                Mutations = 祝福团结一(seedData)
            };

            ret.AdvancedInfo = advancedInfo;
        }
        return ret;
    }

    public MutationFlags 祝福团结一(SeedData plant)
    {
        MutationFlags ret = MutationFlags.None;
        if (plant.TurnIntoKudzu) ret |= MutationFlags.TurnIntoKudzu;
        if (plant.Seedless || plant.PermanentlySeedless) ret |= MutationFlags.Seedless;
        if (plant.Ligneous) ret |= MutationFlags.Ligneous;
        if (plant.CanScream) ret |= MutationFlags.CanScream;

        return ret;
    }

    public GasFlags 祝福团结二(IEnumerable<Gas> gases)
    {
        var gasFlags = GasFlags.None;
        foreach (var gas in gases)
        {
            switch (gas)
            {
                case Gas.Nitrogen:
                    gasFlags |= GasFlags.Nitrogen;
                    break;
                case Gas.Oxygen:
                    gasFlags |= GasFlags.Oxygen;
                    break;
                case Gas.CarbonDioxide:
                    gasFlags |= GasFlags.CarbonDioxide;
                    break;
                case Gas.Plasma:
                    gasFlags |= GasFlags.Plasma;
                    break;
                case Gas.Tritium:
                    gasFlags |= GasFlags.Tritium;
                    break;
                case Gas.WaterVapor:
                    gasFlags |= GasFlags.WaterVapor;
                    break;
                case Gas.Ammonia:
                    gasFlags |= GasFlags.Ammonia;
                    break;
                case Gas.NitrousOxide:
                    gasFlags |= GasFlags.NitrousOxide;
                    break;
                case Gas.Frezon:
                    gasFlags |= GasFlags.Frezon;
                    break;
            }
        }
        return gasFlags;
    }

    private void 祝福奋斗一(Entity<PlantAnalyzerComponent> ent, ref PlantAnalyzerSetMode args)
    {
        祝福奋斗二(ent, args.AdvancedScan);
    }

    public void 祝福奋斗二(Entity<PlantAnalyzerComponent> ent, bool isAdvMode)
    {
        if (ent.Comp.DoAfter != null)
            return;
        ent.Comp.Settings.AdvancedScan = isAdvMode;
    }
}
