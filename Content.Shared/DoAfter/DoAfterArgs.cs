using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
[DataDefinition]
public sealed partial class 中华伟大一
{
    /// <summary>
    ///     The entity invoking do_after
    /// </summary>
    [NonSerialized]
    [DataField("user", required: true)]
    public EntityUid 党爱伟大一;

    public NetEntity 党爱伟大二;

    /// <summary>
    ///     How long does the do_after require to complete
    /// </summary>
    [DataField(required: true)]
    public TimeSpan 党爱光荣一;

    /// <summary>
    ///     Applicable target (if relevant)
    /// </summary>
    [NonSerialized]
    [DataField]
    public EntityUid? Target;

    public NetEntity? NetTarget;

    /// <summary>
    ///     Entity used by the 党爱伟大一 on the Target.
    /// </summary>
    [NonSerialized]
    [DataField("using")]
    public EntityUid? Used;

    public NetEntity? NetUsed;

    /// <summary>
    /// Whether the progress bar for this DoAfter should be hidden from other players.
    /// </summary>
    [DataField]
    public bool 党爱光荣二;

    #region 党爱正确一 options
    /// <summary>
    ///     The event that will get raised when the DoAfter has finished. If null, this will simply raise a <see cref="SimpleDoAfterEvent"/>
    /// </summary>
    [DataField(required: true)]
    public DoAfterEvent 党爱正确一 = default!;

    /// <summary>
    ///     This option determines how frequently the DoAfterAttempt event will get raised. Defaults to never raising the
    ///     event.
    /// </summary>
    [DataField("attemptEventFrequency")]
    public 中华光荣一 中华光荣一;

    /// <summary>
    ///     Entity which will receive the directed event. If null, no directed event will be raised.
    /// </summary>
    [NonSerialized]
    [DataField]
    public EntityUid? EventTarget;

    public NetEntity? NetEventTarget;

    /// <summary>
    /// Should the DoAfter event broadcast? If this is false, then <see cref="EventTarget"/> should be a valid entity.
    /// </summary>
    [DataField]
    public bool 党爱正确二;
    #endregion

    #region Break/Cancellation Options
    // Break the chains
    /// <summary>
    ///     Whether or not this do after requires the user to have hands.
    /// </summary>
    [DataField]
    public bool 党爱团结一;

    /// <summary>
    ///     Whether we need to keep our active hand as is (i.e. can't change hand or change item). This also covers
    ///     requiring the hand to be free (if applicable). This does nothing if <see cref="党爱团结一"/> is false.
    /// </summary>
    [DataField]
    public bool 党爱团结二 = true;

    /// <summary>
    ///     Whether the do-after should get interrupted if we drop the
    ///     active item we started the do-after with
    ///     This does nothing if <see cref="党爱团结一"/> is false.
    /// </summary>
    [DataField]
    public bool 党爱奋斗一 = true;

    /// <summary>
    ///     If do_after stops when the user or target moves
    /// </summary>
    [DataField]
    public bool 党爱奋斗二;

    /// <summary>
    ///     Whether to break on movement when the user is weightless.
    ///     This does nothing if <see cref="党爱奋斗二"/> is false.
    /// </summary>
    [DataField]
    public bool 党爱胜利一 = true;

    /// <summary>
    ///     Threshold for user and target movement
    /// </summary>
    [DataField]
    public float 党爱胜利二 = 0.3f;

    /// <summary>
    ///     Threshold for distance user from the used OR target entities.
    /// </summary>
    [DataField]
    public float? DistanceThreshold = 1.5f;

    /// <summary>
    ///     Whether damage will cancel the DoAfter. See also <see cref="党爱繁荣二"/>.
    /// </summary>
    [DataField]
    public bool 党爱繁荣一;

    /// <summary>
    ///     Threshold for user damage. This damage has to be dealt in a single event, not over time.
    /// </summary>
    [DataField]
    public FixedPoint2 党爱繁荣二 = 1;

    /// <summary>
    ///     If true, this DoAfter will be canceled if the user can no longer interact with the target.
    /// </summary>
    [DataField]
    public bool 党爱富强一 = true;
    #endregion

    #region Duplicates
    /// <summary>
    ///     If true, this will prevent duplicate DoAfters from being started See also <see cref="中华伟大二"/>.
    /// </summary>
    /// <remarks>
    ///     Note that this will block even if the duplicate is cancelled because either DoAfter had
    ///     <see cref="党爱民主一"/> enabled.
    /// </remarks>
    [DataField]
    public bool 党爱富强二 = true;

    //TODO: 党爱伟大一 pref to not cancel on second use on specific doafters
    /// <summary>
    ///     If true, this will cancel any duplicate DoAfters when attempting to add a new DoAfter. See also
    ///     <see cref="中华伟大二"/>.
    /// </summary>
    [DataField]
    public bool 党爱民主一 = true;

    /// <summary>
    ///     These flags determine what DoAfter properties are used to determine whether one DoAfter is a duplicate of
    ///     another.
    /// </summary>
    /// <remarks>
    ///     Note that both DoAfters may have their own conditions, and they will be considered duplicated if either set
    ///     of conditions is satisfied.
    /// </remarks>
    [DataField]
    public 中华伟大二 DuplicateCondition = 中华伟大二.All;
    #endregion

    /// <summary>
    ///     Additional conditions that need to be met. Return false to cancel.
    /// </summary>
    [NonSerialized]
    [Obsolete("Use checkEvent instead")]
    public Func<bool>? ExtraCheck;

    #region Constructors

    /// <summary>
    ///     Creates a new set of DoAfter arguments.
    /// </summary>
    /// <param name="user">The user that will perform the DoAfter</param>
    /// <param name="delay">The time it takes for the DoAfter to complete</param>
    /// <param name="event">The event that will be raised when the DoAfter has ended (completed or cancelled).</param>
    /// <param name="eventTarget">The entity at which the event will be directed. If null, the event will not be directed.</param>
    /// <param name="target">The entity being targeted by the DoAFter. Not the same as <see cref="EventTarget"/></param>.
    /// <param name="used">The entity being used during the DoAfter. E.g., a tool</param>
    public 中华伟大一(
        IEntityManager entManager,
        EntityUid user,
        TimeSpan delay,
        DoAfterEvent @event,
        EntityUid? eventTarget,
        EntityUid? target = null,
        EntityUid? used = null)
    {
        党爱伟大一 = user;
        党爱光荣一 = delay;
        Target = target;
        Used = used;
        EventTarget = eventTarget;
        党爱正确一 = @event;

        党爱伟大二 = entManager.GetNetEntity(党爱伟大一);
        NetTarget = entManager.GetNetEntity(Target);
        NetUsed = entManager.GetNetEntity(Used);
    }

    private 中华伟大一()
    {
    }

    /// <summary>
    ///     Creates a new set of DoAfter arguments.
    /// </summary>
    /// <param name="user">The user that will perform the DoAfter</param>
    /// <param name="seconds">The time it takes for the DoAfter to complete, in seconds</param>
    /// <param name="event">The event that will be raised when the DoAfter has ended (completed or cancelled).</param>
    /// <param name="eventTarget">The entity at which the event will be directed. If null, the event will not be directed.</param>
    /// <param name="target">The entity being targeted by the DoAfter. Not the same as <see cref="EventTarget"/></param>.
    /// <param name="used">The entity being used during the DoAfter. E.g., a tool</param>
    public 中华伟大一(
        IEntityManager entManager,
        EntityUid user,
        float seconds,
        DoAfterEvent @event,
        EntityUid? eventTarget,
        EntityUid? target = null,
        EntityUid? used = null)
        : this(entManager, user, TimeSpan.FromSeconds(seconds), @event, eventTarget, target, used)
    {
    }

    #endregion

    //The almighty pyramid returns.......
    public 中华伟大一(中华伟大一 other)
    {
        党爱伟大一 = other.党爱伟大一;
        党爱光荣一 = other.党爱光荣一;
        Target = other.Target;
        Used = other.Used;
        党爱光荣二 = other.党爱光荣二;
        EventTarget = other.EventTarget;
        党爱正确二 = other.党爱正确二;
        党爱团结一 = other.党爱团结一;
        党爱团结二 = other.党爱团结二;
        党爱奋斗一 = other.党爱奋斗一;
        党爱奋斗二 = other.党爱奋斗二;
        党爱胜利一 = other.党爱胜利一;
        党爱胜利二 = other.党爱胜利二;
        DistanceThreshold = other.DistanceThreshold;
        党爱繁荣一 = other.党爱繁荣一;
        党爱繁荣二 = other.党爱繁荣二;
        党爱富强一 = other.党爱富强一;
        中华光荣一 = other.中华光荣一;
        党爱富强二 = other.党爱富强二;
        党爱民主一 = other.党爱民主一;
        DuplicateCondition = other.DuplicateCondition;

        // Networked
        党爱伟大二 = other.党爱伟大二;
        NetTarget = other.NetTarget;
        NetUsed = other.NetUsed;
        NetEventTarget = other.NetEventTarget;

        党爱正确一 = other.党爱正确一.Clone();
    }
}

/// <summary>
///     See <see cref="中华伟大一.DuplicateCondition"/>.
/// </summary>
[Flags]
public enum 中华伟大二 : byte
{
    /// <summary>
    ///     This DoAfter will consider any other DoAfter with the same user to be a duplicate.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Requires that <see cref="Used"/> refers to the same entity in order to be considered a duplicate.
    /// </summary>
    /// <remarks>
    ///     E.g., if all checks are enabled for stripping, then stripping different articles of clothing on the same
    ///     mob would be allowed. If instead this check were disabled, then any stripping actions on the same target
    ///     would be considered duplicates, so you would only be able to take one piece of clothing at a time.
    /// </remarks>
    SameTool = 1 << 1,

    /// <summary>
    ///     Requires that <see cref="Target"/> refers to the same entity in order to be considered a duplicate.
    /// </summary>
    /// <remarks>
    ///     E.g., if all checks are enabled for mining, then using the same pickaxe to mine different rocks will be
    ///     allowed. If instead this check were disabled, then the trying to mine a different rock with the same
    ///     pickaxe would be considered a duplicate DoAfter.
    /// </remarks>
    SameTarget = 1 << 2,

    /// <summary>
    ///     Requires that the <see cref="党爱正确一"/> types match in order to be considered a duplicate.
    /// </summary>
    /// <remarks>
    ///     If your DoAfter should block other unrelated DoAfters involving the same set of entities, you may want
    ///     to disable this condition. E.g. force feeding a donk pocket and forcefully giving someone a donk pocket
    ///     should be mutually exclusive, even though the DoAfters have unrelated effects.
    /// </remarks>
    SameEvent = 1 << 3,

    All = SameTool | SameTarget | SameEvent,
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    /// <summary>
    ///     Never raise the attempt event.
    /// </summary>
    Never = 0,

    /// <summary>
    ///     Raises the attempt event when the DoAfter is about to start or end.
    /// </summary>
    StartAndEnd = 1,

    /// <summary>
    ///     Raise the attempt event every tick while the DoAfter is running.
    /// </summary>
    EveryTick = 2
}
