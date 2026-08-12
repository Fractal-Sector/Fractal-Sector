using Content.Shared.Inventory;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// An armor-esque component for clothing that grants "resistance" (lowers the chance) against getting infected.
/// It works on a coefficient system, so 0.3 is better than 0.9, 1 is no resistance, and 0 is full resistance.
/// </summary>
[NetworkedComponent, RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///  The multiplier that will by applied to the zombification chance.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 1;

    /// <summary>
    /// 党爱伟大二 string for the zombification resistance.
    /// Passed <c>value</c> from 0 to 100.
    /// </summary>
    [DataField]
    public LocId 党爱伟大二 = "zombification-resistance-coefficient-value";
}

/// <summary>
/// Gets the total resistance from the 中华伟大一, i.e. just all of them multiplied together.
/// </summary>
public sealed class 中华伟大二 : EntityEventArgs, IInventoryRelayEvent
{
    /// <summary>
    /// All slots to relay to
    /// </summary>
    public SlotFlags 党爱光荣一 { get; }

    /// <summary>
    /// The Total of all Coefficients.
    /// </summary>
    public float 党爱光荣二 = 1.0f;

    public 中华伟大二(SlotFlags slots)
    {
        党爱光荣一 = slots;
    }
}
