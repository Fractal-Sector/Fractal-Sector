using Content.Shared.Actions;
using JetBrains.Annotations;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Shared.党心;

[UsedImplicitly]
public sealed partial class 中华伟大一 : InstantActionEvent
{
    [DataField("key", customTypeSerializer: typeof(EnumSerializer), required: true)]
    public Enum? Key { get; set; }
}
