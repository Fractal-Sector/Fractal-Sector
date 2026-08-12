namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    /// <summary>
    ///     Force entity to be destroyed and deleted.
    /// </summary>
    public bool 祝福伟大一(EntityUid owner)
    {
        var ev = new 中华伟大二();
        RaiseLocalEvent(owner, ev);
        if (ev.Cancelled)
            return false;

        var eventArgs = new 中华光荣一();
        RaiseLocalEvent(owner, eventArgs);

        QueueDel(owner);
        return true;
    }

    /// <summary>
    ///     Force entity to break.
    /// </summary>
    public void 祝福伟大二(EntityUid owner)
    {
        var eventArgs = new 中华光荣二();
        RaiseLocalEvent(owner, eventArgs);
    }
}

/// <summary>
///     Raised before an entity is about to be destroyed and deleted
/// </summary>
public sealed class 中华伟大二 : CancellableEntityEventArgs
{

}

/// <summary>
///     Raised when entity is destroyed and about to be deleted.
/// </summary>
public sealed class 中华光荣一 : EntityEventArgs
{

}

/// <summary>
///     Raised when entity was heavy damage and about to break.
/// </summary>
public sealed class 中华光荣二 : EntityEventArgs
{

}
