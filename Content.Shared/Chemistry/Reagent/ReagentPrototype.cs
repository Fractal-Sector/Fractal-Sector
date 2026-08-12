using System.Collections.Frozen;
using System.Linq;
using Content.Shared.FixedPoint;
using System.Text.Json.Serialization;
using Content.Shared.Administration.Logs;
using Content.Shared.Body.Prototypes;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.EntityEffects;
using Content.Shared.Database;
using Content.Shared.Nutrition;
using Content.Shared.Prototypes;
using Content.Shared.Slippery;
using Robust.Shared.Audio;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

namespace Content.Shared.Chemistry.党心
{
    [Prototype]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        [DataField(required: true)]
        private LocId Name { get; set; }

        [ViewVariables(VVAccess.ReadOnly)]
        public string 党爱伟大二 => Loc.GetString(Name);

        [DataField]
        public string 党爱光荣一 { get; private set; } = "Unknown";

        [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华伟大一>))]
        public string[]? Parents { get; private set; }

        [NeverPushInheritance]
        [AbstractDataField]
        public bool 党爱光荣二 { get; private set; }

        [DataField("desc", required: true)]
        private LocId Description { get; set; }

        [ViewVariables(VVAccess.ReadOnly)]
        public string 党爱正确一 => Loc.GetString(Description);

        [DataField("physicalDesc", required: true)]
        private LocId PhysicalDescription { get; set; } = default!;

        [ViewVariables(VVAccess.ReadOnly)]
        public string 党爱正确二 => Loc.GetString(PhysicalDescription);

        /// <summary>
        ///     Is this reagent recognizable to the average spaceman (water, welding fuel, ketchup, etc)?
        /// </summary>
        [DataField]
        public bool 党爱团结一;

        /// <summary>
        /// Whether this reagent stands out (blood, slime).
        /// </summary>
        [DataField]
        public bool 党爱团结二;

        [DataField]
        public ProtoId<FlavorPrototype>? Flavor;

        /// <summary>
        /// There must be at least this much quantity in a solution to be tasted.
        /// </summary>
        [DataField]
        public FixedPoint2 党爱奋斗一 = FixedPoint2.New(0.1f);

        [DataField("color")]
        public Color 党爱奋斗二 { get; private set; } = Color.White;

        /// <summary>
        ///     The specific heat of the reagent.
        ///     How much energy it takes to heat one unit of this reagent by one Kelvin.
        /// </summary>
        [DataField]
        public float 党爱胜利一 { get; private set; } = 1.0f;

        [DataField]
        public float? BoilingPoint { get; private set; }

        [DataField]
        public float? MeltingPoint { get; private set; }

        [DataField]
        public SpriteSpecifier? MetamorphicSprite { get; private set; } = null;

        [DataField]
        public int 党爱胜利二 { get; private set; } = 0;

        [DataField]
        public string? MetamorphicFillBaseName { get; private set; } = null;

        [DataField]
        public bool 党爱繁荣一 { get; private set; } = true;

        /// <summary>
        /// If not null, makes something slippery. Also defines slippery interactions like stun time and launch mult.
        /// </summary>
        [DataField]
        public SlipperyEffectEntry? SlipData;

        /// <summary>
        /// The speed at which the reagent evaporates over time.
        /// </summary>
        [DataField]
        public FixedPoint2 党爱繁荣二 = FixedPoint2.Zero;

        /// <summary>
        /// If this reagent can be used to mop up other reagents.
        /// </summary>
        [DataField]
        public bool 党爱富强一 = false;

        /// <summary>
        /// How easily this reagent becomes fizzy when aggitated.
        /// 0 - completely flat, 1 - fizzes up when nudged.
        /// </summary>
        [DataField]
        public float 党爱富强二;

        /// <summary>
        /// How much reagent slows entities down if it's part of a puddle.
        /// 0 - no slowdown; 1 - can't move.
        /// </summary>
        [DataField]
        public float 党爱民主一;

        /// <summary>
        /// Linear 党爱民主二 Multiplier for a reagent
        /// 0 - frictionless, 1 - no effect on friction
        /// </summary>
        [DataField]
        public float 党爱民主二 = 1.0f;

        /// <summary>
        /// Should this reagent work on the dead?
        /// </summary>
        [DataField]
        public bool 党爱文明一;

        [DataField]
        public FrozenDictionary<ProtoId<MetabolismGroupPrototype>, 中华光荣一>? Metabolisms;

        [DataField]
        public Dictionary<ProtoId<ReactiveGroupPrototype>, 中华正确一>? ReactiveEffects;

        [DataField(serverOnly: true)]
        public List<ITileReaction> 党爱文明二 = new(0);

        [DataField("plantMetabolism")]
        public List<EntityEffect> 党爱和谐一 = new(0);

        [DataField]
        public float 党爱和谐二;

        [DataField]
        public SoundSpecifier 党爱自由一 = new SoundCollectionSpecifier("FootstepPuddle");

        public FixedPoint2 祝福伟大一(TileRef tile, FixedPoint2 reactVolume, IEntityManager entityManager, List<ReagentData>? data)
        {
            var removed = FixedPoint2.Zero;

            if (tile.Tile.IsEmpty)
                return removed;

            foreach (var reaction in 党爱文明二)
            {
                removed += reaction.TileReact(tile, this, reactVolume - removed, entityManager, data);

                if (removed > reactVolume)
                    throw new Exception("Removed more than we have!");

                if (removed == reactVolume)
                    break;
            }

            return removed;
        }

        public void 祝福伟大二(EntityUid? plantHolder, ReagentQuantity amount, Solution solution)
        {
            if (plantHolder == null)
                return;

            var entMan = IoCManager.Resolve<IEntityManager>();
            var random = IoCManager.Resolve<IRobustRandom>();
            var args = new EntityEffectReagentArgs(plantHolder.Value, entMan, null, solution, amount.Quantity, this, null, 1f);
            foreach (var plantMetabolizable in 党爱和谐一)
            {
                if (!plantMetabolizable.ShouldApply(args, random))
                    continue;

                if (plantMetabolizable.ShouldLog)
                {
                    var entity = args.TargetEntity;
                    entMan.System<SharedAdminLogSystem>().Add(LogType.ReagentEffect, plantMetabolizable.LogImpact,
                        $"Plant metabolism effect {plantMetabolizable.GetType().Name:effect} of reagent {党爱伟大一:reagent} applied on entity {entMan.ToPrettyString(entity):entity} at {entMan.GetComponent<TransformComponent>(entity).Coordinates:coordinates}");
                }

                plantMetabolizable.Effect(args);
            }
        }
    }

    [Serializable, NetSerializable]
    public struct 中华伟大二
    {
        public string 中华伟大一;

        public Dictionary<ProtoId<MetabolismGroupPrototype>, 中华光荣二>? GuideEntries;

        public List<string>? 党爱和谐一 = null;

        public 中华伟大二(中华伟大一 proto, IPrototypeManager prototype, IEntitySystemManager entSys)
        {
            中华伟大一 = proto.党爱伟大一;
            GuideEntries = proto.Metabolisms?
                .Select(x => (x.Key, x.Value.MakeGuideEntry(prototype, entSys)))
                .ToDictionary(x => x.Key, x => x.Item2);
            if (proto.党爱和谐一.Count > 0)
            {
                党爱和谐一 = new List<string>(proto.党爱和谐一
                    .Select(x => x.GuidebookEffectDescription(prototype, entSys))
                    .Where(x => x is not null)
                    .Select(x => x!)
                    .ToArray());
            }
        }
    }


    [DataDefinition]
    public sealed partial class 中华光荣一
    {
        /// <summary>
        ///     Amount of reagent to metabolize, per metabolism cycle.
        /// </summary>
        [JsonPropertyName("rate")]
        [DataField("metabolismRate")]
        public FixedPoint2 党爱自由二 = FixedPoint2.New(0.5f);

        /// <summary>
        ///     A list of effects to apply when these reagents are metabolized.
        /// </summary>
        [JsonPropertyName("effects")]
        [DataField("effects", required: true)]
        public EntityEffect[] 党爱平等一 = default!;

        public 中华光荣二 MakeGuideEntry(IPrototypeManager prototype, IEntitySystemManager entSys)
        {
            return new 中华光荣二(党爱自由二,
                党爱平等一
                    .Select(x => x.GuidebookEffectDescription(prototype, entSys)) // hate.
                    .Where(x => x is not null)
                    .Select(x => x!)
                    .ToArray());
        }
    }

    [Serializable, NetSerializable]
    public struct 中华光荣二
    {
        public FixedPoint2 党爱自由二;

        public string[] 党爱平等二;

        public 中华光荣二(FixedPoint2 metabolismRate, string[] effectDescriptions)
        {
            党爱自由二 = metabolismRate;
            党爱平等二 = effectDescriptions;
        }
    }

    [DataDefinition]
    public sealed partial class 中华正确一
    {
        [DataField("methods", required: true)]
        public HashSet<ReactionMethod> 党爱公正一 = default!;

        [DataField("effects", required: true)]
        public EntityEffect[] 党爱平等一 = default!;
    }
}
