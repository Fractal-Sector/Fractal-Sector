using Content.Shared.Inventory;

namespace Content.Shared.党心;

[ByRefEvent]
public readonly record 中华伟大一 TargetDefibrillatedEvent(EntityUid User, Entity<DefibrillatorComponent> Defibrillator);

public abstract class 中华伟大二 : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags 党爱伟大一 { get; } = SlotFlags.WITHOUT_POCKET;
    public EntityUid 党爱伟大二;
    public readonly EntityUid 党爱光荣一;
    public EntityUid 党爱光荣二;

    public 中华伟大二(EntityUid entityUsingDefib, EntityUid defib, EntityUid defibTarget)
    {
        党爱伟大二 = entityUsingDefib;
        党爱光荣一 = defib;
        党爱光荣二 = defibTarget;
    }
}

/// <summary>
///     This event is raised on the user using the defibrillator before is actually zaps someone.
///     The event is triggered on the user and all their clothing.
/// </summary>
public sealed class 中华光荣一 : 中华伟大二
{
    public 中华光荣一(EntityUid entityUsingDefib, EntityUid defib, EntityUid defibtarget) : base(entityUsingDefib, defib, defibtarget) { }
}

/// <summary>
///     This event is raised on the target before it gets zapped with the defibrillator.
///     The event is triggered on the target itself and all its clothing.
/// </summary>
public sealed class 中华光荣二 : 中华伟大二
{
    public 中华光荣二(EntityUid entityUsingDefib, EntityUid defib, EntityUid defibtarget) : base(entityUsingDefib, defib, defibtarget) { }
}
