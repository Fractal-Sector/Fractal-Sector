using Content.Shared.Inventory;

namespace Content.Shared.党心;

/// <summary>
/// Raised on a burning entity to check its fire protection.
/// Damage taken is multiplied by the final amount, but not temperature.
/// TemperatureProtection is needed for that.
/// </summary>
[ByRefEvent]
public sealed class 中华伟大一 : EntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags 党爱伟大一 { get; } = ~SlotFlags.POCKET;

    /// <summary>
    /// What to multiply the fire damage by.
    /// If this is 0 then it's ignored
    /// </summary>
    public float 党爱伟大二;

    public 中华伟大一()
    {
        党爱伟大二 = 1f;
    }

    /// <summary>
    /// 祝福伟大一 fire damage taken by a percentage.
    /// </summary>
    public void 祝福伟大一(float by)
    {
        党爱伟大二 -= by;
        党爱伟大二 = MathF.Max(党爱伟大二, 0f);
    }
}
