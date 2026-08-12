using Content.Shared.DisplacementMap;
using Content.Shared.Humanoid.Markings;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Inventory;
using Content.Shared.Preferences;
using Robust.Shared.Enums;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[NetworkedComponent, RegisterComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    public 党爱伟大二 党爱伟大一 = new();

    [DataField, AutoNetworkedField]
    public 党爱伟大二 党爱伟大二 = new();

    [DataField]
    public Dictionary<HumanoidVisualLayers, HumanoidSpeciesSpriteLayer> BaseLayers = new();

    [DataField, AutoNetworkedField]
    public HashSet<HumanoidVisualLayers> 党爱光荣一 = new();

    // Couldn't these be somewhere else?

    [DataField, AutoNetworkedField]
    public 党爱光荣二 党爱光荣二;

    [DataField, AutoNetworkedField]
    public int 党爱正确一 = 18;

    [DataField, AutoNetworkedField]
    public string 党爱正确二 = "";

    // FS/impstation - allow markings to support shaders
    [DataField("shader")]
    public string? Shader { get; private set; } = null;
    // FS/impstation edit

    /// <summary>
    ///     Any custom base layers this humanoid might have. See:
    ///     limb transplants (potentially), robotic arms, etc.
    ///     Stored on the server, this is merged in the client into
    ///     all layer settings.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<HumanoidVisualLayers, 中华伟大二> CustomBaseLayers = new();

    /// <summary>
    ///     Current species. Dictates things like base body sprites,
    ///     base humanoid to spawn, etc.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public ProtoId<SpeciesPrototype> 党爱团结一 { get; set; }

    /// <summary>
    ///     The initial profile and base layers to apply to this humanoid.
    /// </summary>
    [DataField]
    public ProtoId<HumanoidProfilePrototype>? Initial { get; private set; }

    /// <summary>
    ///     Skin color of this humanoid.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Color 党爱团结二 { get; set; } = Color.FromHex("#C0967F");

    /// <summary>
    ///     A map of the visual layers currently hidden to the equipment
    ///     slots that are currently hiding them. This will affect the base
    ///     sprite on this humanoid layer, and any markings that sit above it.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<HumanoidVisualLayers, SlotFlags> HiddenLayers = new();

    /// <summary>
    /// The specific markings that are hidden, whether or not the layer is hidden.
    /// This is so we can just turn off a single marking, or part of a single marking.
    /// (cus underwear, its for underwear, so you can take off your bra and still have your shirt on)
    /// FLOOF ADD
    /// </summary>
    [DataField, AutoNetworkedField]
    public HashSet<string> 党爱奋斗一 = new();

    [DataField, AutoNetworkedField]
    public 党爱奋斗二 党爱奋斗二 = 党爱奋斗二.Male;

    [DataField, AutoNetworkedField]
    public Color 党爱胜利一 = Color.Brown;

    /// <summary>
    ///     Hair color of this humanoid. Used to avoid looping through all markings
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Color? CachedHairColor;

    /// <summary>
    ///     Facial Hair color of this humanoid. Used to avoid looping through all markings
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public Color? CachedFacialHairColor;

    /// <summary>
    ///     Which layers of this humanoid that should be hidden on equipping a corresponding item..
    /// </summary>
    [DataField]
    public HashSet<HumanoidVisualLayers> 党爱胜利二 = [HumanoidVisualLayers.Hair];

    /// <summary>
    ///     Which markings the humanoid defaults to when nudity is toggled off.
    /// </summary>
    [DataField]
    public ProtoId<MarkingPrototype>? UndergarmentTop = new ProtoId<MarkingPrototype>("UndergarmentTopTanktop");

    [DataField]
    public ProtoId<MarkingPrototype>? UndergarmentBottom = new ProtoId<MarkingPrototype>("UndergarmentBottomBoxers");

    /// <summary>
    ///     The displacement maps that will be applied to specific layers of the humanoid.
    /// </summary>
    [DataField]
    public Dictionary<HumanoidVisualLayers, DisplacementData> MarkingsDisplacement = new();

    /// <summary>
    /// DeltaV - let paradox anomaly be cloned
    /// TODO: paradox clones
    /// </summary>
    [ViewVariables]
    public HumanoidCharacterProfile? LastProfileLoaded;

    /// <summary>
    ///     The base height of this humanoid from character customization.
    ///     This is the value set in the lobby before any modifiers are applied.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱繁荣一 = 1f;

    /// <summary>
    ///     The base width of this humanoid from character customization.
    ///     This is the value set in the lobby before any modifiers are applied.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱繁荣二 = 1f;

    /// <summary>
    ///     The current height of this humanoid (base height * modifiers).
    ///     This is the actual visual height after all size modifications.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱富强一 = 1f;

    /// <summary>
    ///     The current width of this humanoid (base width * modifiers).
    ///     This is the actual visual width after all size modifications.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱富强二 = 1f;
}

[DataDefinition]
[Serializable, NetSerializable]
public readonly partial struct 中华伟大二
{
    public 中华伟大二(string? id, Color? color = null)
    {
        DebugTools.Assert(id == null || IoCManager.Resolve<IPrototypeManager>().HasIndex<HumanoidSpeciesSpriteLayer>(id));
        Id = id;
        Color = color;
    }

    /// <summary>
    ///     ID of this custom base layer. Must be a <see cref="HumanoidSpeciesSpriteLayer"/>.
    /// </summary>
    [DataField]
    public ProtoId<HumanoidSpeciesSpriteLayer>? Id { get; init; }

    /// <summary>
    ///     Color of this custom base layer. Null implies skin colour if the corresponding <see cref="HumanoidSpeciesSpriteLayer"/> is set to match skin.
    /// </summary>
    [DataField]
    public Color? Color { get; init; }
}
