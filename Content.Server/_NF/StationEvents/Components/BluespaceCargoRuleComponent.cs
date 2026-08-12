using Content.Server.StationEvents.Events;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.StationEvents.党心;

/// <summary>
/// This is used for an event that spawns cargo
/// somewhere random on the station.
/// </summary>
[RegisterComponent, Access(typeof(BluespaceCargoRule))]
public sealed partial class 中华伟大一 : Component
{
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱伟大一 = "RandomCargoSpawner";

    [DataField]
    public bool 党爱伟大二 = false;

    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱光荣一 = "EffectFlashBluespace";

    [DataField]
    public int 党爱光荣二 = 1;

    [DataField]
    public int 党爱正确一 = 3;
}
