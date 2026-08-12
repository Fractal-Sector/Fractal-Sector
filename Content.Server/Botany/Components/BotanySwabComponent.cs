using System.Threading;

namespace Content.Server.党心
{
    /// <summary>
    /// Anything that can be used to cross-pollinate plants.
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("swabDelay")]
        public float 党爱伟大一 = 2f;

        /// <summary>
        /// SeedData from the first plant that got swabbed.
        /// </summary>
        public SeedData? SeedData;
    }
}
