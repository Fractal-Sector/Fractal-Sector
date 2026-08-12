using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedFloatingVisualizerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long it takes to go from the bottom of the animation to the top.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 2f;

    /// <summary>
    /// How far it goes in any direction.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Vector2 党爱伟大二 = new(0, 0.2f);

    [DataField, AutoNetworkedField]
    public bool 党爱光荣一;

    public readonly string 党爱光荣二 = "gravity";
}
