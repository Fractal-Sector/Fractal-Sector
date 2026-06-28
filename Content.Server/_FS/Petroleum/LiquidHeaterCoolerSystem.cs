using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Content.Shared._FS.Petroleum;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Power.EntitySystems;
using Content.Shared._Starlight.Plumbing;
using Content.Shared.Verbs;
using Content.Shared.Popups;
using Content.Shared.Chemistry.Components;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using System;

namespace Content.Server._FS.Petroleum;

public sealed class LiquidHeaterCoolerSystem : EntitySystem
{
    [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly PowerReceiverSystem _powerReceiver = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<LiquidHeaterCoolerComponent, GetVerbsEvent<AlternativeVerb>>(AddHeaterCoolerVerbs);
    }

    private void AddHeaterCoolerVerbs(EntityUid uid, LiquidHeaterCoolerComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract) return;

        VerbCategory modeCategory = new("Режим работы", "/Textures/Interface/VerbIcons/gear.png");

        AlternativeVerb turnOff = new()
        {
            Text = "Выключить",
            Category = modeCategory,
            Disabled = component.CurrentMode == HeaterCoolerMode.Off,
            Act = () =>
            {
                component.CurrentMode = HeaterCoolerMode.Off;
                _popup.PopupEntity("Бойлер выключен (режим транзита)", uid, args.User);
            }
        };
        args.Verbs.Add(turnOff);

        AlternativeVerb setHeat = new()
        {
            Text = "Нагрев",
            Category = modeCategory,
            Disabled = component.CurrentMode == HeaterCoolerMode.Heat,
            Act = () =>
            {
                component.CurrentMode = HeaterCoolerMode.Heat;
                _popup.PopupEntity("Бойлер переведен в режим НАГРЕВА", uid, args.User);
            }
        };
        args.Verbs.Add(setHeat);

        AlternativeVerb setCool = new()
        {
            Text = "Охлаждение",
            Category = modeCategory,
            Disabled = component.CurrentMode == HeaterCoolerMode.Cool,
            Act = () =>
            {
                component.CurrentMode = HeaterCoolerMode.Cool;
                _popup.PopupEntity("Бойлер переведен в режим ОХЛАЖДЕНИЯ", uid, args.User);
            }
        };
        args.Verbs.Add(setCool);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = AllEntityQuery<LiquidHeaterCoolerComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var machine, out var xform))
        {
            bool isPowered = _powerReceiver.IsPowered(uid);
            bool isProcessing = false;

            if (machine.CurrentMode == HeaterCoolerMode.Off)
            {
                _appearance.SetData(uid, PlumbingVisuals.Running, false);
                continue;
            }

            if (_solutionContainer.TryGetSolution(uid, machine.SolutionId, out var solutionHolder, out var solution))
            {
                if (solution.Volume > FixedPoint2.Zero && isPowered)
                {
                    isProcessing = true;

                    float temperatureChange = 50f * frameTime;

                    Entity<SolutionComponent> validHolder = (solutionHolder.Value.Owner, solutionHolder.Value.Comp);

                    if (machine.CurrentMode == HeaterCoolerMode.Heat)
                    {
                        if (solution.Temperature < 1000f)
                        {
                            _solutionContainer.SetTemperature(validHolder, solution.Temperature + temperatureChange);
                        }
                    }
                    else if (machine.CurrentMode == HeaterCoolerMode.Cool)
                    {
                        if (solution.Temperature > 100f)
                        {
                            _solutionContainer.SetTemperature(validHolder, solution.Temperature - temperatureChange);
                        }
                    }
                }
            }

            _appearance.SetData(uid, PlumbingVisuals.Running, isProcessing);
        }
    }
}
