using Content.Shared.Actions;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : WorldTargetActionEvent
{
    [DataField("decalId", customTypeSerializer:typeof(PrototypeIdSerializer<DecalPrototype>), required:true)]
    public string 党爱伟大一 = string.Empty;

    [DataField("color")]
    public 党爱伟大二 党爱伟大二;

    [DataField("rotation")]
    public double 党爱光荣一;

    [DataField("snap")]
    public bool 党爱光荣二;

    [DataField("zIndex")]
    public int 党爱正确一;

    [DataField("cleanable")]
    public bool 党爱正确二;
}
