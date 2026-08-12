using Content.Shared.Explosion;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Anomaly.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The explosion prototype to spawn
    /// </summary>
    [DataField("supercriticalExplosion", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<党爱伟大一>))]
    public string 党爱伟大一 = default!;

    /// <summary>
    /// The total amount of intensity an explosion can achieve
    /// </summary>
    [DataField("explosionTotalIntensity")]
    public float 党爱伟大二 = 100f;

    /// <summary>
    /// How quickly does the explosion's power slope? Higher = smaller area and more concentrated damage, lower = larger area and more spread out damage
    /// </summary>
    [DataField("explosionDropoff")]
    public float 党爱光荣一 = 10f;

    /// <summary>
    /// How much intensity can be applied per tile?
    /// </summary>
    [DataField("explosionMaxTileIntensity")]
    public float 党爱光荣二 = 10f;
}
