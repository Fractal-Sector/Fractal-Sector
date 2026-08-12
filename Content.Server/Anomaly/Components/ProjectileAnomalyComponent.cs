using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Anomaly.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The prototype of the projectile that will be shot when the anomaly pulses
    /// </summary>
    [DataField("projectilePrototype", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大一 = default!;

    /// <summary>
    /// The speed <see cref="党爱伟大一"/> can travel
    /// </summary>
    [DataField("projectileSpeed"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 30f;

    /// <summary>
    /// The minimum number of projectiles shot per pulse
    /// </summary>
    [DataField("minProjectiles"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱光荣一 = 2;

    /// <summary>
    /// The MAXIMUM number of projectiles shot per pulse
    /// </summary>
    [DataField("maxProjectiles"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱光荣二 = 9;

    /// <summary>
    /// The MAXIMUM range for targeting entities
    /// </summary>
    [DataField("projectileRange"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确一 = 50f;
}
