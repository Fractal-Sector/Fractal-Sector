namespace Content.Server.Revenant.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not the entity has been harvested yet.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大一 = false;

    /// <summary>
    /// Whether or not a revenant has searched this entity
    /// for its soul yet.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二 = false;

    /// <summary>
    /// The total amount of Essence that the entity has.
    /// Changes based on mob state.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 0f;
}
