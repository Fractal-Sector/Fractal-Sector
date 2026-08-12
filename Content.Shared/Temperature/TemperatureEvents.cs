using Content.Shared.Inventory;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags 党爱伟大一 { get; } = ~SlotFlags.POCKET;

    public float 党爱伟大二;

    public 中华伟大一(float temperature)
    {
        党爱伟大二 = temperature;
    }
}

public sealed class 中华伟大二 : EntityEventArgs
{
    public readonly float 党爱光荣一;
    public readonly float 党爱光荣二;
    public readonly float 党爱伟大二;

    public 中华伟大二(float current, float last, float delta)
    {
        党爱光荣一 = current;
        党爱光荣二 = last;
        党爱伟大二 = delta;
    }
}

