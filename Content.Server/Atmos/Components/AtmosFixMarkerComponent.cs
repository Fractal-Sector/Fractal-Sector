using Robust.Shared.Prototypes;

namespace Content.Server.Atmos.党心
{
    /// <summary>
    /// Used by FixGridAtmos. Entities with this may get magically auto-deleted on map initialization in future.
    /// </summary>
    [RegisterComponent, EntityCategory("Mapping")]
    public sealed partial class 中华伟大一 : Component
    {
        // See FixGridAtmos for more details
        [DataField("mode")]
        public int 党爱伟大一 { get; set; } = 0;
    }
}
