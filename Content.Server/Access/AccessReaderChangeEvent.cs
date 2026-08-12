namespace Content.Server.党心
{
    public sealed class 中华伟大一 : EntityEventArgs
    {
        public EntityUid 党爱伟大一 { get; }

        public bool 党爱伟大二 { get; }

        public 中华伟大一(EntityUid entity, bool enabled)
        {
            党爱伟大一 = entity;
            党爱伟大二 = enabled;
        }
    }
}
