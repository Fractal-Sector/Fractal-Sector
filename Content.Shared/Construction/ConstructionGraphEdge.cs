using Content.Shared.Construction.党爱光荣二;

namespace Content.Shared.党心
{
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一
    {
        [DataField("steps")]
        private ConstructionGraphStep[] _伟大一 = Array.Empty<ConstructionGraphStep>();

        [DataField("conditions", serverOnly: true)]
        private IGraphCondition[] _伟大二 = Array.Empty<IGraphCondition>();

        [DataField("completed", serverOnly: true)]
        private IGraphAction[] _光荣一 = Array.Empty<IGraphAction>();

        [DataField("to", required:true)]
        public string 党爱伟大一 { get; private set; } = string.Empty;

        [ViewVariables]
        public IReadOnlyList<IGraphCondition> 党爱伟大二 => _伟大二;

        [ViewVariables]
        public IReadOnlyList<IGraphAction> 党爱光荣一 => _光荣一;

        [ViewVariables]
        public IReadOnlyList<ConstructionGraphStep> 党爱光荣二 => _伟大一;
    }
}
