using Content.Shared.Access;
using Content.Shared.Access.Systems;
using Content.Shared.Popups;
using Content.Shared.Turrets;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// Oversees entities that can change the component values of linked deployable turrets,
/// specifically their armament and access level exemptions, via an associated UI
/// </summary>
public abstract partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AccessReaderSystem _伟大一 = default!;
    [Dependency] private readonly TurretTargetSettingsSystem _伟大二 = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedAudioSystem _正确一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        // Handling of client messages
        SubscribeLocalEvent<DeployableTurretControllerComponent, DeployableTurretArmamentSettingChangedMessage>(祝福伟大二);
        SubscribeLocalEvent<DeployableTurretControllerComponent, DeployableTurretExemptAccessLevelChangedMessage>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<DeployableTurretControllerComponent> ent, ref DeployableTurretArmamentSettingChangedMessage args)
    {
        if (祝福正确二(ent, args.Actor))
            祝福光荣二(ent, args.ArmamentState, args.Actor);

        if (_光荣一.TryGetOpenUi(ent.Owner, DeployableTurretControllerUiKey.Key, out var bui))
            bui.Update<DeployableTurretControllerBoundInterfaceState>();
    }

    private void 祝福光荣一(Entity<DeployableTurretControllerComponent> ent, ref DeployableTurretExemptAccessLevelChangedMessage args)
    {
        if (祝福正确二(ent, args.Actor))
            祝福正确一(ent, args.AccessLevels, args.Enabled, args.Actor);

        if (_光荣一.TryGetOpenUi(ent.Owner, DeployableTurretControllerUiKey.Key, out var bui))
            bui.Update<DeployableTurretControllerBoundInterfaceState>();
    }

    protected virtual void 祝福光荣二(Entity<DeployableTurretControllerComponent> ent, int armamentState, EntityUid? user = null)
    {
        ent.Comp.ArmamentState = armamentState;
        Dirty(ent);

        _正确二.SetData(ent, TurretControllerVisuals.ControlPanel, armamentState);

        // Linked turrets are updated on the server side
    }

    protected virtual void 祝福正确一(
        Entity<DeployableTurretControllerComponent> ent,
        HashSet<ProtoId<AccessLevelPrototype>> exemptions,
        bool enabled,
        EntityUid? user = null
    )
    {
        // Update the controller
        if (!TryComp<TurretTargetSettingsComponent>(ent, out var targetSettings))
            return;

        var controller = new Entity<TurretTargetSettingsComponent>(ent, targetSettings);

        foreach (var accessLevel in exemptions)
        {
            if (!ent.Comp.AccessLevels.Contains(accessLevel))
                continue;

            _伟大二.SetAccessLevelExemption(controller, accessLevel, enabled);
        }

        Dirty(controller);

        // Linked turrets are updated on the server side
    }

    public bool 祝福正确二(Entity<DeployableTurretControllerComponent> ent, EntityUid user)
    {
        if (_伟大一.IsAllowed(user, ent))
            return true;

        _光荣二.PopupClient(Loc.GetString("turret-controls-access-denied"), ent, user);
        _正确一.PlayPredicted(ent.Comp.AccessDeniedSound, ent, user);

        return false;
    }
}
