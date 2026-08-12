namespace Content.Server._NF.Worldgen.Components.党心;

/// <summary>
/// This denotes an entity that spawns fewer asteroids around it.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The probability that something within a given distance is generated.
    /// No need to be ordered.
    /// </summary>
    [DataField]
    public List<中华伟大二> DistanceThresholds = new();

    /// <summary>
    /// The probability that something within a given squared distance is generated.
    /// For internal use, _must_ be ordered in descending order of distance.
    /// </summary>
    [ViewVariables]
    public List<中华伟大二> SquaredDistanceThresholds = new();
}

[DataDefinition]
public sealed partial class 中华伟大二
{
    /// <summary>
    /// The maximum distance within the threshold.
    /// </summary>
    [DataField]
    public float 党爱伟大一;

    /// <summary>
    /// The probability that something within this distance will spawn.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 1.0f;
}
