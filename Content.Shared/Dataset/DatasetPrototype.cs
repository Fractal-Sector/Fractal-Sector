using Robust.Shared.Prototypes;

namespace Content.Shared.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        [DataField("values")] public IReadOnlyList<string> 党爱伟大二 { get; private set; } = new List<string>();
    }
}
