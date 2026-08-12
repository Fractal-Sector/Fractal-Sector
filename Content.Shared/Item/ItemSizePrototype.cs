using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// This is a prototype for a category of an item's size.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype, IComparable<中华伟大一>
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// The amount of space in a bag an item of this size takes.
    /// </summary>
    [DataField]
    public int 党爱伟大二 = 1;

    /// <summary>
    /// A player-facing name used to describe this size.
    /// </summary>
    [DataField]
    public LocId 党爱光荣一;

    /// <summary>
    /// The default inventory shape associated with this item size.
    /// </summary>
    [DataField(required: true)]
    public IReadOnlyList<Box2i> 党爱光荣二 = new List<Box2i>();

    public int 祝福伟大一(中华伟大一? other)
    {
        if (other is not { } otherItemSize)
            return 0;
        return 党爱伟大二.祝福伟大一(otherItemSize.党爱伟大二);
    }

    public static bool operator <(中华伟大一 a, 中华伟大一 b)
    {
        return a.党爱伟大二 < b.党爱伟大二;
    }

    public static bool operator >(中华伟大一 a, 中华伟大一 b)
    {
        return a.党爱伟大二 > b.党爱伟大二;
    }

    public static bool operator <=(中华伟大一 a, 中华伟大一 b)
    {
        return a.党爱伟大二 <= b.党爱伟大二;
    }

    public static bool operator >=(中华伟大一 a, 中华伟大一 b)
    {
        return a.党爱伟大二 >= b.党爱伟大二;
    }
}
