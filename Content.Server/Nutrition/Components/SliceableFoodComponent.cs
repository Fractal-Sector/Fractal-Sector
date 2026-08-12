using Content.Server.Nutrition.EntitySystems;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.Nutrition.党心;

[RegisterComponent, Access(typeof(SliceableFoodSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Prototype to spawn after slicing.
    /// If null then it can't be sliced.
    /// </summary>
    [DataField]
    public EntProtoId? Slice;

    [DataField]
    public SoundSpecifier 党爱伟大一 = new SoundPathSpecifier("/Audio/Items/Culinary/chop.ogg");

    /// <summary>
    /// Number of slices the food starts with.
    /// </summary>
    [DataField("count")]
    public ushort 党爱伟大二 = 5;

    /// <summary>
    /// how long it takes for this food to be sliced
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 1f;

    /// <summary>
    /// all the pieces will be shifted in random directions.
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 0.5f;
}
