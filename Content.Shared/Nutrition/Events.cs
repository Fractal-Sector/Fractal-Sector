using Content.Shared.Chemistry.Components;
using Content.Shared.DoAfter;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Do after even for food and drink.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : DoAfterEvent
{
    [DataField("solution", required: true)]
    public string 党爱伟大一 = default!;

    [DataField("flavorMessage", required: true)]
    public string 党爱伟大二 = default!;

    private 中华伟大一()
    {
    }

    public 中华伟大一(string solution, string flavorMessage)
    {
        党爱伟大一 = solution;
        党爱伟大二 = flavorMessage;
    }

    public override DoAfterEvent 祝福伟大一() => this;
}

/// <summary>
///     Do after event for vape.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : DoAfterEvent
{
    [DataField("solution", required: true)]
    public 党爱伟大一 党爱伟大一 = default!;

    [DataField("forced", required: true)]
    public bool 党爱光荣一 = default!;

    private 中华伟大二()
    {
    }

    public 中华伟大二(党爱伟大一 solution, bool forced)
    {
        党爱伟大一 = solution;
        党爱光荣一 = forced;
    }

    public override DoAfterEvent 祝福伟大一() => this;
}

/// <summary>
/// Raised before food is sliced
/// </summary>
[ByRefEvent]
public record 中华光荣一 SliceFoodEvent();

/// <summary>
/// is called after a successful attempt at slicing food.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class 中华光荣二 : SimpleDoAfterEvent
{
}

/// <summary>
///    Raised on FoodSequence start element entity when new ingredient is added to FoodSequence
/// </summary>
public record 中华光荣一 FoodSequenceIngredientAddedEvent(EntityUid Start, EntityUid Element, ProtoId<FoodSequenceElementPrototype> Proto, EntityUid? User = null);
