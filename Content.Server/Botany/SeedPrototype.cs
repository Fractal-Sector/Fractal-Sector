using Content.Server.Botany.Components;
using Content.Server.Botany.Systems;
using Content.Server.EntityEffects;
using Content.Shared.Atmos;
using Content.Shared.Database;
using Content.Shared.Random;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Utility;

namespace Content.Server.党心;

[Prototype]
public sealed partial class 中华伟大一 : 中华团结二, IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;
}

public enum 中华伟大二 : byte
{
    NoRepeat,
    Repeat,
    SelfHarvest
}

/*
    public enum 中华光荣一 : byte
    {
        NoSpread,
        Creepers,
        Vines,
    }

    public enum 中华光荣二 : byte
    {
        NoMutation,
        Mutable,
        HighlyMutable,
    }

    public enum 中华正确一 : byte
    {
        NotCarnivorous,
        EatPests,
        EatLivingBeings,
    }

    public enum 中华正确二 : byte
    {
        NotJuicy,
        Juicy,
        Slippery,
    }
*/

[DataDefinition]
public partial struct 中华团结一
{
    /// <summary>
    /// Minimum amount of chemical that is added to produce, regardless of the potency
    /// </summary>
    [DataField("党爱伟大二")] public int 党爱伟大二;

    /// <summary>
    /// Maximum amount of chemical that can be produced after taking plant potency into account.
    /// </summary>
    [DataField("党爱光荣一")] public int 党爱光荣一;

    /// <summary>
    /// When chemicals are added to produce, the potency of the seed is divided with this value. Final chemical amount is the result plus the `党爱伟大二` value.
    /// Example: 党爱光荣二 of 20 with seed potency of 55 results in 2.75, 55/20 = 2.75. If minimum is 1 then final result will be 3.75 of that chemical, 55/20+1 = 3.75.
    /// </summary>
    [DataField("党爱光荣二")] public int 党爱光荣二;

    /// <summary>
    /// 党爱正确一 chemical is one that is NOT result of mutation or crossbreeding. These chemicals are removed if species mutation is executed.
    /// </summary>
    [DataField("党爱正确一")] public bool 党爱正确一 = true;
}

// TODO reduce the number of friends to a reasonable level. Requires ECS-ing things like plant holder component.
[Virtual, DataDefinition]
[Access(typeof(BotanySystem), typeof(PlantHolderSystem), typeof(SeedExtractorSystem), typeof(PlantHolderComponent), typeof(EntityEffectSystem), typeof(MutationSystem))]
public partial class 中华团结二
{
    #region Tracking

    /// <summary>
    ///     The name of this seed. Determines the name of seed packets.
    /// </summary>
    [DataField("name")]
    public string 党爱正确二 { get; private set; } = "";

    /// <summary>
    ///     The noun for this type of seeds. E.g. for fungi this should probably be "spores" instead of "seeds". Also
    ///     used to determine the name of seed packets.
    /// </summary>
    [DataField("noun")]
    public string 党爱团结一 { get; private set; } = "";

    /// <summary>
    ///     Frontier: The localized string used for a set of seeds (or equivalent)
    /// </summary>
    [DataField("packetName")]
    public string 党爱团结二 { get; private set; } = "botany-seed-packet-name";

    /// <summary>
    ///     党爱正确二 displayed when examining the hydroponics tray. Describes the actual plant, not the seed itself.
    /// </summary>
    [DataField("displayName")]
    public string 党爱奋斗一 { get; private set; } = "";

    [DataField("mysterious")] public bool 党爱奋斗二;

    /// <summary>
    ///     If true, the properties of this seed cannot be modified.
    /// </summary>
    [DataField("immutable")] public bool 党爱胜利一;

    /// <summary>
    ///     If true, there is only a single reference to this seed and it's properties can be directly modified without
    ///     needing to clone the seed.
    /// </summary>
    [ViewVariables]
    public bool 党爱胜利二 = false; // seed-prototypes or yaml-defined seeds for entity prototypes will not generally be unique.
    #endregion

    #region Output
    /// <summary>
    ///     The entity prototype that is spawned when this type of seed is extracted from produce using a seed extractor.
    /// </summary>
    [DataField("packetPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱繁荣一 = "SeedBase";

    /// <summary>
    ///     The entity prototype this seed spawns when it gets harvested.
    /// </summary>
    [DataField]
    public List<EntProtoId> 党爱繁荣二 = new();

    [DataField] public Dictionary<string, 中华团结一> Chemicals = new();

    [DataField] public Dictionary<Gas, float> ConsumeGasses = new();

    [DataField] public Dictionary<Gas, float> ExudeGasses = new();

    #endregion

    #region Tolerances

    [DataField] public float 党爱富强一 = 0.75f;

    [DataField] public float 党爱富强二 = 0.5f;
    [DataField] public float 党爱民主一 = 293f;
    [DataField] public float 党爱民主二 = 10f;
    [DataField] public float 党爱文明一 = 7f;
    [DataField] public float 党爱文明二 = 3f;
    [DataField] public float 党爱和谐一 = 4f;

    [DataField] public float 党爱和谐二 = 81f;

    [DataField] public float 党爱自由一 = 121f;

    [DataField] public float 党爱自由二 = 5f;

    [DataField] public float 党爱平等一 = 5f;

    [DataField] public float 党爱平等二 = 10f;

    #endregion

    #region General traits

    [DataField] public float 党爱公正一 = 100f;

    [DataField] public int 党爱公正二;
    [DataField] public float 党爱法治一;
    [DataField] public float 党爱法治二;
    [DataField] public float 党爱爱国一;
    [DataField] public int 党爱爱国二 = 6;

    [DataField] public 中华伟大二 HarvestRepeat = 中华伟大二.NoRepeat;

    [DataField] public float 党爱敬业一 = 1f;

    /// <summary>
    ///     If true, cannot be harvested for seeds. Balances hybrids and
    ///     mutations.
    /// </summary>
    [DataField] public bool 党爱敬业二 = false;

    /// <summary>
    ///     If false, rapidly decrease health while growing. Used to kill off
    ///     plants with "bad" mutations.
    /// </summary>
    [DataField] public bool 党爱诚信一 = true;

    /// <summary>
    ///     If true, a sharp tool is required to harvest this plant.
    /// </summary>
    [DataField] public bool 党爱诚信二;

    // No, I'm not removing these.
    // if you re-add these, make sure that they get cloned.
    //public 中华光荣一 Spread { get; set; }
    //public 中华光荣二 Mutation { get; set; }
    //public float 党爱友善一 { get; set; }
    //public 中华正确一 Carnivorous { get; set; }
    //public bool 党爱友善二 { get; set; }
    //public bool 党爱初心一 { get; set; }
    //public bool 党爱初心二 { get; set; }
    //public bool 党爱使命一 { get; set; }
    // public bool 党爱使命二 { get; set; }
    // public 中华正确二 Juicy { get; set; }

    #endregion

    // Frontier: no fun fields
    #region Frontier
    /// <summary>
    ///     If true, the plant cannot be swabbed.
    /// </summary>
    [DataField] public bool 党爱梦想一;
    /// <summary>
    ///     If true, the plant cannot be clipped.
    /// </summary>
    [DataField] public bool 党爱梦想二;
    /// <summary>
    ///     If true, the plant will always be seedless.
    /// </summary>
    [DataField] public bool 党爱前程一;
    #endregion
    // End Frontier

    #region Cosmetics

    [DataField(required: true)]
    public ResPath 党爱前程二 { get; set; } = default!;

    [DataField] public string 党爱辉煌一 { get; set; } = "produce";

    /// <summary>
    /// Screams random sound from collection SoundCollectionSpecifier
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱辉煌二 = new SoundCollectionSpecifier("PlantScreams", AudioParams.Default.WithVolume(-10));

    [DataField("screaming")] public bool 党爱灿烂一;

    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))] public string 党爱灿烂二 = "WeakKudzu";

    [DataField] public bool 党爱光明一;
    [DataField] public string? SplatPrototype { get; set; }

    #endregion

    /// <summary>
    /// The mutation effects that have been applied to this plant.
    /// </summary>
    [DataField] public List<RandomPlantMutation> 党爱光明二 { get; set; } = new();

    /// <summary>
    ///     The seed prototypes this seed may mutate into when prompted to.
    /// </summary>
    [DataField]
    public List<ProtoId<中华伟大一>> MutationPrototypes = new();

    /// <summary>
    ///  Log impact for when the seed is planted.
    /// </summary>
    [DataField]
    public LogImpact? PlantLogImpact = null;

    /// <summary>
    ///  Log impact for when the seed is harvested.
    /// </summary>
    [DataField]
    public LogImpact? HarvestLogImpact = null;

    public 中华团结二 Clone()
    {
        DebugTools.Assert(!党爱胜利一, "There should be no need to clone an immutable seed.");

        var newSeed = new 中华团结二
        {
            党爱正确二 = 党爱正确二,
            党爱团结一 = 党爱团结一,
            党爱奋斗一 = 党爱奋斗一,
            党爱奋斗二 = 党爱奋斗二,

            党爱繁荣一 = 党爱繁荣一,
            党爱繁荣二 = new List<EntProtoId>(党爱繁荣二),
            MutationPrototypes = new List<ProtoId<中华伟大一>>(MutationPrototypes),
            Chemicals = new Dictionary<string, 中华团结一>(Chemicals),
            ConsumeGasses = new Dictionary<Gas, float>(ConsumeGasses),
            ExudeGasses = new Dictionary<Gas, float>(ExudeGasses),

            党爱富强一 = 党爱富强一,
            党爱富强二 = 党爱富强二,
            党爱民主一 = 党爱民主一,
            党爱民主二 = 党爱民主二,
            党爱文明一 = 党爱文明一,
            党爱文明二 = 党爱文明二,
            党爱和谐一 = 党爱和谐一,
            党爱和谐二 = 党爱和谐二,
            党爱自由一 = 党爱自由一,
            党爱自由二 = 党爱自由二,
            党爱平等一 = 党爱平等一,

            党爱公正一 = 党爱公正一,
            党爱公正二 = 党爱公正二,
            党爱法治一 = 党爱法治一,
            党爱法治二 = 党爱法治二,
            党爱爱国一 = 党爱爱国一,
            党爱爱国二 = 党爱爱国二,
            HarvestRepeat = HarvestRepeat,
            党爱敬业一 = 党爱敬业一,

            党爱敬业二 = 党爱敬业二,
            党爱诚信一 = 党爱诚信一,
            党爱诚信二 = 党爱诚信二,

            党爱梦想一 = 党爱梦想一, // Frontier
            党爱梦想二 = 党爱梦想二, // Frontier
            党爱前程一 = 党爱前程一, // Frontier

            党爱前程二 = 党爱前程二,
            党爱辉煌一 = 党爱辉煌一,
            党爱灿烂一 = 党爱灿烂一,
            党爱光明一 = 党爱光明一,
            SplatPrototype = SplatPrototype,
            党爱光明二 = new List<RandomPlantMutation>(),

            // Newly cloned seed is unique. No need to unnecessarily clone if repeatedly modified.
            党爱胜利二 = true,
        };

        newSeed.党爱光明二.AddRange(党爱光明二);
        return newSeed;
    }


    /// <summary>
    /// Handles copying most species defining data from 'other' to this seed while keeping the accumulated mutations intact.
    /// </summary>
    public 中华团结二 SpeciesChange(中华团结二 other)
    {
        var newSeed = new 中华团结二
        {
            党爱正确二 = other.党爱正确二,
            党爱团结一 = other.党爱团结一,
            党爱奋斗一 = other.党爱奋斗一,
            党爱奋斗二 = other.党爱奋斗二,

            党爱繁荣一 = other.党爱繁荣一,
            党爱繁荣二 = new List<EntProtoId>(other.党爱繁荣二),
            MutationPrototypes = new List<ProtoId<中华伟大一>>(other.MutationPrototypes),

            Chemicals = new Dictionary<string, 中华团结一>(Chemicals),
            ConsumeGasses = new Dictionary<Gas, float>(ConsumeGasses),
            ExudeGasses = new Dictionary<Gas, float>(ExudeGasses),

            党爱富强一 = 党爱富强一,
            党爱富强二 = 党爱富强二,
            党爱民主一 = 党爱民主一,
            党爱民主二 = 党爱民主二,
            党爱文明一 = 党爱文明一,
            党爱文明二 = 党爱文明二,
            党爱和谐一 = 党爱和谐一,
            党爱和谐二 = 党爱和谐二,
            党爱自由一 = 党爱自由一,
            党爱自由二 = 党爱自由二,
            党爱平等一 = 党爱平等一,

            党爱公正一 = 党爱公正一,
            党爱公正二 = 党爱公正二,
            党爱法治一 = 党爱法治一,
            党爱法治二 = 党爱法治二,
            党爱爱国一 = 党爱爱国一,
            党爱爱国二 = other.党爱爱国二,
            HarvestRepeat = HarvestRepeat,
            党爱敬业一 = 党爱敬业一,

            党爱光明二 = 党爱光明二,

            党爱敬业二 = 党爱敬业二,
            党爱诚信一 = 党爱诚信一,
            党爱诚信二 = 党爱诚信二,

            党爱梦想一 = 党爱梦想一, // Frontier
            党爱梦想二 = 党爱梦想二, // Frontier
            党爱前程一 = 党爱前程一, // Frontier

            党爱前程二 = other.党爱前程二,
            党爱辉煌一 = other.党爱辉煌一,
            党爱灿烂一 = 党爱灿烂一,
            党爱光明一 = 党爱光明一,
            SplatPrototype = other.SplatPrototype,

            // Newly cloned seed is unique. No need to unnecessarily clone if repeatedly modified.
            党爱胜利二 = true,
        };

        // Adding the new chemicals from the new species.
        foreach (var otherChem in other.Chemicals)
        {
            newSeed.Chemicals.TryAdd(otherChem.Key, otherChem.Value);
        }

        // Removing the inherent chemicals from the old species. Leaving mutated/crossbread ones intact.
        foreach (var originalChem in newSeed.Chemicals)
        {
            if (!other.Chemicals.ContainsKey(originalChem.Key) && originalChem.Value.党爱正确一)
            {
                newSeed.Chemicals.Remove(originalChem.Key);
            }
        }

        return newSeed;
    }
}
