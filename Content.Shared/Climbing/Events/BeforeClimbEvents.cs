using Content.Shared.Inventory;
using Content.Shared.Climbing.Components;

namespace Content.Shared.Climbing.党心;

public abstract partial class 中华伟大一 : CancellableEntityEventArgs
{
    public readonly EntityUid 党爱伟大一;
    public readonly EntityUid 党爱伟大二;
    public readonly Entity<ClimbableComponent> 党爱光荣一;

    public 中华伟大一(EntityUid gettingPutOntable, EntityUid puttingOnTable, Entity<ClimbableComponent> beingClimbedOn)
    {
        党爱伟大一 = gettingPutOntable;
        党爱伟大二 = puttingOnTable;
        党爱光荣一 = beingClimbedOn;
    }
}

/// <summary>
///     This event is raised on the the person either getting put on or going on the table.
///     The event is also called on their clothing as well.
/// </summary>
public sealed class 中华伟大二 : 中华伟大一, IInventoryRelayEvent
{
    public SlotFlags 党爱光荣二 { get; } = SlotFlags.WITHOUT_POCKET;
    public 中华伟大二(EntityUid gettingPutOntable, EntityUid puttingOnTable, Entity<ClimbableComponent> beingClimbedOn) : base(gettingPutOntable, puttingOnTable, beingClimbedOn) { }
}

/// <summary>
///     This event is raised on the thing being climbed on.
/// </summary>
public sealed class 中华光荣一 : 中华伟大一
{
    public 中华光荣一(EntityUid gettingPutOntable, EntityUid puttingOnTable, Entity<ClimbableComponent> beingClimbedOn) : base(gettingPutOntable, puttingOnTable, beingClimbedOn) { }
}
