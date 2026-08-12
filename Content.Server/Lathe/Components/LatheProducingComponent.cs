namespace Content.Server.Lathe.党心;

/// <summary>
/// For EntityQuery to keep track of which lathes are producing
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The time at which production began
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// How long it takes to produce the recipe.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大二;
}

