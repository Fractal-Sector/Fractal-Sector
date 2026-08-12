using Content.Shared.Maps;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

// Ime a worm
/// <summary>
/// Generates worm corridors.
/// </summary>
public sealed partial class 中华伟大一 : IDunGenLayer
{
    [DataField]
    public int 党爱伟大一 = 2048;

    /// <summary>
    /// How many times to run the worm
    /// </summary>
    [DataField]
    public int 党爱伟大二 = 20;

    /// <summary>
    /// How long to make each worm
    /// </summary>
    [DataField]
    public int 党爱光荣一 = 20;

    /// <summary>
    /// Maximum amount the angle can change in a single step.
    /// </summary>
    [DataField]
    public Angle 党爱光荣二 = Angle.FromDegrees(45);

    /// <summary>
    /// How wide to make the corridor.
    /// </summary>
    [DataField]
    public float 党爱正确一 = 3f;

    [DataField(required: true)]
    public ProtoId<ContentTileDefinition> 党爱正确二;
}
