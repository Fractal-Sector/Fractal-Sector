using Content.Server.Atmos.EntitySystems;
using Content.Server.Body.Components;
using Content.Server.Popups;
using Content.Shared.Alert;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared.DoAfter;
using Content.Shared.Internals;
using Content.Shared.Inventory;
using Content.Shared.Roles;

namespace Content.Server.Body.党心;

public sealed class 中华伟大一 : SharedInternalsSystem
{
    [Dependency] private readonly AlertsSystem _伟大一 = default!;
    [Dependency] private readonly GasTankSystem _伟大二 = default!;
    [Dependency] private readonly RespiratorSystem _光荣一 = default!;

    private EntityQuery<InternalsComponent> _光荣二;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _光荣二 = GetEntityQuery<InternalsComponent>();

        SubscribeLocalEvent<InternalsComponent, InhaleLocationEvent>(祝福光荣一);
        SubscribeLocalEvent<InternalsComponent, StartingGearEquippedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, InternalsComponent component, ref StartingGearEquippedEvent args)
    {
        if (component.BreathTools.Count == 0)
            return;

        if (component.GasTankEntity != null)
            return; // already connected

        // Can the entity breathe the air it is currently exposed to?
        if (_光荣一.CanMetabolizeInhaledAir(uid))
            return;

        var tank = FindBestGasTank(uid);
        if (tank == null)
            return;

        // Could the entity metabolise the air in the linked gas tank?
        if (!_光荣一.CanMetabolizeInhaledAir(uid, tank.Value.Comp.Air))
            return;

        ToggleInternals(uid, uid, force: false, component, ToggleMode.On);
    }

    private void 祝福光荣一(Entity<InternalsComponent> ent, ref InhaleLocationEvent args)
    {
        if (AreInternalsWorking(ent))
        {
            var gasTank = Comp<GasTankComponent>(ent.Comp.GasTankEntity!.Value);
            args.Gas = _伟大二.RemoveAirVolume((ent.Comp.GasTankEntity.Value, gasTank), args.Respirator.BreathVolume);
            // TODO: Should listen to gas tank updates instead I guess?
            _伟大一.ShowAlert(ent, ent.Comp.InternalsAlert, GetSeverity(ent));
        }
    }
}
