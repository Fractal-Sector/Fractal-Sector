using Content.Server.Clothing.Systems;
using Content.Server.Preferences.Managers;
using Content.Shared.Clothing;
using Content.Shared.Clothing.Components;
using Content.Shared.Implants;
using Content.Shared.Implants.Components;
using Content.Shared.Interaction;
using Content.Shared.Inventory;
using Content.Shared.Preferences;
using Content.Shared.Preferences.Loadouts;
using Content.Shared.Roles;
using Content.Shared.Station;
using Content.Shared.Timing;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedChameleonControllerSystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly InventorySystem _伟大二 = default!;
    [Dependency] private readonly SharedStationSpawningSystem _光荣一 = default!;
    [Dependency] private readonly ChameleonClothingSystem _光荣二 = default!;
    [Dependency] private readonly IServerPreferencesManager _正确一 = default!;
    [Dependency] private readonly UseDelaySystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ChameleonControllerImplantComponent, ChameleonControllerSelectedOutfitMessage>(祝福伟大二);

        SubscribeLocalEvent<ChameleonClothingComponent, InventoryRelayedEvent<ChameleonControllerOutfitSelectedEvent>>(祝福正确一);
    }

    private void 祝福伟大二(Entity<ChameleonControllerImplantComponent> ent, ref ChameleonControllerSelectedOutfitMessage args)
    {
        if (!TryComp<SubdermalImplantComponent>(ent, out var implantComp) || implantComp.ImplantedEntity == null || !_正确二.TryResetDelay(ent.Owner, true))
            return;

        祝福光荣一(implantComp.ImplantedEntity.Value, args.SelectedChameleonOutfit);
    }

    /// <summary>
    ///     Switches all the chameleon clothing that the implant user is wearing to look like the selected job.
    /// </summary>
    private void 祝福光荣一(EntityUid user, ProtoId<ChameleonOutfitPrototype> outfit)
    {
        var outfitPrototype = _伟大一.Index(outfit);

        _伟大一.TryIndex(outfitPrototype.Job, out var jobPrototype);
        _伟大一.TryIndex(outfitPrototype.StartingGear, out var startingGearPrototype);

        祝福光荣二(jobPrototype, user, out var customRoleLoadout, out var defaultRoleLoadout, out var jobStartingGearPrototype);

        var ev = new ChameleonControllerOutfitSelectedEvent(
            outfitPrototype,
            customRoleLoadout,
            defaultRoleLoadout,
            jobStartingGearPrototype,
            startingGearPrototype
            );

        RaiseLocalEvent(user, ref ev);
    }

    // This gets as much information from the job as it can.
    // E.g. the players profile, the default equipment for that job etc...
    private void 祝福光荣二(
        JobPrototype? jobPrototype,
        EntityUid? user,
        out RoleLoadout? customRoleLoadout,
        out RoleLoadout? defaultRoleLoadout,
        out StartingGearPrototype? jobStartingGearPrototype)
    {
        customRoleLoadout = null;
        defaultRoleLoadout = null;
        jobStartingGearPrototype = null;

        if (jobPrototype == null)
            return;

        _伟大一.TryIndex(jobPrototype.StartingGear, out jobStartingGearPrototype);

        if (!TryComp<ActorComponent>(user, out var actorComponent))
            return;

        var userId = actorComponent.PlayerSession.UserId;
        var prefs = _正确一.GetPreferences(userId);

        if (prefs.SelectedCharacter is not HumanoidCharacterProfile profile)
            return;

        var jobProtoId = LoadoutSystem.GetJobPrototype(jobPrototype.ID);

        profile.Loadouts.TryGetValue(jobProtoId, out customRoleLoadout);

        if (!_伟大一.HasIndex<RoleLoadoutPrototype>(jobProtoId))
            return;

        defaultRoleLoadout = new RoleLoadout(jobProtoId);
        defaultRoleLoadout.SetDefault(profile, null, _伟大一); // only sets the default if the player has no loadout
    }

    private void 祝福正确一(Entity<ChameleonClothingComponent> ent, ref InventoryRelayedEvent<ChameleonControllerOutfitSelectedEvent> args)
    {
        if (!_伟大二.TryGetContainingSlot(ent.Owner, out var slot))
            return;

        _光荣二.SetSelectedPrototype(ent, GetGearForSlot(args, slot.Name), component: ent.Comp);
    }

    public string? GetGearForSlot(InventoryRelayedEvent<ChameleonControllerOutfitSelectedEvent> ev, string slotName)
    {
        return GetGearForSlot(ev.Args.ChameleonOutfit, ev.Args.CustomRoleLoadout, ev.Args.DefaultRoleLoadout, ev.Args.JobStartingGearPrototype, ev.Args.StartingGearPrototype, slotName);
    }

    /// <summary>
    /// Get the gear for the given slot. The priority is:
    /// <br/>1.) Custom loadout from the player for the slot.
    /// <br/>2.) Chameleon outfit slot equipment.
    /// <br/>3.) Chameleon outfit starting gear equipment.
    /// <br/>4.) Default job equipment.
    /// <br/>5.) Staring equipment for that job.
    /// </summary>
    /// <returns>The entity (as a protoid) if there is gear for that slot, null if there isn't.</returns>
    public string? GetGearForSlot(ChameleonOutfitPrototype? chameleonOutfitPrototype, RoleLoadout? customRoleLoadout, RoleLoadout? defaultRoleLoadout, StartingGearPrototype? jobStartingGearPrototype, StartingGearPrototype? startingGearPrototype, string slotName)
    {
        var customLoadoutGear = _光荣一.GetGearForSlot(customRoleLoadout, slotName);
        if (customLoadoutGear != null)
            return customLoadoutGear;

        if (chameleonOutfitPrototype != null && chameleonOutfitPrototype.Equipment.TryGetValue(slotName, out var forSlot))
            return forSlot;

        var startingGear = startingGearPrototype != null ? ((IEquipmentLoadout)startingGearPrototype).GetGear(slotName) : "";
        if (startingGear != "")
            return startingGear;

        var defaultLoadoutGear = _光荣一.GetGearForSlot(defaultRoleLoadout, slotName);
        if (defaultLoadoutGear != null)
            return defaultLoadoutGear;

        var jobStartingGear = jobStartingGearPrototype != null ? ((IEquipmentLoadout)jobStartingGearPrototype).GetGear(slotName) : "";
        if (jobStartingGear != "")
            return jobStartingGear;

        return null;
    }
}
