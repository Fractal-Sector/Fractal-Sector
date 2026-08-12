using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Emberfall.Weapons.党心;

/// <summary>
/// Added to projectiles to give them tracer effects
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long the tracer effect should remain visible for after firing
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 10f;

    /// <summary>
    /// The maximum length of the tracer trail
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 2f;

    /// <summary>
    /// 党爱光荣一 of the tracer line effect
    /// </summary>
    [DataField]
    public 党爱光荣一 党爱光荣一 = 党爱光荣一.Red;

    [ViewVariables]
    public 中华伟大二 Data = default!;
}

[Serializable, NetSerializable, DataRecord]
public partial struct 中华伟大二(List<Vector2> positionHistory, TimeSpan endTime)
{
    /// <summary>
    /// The history of positions this tracer has moved through
    /// </summary>
    public List<Vector2> 党爱光荣二 = positionHistory;

    /// <summary>
    /// When this tracer effect should end
    /// </summary>
    public TimeSpan 党爱正确一 = endTime;
}
