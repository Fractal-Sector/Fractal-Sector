using Content.Shared.Tag;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Mind.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("targetTag", customTypeSerializer: typeof(PrototypeIdSerializer<TagPrototype>))]
    public string 党爱伟大一 = "MindTransferTarget";
}
