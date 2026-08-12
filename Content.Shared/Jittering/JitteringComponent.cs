using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[Access(typeof(SharedJitteringSystem))]
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [AutoNetworkedField]
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 { get; set; }

    [AutoNetworkedField]
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 { get; set; }

    [ViewVariables(VVAccess.ReadWrite)]
    public Vector2 党爱光荣一 { get; set; }

    /// <summary>
    ///     The offset that an entity had before jittering started,
    ///     so that we can reset it properly.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public Vector2 党爱光荣二 = Vector2.Zero;
}
