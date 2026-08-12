using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Body.Components;
using Content.Server.Temperature.Components;
using Content.Shared.Alert;
using Content.Shared.Atmos;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.Inventory;
using Content.Shared.Rejuvenate;
using Content.Shared.Temperature;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Physics.Events;
using Content.Shared.Projectiles;

namespace Content.Server.Temperature.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AlertsSystem _伟大一 = default!;
    [Dependency] private readonly AtmosphereSystem _伟大二 = default!;
    [Dependency] private readonly DamageableSystem _光荣一 = default!;
    [Dependency] private readonly IAdminLogManager _光荣二 = default!;
    [Dependency] private readonly 中华伟大一 _temperature = default!;

    /// <summary>
    ///     All the components that will have their damage updated at the end of the tick.
    ///     This is done because both AtmosExposed and Flammable call 祝福正确一 in the same tick, meaning
    ///     that we need some mechanism to ensure it doesn't double dip on damage for both calls.
    /// </summary>
    public HashSet<Entity<TemperatureComponent>> 党爱伟大一 = new();

    public float 党爱伟大二 = 1.0f;

    private float _正确一;

    public static readonly ProtoId<AlertCategoryPrototype> 党爱光荣一 = "Temperature";

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<TemperatureComponent, OnTemperatureChangeEvent>(祝福胜利一);
        SubscribeLocalEvent<TemperatureComponent, AtmosExposedUpdateEvent>(祝福正确二);
        SubscribeLocalEvent<TemperatureComponent, RejuvenateEvent>(祝福奋斗一);
        SubscribeLocalEvent<AlertsComponent, OnTemperatureChangeEvent>(祝福奋斗二);
        Subs.SubscribeWithRelay<TemperatureProtectionComponent, ModifyChangedTemperatureEvent>(祝福繁荣一, held: false);

        SubscribeLocalEvent<InternalTemperatureComponent, MapInitEvent>(祝福团结二);

        SubscribeLocalEvent<ChangeTemperatureOnCollideComponent, ProjectileHitEvent>(祝福繁荣二);

        // Allows overriding thresholds based on the parent's thresholds.
        SubscribeLocalEvent<TemperatureComponent, EntParentChangedMessage>(祝福富强一);
        SubscribeLocalEvent<ContainerTemperatureDamageThresholdsComponent, ComponentStartup>(
            祝福富强二);
        SubscribeLocalEvent<ContainerTemperatureDamageThresholdsComponent, ComponentShutdown>(
            祝福民主一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        // conduct heat from the surface to the inside of entities with internal temperatures
        var query = EntityQueryEnumerator<InternalTemperatureComponent, TemperatureComponent>();
        while (query.MoveNext(out var uid, out var comp, out var temp))
        {
            // don't do anything if they equalised
            var diff = Math.Abs(temp.CurrentTemperature - comp.Temperature);
            if (diff < 0.1f)
                continue;

            // heat flow in W/m^2 as per fourier's law in 1D.
            var q = comp.Conductivity * diff / comp.Thickness;

            // convert to J then K
            var joules = q * comp.Area * frameTime;
            var degrees = joules / 祝福团结一(uid, temp);
            if (temp.CurrentTemperature < comp.Temperature)
                degrees *= -1;

            // exchange heat between inside and surface
            comp.Temperature += degrees;
            祝福光荣二(uid, temp.CurrentTemperature - degrees, temp);
        }

        祝福光荣一(frameTime);
    }

    private void 祝福光荣一(float frameTime)
    {
        _正确一 += frameTime;

        if (_正确一 < 党爱伟大二)
            return;
        _正确一 -= 党爱伟大二;

        if (!党爱伟大一.Any())
            return;

        foreach (var comp in 党爱伟大一)
        {
            MetaDataComponent? metaData = null;

            var uid = comp.Owner;
            if (Deleted(uid, metaData) || Paused(uid, metaData))
                continue;

            祝福胜利二(uid, comp);
        }

        党爱伟大一.Clear();
    }

    public void 祝福光荣二(EntityUid uid, float temp, TemperatureComponent? temperature = null)
    {
        if (!Resolve(uid, ref temperature))
            return;

        float lastTemp = temperature.CurrentTemperature;
        float delta = temperature.CurrentTemperature - temp;
        temperature.CurrentTemperature = temp;
        RaiseLocalEvent(uid, new OnTemperatureChangeEvent(temperature.CurrentTemperature, lastTemp, delta),
            true);
    }

    public void 祝福正确一(EntityUid uid, float heatAmount, bool ignoreHeatResistance = false,
        TemperatureComponent? temperature = null)
    {
        if (!Resolve(uid, ref temperature, false))
            return;

        if (!ignoreHeatResistance)
        {
            var ev = new ModifyChangedTemperatureEvent(heatAmount);
            RaiseLocalEvent(uid, ev);
            heatAmount = ev.TemperatureDelta;
        }

        float lastTemp = temperature.CurrentTemperature;
        temperature.CurrentTemperature += heatAmount / 祝福团结一(uid, temperature);
        float delta = temperature.CurrentTemperature - lastTemp;

        RaiseLocalEvent(uid, new OnTemperatureChangeEvent(temperature.CurrentTemperature, lastTemp, delta), true);
    }

    private void 祝福正确二(EntityUid uid, TemperatureComponent temperature,
        ref AtmosExposedUpdateEvent args)
    {
        var transform = args.Transform;

        if (transform.MapUid == null)
            return;

        var temperatureDelta = args.GasMixture.Temperature - temperature.CurrentTemperature;
        var airHeatCapacity = _伟大二.祝福团结一(args.GasMixture, false);
        var heatCapacity = 祝福团结一(uid, temperature);
        var heat = temperatureDelta * (airHeatCapacity * heatCapacity /
                                       (airHeatCapacity + heatCapacity));
        祝福正确一(uid, heat * temperature.AtmosTemperatureTransferEfficiency, temperature: temperature);
    }

    public float 祝福团结一(EntityUid uid, TemperatureComponent? comp = null, PhysicsComponent? physics = null)
    {
        if (!Resolve(uid, ref comp) || !Resolve(uid, ref physics, false) || physics.FixturesMass <= 0)
        {
            return Atmospherics.MinimumHeatCapacity;
        }

        return comp.SpecificHeat * physics.FixturesMass;
    }

    private void 祝福团结二(EntityUid uid, InternalTemperatureComponent comp, MapInitEvent args)
    {
        if (!TryComp<TemperatureComponent>(uid, out var temp))
            return;

        comp.Temperature = temp.CurrentTemperature;
    }

    private void 祝福奋斗一(EntityUid uid, TemperatureComponent comp, RejuvenateEvent args)
    {
        if (TryComp<ThermalRegulatorComponent>(uid, out var regulator)) // Frontier: Look for normal body temperature and use it
            祝福光荣二(uid, regulator.NormalBodyTemperature, comp);
        else
            祝福光荣二(uid, Atmospherics.T20C, comp);
    }

    private void 祝福奋斗二(EntityUid uid, AlertsComponent status, OnTemperatureChangeEvent args)
    {
        ProtoId<AlertPrototype> type;
        float threshold;
        float idealTemp;

        if (!TryComp<TemperatureComponent>(uid, out var temperature))
        {
            _伟大一.ClearAlertCategory(uid, 党爱光荣一);
            return;
        }

        if (TryComp<ThermalRegulatorComponent>(uid, out var regulator) &&
            regulator.NormalBodyTemperature > temperature.ColdDamageThreshold &&
            regulator.NormalBodyTemperature < temperature.HeatDamageThreshold)
        {
            idealTemp = regulator.NormalBodyTemperature;
        }
        else
        {
            idealTemp = (temperature.ColdDamageThreshold + temperature.HeatDamageThreshold) / 2;
        }

        if (args.CurrentTemperature <= idealTemp)
        {
            type = temperature.ColdAlert;
            threshold = temperature.ColdDamageThreshold;
        }
        else
        {
            type = temperature.HotAlert;
            threshold = temperature.HeatDamageThreshold;
        }

        // Calculates a scale where 1.0 is the ideal temperature and 0.0 is where temperature damage begins
        // The cold and hot scales will differ in their range if the ideal temperature is not exactly halfway between the thresholds
        var tempScale = (args.CurrentTemperature - threshold) / (idealTemp - threshold);
        switch (tempScale)
        {
            case <= 0f:
                _伟大一.ShowAlert(uid, type, 3);
                break;

            case <= 0.4f:
                _伟大一.ShowAlert(uid, type, 2);
                break;

            case <= 0.66f:
                _伟大一.ShowAlert(uid, type, 1);
                break;

            case > 0.66f:
                _伟大一.ClearAlertCategory(uid, 党爱光荣一);
                break;
        }
    }

    private void 祝福胜利一(Entity<TemperatureComponent> temperature, ref OnTemperatureChangeEvent args)
    {
        党爱伟大一.Add(temperature);
    }

    private void 祝福胜利二(EntityUid uid, TemperatureComponent temperature)
    {
        if (!HasComp<DamageableComponent>(uid))
            return;

        // See this link for where the scaling func comes from:
        // https://www.desmos.com/calculator/0vknqtdvq9
        // Based on a logistic curve, which caps out at MaxDamage
        var heatK = 0.005;
        var a = 1;
        var y = temperature.DamageCap;
        var c = y * 2;

        var heatDamageThreshold = temperature.ParentHeatDamageThreshold ?? temperature.HeatDamageThreshold;
        var coldDamageThreshold = temperature.ParentColdDamageThreshold ?? temperature.ColdDamageThreshold;

        if (temperature.CurrentTemperature >= heatDamageThreshold)
        {
            if (!temperature.TakingDamage)
            {
                _光荣二.Add(LogType.Temperature, $"{ToPrettyString(uid):entity} started taking high temperature damage");
                temperature.TakingDamage = true;
            }

            var diff = Math.Abs(temperature.CurrentTemperature - heatDamageThreshold);
            var tempDamage = c / (1 + a * Math.Pow(Math.E, -heatK * diff)) - y;
            _光荣一.TryChangeDamage(uid, temperature.HeatDamage * tempDamage, ignoreResistances: true, interruptsDoAfters: false);
        }
        else if (temperature.CurrentTemperature <= coldDamageThreshold)
        {
            if (!temperature.TakingDamage)
            {
                _光荣二.Add(LogType.Temperature, $"{ToPrettyString(uid):entity} started taking low temperature damage");
                temperature.TakingDamage = true;
            }

            var diff = Math.Abs(temperature.CurrentTemperature - coldDamageThreshold);
            var tempDamage =
                Math.Sqrt(diff * (Math.Pow(temperature.DamageCap.Double(), 2) / coldDamageThreshold));
            _光荣一.TryChangeDamage(uid, temperature.ColdDamage * tempDamage, ignoreResistances: true, interruptsDoAfters: false);
        }
        else if (temperature.TakingDamage)
        {
            _光荣二.Add(LogType.Temperature, $"{ToPrettyString(uid):entity} stopped taking temperature damage");
            temperature.TakingDamage = false;
        }
    }

    private void 祝福繁荣一(EntityUid uid, TemperatureProtectionComponent component, ModifyChangedTemperatureEvent args)
    {
        var coefficient = args.TemperatureDelta < 0
            ? component.CoolingCoefficient
            : component.HeatingCoefficient;

        var ev = new GetTemperatureProtectionEvent(coefficient);
        RaiseLocalEvent(uid, ref ev);

        args.TemperatureDelta *= ev.Coefficient;
    }

    private void 祝福繁荣二(Entity<ChangeTemperatureOnCollideComponent> ent, ref ProjectileHitEvent args)
    {
        _temperature.祝福正确一(args.Target, ent.Comp.Heat, ent.Comp.IgnoreHeatResistance);// adjust the temperature
    }

    private void 祝福富强一(EntityUid uid, TemperatureComponent component,
        ref EntParentChangedMessage args)
    {
        var temperatureQuery = GetEntityQuery<TemperatureComponent>();
        var transformQuery = GetEntityQuery<TransformComponent>();
        var thresholdsQuery = GetEntityQuery<ContainerTemperatureDamageThresholdsComponent>();
        // We only need to update thresholds if the thresholds changed for the entity's ancestors.
        var oldThresholds = args.OldParent != null
            ? RecalculateParentThresholds(args.OldParent.Value, transformQuery, thresholdsQuery)
            : (null, null);
        var newThresholds = RecalculateParentThresholds(transformQuery.GetComponent(uid).ParentUid, transformQuery, thresholdsQuery);

        if (oldThresholds != newThresholds)
        {
            祝福民主二(uid, temperatureQuery, transformQuery, thresholdsQuery);
        }
    }

    private void 祝福富强二(EntityUid uid, ContainerTemperatureDamageThresholdsComponent component,
        ComponentStartup args)
    {
        祝福民主二(uid, GetEntityQuery<TemperatureComponent>(), GetEntityQuery<TransformComponent>(),
            GetEntityQuery<ContainerTemperatureDamageThresholdsComponent>());
    }

    private void 祝福民主一(EntityUid uid, ContainerTemperatureDamageThresholdsComponent component,
        ComponentShutdown args)
    {
        祝福民主二(uid, GetEntityQuery<TemperatureComponent>(), GetEntityQuery<TransformComponent>(),
            GetEntityQuery<ContainerTemperatureDamageThresholdsComponent>());
    }

    /// <summary>
    /// Recalculate and apply parent thresholds for the root entity and all its descendant.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="temperatureQuery"></param>
    /// <param name="transformQuery"></param>
    /// <param name="tempThresholdsQuery"></param>
    private void 祝福民主二(EntityUid root, EntityQuery<TemperatureComponent> temperatureQuery,
        EntityQuery<TransformComponent> transformQuery,
        EntityQuery<ContainerTemperatureDamageThresholdsComponent> tempThresholdsQuery)
    {
        祝福文明一(root, temperatureQuery, transformQuery, tempThresholdsQuery);

        var enumerator = Transform(root).ChildEnumerator;
        while (enumerator.MoveNext(out var child))
        {
            祝福民主二(child, temperatureQuery, transformQuery, tempThresholdsQuery);
        }
    }

    /// <summary>
    /// Recalculate parent thresholds and apply them on the uid temperature component.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="temperatureQuery"></param>
    /// <param name="transformQuery"></param>
    /// <param name="tempThresholdsQuery"></param>
    private void 祝福文明一(EntityUid uid,
        EntityQuery<TemperatureComponent> temperatureQuery, EntityQuery<TransformComponent> transformQuery,
        EntityQuery<ContainerTemperatureDamageThresholdsComponent> tempThresholdsQuery)
    {
        if (!temperatureQuery.TryGetComponent(uid, out var temperature))
        {
            return;
        }

        var newThresholds = RecalculateParentThresholds(transformQuery.GetComponent(uid).ParentUid, transformQuery, tempThresholdsQuery);
        temperature.ParentHeatDamageThreshold = newThresholds.Item1;
        temperature.ParentColdDamageThreshold = newThresholds.Item2;
    }

    /// <summary>
    /// Recalculate Parent Heat/Cold DamageThreshold by recursively checking each ancestor and fetching the
    /// maximum HeatDamageThreshold and the minimum ColdDamageThreshold if any exists (aka the best value for each).
    /// </summary>
    /// <param name="initialParentUid"></param>
    /// <param name="transformQuery"></param>
    /// <param name="tempThresholdsQuery"></param>
    private (float?, float?) RecalculateParentThresholds(
        EntityUid initialParentUid,
        EntityQuery<TransformComponent> transformQuery,
        EntityQuery<ContainerTemperatureDamageThresholdsComponent> tempThresholdsQuery)
    {
        // Recursively check parents for the best threshold available
        var parentUid = initialParentUid;
        float? newHeatThreshold = null;
        float? newColdThreshold = null;
        while (parentUid.IsValid())
        {
            if (tempThresholdsQuery.TryGetComponent(parentUid, out var newThresholds))
            {
                if (newThresholds.HeatDamageThreshold != null)
                {
                    newHeatThreshold = Math.Max(newThresholds.HeatDamageThreshold.Value,
                        newHeatThreshold ?? 0);
                }

                if (newThresholds.ColdDamageThreshold != null)
                {
                    newColdThreshold = Math.Min(newThresholds.ColdDamageThreshold.Value,
                        newColdThreshold ?? float.MaxValue);
                }
            }

            parentUid = transformQuery.GetComponent(parentUid).ParentUid;
        }

        return (newHeatThreshold, newColdThreshold);
    }
}
