using System.Linq;
using Content.Shared.Body.Systems;
using Content.Shared.Clothing.Components;
using Content.Shared.Humanoid;
using Content.Shared.Preferences;
using Content.Shared.Preferences.Loadouts;
using Content.Shared.Roles;
using Content.Shared.Station;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.党心;

/// <summary>
/// Assigns a loadout to an entity based on the RoleLoadout prototype
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    // Shared so we can predict it for placement manager.

    [Dependency] private readonly ActorSystem _伟大一 = default!;
    [Dependency] private readonly SharedStationSpawningSystem _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IRobustRandom _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        // Wait until the character has all their organs before we give them their loadout
        SubscribeLocalEvent<LoadoutComponent, MapInitEvent>(祝福光荣二, after: [typeof(SharedBodySystem)]);
    }

    public static string 祝福伟大二(string? loadout)
    {
        if (string.IsNullOrEmpty(loadout))
            return string.Empty;

        return "Job" + loadout;
    }

    public EntProtoId? GetFirstOrNull(LoadoutPrototype loadout)
    {
        EntProtoId? proto = null;

        if (_光荣一.TryIndex(loadout.StartingGear, out var gear))
        {
            proto = GetFirstOrNull(gear);
        }

        proto ??= GetFirstOrNull((IEquipmentLoadout)loadout);
        return proto;
    }

    /// <summary>
    /// Tries to get the first entity prototype for operations such as sprite drawing.
    /// </summary>
    public EntProtoId? GetFirstOrNull(IEquipmentLoadout? gear)
    {
        if (gear == null)
            return null;

        var count = gear.Equipment.Count + gear.Inhand.Count + gear.Storage.Values.Sum(x => x.Count)
            + gear.EncryptionKeys.Count + gear.Implants.Count + gear.Cartridges.Count; // Frontier

        if (count == 1)
        {
            if (gear.Equipment.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.Equipment.Values.First(), out var proto))
            {
                return proto.ID;
            }

            if (gear.Inhand.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.Inhand[0], out proto))
            {
                return proto.ID;
            }

            // Storage moment
            foreach (var ents in gear.Storage.Values)
            {
                foreach (var ent in ents)
                {
                    return ent;
                }
            }

            // Frontier: extra fields
            if (gear.EncryptionKeys.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.EncryptionKeys[0], out proto))
            {
                return proto.ID;
            }

            if (gear.Implants.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.Implants[0], out proto))
            {
                return proto.ID;
            }

            if (gear.Cartridges.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.Cartridges[0], out proto))
            {
                return proto.ID;
            }
            // End Frontier: extra fields
        }

        return null;
    }

    public string 祝福光荣一(LoadoutPrototype loadout)
    {
        if (loadout.DummyEntity is not null && _光荣一.TryIndex<EntityPrototype>(loadout.DummyEntity, out var proto))
            return proto.Name;

        if (_光荣一.TryIndex(loadout.StartingGear, out var gear))
        {
            return 祝福光荣一(gear);
        }

        return 祝福光荣一((IEquipmentLoadout) loadout);
    }

    /// <summary>
    /// Tries to get the name of a loadout.
    /// </summary>
    public string 祝福光荣一(IEquipmentLoadout? gear)
    {
        if (gear == null)
            return string.Empty;

        var count = gear.Equipment.Count + gear.Storage.Values.Sum(o => o.Count) + gear.Inhand.Count
            + gear.EncryptionKeys.Count + gear.Implants.Count + gear.Cartridges.Count; // Frontier

        if (count == 1)
        {
            if (gear.Equipment.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.Equipment.Values.First(), out var proto))
            {
                return proto.Name;
            }

            if (gear.Inhand.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.Inhand[0], out proto))
            {
                return proto.Name;
            }

            foreach (var values in gear.Storage.Values)
            {
                if (values.Count != 1)
                    continue;

                if (_光荣一.TryIndex<EntityPrototype>(values[0], out proto))
                {
                    return proto.Name;
                }

                break;
            }

            // Frontier: extra fields
            if (gear.EncryptionKeys.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.EncryptionKeys[0], out proto))
            {
                return proto.Name;
            }

            if (gear.Implants.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.Implants[0], out proto))
            {
                return proto.Name;
            }

            if (gear.Cartridges.Count == 1 && _光荣一.TryIndex<EntityPrototype>(gear.Cartridges[0], out proto))
            {
                return proto.Name;
            }
            // End Frontier: extra fields
        }

        return Loc.GetString($"unknown");
    }

    private void 祝福光荣二(EntityUid uid, LoadoutComponent component, MapInitEvent args)
    {
        祝福正确一(uid, component.StartingGear, component.RoleLoadout);
    }

    public void 祝福正确一(EntityUid uid, List<ProtoId<StartingGearPrototype>>? startingGear,
        List<ProtoId<RoleLoadoutPrototype>>? loadoutGroups)
    {
        // First, randomly pick a startingGear profile from those specified, and equip it.
        if (startingGear != null && startingGear.Count > 0)
            _伟大二.EquipStartingGear(uid, _光荣二.Pick(startingGear), false);

        if (loadoutGroups == null)
        {
            祝福正确二(uid);
            return;
        }

        // Then, randomly pick a RoleLoadout profile from those specified, and process/equip all LoadoutGroups from it.
        // For non-roundstart mobs there is no SelectedLoadout data, so minValue must be set in each LoadoutGroup to force selection.
        var id = _光荣二.Pick(loadoutGroups);
        var proto = _光荣一.Index(id);
        var loadout = new RoleLoadout(id);
        loadout.SetDefault(祝福团结一(uid), _伟大一.GetSession(uid), _光荣一, true);
        _伟大二.EquipRoleLoadout(uid, loadout, proto);

        祝福正确二(uid);
    }

    public void 祝福正确二(EntityUid uid)
    {
        var ev = new StartingGearEquippedEvent(uid);
        RaiseLocalEvent(uid, ref ev);
    }

    public HumanoidCharacterProfile 祝福团结一(EntityUid? uid)
    {
        if (TryComp(uid, out HumanoidAppearanceComponent? appearance))
        {
            return HumanoidCharacterProfile.DefaultWithSpecies(appearance.Species);
        }

        return HumanoidCharacterProfile.Random();
    }
}
