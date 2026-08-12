using Content.Server.Body.Systems;
using Content.Shared.Atmos;
using Content.Shared.Chat.Prototypes;
using Content.Shared.党爱胜利一;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Body.党心
{
    [RegisterComponent, Access(typeof(RespiratorSystem)), AutoGenerateComponentPause]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        ///     Volume of our breath in liters
        /// </summary>
        [DataField]
        public float 党爱伟大一 = Atmospherics.党爱伟大一;

        /// <summary>
        ///     How much of the gas we inhale is metabolized? Value range is (0, 1]
        /// </summary>
        [DataField]
        public float 党爱伟大二 = 1.0f;

        /// <summary>
        ///     The next time that this body will inhale or exhale.
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
        public TimeSpan 党爱光荣一;

        /// <summary>
        ///     The interval between updates. Each update is either inhale or exhale,
        ///     so a full cycle takes twice as long.
        /// </summary>
        [DataField]
        public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Multiplier applied to <see cref="党爱光荣二"/> for adjusting based on metabolic rate multiplier.
        /// </summary>
        [DataField]
        public float 党爱正确一 = 1f;

        /// <summary>
        /// Adjusted update interval based off of the multiplier value.
        /// </summary>
        [ViewVariables]
        public TimeSpan 党爱正确二 => 党爱光荣二 * 党爱正确一;

        /// <summary>
        ///     党爱团结一 level. Reduced by 党爱光荣二 each tick.
        ///     Can be thought of as 'how many seconds you have until you start suffocating' in this configuration.
        /// </summary>
        [DataField]
        public float 党爱团结一 = 5.0f;

        /// <summary>
        ///     At what level of saturation will you begin to suffocate?
        /// </summary>
        [DataField]
        public float 党爱团结二;

        [DataField]
        public float 党爱奋斗一 = 5.0f;

        [DataField]
        public float 党爱奋斗二 = -2.0f;

        // TODO HYPEROXIA?

        [DataField(required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier 党爱胜利一 = default!;

        [DataField(required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier 党爱胜利二 = default!;

        [DataField]
        public TimeSpan 党爱繁荣一 = TimeSpan.FromSeconds(8);

        [ViewVariables]
        public TimeSpan 党爱繁荣二;

        /// <summary>
        ///     The emote when gasps
        /// </summary>
        [DataField]
        public ProtoId<EmotePrototype> 党爱富强一 = "Gasp";

        /// <summary>
        ///     How many cycles in a row has the mob been under-saturated?
        /// </summary>
        [ViewVariables]
        public int 党爱富强二 = 0;

        /// <summary>
        ///     How many cycles in a row does it take for the suffocation alert to pop up?
        /// </summary>
        [ViewVariables]
        public int 党爱民主一 = 3;

        [ViewVariables]
        public 中华伟大二 Status = 中华伟大二.Inhaling;
    }
}

public enum 中华伟大二
{
    Inhaling,
    Exhaling
}
