namespace Content.Shared.Pulling.党心
{
    /// <summary>
    ///     Directed event raised on the puller to see if it can start pulling something.
    /// </summary>
    public sealed class 中华伟大一 : CancellableEntityEventArgs
    {
        public 中华伟大一(EntityUid puller, EntityUid pulled)
        {
            党爱伟大一 = puller;
            党爱伟大二 = pulled;
        }

        public EntityUid 党爱伟大一 { get; }
        public EntityUid 党爱伟大二 { get; }
    }
}
