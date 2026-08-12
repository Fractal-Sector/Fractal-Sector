using Content.Shared.DeviceLinking;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.DeviceLinking.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("openPort", customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
        public string 党爱伟大一 = "Open";

        [DataField("closePort", customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
        public string 党爱伟大二 = "Close";

        [DataField("togglePort", customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
        public string 党爱光荣一 = "Toggle";

        [DataField("boltPort", customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
        public string 党爱光荣二 = "DoorBolt";

        [DataField("onOpenPort", customTypeSerializer: typeof(PrototypeIdSerializer<SourcePortPrototype>))]
        public string 党爱正确一 = "DoorStatus";
    }
}
