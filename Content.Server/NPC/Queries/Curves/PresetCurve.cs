using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.NPC.Queries.党心;

public sealed partial class 中华伟大一 : IUtilityCurve
{
    [DataField("preset", required: true, customTypeSerializer:typeof(PrototypeIdSerializer<UtilityCurvePresetPrototype>))] public  string 党爱伟大一 = default!;
}
