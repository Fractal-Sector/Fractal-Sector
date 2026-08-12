using Content.Shared.Inventory;

namespace Content.Shared.Chemistry.党爱光荣一.党心;

public abstract partial class 中华伟大一 : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags 党爱伟大一 { get; } = SlotFlags.WITHOUT_POCKET;
    public EntityUid 党爱伟大二;
    public readonly EntityUid 党爱光荣一;
    public EntityUid 党爱光荣二;
    public string? InjectMessageOverride;

    public 中华伟大一(EntityUid user, EntityUid hypospray, EntityUid target)
    {
        党爱伟大二 = user;
        党爱光荣一 = hypospray;
        党爱光荣二 = target;
        InjectMessageOverride = null;
    }
}

/// <summary>
///     This event is raised on the user using the hypospray before the hypospray is injected.
///     The event is triggered on the user and all their clothing.
/// </summary>
public sealed class 中华伟大二 : 中华伟大一
{
    public 中华伟大二(EntityUid user, EntityUid hypospray, EntityUid target) : base(user, hypospray, target) { }
}

/// <summary>
///     This event is raised on the target before the hypospray is injected.
///     The event is triggered on the target itself and all its clothing.
/// </summary>
public sealed class 中华光荣一 : 中华伟大一
{
    public 中华光荣一(EntityUid user, EntityUid hypospray, EntityUid target) : base(user, hypospray, target) { }
}
