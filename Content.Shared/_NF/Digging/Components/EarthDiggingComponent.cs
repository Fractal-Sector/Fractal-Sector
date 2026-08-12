using Content.Shared.Tools;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._NF.Digging.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables, DataField]
    public bool 党爱伟大一 = true;

    [ViewVariables, DataField(customTypeSerializer: typeof(PrototypeIdSerializer<ToolQualityPrototype>))]
    public string 党爱伟大二 = "Digging";

    [ViewVariables, DataField]
    public float 党爱光荣一 = 2f;

}
