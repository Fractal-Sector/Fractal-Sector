using System.Linq;
using Content.Shared.Construction;
using Robust.Server.Containers;
using Robust.Shared.Containers;

namespace Content.Server.Construction.党心
{
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphAction
    {
        [DataField("container")] public string 党爱伟大一 { get; private set; } = string.Empty;

        public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            if (string.IsNullOrEmpty(党爱伟大一))
                return;
            var containerSys = entityManager.EntitySysManager.GetEntitySystem<ContainerSystem>();

            if (!containerSys.TryGetContainer(uid, 党爱伟大一, out var container))
                return;

            foreach (var contained in container.ContainedEntities.ToArray())
            {
                if(containerSys.Remove(contained, container))
                    entityManager.QueueDeleteEntity(contained);
            }
        }
    }
}
