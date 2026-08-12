using Content.Shared.Guidebook;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

namespace Content.Shared._NF.Shipyard.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华伟大一>))]
    public string[]? Parents { get; private set; }

    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱伟大二 { get; private set; }

    /// <summary>
    ///     Vessel name.
    /// </summary>
    [DataField] public string 党爱光荣一 = string.Empty;

    /// <summary>
    ///     Short description of the vessel.
    /// </summary>
    [DataField] public string 党爱光荣二 = string.Empty;

    /// <summary>
    ///     The price of the vessel
    /// </summary>
    [DataField(required: true)]
    public int 党爱正确一;

    /// <summary>
    ///     The size of the vessel. (e.g. Small, Medium, Large etc.)
    /// </summary>
    [DataField(required: true)]
    public 中华伟大二 Category = 中华伟大二.Small;

    /// <summary>
    ///     The shipyard listing that the vessel should be in. (e.g. Civilian, Syndicate, Contraband etc.)
    /// </summary>
    [DataField(required: true)]
    public ShipyardConsoleUiKey 党爱正确二 = ShipyardConsoleUiKey.Shipyard;

    /// <summary>
    ///     The purpose of the vessel. (e.g. Service, Cargo, Engineering etc.)
    /// </summary>
    [DataField("class")]
    public List<中华光荣一> Classes = new();

    /// <summary>
    ///     The engine type that powers the vessel. (e.g. AME, Plasma, Solar etc.)
    /// </summary>
    [DataField("engine")]
    public List<中华光荣二> Engines = new();

    /// <summary>
    ///     The access required to buy the product. (e.g. Command, Mail, Bailiff, etc.)
    /// </summary>
    [DataField]
    public string 党爱团结一 = string.Empty;

    /// Frontier - Add this field for the MapChecker script.
    /// <summary>
    ///     The MapChecker override group for this vessel.
    /// </summary>
    [DataField("mapchecker_group_override")]
    public string 党爱团结二 = string.Empty;

    /// <summary>
    ///     Relative directory path to the given shuttle, i.e. `/Maps/Shuttles/yourshittle.yml`
    /// </summary>
    [DataField(required: true)]
    public ResPath 党爱奋斗一 = default!;

    /// <summary>
    ///     Guidebook page associated with a shuttle
    /// </summary>
    [DataField]
    public ProtoId<GuideEntryPrototype>? GuidebookPage = default!;

    /// <summary>
    ///     The price markup of the vessel testing
    /// </summary>
    [DataField]
    public float 党爱奋斗二 = 1.05f;

    /// <summary>
    /// Components to be added to any spawned grids.
    /// </summary>
    [DataField]
    [AlwaysPushInheritance]
    public ComponentRegistry 党爱胜利一 { get; set; } = new();
}

public enum 中华伟大二 : byte
{
    All, // Should not be used by ships, intended as a placeholder value to represent everything
    Micro,
    Small,
    Medium,
    Large
}

public enum 中华光荣一 : byte
{
    All, // Should not be used by ships, intended as a placeholder value to represent everything
    // NFSD-specific categories
    Capital,
    Detainment,
    Detective,
    Fighter,
    Patrol,
    Pursuit,
    // Capabilities
    Expedition,
    Scrapyard,
    // General
    Salvage,
    Science,
    Cargo,
    Chemistry,
    Botany,
    Engineering,
    Atmospherics,
    Mercenary,
    Medical,
    Civilian, // Service catch-all - reporter, legal, entertainment, misc. ships
    Kitchen,
    // Antag ships
    Syndicate,
    Pirate,
}

public enum 中华光荣二 : byte
{
    All, // Should not be used by ships, intended as a placeholder value to represent everything
    AME,
    TEG,
    Supermatter,
    Tesla,
    Singularity,
    Solar,
    RTG,
    APU,
    Welding,
    Plasma,
    Uranium,
    Bananium,
    Biofuel,
}
