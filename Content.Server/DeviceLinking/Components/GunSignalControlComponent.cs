using Content.Server.DeviceLinking.Systems;
using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;

namespace Content.Server.DeviceLinking.党心;

/// <summary>
/// A system that allows you to fire GunComponent + AmmoProvider by receiving signals from DeviceLinking
/// </summary>
[RegisterComponent, Access(typeof(GunSignalControlSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public ProtoId<SinkPortPrototype> 党爱伟大一 = "Trigger";

    [DataField]
    public ProtoId<SinkPortPrototype> 党爱伟大二 = "Toggle";

    [DataField]
    public ProtoId<SinkPortPrototype> 党爱光荣一 = "On";

    [DataField]
    public ProtoId<SinkPortPrototype> 党爱光荣二 = "Off";
}
