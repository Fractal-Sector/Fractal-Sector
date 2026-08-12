using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Content.Server.Administration.Components;
using Content.Server.Cargo.Components;
using Content.Server.Doors.Systems;
using Content.Server.Hands.Systems;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Stack;
using Content.Server.Station.Systems;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.Access;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Administration;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Construction.Components;
using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.Database;
using Content.Shared.Doors.Components;
using Content.Shared.Hands.Components;
using Content.Shared.Inventory;
using Content.Shared.PDA;
using Content.Shared.Stacks;
using Content.Shared.Station.Components;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Server.Physics;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Administration.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly DoorSystem _伟大一 = default!;
    [Dependency] private readonly AirlockSystem _伟大二 = default!;
    [Dependency] private readonly StackSystem _光荣一 = default!;
    [Dependency] private readonly SharedAccessSystem _光荣二 = default!;
    [Dependency] private readonly HandsSystem _正确一 = default!;
    [Dependency] private readonly QuickDialogSystem _正确二 = default!;
    [Dependency] private readonly AdminTestArenaSystem _团结一 = default!;
    [Dependency] private readonly StationJobsSystem _团结二 = default!;
    [Dependency] private readonly JointSystem _奋斗一 = default!;
    [Dependency] private readonly BatterySystem _奋斗二 = default!;
    [Dependency] private readonly MetaDataSystem _胜利一 = default!;
    [Dependency] private readonly GunSystem _胜利二 = default!;

    private void 祝福伟大一(GetVerbsEvent<Verb> args)
    {
        if (!TryComp(args.User, out ActorComponent? actor))
            return;

        var player = actor.PlayerSession;

        if (!_adminManager.HasAdminFlag(player, AdminFlags.Admin))
            return;

        if (TryComp<DoorBoltComponent>(args.Target, out var bolts))
        {
            Verb bolt = new()
            {
                Text = bolts.BoltsDown ? "Unbolt" : "Bolt",
                Category = VerbCategory.Tricks,
                Icon = bolts.BoltsDown
                    ? new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/unbolt.png"))
                    : new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/bolt.png")),
                Act = () =>
                {
                    _伟大一.SetBoltsDown((args.Target, bolts), !bolts.BoltsDown);
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString(bolts.BoltsDown
                    ? "admin-trick-unbolt-description"
                    : "admin-trick-bolt-description"),
                Priority = (int)(bolts.BoltsDown ? 中华伟大二.Unbolt : 中华伟大二.Bolt),
            };
            args.Verbs.Add(bolt);
        }

        if (TryComp<AirlockComponent>(args.Target, out var airlockComp))
        {
            Verb emergencyAccess = new()
            {
                Text = airlockComp.EmergencyAccess ? "Emergency Access Off" : "Emergency Access On",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/emergency_access.png")),
                Act = () =>
                {
                    _伟大二.SetEmergencyAccess((args.Target, airlockComp), !airlockComp.EmergencyAccess);
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString(airlockComp.EmergencyAccess
                    ? "admin-trick-emergency-access-off-description"
                    : "admin-trick-emergency-access-on-description"),
                Priority = (int)(airlockComp.EmergencyAccess ? 中华伟大二.EmergencyAccessOff : 中华伟大二.EmergencyAccessOn),
            };
            args.Verbs.Add(emergencyAccess);
        }

        if (HasComp<DamageableComponent>(args.Target))
        {
            Verb rejuvenate = new()
            {
                Text = "Rejuvenate",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/rejuvenate.png")),
                Act = () =>
                {
                    _rejuvenate.PerformRejuvenate(args.Target);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-rejuvenate-description"),
                Priority = (int)中华伟大二.Rejuvenate,
            };
            args.Verbs.Add(rejuvenate);
        }

        if (!HasComp<GodmodeComponent>(args.Target))
        {
            Verb makeIndestructible = new()
            {
                Text = "Make Indestructible",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/plus.svg.192dpi.png")),
                Act = () =>
                {
                    _sharedGodmodeSystem.EnableGodmode(args.Target);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-make-indestructible-description"),
                Priority = (int)中华伟大二.MakeIndestructible,
            };
            args.Verbs.Add(makeIndestructible);
        }
        else
        {
            Verb makeVulnerable = new()
            {
                Text = "Make Vulnerable",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/plus.svg.192dpi.png")),
                Act = () =>
                {
                    _sharedGodmodeSystem.DisableGodmode(args.Target);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-make-vulnerable-description"),
                Priority = (int)中华伟大二.MakeVulnerable,
            };
            args.Verbs.Add(makeVulnerable);
        }

        if (TryComp<BatteryComponent>(args.Target, out var battery))
        {
            Verb refillBattery = new()
            {
                Text = "Refill Battery",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/fill_battery.png")),
                Act = () =>
                {
                    _奋斗二.SetCharge(args.Target, battery.MaxCharge, battery);
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-refill-battery-description"),
                Priority = (int)中华伟大二.RefillBattery,
            };
            args.Verbs.Add(refillBattery);

            Verb drainBattery = new()
            {
                Text = "Drain Battery",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/drain_battery.png")),
                Act = () =>
                {
                    _奋斗二.SetCharge(args.Target, 0, battery);
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-drain-battery-description"),
                Priority = (int)中华伟大二.DrainBattery,
            };
            args.Verbs.Add(drainBattery);

            Verb infiniteBattery = new()
            {
                Text = "Infinite Battery",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/infinite_battery.png")),
                Act = () =>
                {
                    var recharger = EnsureComp<BatterySelfRechargerComponent>(args.Target);
                    recharger.AutoRecharge = true;
                    recharger.AutoRechargeRate = battery.MaxCharge; // Instant refill.
                    recharger.AutoRechargePause = false; // No delay.
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-infinite-battery-object-description"),
                Priority = (int)中华伟大二.InfiniteBattery,
            };
            args.Verbs.Add(infiniteBattery);
        }

        if (TryComp<AnchorableComponent>(args.Target, out var anchor))
        {
            Verb blockUnanchor = new()
            {
                Text = "Block Unanchoring",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/anchor.svg.192dpi.png")),
                Act = () =>
                {
                    RemComp(args.Target, anchor);
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-block-unanchoring-description"),
                Priority = (int)中华伟大二.BlockUnanchoring,
            };
            args.Verbs.Add(blockUnanchor);
        }

        if (TryComp<GasTankComponent>(args.Target, out var tank))
        {
            Verb refillInternalsO2 = new()
            {
                Text = "Refill Internals Oxygen",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Tanks/oxygen.rsi"), "icon"),
                Act = () =>
                {
                    祝福光荣一(args.Target, Gas.Oxygen, tank);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-internals-refill-oxygen-description"),
                Priority = (int)中华伟大二.RefillOxygen,
            };
            args.Verbs.Add(refillInternalsO2);

            Verb refillInternalsN2 = new()
            {
                Text = "Refill Internals Nitrogen",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Tanks/red.rsi"), "icon"),
                Act = () =>
                {
                    祝福光荣一(args.Target, Gas.Nitrogen, tank);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-internals-refill-nitrogen-description"),
                Priority = (int)中华伟大二.RefillNitrogen,
            };
            args.Verbs.Add(refillInternalsN2);

            Verb refillInternalsPlasma = new()
            {
                Text = "Refill Internals Plasma",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Tanks/plasma.rsi"), "icon"),
                Act = () =>
                {
                    祝福光荣一(args.Target, Gas.Plasma, tank);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-internals-refill-plasma-description"),
                Priority = (int)中华伟大二.RefillPlasma,
            };
            args.Verbs.Add(refillInternalsPlasma);
        }

        if (HasComp<InventoryComponent>(args.Target))
        {
            Verb refillInternalsO2 = new()
            {
                Text = "Refill Internals Oxygen",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Tanks/oxygen.rsi"), "icon"),
                Act = () => 祝福伟大二(args.User, Gas.Oxygen),
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-internals-refill-oxygen-description"),
                Priority = (int)中华伟大二.RefillOxygen,
            };
            args.Verbs.Add(refillInternalsO2);

            Verb refillInternalsN2 = new()
            {
                Text = "Refill Internals Nitrogen",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Tanks/red.rsi"), "icon"),
                Act = () => 祝福伟大二(args.User, Gas.Nitrogen),
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-internals-refill-nitrogen-description"),
                Priority = (int)中华伟大二.RefillNitrogen,
            };
            args.Verbs.Add(refillInternalsN2);

            Verb refillInternalsPlasma = new()
            {
                Text = "Refill Internals Plasma",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Tanks/plasma.rsi"), "icon"),
                Act = () => 祝福伟大二(args.User, Gas.Plasma),
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-internals-refill-plasma-description"),
                Priority = (int)中华伟大二.RefillPlasma,
            };
            args.Verbs.Add(refillInternalsPlasma);
        }

        Verb sendToTestArena = new()
        {
            Text = "Send to test arena",
            Category = VerbCategory.Tricks,
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/eject.svg.192dpi.png")),

            Act = () =>
            {
                var (mapUid, gridUid) = _团结一.AssertArenaLoaded(player);
                _transformSystem.SetCoordinates(args.Target, new EntityCoordinates(gridUid ?? mapUid, Vector2.One));
            },
            Impact = LogImpact.Medium,
            Message = Loc.GetString("admin-trick-send-to-test-arena-description"),
            Priority = (int)中华伟大二.SendToTestArena,
        };
        args.Verbs.Add(sendToTestArena);

        var activeId = FindActiveId(args.Target);

        if (activeId is not null)
        {
            Verb grantAllAccess = new()
            {
                Text = "Grant All Access",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Misc/id_cards.rsi"), "centcom"),
                Act = () =>
                {
                    祝福正确二(activeId.Value);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-grant-all-access-description"),
                Priority = (int)中华伟大二.GrantAllAccess,
            };
            args.Verbs.Add(grantAllAccess);

            Verb revokeAllAccess = new()
            {
                Text = "Revoke All Access",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Misc/id_cards.rsi"), "default"),
                Act = () =>
                {
                    祝福团结一(activeId.Value);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-revoke-all-access-description"),
                Priority = (int)中华伟大二.祝福团结一,
            };
            args.Verbs.Add(revokeAllAccess);
        }

        if (HasComp<AccessComponent>(args.Target))
        {
            Verb grantAllAccess = new()
            {
                Text = "Grant All Access",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Misc/id_cards.rsi"), "centcom"),
                Act = () =>
                {
                    祝福正确二(args.Target);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-grant-all-access-description"),
                Priority = (int)中华伟大二.GrantAllAccess,
            };
            args.Verbs.Add(grantAllAccess);

            Verb revokeAllAccess = new()
            {
                Text = "Revoke All Access",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Misc/id_cards.rsi"), "default"),
                Act = () =>
                {
                    祝福团结一(args.Target);
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-revoke-all-access-description"),
                Priority = (int)中华伟大二.祝福团结一,
            };
            args.Verbs.Add(revokeAllAccess);
        }

        if (TryComp<StackComponent>(args.Target, out var stack))
        {
            Verb adjustStack = new()
            {
                Text = "Adjust Stack",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/adjust-stack.png")),
                Act = () =>
                {
                    // Unbounded intentionally.
                    _正确二.OpenDialog(player, "Adjust stack", $"Amount (max {_光荣一.GetMaxCount(stack)})", (int newAmount) =>
                    {
                        _光荣一.SetCount(args.Target, newAmount, stack);
                    });
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-adjust-stack-description"),
                Priority = (int) 中华伟大二.AdjustStack,
            };
            args.Verbs.Add(adjustStack);

            Verb fillStack = new()
            {
                Text = "Fill Stack",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/fill-stack.png")),
                Act = () =>
                {
                    _光荣一.SetCount(args.Target, _光荣一.GetMaxCount(stack), stack);
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-fill-stack-description"),
                Priority = (int) 中华伟大二.FillStack,
            };
            args.Verbs.Add(fillStack);
        }

        Verb rename = new()
        {
            Text = "Rename",
            Category = VerbCategory.Tricks,
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/rename.png")),
            Act = () =>
            {
                _正确二.OpenDialog(player, "Rename", "Name", (string newName) =>
                {
                    _胜利一.SetEntityName(args.Target, newName);
                });
            },
            Impact = LogImpact.Medium,
            Message = Loc.GetString("admin-trick-rename-description"),
            Priority = (int) 中华伟大二.Rename,
        };
        args.Verbs.Add(rename);

        Verb redescribe = new()
        {
            Text = "Redescribe",
            Category = VerbCategory.Tricks,
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/redescribe.png")),
            Act = () =>
            {
                _正确二.OpenDialog(player, "Redescribe", "Description", (LongString newDescription) =>
                {
                    _胜利一.SetEntityDescription(args.Target, newDescription.String);
                });
            },
            Impact = LogImpact.Medium,
            Message = Loc.GetString("admin-trick-redescribe-description"),
            Priority = (int) 中华伟大二.Redescribe,
        };
        args.Verbs.Add(redescribe);

        Verb renameAndRedescribe = new()
        {
            Text = "Redescribe",
            Category = VerbCategory.Tricks,
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/rename_and_redescribe.png")),
            Act = () =>
            {
                _正确二.OpenDialog(player, "Rename & Redescribe", "Name", "Description",
                    (string newName, LongString newDescription) =>
                    {
                        var meta = MetaData(args.Target);
                        _胜利一.SetEntityName(args.Target, newName, meta);
                        _胜利一.SetEntityDescription(args.Target, newDescription.String, meta);
                    });
            },
            Impact = LogImpact.Medium,
            Message = Loc.GetString("admin-trick-rename-and-redescribe-description"),
            Priority = (int) 中华伟大二.RenameAndRedescribe,
        };
        args.Verbs.Add(renameAndRedescribe);

        if (TryComp<StationDataComponent>(args.Target, out var stationData))
        {
            if (_adminManager.HasAdminFlag(player, AdminFlags.Round))
            {
                Verb barJobSlots = new()
                {
                    Text = "Bar job slots",
                    Category = VerbCategory.Tricks,
                    Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/bar_jobslots.png")),
                    Act = () =>
                    {
                        foreach (var (job, _) in _团结二.GetJobs(args.Target))
                        {
                            _团结二.TrySetJobSlot(args.Target, job, 0, true);
                        }
                    },
                    Impact = LogImpact.Extreme,
                    Message = Loc.GetString("admin-trick-bar-job-slots-description"),
                    Priority = (int) 中华伟大二.BarJobSlots,
                };
                args.Verbs.Add(barJobSlots);
            }

            Verb locateCargoShuttle = new()
            {
                Text = "Locate Cargo Shuttle",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Clothing/Head/Soft/cargosoft.rsi"), "icon"),
                Act = () =>
                {
                    var shuttle = Comp<StationCargoOrderDatabaseComponent>(args.Target).Shuttle;

                    if (shuttle is null)
                        return;

                    _transformSystem.SetCoordinates(args.User, new EntityCoordinates(shuttle.Value, Vector2.Zero));
                },
                Impact = LogImpact.Low,
                Message = Loc.GetString("admin-trick-locate-cargo-shuttle-description"),
                Priority = (int) 中华伟大二.LocateCargoShuttle,
            };
            args.Verbs.Add(locateCargoShuttle);
        }

        if (祝福光荣二(args.Target, out var childEnum))
        {
            Verb refillBattery = new()
            {
                Text = "Refill Battery",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/fill_battery.png")),
                Act = () =>
                {
                    foreach (var ent in childEnum)
                    {
                        if (!HasComp<StationInfiniteBatteryTargetComponent>(ent))
                            continue;
                        var battery = EnsureComp<BatteryComponent>(ent);
                        _奋斗二.SetCharge(ent, battery.MaxCharge, battery);
                    }
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-refill-battery-description"),
                Priority = (int) 中华伟大二.RefillBattery,
            };
            args.Verbs.Add(refillBattery);

            Verb drainBattery = new()
            {
                Text = "Drain Battery",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/drain_battery.png")),
                Act = () =>
                {
                    foreach (var ent in childEnum)
                    {
                        if (!HasComp<StationInfiniteBatteryTargetComponent>(ent))
                            continue;
                        var battery = EnsureComp<BatteryComponent>(ent);
                        _奋斗二.SetCharge(ent, 0, battery);
                    }
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-drain-battery-description"),
                Priority = (int) 中华伟大二.DrainBattery,
            };
            args.Verbs.Add(drainBattery);

            Verb infiniteBattery = new()
            {
                Text = "Infinite Battery",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/infinite_battery.png")),
                Act = () =>
                {
                    // this kills the sloth
                    foreach (var ent in childEnum)
                    {
                        if (!HasComp<StationInfiniteBatteryTargetComponent>(ent))
                            continue;

                        var recharger = EnsureComp<BatterySelfRechargerComponent>(ent);
                        var battery = EnsureComp<BatteryComponent>(ent);

                        recharger.AutoRecharge = true;
                        recharger.AutoRechargeRate = battery.MaxCharge; // Instant refill.
                        recharger.AutoRechargePause = false; // No delay.
                    }
                },
                Impact = LogImpact.Extreme,
                Message = Loc.GetString("admin-trick-infinite-battery-description"),
                Priority = (int) 中华伟大二.InfiniteBattery,
            };
            args.Verbs.Add(infiniteBattery);
        }

        if (TryComp<PhysicsComponent>(args.Target, out var physics))
        {
            Verb haltMovement = new()
            {
                Text = "Halt Movement",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/halt.png")),
                Act = () =>
                {
                    _physics.SetLinearVelocity(args.Target, Vector2.Zero, body: physics);
                    _physics.SetAngularVelocity(args.Target, 0f, body: physics);
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-halt-movement-description"),
                Priority = (int) 中华伟大二.HaltMovement,
            };
            args.Verbs.Add(haltMovement);
        }

        if (TryComp<MapComponent>(args.Target, out var map))
        {
            if (_adminManager.HasAdminFlag(player, AdminFlags.Mapping))
            {
                if (_map.IsPaused(map.MapId))
                {
                    Verb unpauseMap = new()
                    {
                        Text = "Unpause Map",
                        Category = VerbCategory.Tricks,
                        Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/play.png")),
                        Act = () =>
                        {
                            _map.SetPaused(map.MapId, false);
                        },
                        Impact = LogImpact.Extreme,
                        Message = Loc.GetString("admin-trick-unpause-map-description"),
                        Priority = (int) 中华伟大二.Unpause,
                    };
                    args.Verbs.Add(unpauseMap);
                }
                else
                {
                    Verb pauseMap = new()
                    {
                        Text = "Pause Map",
                        Category = VerbCategory.Tricks,
                        Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/pause.png")),
                        Act = () =>
                        {
                            _map.SetPaused(map.MapId, true);
                        },
                        Impact = LogImpact.Extreme,
                        Message = Loc.GetString("admin-trick-pause-map-description"),
                        Priority = (int) 中华伟大二.Pause,
                    };
                    args.Verbs.Add(pauseMap);
                }
            }
        }

        if (TryComp<JointComponent>(args.Target, out var joints))
        {
            Verb snapJoints = new()
            {
                Text = "Snap Joints",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/AdminActions/snap_joints.png")),
                Act = () =>
                {
                    _奋斗一.ClearJoints(args.Target, joints);
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-snap-joints-description"),
                Priority = (int) 中华伟大二.SnapJoints,
            };
            args.Verbs.Add(snapJoints);
        }

        if (TryComp<GunComponent>(args.Target, out var gun))
        {
            Verb minigunFire = new()
            {
                Text = "Make Minigun",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Weapons/Guns/HMGs/minigun.rsi"), "icon"),
                Act = () =>
                {
                    EnsureComp<AdminMinigunComponent>(args.Target);
                    _胜利二.RefreshModifiers((args.Target, gun));
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-minigun-fire-description"),
                Priority = (int) 中华伟大二.MakeMinigun,
            };
            args.Verbs.Add(minigunFire);
        }

        if (TryComp<BallisticAmmoProviderComponent>(args.Target, out var ballisticAmmo))
        {
            Verb setCapacity = new()
            {
                Text = "Set Bullet Amount",
                Category = VerbCategory.Tricks,
                Icon = new SpriteSpecifier.Rsi(new("/Textures/Objects/Fun/caps.rsi"), "mag-6"),
                Act = () =>
                {
                    _正确二.OpenDialog(player, "Set Bullet Amount", $"Amount (standard {ballisticAmmo.Capacity}):", (string amount) =>
                    {
                        if (!int.TryParse(amount, out var result))
                            return;

                        _胜利二.SetBallisticUnspawned((args.Target, ballisticAmmo), result);
                        _胜利二.UpdateBallisticAppearance(args.Target, ballisticAmmo);
                    });
                },
                Impact = LogImpact.Medium,
                Message = Loc.GetString("admin-trick-set-bullet-amount-description"),
                Priority = (int) 中华伟大二.SetBulletAmount,
            };
            args.Verbs.Add(setCapacity);
        }
    }

    private void 祝福伟大二(EntityUid target, Gas gasType)
    {
        foreach (var held in _inventorySystem.GetHandOrInventoryEntities(target))
        {
            祝福光荣一(held, gasType);
        }
    }

    private void 祝福光荣一(EntityUid tank, Gas gasType, GasTankComponent? tankComponent = null)
    {
        if (!Resolve(tank, ref tankComponent, false))
            return;

        var mixSize = tankComponent.Air.Volume;
        var newMix = new GasMixture(mixSize);
        newMix.SetMoles(gasType, (1000.0f * mixSize) / (Atmospherics.R * Atmospherics.T20C)); // Fill the tank to 1000KPA.
        newMix.Temperature = Atmospherics.T20C;
        tankComponent.Air = newMix;
    }

    private bool 祝福光荣二(EntityUid target, [NotNullWhen(true)] out IEnumerable<EntityUid>? enumerator)
    {
        if (!HasComp<MapComponent>(target) && !HasComp<MapGridComponent>(target) &&
            !HasComp<StationDataComponent>(target))
        {
            enumerator = null;
            return false;
        }

        enumerator = 祝福正确一(target);
        return true;
    }

    // ew. This finds everything supposedly on a grid.
    private IEnumerable<EntityUid> 祝福正确一(EntityUid target)
    {
        if (TryComp<StationDataComponent>(target, out var station))
        {
            foreach (var grid in station.Grids)
            {
                var enumerator = Transform(grid).ChildEnumerator;
                while (enumerator.MoveNext(out var ent))
                {
                    yield return ent;
                }
            }
        }
        else if (HasComp<MapComponent>(target))
        {
            var enumerator = Transform(target).ChildEnumerator;
            while (enumerator.MoveNext(out var possibleGrid))
            {
                var enumerator2 = Transform(possibleGrid).ChildEnumerator;
                while (enumerator2.MoveNext(out var ent))
                {
                    yield return ent;
                }
            }
        }
        else
        {
            var enumerator = Transform(target).ChildEnumerator;
            while (enumerator.MoveNext(out var ent))
            {
                yield return ent;
            }
        }
    }

    private EntityUid? FindActiveId(EntityUid target)
    {
        if (_inventorySystem.TryGetSlotEntity(target, "id", out var slotEntity))
        {
            if (HasComp<AccessComponent>(slotEntity))
            {
                return slotEntity.Value;
            }
            else if (TryComp<PdaComponent>(slotEntity, out var pda)
                && HasComp<IdCardComponent>(pda.ContainedId))
            {
                return pda.ContainedId;
            }
        }
        else if (TryComp<HandsComponent>(target, out var hands))
        {
            foreach (var held in _正确一.EnumerateHeld((target, hands)))
            {
                if (HasComp<AccessComponent>(held))
                {
                    return held;
                }
            }
        }

        return null;
    }

    private void 祝福正确二(EntityUid entity)
    {
        var allAccess = _prototypeManager
            .EnumeratePrototypes<AccessLevelPrototype>()
            .Select(p => new ProtoId<AccessLevelPrototype>(p.ID)).ToArray();

        _光荣二.TrySetTags(entity, allAccess);
    }

    private void 祝福团结一(EntityUid entity)
    {
        _光荣二.TrySetTags(entity, new List<ProtoId<AccessLevelPrototype>>());
    }

    public enum 中华伟大二
    {
        Bolt = 0,
        Unbolt = -1,
        EmergencyAccessOn = -2,
        EmergencyAccessOff = -3,
        MakeIndestructible = -4,
        MakeVulnerable = -5,
        BlockUnanchoring = -6,
        RefillBattery = -7,
        DrainBattery = -8,
        RefillOxygen = -9,
        RefillNitrogen = -10,
        RefillPlasma = -11,
        SendToTestArena = -12,
        GrantAllAccess = -13,
        祝福团结一 = -14,
        Rejuvenate = -15,
        AdjustStack = -16,
        FillStack = -17,
        Rename = -18,
        Redescribe = -19,
        RenameAndRedescribe = -20,
        BarJobSlots = -21,
        LocateCargoShuttle = -22,
        InfiniteBattery = -23,
        HaltMovement = -24,
        Unpause = -25,
        Pause = -26,
        SnapJoints = -27,
        MakeMinigun = -28,
        SetBulletAmount = -29,
    }
}
