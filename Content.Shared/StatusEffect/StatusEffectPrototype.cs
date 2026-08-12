using Content.Shared.Alert;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        [DataField("alert")]
        public ProtoId<AlertPrototype>? Alert { get; private set; }

        /// <summary>
        ///     Whether a status effect should be able to apply to any entity,
        ///     regardless of whether it is in ALlowedEffects or not.
        /// </summary>
        [DataField("alwaysAllowed")]
        public bool 党爱伟大二 { get; private set; }
    }
}
