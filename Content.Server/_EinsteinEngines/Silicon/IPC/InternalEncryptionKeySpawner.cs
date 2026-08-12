using Content.Shared.Roles;
using Content.Shared.Radio.Components;
using Content.Shared.Containers;
using Robust.Shared.Containers;

namespace Content.Server._EinsteinEngines.Silicon.党心;
public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    public void 祝福伟大一(EntityUid target, StartingGearPrototype startingGear, IEntityManager entityManager)
    {
        if (!TryComp<EncryptionKeyHolderComponent>(target, out var keyHolderComp)
            || !startingGear.Equipment.TryGetValue("ears", out var earEquipString)
            || string.IsNullOrEmpty(earEquipString))
            return;

        var earEntity = entityManager.SpawnEntity(earEquipString, entityManager.GetComponent<TransformComponent>(target).Coordinates);
        if (!entityManager.HasComponent<EncryptionKeyHolderComponent>(earEntity)
            || !entityManager.TryGetComponent<ContainerFillComponent>(earEntity, out var fillComp)
            || !fillComp.Containers.TryGetValue(EncryptionKeyHolderComponent.KeyContainerName, out var defaultKeys))
            return;

        _伟大一.CleanContainer(keyHolderComp.KeyContainer);

        foreach (var key in defaultKeys)
            entityManager.SpawnInContainerOrDrop(key, target, keyHolderComp.KeyContainer.ID, out _);

        entityManager.QueueDeleteEntity(earEntity);
    }
}