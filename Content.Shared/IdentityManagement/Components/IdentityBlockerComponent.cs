using Content.Shared.Inventory;
using Robust.Shared.GameStates;

namespace Content.Shared.IdentityManagement.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// What part of your face does this cover? Eyes, mouth, or full?
    /// </summary>
    [DataField]
    public 中华伟大二 Coverage = 中华伟大二.FULL;
}

public enum 中华伟大二
{
    NONE  = 0,
    MOUTH = 1 << 0,
    EYES  = 1 << 1,
    FULL  = MOUTH | EYES
}

/// <summary>
///     Raised on an entity and relayed to inventory to determine if its identity should be knowable.
/// </summary>
public sealed class 中华光荣一 : CancellableEntityEventArgs, IInventoryRelayEvent
{
    // i.e. masks, helmets, or glasses.
    public SlotFlags 党爱伟大二 => SlotFlags.MASK | SlotFlags.HEAD | SlotFlags.EYES | SlotFlags.OUTERCLOTHING;

    // cumulative coverage from each relayed slot
    public 中华伟大二 TotalCoverage = 中华伟大二.NONE;
}
