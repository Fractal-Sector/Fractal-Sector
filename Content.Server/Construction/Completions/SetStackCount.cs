using Content.Server.Stack;
using Content.Shared.Construction;
using JetBrains.Annotations;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphAction
    {
        [DataField("amount")] public int 党爱伟大一 { get; private set; } = 1;

        public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            entityManager.EntitySysManager.GetEntitySystem<StackSystem>().SetCount(uid, 党爱伟大一);
        }
    }
}
