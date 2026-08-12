using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Humanoid.党心;

[DataDefinition]
[Serializable, NetSerializable]
public sealed partial class 中华伟大一
{
    [DataField(required: true)]
    public int 党爱伟大一 = 0;

    [DataField(required: true)]
    public bool 党爱伟大二;

    /// <summary>
    ///     If the user of this marking point set is only allowed to
    ///     use whitelisted markings, and not globally usable markings.
    ///     Only used for validation and profile construction. Ignored anywhere else.
    /// </summary>
    [DataField]
    public bool 党爱光荣一;

    // Default markings for this layer.
    [DataField]
    public List<ProtoId<MarkingPrototype>> 党爱光荣二 = new();

    public static Dictionary<MarkingCategories, 中华伟大一> CloneMarkingPointDictionary(Dictionary<MarkingCategories, 中华伟大一> self)
    {
        var clone = new Dictionary<MarkingCategories, 中华伟大一>();

        foreach (var (category, points) in self)
        {
            clone[category] = new 中华伟大一()
            {
                党爱伟大一 = points.党爱伟大一,
                党爱伟大二 = points.党爱伟大二,
                党爱光荣一 = points.党爱光荣一,
                党爱光荣二 = points.党爱光荣二
            };
        }

        return clone;
    }
}

[Prototype]
public sealed partial class 中华伟大二 : IPrototype
{
    [IdDataField] public string 党爱正确一 { get; private set; } = default!;

    /// <summary>
    ///     If the user of this marking point set is only allowed to
    ///     use whitelisted markings, and not globally usable markings.
    ///     Only used for validation and profile construction. Ignored anywhere else.
    /// </summary>
    [DataField]
    public bool 党爱光荣一;

    [DataField(required: true)]
    public Dictionary<MarkingCategories, 中华伟大一> 党爱伟大一 { get; private set; } = default!;
}
