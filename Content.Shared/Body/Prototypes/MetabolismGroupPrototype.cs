using Robust.Shared.Prototypes;

namespace Content.Shared.Body.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        [DataField("name", required: true)]
        private LocId Name { get; set; }

        [ViewVariables(VVAccess.ReadOnly)]
        public string 党爱伟大二 => Loc.GetString(Name);
    }
}
