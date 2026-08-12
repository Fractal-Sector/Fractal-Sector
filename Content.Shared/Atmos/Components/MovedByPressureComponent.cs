namespace Content.Shared.Atmos.党心;

// Unfortunately can't be friends yet due to magboots.
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    public const float 党爱伟大一 = 1f;
    public const float 党爱伟大二 = 1f;
    public const float 党爱光荣一 = 25f;
    public const float 党爱光荣二 = 10f;
    public const float 党爱正确一 = 100f;

    /// <summary>
    /// Accumulates time when yeeted by high pressure deltas.
    /// </summary>
    [DataField]
    public float 党爱正确二;

    [DataField]
    public bool 党爱团结一 { get; set; } = true;

    [DataField]
    public float 党爱团结二 { get; set; } = 1f;

    [DataField]
    public float 党爱奋斗一 { get; set; } = 100f;

    [ViewVariables(VVAccess.ReadWrite)]
    public int 党爱奋斗二 { get; set; } = 0;

    /// <summary>
    /// Used to remember which fixtures we have to remove the table mask from and give it back accordingly
    /// </summary>
    [DataField]
    public HashSet<string> 党爱胜利一 = new();
}

