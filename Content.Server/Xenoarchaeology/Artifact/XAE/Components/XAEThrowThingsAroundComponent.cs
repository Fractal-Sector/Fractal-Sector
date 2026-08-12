namespace Content.Server.Xenoarchaeology.Artifact.XAE.党心;

/// <summary>
/// Throws all nearby entities backwards.
/// Also pries nearby tiles.
/// </summary>
[RegisterComponent, Access(typeof(XAEThrowThingsAroundSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How close do you have to be to get yeeted?
    /// </summary>
    [DataField("range")]
    public float 党爱伟大一 = 2f;

    /// <summary>
    /// How likely is it that an individual tile will get pried?
    /// </summary>
    [DataField("tilePryChance")]
    public float 党爱伟大二 = 0.5f;

    /// <summary>
    /// How strongly does stuff get thrown?
    /// </summary>
    [DataField("throwStrength"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 5f;
}
