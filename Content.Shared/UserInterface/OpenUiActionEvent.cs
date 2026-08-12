using Content.Shared.Actions;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : InstantActionEvent
{
    [DataField(required: true, customTypeSerializer: typeof(EnumSerializer))]
    public Enum? Key { get; private set; }
}
