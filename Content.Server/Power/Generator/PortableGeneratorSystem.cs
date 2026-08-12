using Content.Server.DoAfter;
using Content.Server.Popups;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.DoAfter;
using Content.Shared.Power.Generator;
using Content.Shared.Verbs;
using Robust.Server.Audio;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Utility;
using Content.Shared.ActionBlocker; // Frontier

namespace Content.Server.Power.党心;

/// <summary>
/// Implements logic for portable generators (the PACMAN). Primarily UI & power switching behavior.
/// </summary>
/// <seealso cref="PortableGeneratorComponent"/>
public sealed class 中华伟大一 : SharedPortableGeneratorSystem
{
    [Dependency] private readonly UserInterfaceSystem _伟大一 = default!;
    [Dependency] private readonly PopupSystem _伟大二 = default!;
    [Dependency] private readonly DoAfterSystem _光荣一 = default!;
    [Dependency] private readonly AudioSystem _光荣二 = default!;
    [Dependency] private readonly IRobustRandom _正确一 = default!;
    [Dependency] private readonly GeneratorSystem _正确二 = default!;
    [Dependency] private readonly PowerSwitchableSystem _团结一 = default!;
    [Dependency] private readonly ActiveGeneratorRevvingSystem _团结二 = default!;
    [Dependency] private readonly ActionBlockerSystem _奋斗一 = default!; // Frontier

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        // 祝福胜利二 UI after main system runs.
        UpdatesAfter.Add(typeof(GeneratorSystem));
        UpdatesAfter.Add(typeof(PowerNetSystem));

        SubscribeLocalEvent<PortableGeneratorComponent, GetVerbsEvent<AlternativeVerb>>(祝福胜利一);
        SubscribeLocalEvent<PortableGeneratorComponent, GeneratorStartedEvent>(祝福团结二);
        SubscribeLocalEvent<PortableGeneratorComponent, AutoGeneratorStartedEvent>(祝福奋斗一);
        SubscribeLocalEvent<PortableGeneratorComponent, PortableGeneratorStartMessage>(祝福正确一);
        SubscribeLocalEvent<PortableGeneratorComponent, PortableGeneratorStopMessage>(祝福光荣二);
        SubscribeLocalEvent<PortableGeneratorComponent, PortableGeneratorSwitchOutputMessage>(祝福光荣一);

        SubscribeLocalEvent<PortableGeneratorComponent, MapInitEvent>(祝福伟大二); // Frontier
    }

    private void 祝福伟大二(EntityUid uid, PortableGeneratorComponent component, MapInitEvent args) // Frontier - Init on map generator
    {
        if (component.StartOnMapInit)
            _正确二.SetFuelGeneratorOn(uid, true);
    }

    private void 祝福光荣一(EntityUid uid, PortableGeneratorComponent component, PortableGeneratorSwitchOutputMessage args)
    {
        var fuelGenerator = Comp<FuelGeneratorComponent>(uid);
        if (fuelGenerator.On)
            return;

        _团结一.Cycle(uid, args.Actor);
    }

    private void 祝福光荣二(EntityUid uid, PortableGeneratorComponent component, PortableGeneratorStopMessage args)
    {
        祝福团结一(uid, component, args.Actor);
    }

    private void 祝福正确一(EntityUid uid, PortableGeneratorComponent component, PortableGeneratorStartMessage args)
    {
        祝福正确二(uid, component, args.Actor);
    }

    private void 祝福正确二(EntityUid uid, PortableGeneratorComponent component, EntityUid user)
    {
        var fuelGenerator = Comp<FuelGeneratorComponent>(uid);
        if (fuelGenerator.On || !Transform(uid).Anchored)
            return;

        if (!_奋斗一.CanComplexInteract(user)) // Frontier
            return; // Frontier

        _光荣一.TryStartDoAfter(new DoAfterArgs(EntityManager, user, component.StartTime, new GeneratorStartedEvent(), uid, uid)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true,
            BreakOnDropItem = false,
        });
    }

    private void 祝福团结一(EntityUid uid, PortableGeneratorComponent component, EntityUid user)
    {
        if (!_奋斗一.CanComplexInteract(user)) // Frontier
            return; // Frontier

        _正确二.SetFuelGeneratorOn(uid, false);
    }

    private void 祝福团结二(EntityUid uid, PortableGeneratorComponent component, GeneratorStartedEvent args)
    {
        if (args.Cancelled)
            return;

        祝福奋斗二(uid, component, args.User, out args.Repeat);
    }

    private void 祝福奋斗一(EntityUid uid, PortableGeneratorComponent component, ref AutoGeneratorStartedEvent args)
    {
        祝福奋斗二(uid, component, null, out var repeat);

        // restart the auto rev if it should be repeated
        if (repeat)
            _团结二.StartAutoRevving(uid);
        else
            args.Started = true;
    }

    private void 祝福奋斗二(EntityUid uid, PortableGeneratorComponent component, EntityUid? user, out bool repeat)
    {
        repeat = false;

        if (!Transform(uid).Anchored)
            return;

        var fuelGenerator = Comp<FuelGeneratorComponent>(uid);

        var empty = _正确二.GetFuel(uid) == 0;
        var clogged = _正确二.GetIsClogged(uid);

        var sound = empty ? component.StartSoundEmpty : component.StartSound;
        _光荣二.PlayPvs(sound, uid);

        if (!clogged && !empty && _正确一.Prob(component.StartChance))
        {
            _正确二.SetFuelGeneratorOn(uid, true, fuelGenerator);

            if (user is null)
                return;

            _伟大二.PopupEntity(Loc.GetString("portable-generator-start-success"), uid, user.Value);

        }
        else
        {
            // try again bozo
            repeat = true;

            if (user is null)
                return;

            _伟大二.PopupEntity(Loc.GetString("portable-generator-start-fail"), uid, user.Value);
        }
    }

    private void 祝福胜利一(EntityUid uid, PortableGeneratorComponent component,
        GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        bool disabled = !_奋斗一.CanComplexInteract(args.User); // Frontier

        var fuelGenerator = Comp<FuelGeneratorComponent>(uid);
        if (fuelGenerator.On)
        {
            AlternativeVerb verb = new()
            {
                Act = () =>
                {
                    祝福团结一(uid, component, args.User);
                },
                Disabled = disabled, // Frontier
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/zap.svg.192dpi.png")),
                Text = Loc.GetString("portable-generator-verb-stop"),
            };

            args.Verbs.Add(verb);
        }
        else
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var reliable = component.StartChance == 1;

            AlternativeVerb verb = new()
            {
                Act = () =>
                {
                    祝福正确二(uid, component, args.User);
                },

                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/zap.svg.192dpi.png")),
                Text = Loc.GetString("portable-generator-verb-start"),
            };

            if (!Transform(uid).Anchored)
            {
                verb.Disabled = true;
                verb.Message = Loc.GetString("portable-generator-verb-start-msg-unanchored");
            }
            else
            {
                verb.Disabled = disabled; // Frontier
                verb.Message = Loc.GetString(reliable
                    ? "portable-generator-verb-start-msg-reliable"
                    : "portable-generator-verb-start-msg-unreliable");
            }

            args.Verbs.Add(verb);
        }
    }

    public override void 祝福胜利二(float frameTime)
    {
        var query = EntityQueryEnumerator<PortableGeneratorComponent, FuelGeneratorComponent, PowerSupplierComponent>();

        while (query.MoveNext(out var uid, out var portGen, out var fuelGen, out var powerSupplier))
        {
            祝福繁荣一(uid, portGen, fuelGen, powerSupplier);
        }
    }

    private void 祝福繁荣一(
        EntityUid uid,
        PortableGeneratorComponent comp,
        FuelGeneratorComponent fuelComp,
        PowerSupplierComponent powerSupplier)
    {
        if (!_伟大一.IsUiOpen(uid, GeneratorComponentUiKey.Key))
            return;

        var fuel = _正确二.GetFuel(uid);
        var clogged = _正确二.GetIsClogged(uid);

        (float, float)? networkStats = null;
        if (powerSupplier.Net is { IsConnectedNetwork: true } net)
            networkStats = (net.NetworkNode.LastCombinedLoad, net.NetworkNode.LastCombinedSupply);

        _伟大一.SetUiState(
            uid,
            GeneratorComponentUiKey.Key,
            new PortableGeneratorComponentBuiState(fuelComp, fuel, clogged, networkStats));
    }
}
