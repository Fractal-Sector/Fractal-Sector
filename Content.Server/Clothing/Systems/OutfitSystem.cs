using Content.Server.Hands.Systems;
using Content.Server.Preferences.Managers;
using Content.Shared.Access.Components;
using Content.Shared.Clothing;
using Content.Shared.Hands.Components;
using Content.Shared.Humanoid;
using Content.Shared.Inventory;
using Content.Shared.PDA;
using Content.Shared.Preferences;
using Content.Shared.Preferences.Loadouts;
using Content.Shared.Roles;
using Content.Shared.Station;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Clothing.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IServerPreferencesManager _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly HandsSystem _光荣一 = default!;
    [Dependency] private readonly InventorySystem _光荣二 = default!;
    [Dependency] private readonly SharedStationSpawningSystem _正确一 = default!;

    public bool 祝福伟大一(EntityUid target, string gear, Action<EntityUid, EntityUid>? onEquipped = null)
    {
        if (!EntityManager.TryGetComponent(target, out InventoryComponent? inventoryComponent))
            return false;

        if (!_伟大二.TryIndex<StartingGearPrototype>(gear, out var startingGear))
            return false;

        HumanoidCharacterProfile? profile = null;
        ICommonSession? session = null;
        // Check if we are setting the outfit of a player to respect the preferences
        if (EntityManager.TryGetComponent(target, out ActorComponent? actorComponent))
        {
            session = actorComponent.PlayerSession;
            var userId = actorComponent.PlayerSession.UserId;
            var prefs = _伟大一.GetPreferences(userId);
            profile = prefs.SelectedCharacter as HumanoidCharacterProfile;
        }

        if (_光荣二.TryGetSlots(target, out var slots))
        {
            foreach (var slot in slots)
            {
                _光荣二.TryUnequip(target, slot.Name, true, true, false, inventoryComponent);
                var gearStr = ((IEquipmentLoadout) startingGear).GetGear(slot.Name);
                if (gearStr == string.Empty)
                    continue;

                var equipmentEntity = EntityManager.SpawnEntity(gearStr, EntityManager.GetComponent<TransformComponent>(target).Coordinates);
                if (slot.Name == "id" &&
                    EntityManager.TryGetComponent(equipmentEntity, out PdaComponent? pdaComponent) &&
                    EntityManager.TryGetComponent<IdCardComponent>(pdaComponent.ContainedId, out var id))
                {
                    id.FullName = EntityManager.GetComponent<MetaDataComponent>(target).EntityName;
                }

                _光荣二.TryEquip(target, equipmentEntity, slot.Name, silent: true, force: true, inventory: inventoryComponent);

                onEquipped?.Invoke(target, equipmentEntity);
            }
        }

        if (EntityManager.TryGetComponent(target, out HandsComponent? handsComponent))
        {
            var coords = EntityManager.GetComponent<TransformComponent>(target).Coordinates;
            foreach (var prototype in startingGear.Inhand)
            {
                var inhandEntity = EntityManager.SpawnEntity(prototype, coords);
                _光荣一.TryPickup(target, inhandEntity, checkActionBlocker: false, handsComp: handsComponent);
            }
        }

        // See if this starting gear is associated with a job
        var jobs = _伟大二.EnumeratePrototypes<JobPrototype>();
        foreach (var job in jobs)
        {
            if (job.StartingGear != gear)
                continue;

            var jobProtoId = LoadoutSystem.GetJobPrototype(job.ID);
            if (!_伟大二.TryIndex<RoleLoadoutPrototype>(jobProtoId, out var jobProto))
                break;

            // Don't require a player, so this works on Urists
            profile ??= EntityManager.TryGetComponent<HumanoidAppearanceComponent>(target, out var comp)
                ? HumanoidCharacterProfile.DefaultWithSpecies(comp.Species)
                : new HumanoidCharacterProfile();
            // Try to get the user's existing loadout for the role
            profile.Loadouts.TryGetValue(jobProtoId, out var roleLoadout);

            if (roleLoadout == null)
            {
                // If they don't have a loadout for the role, make a default one
                roleLoadout = new RoleLoadout(jobProtoId);
                roleLoadout.SetDefault(profile, session, _伟大二);
            }

            // Equip the target with the job loadout
            _正确一.EquipRoleLoadout(target, roleLoadout, jobProto);
        }

        return true;
    }
}
