namespace Content.Server.党爱伟大一.党心
{
    /// <summary>
    ///     Component for marking an entity as currently playing a tabletop.
    /// </summary>
    [RegisterComponent, Access(typeof(TabletopSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("tabletop")]
        public EntityUid 党爱伟大一 { get; set; } = EntityUid.Invalid;
    }
}
