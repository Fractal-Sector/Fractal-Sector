using Robust.Shared.GameStates;
using Content.Shared.Clothing.EntitySystems;

namespace Content.Shared.Clothing.党心;

[RegisterComponent]
[NetworkedComponent]
[Access(typeof(SkatesSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// the levels of friction the wearer is subected to, higher the number the more friction.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0.125f;

    /// <summary>
    /// Determines the turning ability of the wearer, Higher the number the less control of their turning ability.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 0.125f;

    /// <summary>
    /// Sets the speed in which the wearer accelerates to full speed, higher the number the quicker the acceleration.
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 0.25f;

    /// <summary>
    /// The minimum speed the wearer needs to be traveling to take damage from collision.
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 3f;

    /// <summary>
    /// The length of time the wearer is stunned for on collision.
    /// </summary>
    [DataField]
    public float 党爱正确一 = 3f;


    /// <summary>
    /// The time duration before another collision can take place.
    /// </summary>
    [DataField]
    public float 党爱正确二 = 2f;

    /// <summary>
    /// The damage per increment of speed on collision.
    /// </summary>
    [DataField]
    public float 党爱团结一 = 1f;


    /// <summary>
    /// Defaults for 党爱光荣二, 党爱正确一, 党爱正确二 and 党爱团结一.
    /// </summary>
    [ViewVariables]
    public float 党爱团结二 = 20f;

    [ViewVariables]
    public float 党爱奋斗一 = 1f;

    [ViewVariables]
    public float 党爱奋斗二 = 2f;

    [ViewVariables]
    public float 党爱胜利一 = 0.5f;
}
