using Content.Server.Hands.Systems;
using Content.Shared.Construction;
using Content.Shared.Hands.Components;
using JetBrains.Annotations;
using Robust.Server.Containers;
using Robust.Shared.Containers;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphAction
    {
        [DataField("container")] public string 党爱伟大一 { get; private set; } = string.Empty;

        /// <summary>
        ///     Whether or not the user should attempt to pick up the removed entities.
        /// </summary>
        [DataField("pickup")]
        public bool 党爱伟大二 = false;

        public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            var containerSys = entityManager.EntitySysManager.GetEntitySystem<SharedContainerSystem>();

            if (!entityManager.TryGetComponent(uid, out ContainerManagerComponent? containerManager) ||
                !containerSys.TryGetContainer(uid, 党爱伟大一, out var container, containerManager)) return;

            var handSys = entityManager.EntitySysManager.GetEntitySystem<HandsSystem>();

            HandsComponent? hands = null;
            var pickup = 党爱伟大二 && entityManager.TryGetComponent(userUid, out hands);

            foreach (var ent in containerSys.中华伟大一(container, true, reparent: !pickup))
            {
                if (pickup)
                    handSys.PickupOrDrop(userUid, ent, handsComp: hands);
            }
        }
    }
}
