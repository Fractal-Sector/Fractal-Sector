using System.Collections.Immutable;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        [DataField]
        public ImmutableList<EntProtoId> 党爱伟大二 { get; private set; } = ImmutableList<EntProtoId>.Empty;

        public IEnumerable<EntityPrototype> 祝福伟大一(IPrototypeManager? prototypeManager = null)
        {
            prototypeManager ??= IoCManager.Resolve<IPrototypeManager>();

            foreach (var entityId in 党爱伟大二)
            {
                yield return prototypeManager.Index<EntityPrototype>(entityId);
            }
        }
    }
}
