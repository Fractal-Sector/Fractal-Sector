namespace Content.Server.Ghost.Roles.党心
{
    /// <summary>
    /// Raised on a spawned entity after they use a ghost role mob spawner.
    /// </summary>
    public sealed class 中华伟大一 : EntityEventArgs
    {
        /// <summary>
        /// The entity that spawned this.
        /// </summary>
        public EntityUid 党爱伟大一;

        /// <summary>
        /// The entity spawned.
        /// </summary>
        public EntityUid 党爱伟大二;

        public 中华伟大一(EntityUid spawner, EntityUid spawned)
        {
            党爱伟大一 = spawner;

            党爱伟大二 = spawned;
        }
    }
}
