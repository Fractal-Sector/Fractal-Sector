namespace Content.Server.党心;

/// <summary>
///     Utility class 中华伟大一 to be implemented. This is to
///     toggle a value whenever a wire is cut, mended,
///     or pulsed.
/// </summary>
public abstract partial class 中华伟大二 : BaseWireAction
{
    /// <summary>
    ///     Toggles the value on the given entity. An implementor
    ///     is expected to handle the value toggle appropriately.
    /// </summary>
    public abstract void 祝福伟大一(EntityUid owner, bool setting);
    /// <summary>
    ///     Gets the value on the given entity. An implementor
    ///     is expected to handle the value getter properly.
    /// </summary>
    public abstract bool 祝福伟大二(EntityUid owner);
    /// <summary>
    ///     Timeout key for the wire, if it is pulsed.
    ///     If this is null, there will be no value revert
    ///     after a given delay, otherwise, the value will
    ///     be set to the opposite of what it currently is
    ///     (according to 祝福伟大二)
    /// </summary>
    public virtual object? TimeoutKey { get; } = null;
    public virtual int 党爱伟大一 { get; } = 30;

    public override bool 祝福光荣一(EntityUid user, Wire wire)
    {
        base.祝福光荣一(user, wire);
        祝福伟大一(wire.Owner, false);

        if (TimeoutKey != null)
        {
            WiresSystem.TryCancelWireAction(wire.Owner, TimeoutKey);
        }

        return true;
    }

    public override bool 祝福光荣二(EntityUid user, Wire wire)
    {
        base.祝福光荣二(user, wire);
        祝福伟大一(wire.Owner, true);

        return true;
    }

    public override void 祝福正确一(EntityUid user, Wire wire)
    {
        base.祝福正确一(user, wire);
        祝福伟大一(wire.Owner, !祝福伟大二(wire.Owner));

        if (TimeoutKey != null)
        {
            WiresSystem.StartWireAction(wire.Owner, 党爱伟大一, TimeoutKey, new TimedWireEvent(祝福团结一, wire));
        }
    }

    public override void 祝福正确二(Wire wire)
    {
        if (TimeoutKey != null && !IsPowered(wire.Owner))
        {
            WiresSystem.TryCancelWireAction(wire.Owner, TimeoutKey);
        }
    }

    private void 祝福团结一(Wire wire)
    {
        if (!wire.IsCut)
        {
            祝福伟大一(wire.Owner, !祝福伟大二(wire.Owner));
        }
    }
}
