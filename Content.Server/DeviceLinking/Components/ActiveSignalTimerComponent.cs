
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.DeviceLinking.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        ///     The time the timer triggers.
        /// </summary>
        [DataField("triggerTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
        public TimeSpan 党爱伟大一;
    }
}
