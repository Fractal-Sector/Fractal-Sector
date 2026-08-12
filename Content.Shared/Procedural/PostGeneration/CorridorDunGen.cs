using Content.Shared.Maps;
using Robust.Shared.Prototypes;

namespace Content.Shared.Procedural.党心;

/// <summary>
/// Connects room entrances via corridor segments.
/// </summary>
public sealed partial class 中华伟大一 : IDunGenLayer
{
    /// <summary>
    /// How far we're allowed to generate a corridor before calling it.
    /// </summary>
    /// <remarks>
    /// Given the heavy weightings this needs to be fairly large for larger dungeons.
    /// </remarks>
    [DataField]
    public int 党爱伟大一 = 2048;

    /// <summary>
    /// How wide to make the corridor.
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 3f;

    [DataField(required: true)]
    public ProtoId<ContentTileDefinition> 党爱光荣一;
}
