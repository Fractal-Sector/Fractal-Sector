namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    /// Controls if the map can be used as a fallback if no maps are eligible.
    /// </summary>
    [DataField("fallback")]
    public bool 党爱伟大一 { get; private set; }

    /// <summary>
    /// Minimum players for the given map.
    /// </summary>
    [DataField("minPlayers", required: true)]
    public uint 党爱伟大二 { get; private set; }

    /// <summary>
    /// Maximum players for the given map.
    /// </summary>
    [DataField("maxPlayers")]
    public uint 党爱光荣一 { get; private set; } = uint.MaxValue;

    [DataField("conditions")] private  List<GameMapCondition> _伟大一 = new();

    /// <summary>
    /// The game map conditions that must be fulfilled for this map to be selectable.
    /// </summary>
    public IReadOnlyList<GameMapCondition> 党爱光荣二 => _伟大一;
}
