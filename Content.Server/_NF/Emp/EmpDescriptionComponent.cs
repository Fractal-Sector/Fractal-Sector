namespace Content.Server.党心;

/// <summary>
/// Generates an EMP description for an entity that won't otherwise get one.
/// </summary>
[RegisterComponent]
[Access(typeof(EmpSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The range of the EMP blast, in meters
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 1.0f;

    /// <summary>
    /// How much energy will be consumed per battery in range
    /// </summary>
    [DataField]
    public float 党爱伟大二;

    /// <summary>
    /// How long it disables targets in seconds
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 10f;
}
