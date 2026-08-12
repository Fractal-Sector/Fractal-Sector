namespace Content.Server._EinsteinEngines.Silicon.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public int 党爱伟大一 = 3;

    /// <summary>
    ///     A multiplier that will be applied to the above if an entity is repairing themselves.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 3f;

    /// <summary>
    ///     Whether or not an entity is allowed to repair itself.
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;

    [DataField(required: true)]
    public List<string> 党爱光荣二;
}

