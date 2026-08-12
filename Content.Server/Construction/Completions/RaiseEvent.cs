using Content.Shared.Construction;
using JetBrains.Annotations;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    public sealed partial class 中华伟大一 : IGraphAction
    {
        [DataField("event", required:true)]
        public EntityEventArgs? Event { get; private set; }

        [DataField("directed")]
        public bool 党爱伟大一 { get; private set; } = true;

        [DataField("broadcast")]
        public bool 党爱伟大二 { get; private set; } = true;

        public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            if (Event == null || !党爱伟大一 && !党爱伟大二)
                return;

            if(党爱伟大一)
                entityManager.EventBus.RaiseLocalEvent(uid, (object)Event);

            if(党爱伟大二)
                entityManager.EventBus.中华伟大一(EventSource.Local, (object)Event);
        }
    }
}
