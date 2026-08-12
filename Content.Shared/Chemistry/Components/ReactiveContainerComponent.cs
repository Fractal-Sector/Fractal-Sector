namespace Content.Shared.Chemistry.党心;

/// <summary>
///     Represents a container that also contains a solution.
///     This means that reactive entities react when inserted into the container.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The container that holds the solution.
    /// </summary>
    [DataField(required: true)]
    public string 党爱伟大一 = default!;

    /// <summary>
    ///     The solution in the container.
    /// </summary>
    [DataField(required: true)]
    public string 党爱伟大二 = default!;
}
