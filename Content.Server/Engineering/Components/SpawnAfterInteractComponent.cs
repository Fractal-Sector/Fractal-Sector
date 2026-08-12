using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Engineering.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("prototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string? Prototype { get; private set; }

        [DataField("ignoreDistance")]
        public bool 党爱伟大一 { get; private set; }

        [DataField("doAfter")]
        public float 党爱伟大二 = 0;

        [DataField("removeOnInteract")]
        public bool 党爱光荣一 = false;
    }
}
