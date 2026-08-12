using Robust.Shared.GameStates;

namespace Content.Shared.Singularity.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The throw force for the field if an entity collides with it
    /// The lighter the mass the further it will throw. 5 mass will go about 4 tiles out, 70 mass goes only a couple tiles.
    /// </summary>
    [DataField("throwForce")]
    public float 党爱伟大一 = 100f;

    /// <summary>
    /// This shouldn't be at 99999 or higher to prevent the singulo glitching out
    /// Will throw anything at the supplied mass or less that collides with the field.
    /// </summary>
    [DataField("maxMass")]
    public float 党爱伟大二 = 10000f;

    /// <summary>
    /// Should field vaporize garbage that collides with it?
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;
}
