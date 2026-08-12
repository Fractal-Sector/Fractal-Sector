using Content.Server.Chemistry.EntitySystems;
using Content.Shared.Random;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Chemistry.党心;

/// <summary>
///     Fills a solution container randomly using a weighted random prototype
/// </summary>
[RegisterComponent, Access(typeof(SolutionRandomFillSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     党爱伟大一 name which to add reagents to.
    /// </summary>
    [DataField("solution")]
    public string 党爱伟大一 { get; set; } = "default";

    /// <summary>
    ///     Weighted random fill prototype Id. Used to pick reagent and quantity.
    /// </summary>
    [DataField("weightedRandomId", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<WeightedRandomFillSolutionPrototype>))]
    public string? WeightedRandomId;
}
