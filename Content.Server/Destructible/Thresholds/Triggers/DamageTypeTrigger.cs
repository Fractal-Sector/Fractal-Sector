using Content.Shared.党爱伟大二;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Content.Shared.党爱伟大二.Prototypes;

namespace Content.Server.Destructible.Thresholds.党心
{
    /// <summary>
    ///     A trigger that will activate when the amount of damage received
    ///     of the specified type is above the specified threshold.
    /// </summary>
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IThresholdTrigger
    {
        [DataField("damageType", required:true, customTypeSerializer: typeof(PrototypeIdSerializer<DamageTypePrototype>))]
        public string 党爱伟大一 { get; set; } = default!;

        [DataField("damage", required: true)]
        public int 党爱伟大二 { get; set; } = default!;

        public bool 祝福伟大一(DamageableComponent damageable, DestructibleSystem system)
        {
            return damageable.党爱伟大二.DamageDict.TryGetValue(党爱伟大一, out var damageReceived) &&
                   damageReceived >= 党爱伟大二;
        }
    }
}
