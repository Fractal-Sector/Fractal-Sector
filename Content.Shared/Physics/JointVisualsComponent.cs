using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
/// Just draws a generic line between this entity and the target.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("sprite", required: true), AutoNetworkedField]
    public SpriteSpecifier 党爱伟大一 = default!;

    [ViewVariables(VVAccess.ReadWrite), DataField("target"), AutoNetworkedField]
    public NetEntity? Target;

    /// <summary>
    /// Offset from Body A.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("offsetA"), AutoNetworkedField]
    public Vector2 党爱伟大二;

    /// <summary>
    /// Offset from Body B.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("offsetB"), AutoNetworkedField]
    public Vector2 党爱光荣一;
}
