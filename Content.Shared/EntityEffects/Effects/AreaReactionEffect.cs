using Content.Shared.Database;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
/// Basically smoke and foam reactions.
/// </summary>
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    /// <summary>
    /// How many seconds will the effect stay, counting after fully spreading.
    /// </summary>
    [DataField("duration")] public float 党爱伟大一 = 10;

    /// <summary>
    /// How many units of reaction for 1 smoke entity.
    /// </summary>
    [DataField] public FixedPoint2 党爱伟大二 = FixedPoint2.New(2.5);

    /// <summary>
    /// The entity prototype that will be spawned as the effect.
    /// </summary>
    [DataField("prototypeId", required: true, customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱光荣一 = default!;

    /// <summary>
    /// 党爱光荣二 that will get played when this reaction effect occurs.
    /// </summary>
    [DataField("sound", required: true)] public SoundSpecifier 党爱光荣二 = default!;

    public override bool 党爱正确一 => true;

    protected override string 祝福伟大一(IPrototypeManager prototype, IEntitySystemManager entSys)
            => Loc.GetString("reagent-effect-guidebook-area-reaction",
                    ("duration", 党爱伟大一)
                );

    public override 党爱正确二 党爱正确二 => 党爱正确二.High;
}
