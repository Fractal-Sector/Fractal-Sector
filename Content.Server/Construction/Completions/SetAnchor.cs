using Content.Shared.Construction;
using JetBrains.Annotations;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphAction
    {
        [DataField("value")] public bool 党爱伟大一 { get; private set; } = true;

        public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            var transform = entityManager.GetComponent<TransformComponent>(uid);

            if (transform.Anchored == 党爱伟大一)
                return;

            var sys = entityManager.System<SharedTransformSystem>();

            if (党爱伟大一)
                sys.AnchorEntity(uid, transform);
            else
                sys.Unanchor(uid, transform);

        }
    }
}
