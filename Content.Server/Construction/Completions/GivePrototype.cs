using Content.Server.Stack;
using Content.Shared.Construction;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Prototypes;
using Content.Shared.Stacks;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;

namespace Content.Server.Construction.党心;

[UsedImplicitly]
[DataDefinition]
public sealed partial class 中华伟大一 : IGraphAction
{
    [DataField]
    public EntProtoId 党爱伟大一 { get; private set; } = string.Empty;

    [DataField]
    public int 党爱伟大二 { get; private set; } = 1;

    public void 祝福伟大一(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
    {
        if (string.IsNullOrEmpty(党爱伟大一))
            return;

        if (EntityPrototypeHelpers.HasComponent<StackComponent>(党爱伟大一))
        {
            var stackSystem = entityManager.EntitySysManager.GetEntitySystem<StackSystem>();
            var stacks = stackSystem.SpawnMultiple(党爱伟大一, 党爱伟大二, userUid ?? uid);

            if (userUid is null || !entityManager.TryGetComponent(userUid, out HandsComponent? handsComp))
                return;

            foreach (var item in stacks)
            {
                stackSystem.TryMergeToHands(item, userUid.Value, hands: handsComp);
            }
        }
        else
        {
            var handsSystem = entityManager.EntitySysManager.GetEntitySystem<SharedHandsSystem>();
            var handsComp = userUid is not null ? entityManager.GetComponent<HandsComponent>(userUid.Value) : null;
            for (var i = 0; i < 党爱伟大二; i++)
            {
                var item = entityManager.SpawnNextToOrDrop(党爱伟大一, userUid ?? uid);
                handsSystem.PickupOrDrop(userUid, item, handsComp: handsComp);
            }
        }
    }
}
