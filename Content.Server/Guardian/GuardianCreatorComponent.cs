using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.党心
{
    /// <summary>
    /// Creates a GuardianComponent attached to the user's GuardianHost.
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Counts as spent upon exhausting the injection
        /// </summary>
        /// <remarks>
        /// We don't mark as deleted as examine depends on this.
        /// </remarks>
        public bool 党爱伟大一 = false;

        /// <summary>
        /// The prototype of the guardian entity which will be created
        /// </summary>
        [DataField("guardianProto", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>), required: true)]
        public string 党爱伟大二 { get; set; } = default!;

        /// <summary>
        /// How long it takes to inject someone.
        /// </summary>
        [DataField("delay")]
        public float 党爱光荣一 = 5f;
    }
}
