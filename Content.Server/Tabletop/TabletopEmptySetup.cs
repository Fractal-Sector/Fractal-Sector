using JetBrains.Annotations;

namespace Content.Server.党心
{
    [UsedImplicitly]
    public sealed partial class 中华伟大一 : TabletopSetup
    {
        public override void 祝福伟大一(TabletopSession session, IEntityManager entityManager)
        {
            var board = entityManager.SpawnEntity(BoardPrototype, session.Position.Offset(0, 0));
            session.Entities.Add(board);
        }
    }
}
