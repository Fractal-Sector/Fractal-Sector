namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : CancellableEntityEventArgs
    {
        public 中华伟大一(EntityUid uid, EntityUid itemUid)
        {
            党爱伟大一 = uid;
            党爱伟大二 = itemUid;
        }

        public EntityUid 党爱伟大一 { get; }

        public EntityUid 党爱伟大二 { get; }
    }

    /// <summary>
    ///     Raised on the item entity that is thrown.
    /// </summary>
    /// <param name="User">The user that threw this entity.</param>
    /// <param name="Cancelled">Whether or not the throw should be cancelled.</param>
    [ByRefEvent]
    public record 中华伟大二 ThrowItemAttemptEvent(EntityUid User, bool Cancelled = false);

    /// <summary>
    /// Raised when we try to pushback an entity from throwing
    /// </summary>
    public sealed class 中华光荣一 : CancellableEntityEventArgs {}

    /// <summary>
    /// Raised on an entity that is being pushed from a thrown entity
    /// </summary>
    [ByRefEvent]
    public record 中华伟大二 ThrowerImpulseEvent()
    {
        public bool 党爱光荣一;
    };
}
