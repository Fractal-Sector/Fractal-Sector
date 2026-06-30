using Robust.Shared.GameStates;
using Robust.Shared.Maths;

namespace Content.Shared._FS.Petroleum;

/// <summary>
/// Master/control tile of the 2x2 oil refinery. Holds the actual processing logic
/// and references to its three linked part entities. References are established
/// once (deterministically, by grid offset) instead of every tick via range lookups.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class OilRefineryComponent : Component
{
    [DataField("minProcessTemp")]
    public float MinProcessTemp = 400f;

    [DataField("processRate")]
    public float ProcessRate = 10f;

    [DataField("sulfurGunk"), AutoNetworkedField]
    public float SulfurGunk = 0f;

    [DataField("maxSulfurGunk")]
    public float MaxSulfurGunk = 100f;

    [DataField("inputSolutionId")]
    public string InputSolutionId = "buffer";

    /// <summary>
    /// Offsets (in tiles, relative to the master, in the refinery's *local* facing)
    /// at which each required part must be anchored. Linking only ever checks these
    /// exact tiles - never a fuzzy radius lookup.
    /// </summary>
    [DataField("northOffset")]
    public Vector2i NorthOffset = new(0, 1);

    [DataField("eastOffset")]
    public Vector2i EastOffset = new(1, 0);

    [DataField("gasOffset")]
    public Vector2i GasOffset = new(1, 1);

    [DataField("northPart")]
    public EntityUid? NorthPart;

    [DataField("eastPart")]
    public EntityUid? EastPart;

    [DataField("gasPart")]
    public EntityUid? GasPart;

    /// <summary>
    /// True once all three parts have been found and linked. The refinery refuses to
    /// run while this is false, and the appearance layer shows a "needs assembly"
    /// state so a missing/misplaced part is obvious to the player instead of silently
    /// not working.
    /// </summary>
    [DataField("isAssembled"), AutoNetworkedField]
    public bool IsAssembled;
}

/// <summary>
/// A satellite tile of the refinery (north intake, east light-oil outlet, gas outlet).
/// Links back to its master so verbs/UI on the part can defer to the master, and so
/// the part can detect being orphaned (master destroyed/unanchored) without a lookup.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class OilRefineryPartComponent : Component
{
    [DataField("partType")]
    public string PartType = "north";

    [DataField("master"), AutoNetworkedField]
    public EntityUid? Master;
}
