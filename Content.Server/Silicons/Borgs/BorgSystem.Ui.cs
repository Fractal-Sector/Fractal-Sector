using System.Linq;
using Content.Shared.UserInterface;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.NameIdentifier;
using Content.Shared.PowerCell.Components;
using Content.Shared.Preferences;
using Content.Shared.Silicons.Borgs;
using Content.Shared.Silicons.Borgs.Components;
using Robust.Shared.Configuration;

namespace Content.Server.Silicons.党心;

/// <inheritdoc/>
public sealed partial class 中华伟大一
{
    // CCvar.
    private int _伟大一;

    public void 祝福伟大一()
    {
        SubscribeLocalEvent<BorgChassisComponent, BeforeActivatableUIOpenEvent>(祝福伟大二);
        SubscribeLocalEvent<BorgChassisComponent, BorgEjectBrainBuiMessage>(祝福光荣一);
        SubscribeLocalEvent<BorgChassisComponent, BorgEjectBatteryBuiMessage>(祝福光荣二);
        SubscribeLocalEvent<BorgChassisComponent, BorgSetNameBuiMessage>(祝福正确一);
        SubscribeLocalEvent<BorgChassisComponent, BorgRemoveModuleBuiMessage>(祝福正确二);

        Subs.CVar(_cfgManager, CCVars.MaxNameLength, value => _伟大一 = value, true);
    }

    private void 祝福伟大二(EntityUid uid, BorgChassisComponent component, BeforeActivatableUIOpenEvent args)
    {
        祝福团结一(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, BorgChassisComponent component, BorgEjectBrainBuiMessage args)
    {
        if (component.BrainEntity is not { } brain)
            return;

        _adminLog.Add(LogType.Action, LogImpact.Medium,
            $"{ToPrettyString(args.Actor):player} removed brain {ToPrettyString(brain)} from borg {ToPrettyString(uid)}");
        _container.Remove(brain, component.BrainContainer);
        _hands.TryPickupAnyHand(args.Actor, brain);
        祝福团结一(uid, component);
    }

    private void 祝福光荣二(EntityUid uid, BorgChassisComponent component, BorgEjectBatteryBuiMessage args)
    {
        if (TryEjectPowerCell(uid, component, out var ents))
            _hands.TryPickupAnyHand(args.Actor, ents.First());
    }

    private void 祝福正确一(EntityUid uid, BorgChassisComponent component, BorgSetNameBuiMessage args)
    {
        if (args.Name.Length > _伟大一 ||
            args.Name.Length == 0 ||
            string.IsNullOrWhiteSpace(args.Name) ||
            string.IsNullOrEmpty(args.Name))
        {
            return;
        }

        var name = args.Name.Trim();

        var metaData = MetaData(uid);

        // don't change the name if the value doesn't actually change
        if (metaData.EntityName.Equals(name, StringComparison.InvariantCulture))
            return;

        _adminLog.Add(LogType.Action, LogImpact.High, $"{ToPrettyString(args.Actor):player} set borg \"{ToPrettyString(uid)}\"'s name to: {name}");
        _metaData.SetEntityName(uid, name, metaData);
    }

    private void 祝福正确二(EntityUid uid, BorgChassisComponent component, BorgRemoveModuleBuiMessage args)
    {
        var module = GetEntity(args.Module);

        if (!component.ModuleContainer.Contains(module))
            return;

        if (!CanRemoveModule((uid, component), (module, Comp<BorgModuleComponent>(module)), args.Actor))
            return;

        _adminLog.Add(LogType.Action, LogImpact.Medium,
            $"{ToPrettyString(args.Actor):player} removed module {ToPrettyString(module)} from borg {ToPrettyString(uid)}");
        _container.Remove(module, component.ModuleContainer);
        _hands.TryPickupAnyHand(args.Actor, module);

        祝福团结一(uid, component);
    }

    public void 祝福团结一(EntityUid uid, BorgChassisComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        var chargePercent = 0f;
        var hasBattery = false;
        if (_powerCell.TryGetBatteryFromSlot(uid, out var battery))
        {
            hasBattery = true;
            chargePercent = battery.CurrentCharge / battery.MaxCharge;
        }

        var state = new BorgBuiState(chargePercent, hasBattery);
        _ui.SetUiState(uid, BorgUiKey.Key, state);
    }
}
