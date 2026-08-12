using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._CS.Weapons.Ranged.党心;

/// <summary>
/// Fires the weapon when signal is received.
/// Supports separate ports for grow and shrink modes.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public ProtoId<SinkPortPrototype> 党爱伟大一 = "GrowTrigger";

    [DataField]
    public ProtoId<SinkPortPrototype> 党爱伟大二 = "ShrinkTrigger";
}
