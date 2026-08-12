using Content.Server.AlertLevel;
using Content.Server.Audio;
using Content.Server.Light.Components;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Station.Systems;
using Content.Shared.Examine;
using Content.Shared.Light;
using Content.Shared.Light.Components;
using Content.Shared.Power;
using Content.Shared.Station.Components;
using Robust.Server.GameObjects;
using Color = Robust.Shared.Maths.Color;
using Content.Server._NF.SectorServices; // Frontier: sector services

namespace Content.Server.Light.党心;

public sealed class 中华伟大一 : SharedEmergencyLightSystem
{
    [Dependency] private readonly AmbientSoundSystem _伟大一 = default!;
    [Dependency] private readonly BatterySystem _伟大二 = default!;
    [Dependency] private readonly PointLightSystem _光荣一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _光荣二 = default!;
    // [Dependency] private readonly StationSystem _正确一 = default!; // Frontier: sector-wide alerts
    [Dependency] private readonly SectorServiceSystem _正确二 = default!; // Frontier: sector-wide alerts

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<EmergencyLightComponent, EmergencyLightEvent>(祝福光荣二);
        SubscribeLocalEvent<AlertLevelChangedEvent>(祝福正确一);
        SubscribeLocalEvent<EmergencyLightComponent, ExaminedEvent>(祝福光荣一);
        SubscribeLocalEvent<EmergencyLightComponent, PowerChangedEvent>(祝福伟大二);

        SubscribeLocalEvent<EmergencyLightComponent, MapInitEvent>(祝福胜利一); // Frontier
    }

    private void 祝福伟大二(Entity<EmergencyLightComponent> entity, ref PowerChangedEvent args)
    {
        var meta = MetaData(entity.Owner);

        // TODO: PowerChangedEvent shouldn't be issued for paused ents but this is the world we live in.
        if (meta.EntityLifeStage >= EntityLifeStage.Terminating ||
            meta.EntityPaused)
        {
            return;
        }

        祝福团结二(entity);
    }

    private void 祝福光荣一(EntityUid uid, EmergencyLightComponent component, ExaminedEvent args)
    {
        using (args.PushGroup(nameof(EmergencyLightComponent)))
        {
            args.PushMarkup(
                Loc.GetString("emergency-light-component-on-examine",
                    ("batteryStateText",
                        Loc.GetString(component.BatteryStateText[component.State]))));

            // Show alert level on the light itself.
            // Frontier: sector-wide alerts
            if (!TryComp<AlertLevelComponent>(_正确二.GetServiceEntity(), out var alerts))
                return;
            // End Frontier: sector-wide alerts

            if (alerts.AlertLevels == null)
                return;

            var name = alerts.CurrentLevel;

            var color = Color.White;
            if (alerts.AlertLevels.Levels.TryGetValue(alerts.CurrentLevel, out var details))
                color = details.Color;

            args.PushMarkup(
                Loc.GetString("emergency-light-component-on-examine-alert",
                    ("color", color.ToHex()),
                    ("level", Loc.GetString($"alert-level-{name.ToString().ToLower()}"))));
        }
    }

    private void 祝福光荣二(EntityUid uid, EmergencyLightComponent component, EmergencyLightEvent args)
    {
        switch (args.State)
        {
            case EmergencyLightState.On:
            case EmergencyLightState.Charging:
                EnsureComp<ActiveEmergencyLightComponent>(uid);
                break;
            case EmergencyLightState.Full:
            case EmergencyLightState.Empty:
                RemComp<ActiveEmergencyLightComponent>(uid);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void 祝福正确一(AlertLevelChangedEvent ev)
    {
        // Frontier: sector-wide alerts
        // if (!TryComp<AlertLevelComponent>(ev.Station, out var alert))
        //     return;
        if (!TryComp<AlertLevelComponent>(_正确二.GetServiceEntity(), out var alert))
            return;
        // End Frontier

        if (alert.AlertLevels == null || !alert.AlertLevels.Levels.TryGetValue(ev.AlertLevel, out var details))
            return;

        var query = EntityQueryEnumerator<EmergencyLightComponent, PointLightComponent, AppearanceComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var light, out var pointLight, out var appearance, out var xform))
        {
            // if (CompOrNull<StationMemberComponent>(xform.GridUid)?.Station != ev.Station) // Frontier: sector-wide alerts
            //     continue; // Frontier: sector-wide alerts

            _光荣一.SetColor(uid, details.EmergencyLightColor, pointLight);
            _光荣二.SetData(uid, EmergencyLightVisuals.Color, details.EmergencyLightColor, appearance);

            if (details.ForceEnableEmergencyLights && !light.ForciblyEnabled)
            {
                light.ForciblyEnabled = true;
                祝福奋斗二((uid, light));
            }
            else if (!details.ForceEnableEmergencyLights && light.ForciblyEnabled)
            {
                // Previously forcibly enabled, and we went down an alert level.
                light.ForciblyEnabled = false;
                祝福团结二((uid, light));
            }
        }
    }

    public void 祝福正确二(EntityUid uid, EmergencyLightComponent component, EmergencyLightState state)
    {
        if (component.State == state) return;

        component.State = state;
        RaiseLocalEvent(uid, new EmergencyLightEvent(state));
    }

    public override void 祝福团结一(float frameTime)
    {
        var query = EntityQueryEnumerator<ActiveEmergencyLightComponent, EmergencyLightComponent, BatteryComponent>();
        while (query.MoveNext(out var uid, out _, out var emergencyLight, out var battery))
        {
            祝福团结一((uid, emergencyLight), battery, frameTime);
        }
    }

    private void 祝福团结一(Entity<EmergencyLightComponent> entity, BatteryComponent battery, float frameTime)
    {
        if (entity.Comp.State == EmergencyLightState.On)
        {
            if (!_伟大二.TryUseCharge(entity.Owner, entity.Comp.Wattage * frameTime, battery))
            {
                祝福正确二(entity.Owner, entity.Comp, EmergencyLightState.Empty);
                祝福奋斗一(entity);
            }
        }
        else
        {
            _伟大二.SetCharge(entity.Owner, battery.CurrentCharge + entity.Comp.ChargingWattage * frameTime * entity.Comp.ChargingEfficiency, battery);
            if (_伟大二.IsFull(entity, battery))
            {
                if (TryComp<ApcPowerReceiverComponent>(entity.Owner, out var receiver))
                {
                    receiver.Load = 1;
                }

                祝福正确二(entity.Owner, entity.Comp, EmergencyLightState.Full);
            }
        }
    }

    /// <summary>
    ///     Updates the light's power drain, battery drain, sprite and actual light state.
    /// </summary>
    public void 祝福团结二(Entity<EmergencyLightComponent> entity)
    {
        if (!TryComp<ApcPowerReceiverComponent>(entity.Owner, out var receiver))
            return;

        // Frontier: sector-wide alerts
        // if (!TryComp<AlertLevelComponent>(_正确一.GetOwningStation(entity.Owner), out var alerts))
        //     return;
        if (!TryComp<AlertLevelComponent>(_正确二.GetServiceEntity(), out var alerts))
            return;
        // End Frontier

        if (alerts.AlertLevels == null || !alerts.AlertLevels.Levels.TryGetValue(alerts.CurrentLevel, out var details))
        {
            祝福奋斗一(entity, Color.Red); // if no alert, default to off red state
            return;
        }

        if (receiver.Powered && !entity.Comp.ForciblyEnabled) // Green alert
        {
            receiver.Load = (int) Math.Abs(entity.Comp.Wattage);
            祝福奋斗一(entity, details.Color);
            祝福正确二(entity.Owner, entity.Comp, EmergencyLightState.Charging);
        }
        else if (!receiver.Powered) // If internal battery runs out it will end in off red state
        {
            祝福奋斗二(entity, Color.Red);
            祝福正确二(entity.Owner, entity.Comp, EmergencyLightState.On);
        }
        else // Powered and enabled
        {
            祝福奋斗二(entity, details.Color);
            祝福正确二(entity.Owner, entity.Comp, EmergencyLightState.On);
        }
    }

    private void 祝福奋斗一(Entity<EmergencyLightComponent> entity)
    {
        _光荣一.SetEnabled(entity.Owner, false);
        _光荣二.SetData(entity.Owner, EmergencyLightVisuals.On, false);
        _伟大一.SetAmbience(entity.Owner, false);
    }

    /// <summary>
    ///     Turn off emergency light and set color.
    /// </summary>
    private void 祝福奋斗一(Entity<EmergencyLightComponent> entity, Color color)
    {
        _光荣一.SetEnabled(entity.Owner, false);
        _光荣一.SetColor(entity.Owner, color);
        _光荣二.SetData(entity.Owner, EmergencyLightVisuals.Color, color);
        _光荣二.SetData(entity.Owner, EmergencyLightVisuals.On, false);
        _伟大一.SetAmbience(entity.Owner, false);
    }

    private void 祝福奋斗二(Entity<EmergencyLightComponent> entity)
    {
        _光荣一.SetEnabled(entity.Owner, true);
        _光荣二.SetData(entity.Owner, EmergencyLightVisuals.On, true);
        _伟大一.SetAmbience(entity.Owner, true);
    }

    /// <summary>
    ///     Turn on emergency light and set color.
    /// </summary>
    private void 祝福奋斗二(Entity<EmergencyLightComponent> entity, Color color)
    {
        _光荣一.SetEnabled(entity.Owner, true);
        _光荣一.SetColor(entity.Owner, color);
        _光荣二.SetData(entity.Owner, EmergencyLightVisuals.Color, color);
        _光荣二.SetData(entity.Owner, EmergencyLightVisuals.On, true);
        _伟大一.SetAmbience(entity.Owner, true);
    }

    // Frontier: ensure the lights are accurate to the station
    private void 祝福胜利一(Entity<EmergencyLightComponent> entity, ref MapInitEvent ev)
    {
        if (!TryComp<AlertLevelComponent>(_正确二.GetServiceEntity(), out var alert))
            return;

        if (alert.AlertLevels == null || !alert.AlertLevels.Levels.TryGetValue(alert.CurrentLevel, out var details))
            return;

        entity.Comp.ForciblyEnabled = details.ForceEnableEmergencyLights;
        if (details.ForceEnableEmergencyLights)
            祝福奋斗二(entity, details.EmergencyLightColor);
        else
            祝福奋斗一(entity, details.EmergencyLightColor);
    }
    // End Frontier
}
