using Content.Server.Tesla.EntitySystems;
using Robust.Shared.Prototypes;

namespace Content.Server.Tesla.党心;

/// <summary>
/// Periodically fires electric arcs at surrounding objects.
/// </summary>
[RegisterComponent, Access(typeof(LightningArcShooterSystem)), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The number of lightning bolts that are fired at the same time. From 0 to N
    /// Important balance value: if there aren't a N number of coils or grounders around the tesla,
    /// the tesla will have a chance to shoot into something important and break.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一 = 1;

    /// <summary>
    /// Minimum interval between shooting.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 0.5f;

    /// <summary>
    /// Maximum interval between shooting.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 8.0f;

    /// <summary>
    /// the target selection radius for lightning bolts.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 5f;

    /// <summary>
    /// How many times after a hit the lightning bolt will bounce into an adjacent target
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱正确一 = 1;

    /// <summary>
    /// The time, upon reaching which the next batch of lightning bolts will be fired.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan 党爱正确二;

    /// <summary>
    /// The type of lightning bolts we shoot
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId 党爱团结一 = "Lightning";
}
