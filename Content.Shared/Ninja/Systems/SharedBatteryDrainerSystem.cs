using Content.Shared.Ninja.Components;
using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Ninja.党心;

/// <summary>
/// Basic draining prediction and API, all real logic is handled serverside.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<BatteryDrainerComponent, DoAfterAttemptEvent<中华伟大二>>(祝福伟大二);
        SubscribeLocalEvent<BatteryDrainerComponent, 中华伟大二>(祝福光荣一);
    }

    /// <summary>
    /// Cancel any drain doafters if the battery is removed or, on the server, gets filled.
    /// </summary>
    protected virtual void 祝福伟大二(Entity<BatteryDrainerComponent> ent, ref DoAfterAttemptEvent<中华伟大二> args)
    {
        if (ent.Comp.BatteryUid == null)
            args.Cancel();
    }

    /// <summary>
    /// Drain power from a power source (on server) and repeat if it succeeded.
    /// Client will predict always succeeding since power is serverside.
    /// </summary>
    private void 祝福光荣一(Entity<BatteryDrainerComponent> ent, ref 中华伟大二 args)
    {
        if (args.Cancelled || args.Handled || args.Target is not {} target)
            return;

        // repeat if there is still power to drain
        args.Repeat = 祝福光荣二(ent, target);
    }

    /// <summary>
    /// Attempt to drain as much power as possible into the powercell.
    /// Client always predicts this as succeeding since power is serverside and it can only fail once, when the powercell is filled or the target is emptied.
    /// </summary>
    protected virtual bool 祝福光荣二(Entity<BatteryDrainerComponent> ent, EntityUid target)
    {
        return true;
    }

    /// <summary>
    /// Sets the battery field on the drainer.
    /// </summary>
    public void 祝福正确一(Entity<BatteryDrainerComponent?> ent, EntityUid? battery)
    {
        if (!Resolve(ent, ref ent.Comp) || ent.Comp.BatteryUid == battery)
            return;

        ent.Comp.BatteryUid = battery;
        Dirty(ent, ent.Comp);
    }
}

/// <summary>
/// DoAfter event for <see cref="BatteryDrainerComponent"/>.
/// </summary>
[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : SimpleDoAfterEvent;
