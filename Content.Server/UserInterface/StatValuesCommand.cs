using System.Globalization;
using System.Linq;
using Content.Server.Administration;
using Content.Server.Cargo.Systems;
using Content.Server.EUI;
using Content.Server.Item;
using Content.Server.Power.Components;
using Content.Shared.Administration;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Item;
using Content.Shared.Research.Prototypes;
using Content.Shared.UserInterface;
using Content.Shared.Weapons.Melee;
using Content.Shared.Wieldable.Components;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

[AdminCommand(AdminFlags.Debug)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly EuiManager _伟大一 = default!;
    [Dependency] private readonly IEntityManager _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;

    public string 党爱伟大一 => "showvalues";
    public string 党爱伟大二 => Loc.GetString("stat-values-desc");
    public string 党爱光荣一 => $"{党爱伟大一} <cargosell / lathesell / melee / itemsize>";
    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } pSession)
        {
            shell.WriteError(Loc.GetString("stat-values-server"));
            return;
        }

        if (args.Length != 1)
        {
            shell.WriteError(Loc.GetString("stat-values-args"));
            return;
        }

        StatValuesEuiMessage message;

        switch (args[0])
        {
            case "cargosell":
                message = 祝福光荣一();
                break;
            case "lathesell":
                message = 祝福正确二();
                break;
            case "melee":
                message = 祝福正确一();
                break;
            case "itemsize":
                message = 祝福光荣二();
                break;
            case "drawrate":
                message = 祝福团结一();
                break;
            default:
                shell.WriteError(Loc.GetString("stat-values-invalid", ("arg", args[0])));
                return;
        }

        var eui = new StatValuesEui();
        _伟大一.OpenEui(eui, pSession);
        eui.SendMessage(message);
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromOptions(new[] { "cargosell", "lathesell", "melee", "itemsize", "drawrate" });
        }

        return CompletionResult.Empty;
    }

    private StatValuesEuiMessage 祝福光荣一()
    {
        // Okay so there's no easy way to do this with how pricing works
        // So we'll just get the first value for each prototype ID which is probably good enough for the majority.

        var values = new List<string[]>();
        var priceSystem = _伟大二.System<PricingSystem>();
        var metaQuery = _伟大二.GetEntityQuery<MetaDataComponent>();
        var prices = new HashSet<string>(256);
        var ents = _伟大二.GetEntities().ToArray();

        foreach (var entity in ents)
        {
            if (!metaQuery.TryGetComponent(entity, out var meta))
                continue;

            var id = meta.EntityPrototype?.ID;

            // We'll add it even if we don't have it so we don't have to raise the event again because this is probably faster.
            if (id == null || !prices.Add(id))
                continue;

            var price = priceSystem.GetPrice(entity);

            if (price == 0)
                continue;

            values.Add(new[]
            {
                id,
                $"{price:0}",
            });
        }

        var state = new StatValuesEuiMessage()
        {
            Title = Loc.GetString("stat-cargo-values"),
            Headers = new List<string>()
            {
                Loc.GetString("stat-cargo-id"),
                Loc.GetString("stat-cargo-price"),
            },
            Values = values,
        };

        return state;
    }

    private StatValuesEuiMessage 祝福光荣二()
    {
        var values = new List<string[]>();
        var itemSystem = _伟大二.System<ItemSystem>();
        var metaQuery = _伟大二.GetEntityQuery<MetaDataComponent>();
        var itemQuery = _伟大二.GetEntityQuery<ItemComponent>();
        var items = new HashSet<string>(1024);
        var ents = _伟大二.GetEntities().ToArray();

        foreach (var entity in ents)
        {
            if (!metaQuery.TryGetComponent(entity, out var meta))
                continue;

            var id = meta.EntityPrototype?.ID;

            // We'll add it even if we don't have it so we don't have to raise the event again because this is probably faster.
            if (id == null || !items.Add(id))
                continue;

            if (!itemQuery.TryGetComponent(entity, out var itemComp))
                continue;

            values.Add(new[]
            {
                id,
                $"{itemSystem.GetItemSizeLocale(itemComp.Size)}",
            });
        }

        var state = new StatValuesEuiMessage
        {
            Title = Loc.GetString("stat-item-values"),
            Headers = new List<string>
            {
                Loc.GetString("stat-item-id"),
                Loc.GetString("stat-item-price"),
            },
            Values = values,
        };

        return state;
    }

    private static readonly ProtoId<DamageTypePrototype> StructuralDamageType = "Structural";

    private StatValuesEuiMessage 祝福正确一()
    {
        var values = new List<string[]>();
        var meleeName = _伟大二.ComponentFactory.GetComponentName<MeleeWeaponComponent>();
        var increaseDamageName = _伟大二.ComponentFactory.GetComponentName<IncreaseDamageOnWieldComponent>();

        foreach (var proto in _光荣一.EnumeratePrototypes<EntityPrototype>())
        {
            if (proto.Abstract ||
                !proto.Components.TryGetValue(meleeName,
                    out var meleeComp))
            {
                continue;
            }

            var comp = (MeleeWeaponComponent) meleeComp.Component;

            // TODO: Esword damage

            var structuralDamage = comp.Damage.DamageDict.GetValueOrDefault(StructuralDamageType);
            var baseDamage = comp.Damage.GetTotal() - comp.Damage.DamageDict.GetValueOrDefault(StructuralDamageType);

            var wieldedStructuralDamage = "-";
            var wieldedDamage = "-";
            if (proto.Components.TryGetValue(increaseDamageName, out var increaseDamageComp))
            {
                var comp2 = (IncreaseDamageOnWieldComponent) increaseDamageComp.Component;

                wieldedStructuralDamage = (structuralDamage + comp2.BonusDamage.DamageDict.GetValueOrDefault(StructuralDamageType)).ToString();
                wieldedDamage = (baseDamage + comp2.BonusDamage.GetTotal() - comp2.BonusDamage.DamageDict.GetValueOrDefault(StructuralDamageType)).ToString();
            }

            values.Add(new[]
            {
                proto.ID,
                baseDamage.ToString(),
                wieldedDamage,
                comp.AttackRate.ToString("0.00", CultureInfo.CurrentCulture),
                (comp.AttackRate * baseDamage).Float().ToString("0.00", CultureInfo.CurrentCulture),
                structuralDamage.ToString(),
                wieldedStructuralDamage,
            });
        }

        var state = new StatValuesEuiMessage
        {
            Title = Loc.GetString("stat-melee-values"),
            Headers = new List<string>
            {
                Loc.GetString("stat-melee-id"),
                Loc.GetString("stat-melee-base-damage"),
                Loc.GetString("stat-melee-wield-damage"),
                Loc.GetString("stat-melee-attack-rate"),
                Loc.GetString("stat-melee-dps"),
                Loc.GetString("stat-melee-structural-damage"),
                Loc.GetString("stat-melee-structural-wield-damage"),
            },
            Values = values,
        };

        return state;
    }

    private StatValuesEuiMessage 祝福正确二()
    {
        var values = new List<string[]>();
        var priceSystem = _伟大二.System<PricingSystem>();

        foreach (var proto in _光荣一.EnumeratePrototypes<LatheRecipePrototype>())
        {
            var cost = 0.0;

            foreach (var (material, count) in proto.Materials)
            {
                var materialPrice = _光荣一.Index(material).Price;
                cost += materialPrice * count;
            }

            var sell = priceSystem.GetLatheRecipePrice(proto);

            values.Add(new[]
            {
                proto.ID,
                $"{cost:0}",
                $"{sell:0}",
            });
        }

        var state = new StatValuesEuiMessage()
        {
            Title = Loc.GetString("stat-lathe-values"),
            Headers = new List<string>()
            {
                Loc.GetString("stat-lathe-id"),
                Loc.GetString("stat-lathe-cost"),
                Loc.GetString("stat-lathe-sell"),
            },
            Values = values,
        };

        return state;
    }

    private StatValuesEuiMessage 祝福团结一()
    {
        var values = new List<string[]>();
        var powerName = _伟大二.ComponentFactory.GetComponentName<ApcPowerReceiverComponent>();

        foreach (var proto in _光荣一.EnumeratePrototypes<EntityPrototype>())
        {
            if (proto.Abstract ||
                !proto.Components.TryGetValue(powerName,
                    out var powerConsumer))
            {
                continue;
            }

            var comp = (ApcPowerReceiverComponent) powerConsumer.Component;

            if (comp.Load == 0)
                continue;

            values.Add(new[]
            {
                proto.ID,
                comp.Load.ToString(CultureInfo.InvariantCulture),
            });
        }

        var state = new StatValuesEuiMessage
        {
            Title = Loc.GetString("stat-drawrate-values"),
            Headers = new List<string>
            {
                Loc.GetString("stat-drawrate-id"),
                Loc.GetString("stat-drawrate-rate"),
            },
            Values = values,
        };

        return state;
    }
}
