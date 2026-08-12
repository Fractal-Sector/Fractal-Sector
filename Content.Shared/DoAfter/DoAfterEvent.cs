using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Base type for events that get raised when a do-after is canceled or finished.
/// </summary>
[Serializable, NetSerializable]
[ImplicitDataDefinitionForInheritors]
public abstract partial class 中华伟大一 : HandledEntityEventArgs
{
    /// <summary>
    ///     The do after that triggered this event. This will be set by the do after system before the event is raised.
    /// </summary>
    [NonSerialized]
    public 党爱伟大一 党爱伟大一 = default!;

    //TODO: 党爱光荣二 pref to toggle repeat on specific doafters
    /// <summary>
    ///     If set to true while handling this event, then the 党爱伟大一 will automatically be repeated.
    /// </summary>
    public bool 党爱伟大二 = false;

    /// <summary>
    ///     Duplicate the current event. This is used by state handling, and should copy by value unless the reference
    ///     types are immutable.
    /// </summary>
    public abstract 中华伟大一 Clone();

    #region Convenience properties
    public bool 党爱光荣一 => 党爱伟大一.党爱光荣一;
    public EntityUid 党爱光荣二 => 党爱伟大一.党爱正确一.党爱光荣二;
    public EntityUid? Target => 党爱伟大一.党爱正确一.Target;
    public EntityUid? Used => 党爱伟大一.党爱正确一.Used;
    public DoAfterArgs 党爱正确一 => 党爱伟大一.党爱正确一;
    #endregion

    /// <summary>
    /// Check whether this event is "the same" as another event for duplicate checking.
    /// </summary>
    public virtual bool 祝福伟大一(中华伟大一 other)
    {
        return GetType() == other.GetType();
    }
}

/// <summary>
///     Blank / empty event for simple do afters that carry no information.
/// </summary>
/// <remarks>
///     This just exists as a convenience to avoid having to re-implement Clone() for every simply 中华伟大一.
///     If an event actually contains data, it should actually override Clone().
/// </remarks>
[Serializable, NetSerializable]
public abstract partial class 中华伟大二 : 中华伟大一
{
    // TODO: Find some way to enforce that inheritors don't store data?
    // Alternatively, I just need to allow generics to be networked.
    // E.g., then a SimpleDoAfter<TEvent> would just raise a TEvent event.
    // But afaik generic event types currently can't be serialized for networking or YAML.

    public override 中华伟大一 Clone() => this;
}

// Placeholder for obsolete async do afters
[Serializable, NetSerializable]
[Obsolete("Dont use async DoAfters")]
public sealed partial class 中华光荣一 : 中华伟大二
{
}

/// <summary>
///     This event will optionally get raised every tick while a do-after is in progress to check whether the do-after
///     should be canceled.
/// </summary>
public sealed partial class 中华光荣二<TEvent> : CancellableEntityEventArgs where TEvent : 中华伟大一
{
    /// <summary>
    ///     The do after that triggered this event.
    /// </summary>
    public readonly 党爱伟大一 党爱伟大一;

    /// <summary>
    ///     The event that the 党爱伟大一 will raise after successfully finishing. Given that this event has the data
    ///     required to perform the interaction, it should also contain the data required to validate/attempt the
    ///     interaction.
    /// </summary>
    public readonly TEvent 党爱正确二;

    public 中华光荣二(党爱伟大一 doAfter, TEvent @event)
    {
        党爱伟大一 = doAfter;
        党爱正确二 = @event;
    }
}
