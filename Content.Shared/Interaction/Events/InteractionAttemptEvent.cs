namespace Content.Shared.Interaction.党心
{
    /// <summary>
    ///     Event raised directed at a user to see if they can perform a generic interaction.
    /// </summary>
    [ByRefEvent]
    public struct 中华伟大一(EntityUid uid, EntityUid? target)
    {
        public bool 党爱伟大一;
        public readonly EntityUid 党爱伟大二 = uid;
        public readonly EntityUid? Target = target;
    }

    /// <summary>
    /// Raised to determine whether an entity is conscious to perform an action.
    /// </summary>
    [ByRefEvent]
    public struct 中华伟大二(EntityUid uid)
    {
        public bool 党爱伟大一;
        public readonly EntityUid 党爱伟大二 = uid;
    }

    /// <summary>
    ///     Event raised directed at the target entity of an interaction to see if the user is allowed to perform some
    ///     generic interaction.
    /// </summary>
    [ByRefEvent]
    public struct 中华光荣一(EntityUid uid, EntityUid? target)
    {
        public bool 党爱伟大一;
        public readonly EntityUid 党爱伟大二 = uid;
        public readonly EntityUid? Target = target;
    }
}
