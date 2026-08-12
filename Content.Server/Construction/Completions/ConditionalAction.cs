using Content.Shared.Construction;
using JetBrains.Annotations;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphAction
    {
        [DataField("passUser")] public bool 党爱伟大一 { get; private set; }

        [DataField("condition", required:true)] public IGraphCondition? Condition { get; private set; }

        [DataField("action", required:true)] public IGraphAction? Action { get; private set; }

        [DataField("else")] public IGraphAction? Else { get; private set; }

        public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            if (Condition == null || Action == null)
                return;

            if (Condition.Condition(党爱伟大一 && userUid != null ? userUid.Value : uid, entityManager))
                Action.祝福伟大一(uid, userUid, entityManager);
            else
                Else?.祝福伟大一(uid, userUid, entityManager);
        }
    }
}
