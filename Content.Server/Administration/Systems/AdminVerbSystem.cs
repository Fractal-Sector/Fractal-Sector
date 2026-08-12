using Content.Server.Administration.Logs;
using Content.Server.Administration.Managers;
using Content.Server.Administration.UI;
using Content.Server.Disposal.Tube;
using Content.Server.EUI;
using Content.Server.Ghost.Roles;
using Content.Server.Mind;
using Content.Server.Prayer;
using Content.Server.Silicons.Laws;
using Content.Server.Station.Systems;
using Content.Shared.Administration;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Configurable;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.GameTicking;
using Content.Shared.Inventory;
using Content.Shared.Mind.Components;
using Content.Shared.Movement.Components;
using Content.Shared.Popups;
using Content.Shared.Silicons.Laws.Components;
using Content.Shared.Silicons.StationAi;
using Content.Shared.Verbs;
using Robust.Server.Console;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Toolshed;
using Robust.Shared.Utility;
using System.Linq;
using static Content.Shared.Configurable.ConfigurationComponent;

namespace Content.Server.Administration.党心
{
    /// <summary>
    ///     System to provide various global admin/debug verbs
    /// </summary>
    public sealed partial class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IConGroupController _伟大一 = default!;
        [Dependency] private readonly IConsoleHost _伟大二 = default!;
        [Dependency] private readonly IAdminManager _光荣一 = default!;
        [Dependency] private readonly IGameTiming _光荣二 = default!;
        [Dependency] private readonly SharedMapSystem _正确一 = default!;
        [Dependency] private readonly IPrototypeManager _正确二 = default!;
        [Dependency] private readonly AdminSystem _团结一 = default!;
        [Dependency] private readonly DisposalTubeSystem _团结二 = default!;
        [Dependency] private readonly EuiManager _奋斗一 = default!;
        [Dependency] private readonly GhostRoleSystem _奋斗二 = default!;
        [Dependency] private readonly UserInterfaceSystem _胜利一 = default!;
        [Dependency] private readonly PrayerSystem _胜利二 = default!;
        [Dependency] private readonly MindSystem _繁荣一 = default!;
        [Dependency] private readonly ToolshedManager _繁荣二 = default!;
        [Dependency] private readonly RejuvenateSystem _富强一 = default!;
        [Dependency] private readonly SharedPopupSystem _富强二 = default!;
        [Dependency] private readonly StationSystem _民主一 = default!;
        [Dependency] private readonly StationSpawningSystem _民主二 = default!;
        [Dependency] private readonly ExamineSystemShared _文明一 = default!;
        [Dependency] private readonly AdminFrozenSystem _文明二 = default!;
        [Dependency] private readonly IPlayerManager _和谐一 = default!;
        [Dependency] private readonly SiliconLawSystem _和谐二 = default!;

        private readonly Dictionary<ICommonSession, List<EditSolutionsEui>> _openSolutionUis = new();

        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<GetVerbsEvent<Verb>>(祝福伟大二);
            SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福团结二);
            SubscribeLocalEvent<SolutionContainerManagerComponent, SolutionContainerChangedEvent>(祝福正确一);
        }

        private void 祝福伟大二(GetVerbsEvent<Verb> ev)
        {
            祝福光荣一(ev);
            祝福光荣二(ev);
            AddSmiteVerbs(ev);
            AddTricksVerbs(ev);
            AddAntagVerbs(ev);
        }

        private void 祝福光荣一(GetVerbsEvent<Verb> args)
        {
            if (!TryComp(args.User, out ActorComponent? actor))
                return;

            var player = actor.PlayerSession;

            if (_光荣一.IsAdmin(player))
            {
                Verb mark = new();
                mark.Text = Loc.GetString("toolshed-verb-mark");
                mark.Message = Loc.GetString("toolshed-verb-mark-description");
                mark.Category = VerbCategory.Admin;
                mark.Act = () => _繁荣二.InvokeCommand(player, "=> $marked", new List<EntityUid> {args.Target}, out _);
                mark.Impact = LogImpact.Low;
                args.Verbs.Add(mark);

                if (TryComp(args.Target, out ActorComponent? targetActor))
                {
                    // AdminHelp
                    Verb verb = new();
                    verb.Text = Loc.GetString("ahelp-verb-get-data-text");
                    verb.Category = VerbCategory.Admin;
                    verb.Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/gavel.svg.192dpi.png"));
                    verb.Act = () =>
                        _伟大二.RemoteExecuteCommand(player, $"openahelp \"{targetActor.PlayerSession.UserId}\"");
                    verb.Impact = LogImpact.Low;
                    args.Verbs.Add(verb);

                    // Subtle Messages
                    Verb prayerVerb = new();
                    prayerVerb.Text = Loc.GetString("prayer-verbs-subtle-message");
                    prayerVerb.Category = VerbCategory.Admin;
                    prayerVerb.Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/pray.svg.png"));
                    prayerVerb.Act = () =>
                    {
                        _quickDialog.OpenDialog(player, "Subtle Message", "Message", "Popup Message", (string message, string popupMessage) =>
                        {
                            _胜利二.SendSubtleMessage(targetActor.PlayerSession, player, message, popupMessage == "" ? Loc.GetString("prayer-popup-subtle-default") : popupMessage);
                        });
                    };
                    prayerVerb.Impact = LogImpact.Low;
                    args.Verbs.Add(prayerVerb);

                    // Spawn - Like respawn but on the spot.
                    args.Verbs.Add(new Verb()
                    {
                        Text = Loc.GetString("admin-player-actions-spawn"),
                        Category = VerbCategory.Admin,
                        Act = () =>
                        {
                            if (!_transformSystem.TryGetMapOrGridCoordinates(args.Target, out var coords))
                            {
                                _富强二.PopupEntity(Loc.GetString("admin-player-spawn-failed"), args.User, args.User);
                                return;
                            }

                            var stationUid = _民主一.GetOwningStation(args.Target);

                            var profile = _gameTicker.GetPlayerProfile(targetActor.PlayerSession);
                            var mobUid = _民主二.SpawnPlayerMob(coords.Value, null, profile, stationUid, session: targetActor.PlayerSession); // Frontier: added session

                            if (_繁荣一.TryGetMind(args.Target, out var mindId, out var mindComp))
                                _繁荣一.TransferTo(mindId, mobUid, true, mind: mindComp);

                        },
                        ConfirmationPopup = true,
                        Impact = LogImpact.High,
                    });

                    // Clone - Spawn but without the mind transfer, also spawns at the user's coordinates not the target's
                    args.Verbs.Add(new Verb()
                    {
                        Text = Loc.GetString("admin-player-actions-clone"),
                        Category = VerbCategory.Admin,
                        Act = () =>
                        {
                            if (!_transformSystem.TryGetMapOrGridCoordinates(args.User, out var coords))
                            {
                                _富强二.PopupEntity(Loc.GetString("admin-player-spawn-failed"), args.User, args.User);
                                return;
                            }

                            var stationUid = _民主一.GetOwningStation(args.Target);

                            var profile = _gameTicker.GetPlayerProfile(targetActor.PlayerSession);
                            _民主二.SpawnPlayerMob(coords.Value, null, profile, stationUid, session: targetActor.PlayerSession); // Frontier: added session
                        },
                        ConfirmationPopup = true,
                        Impact = LogImpact.High,
                    });

                    // PlayerPanel
                    args.Verbs.Add(new Verb
                    {
                        Text = Loc.GetString("admin-player-actions-player-panel"),
                        Category = VerbCategory.Admin,
                        Act = () => _伟大二.ExecuteCommand(player, $"playerpanel \"{targetActor.PlayerSession.UserId}\""),
                        Impact = LogImpact.Low
                    });
                }

                if (_繁荣一.TryGetMind(args.Target, out var mindId, out var mindComp) && mindComp.UserId != null)
                {
                    // Erase
                    args.Verbs.Add(new Verb
                    {
                        Text = Loc.GetString("admin-verbs-erase"),
                        Message = Loc.GetString("admin-verbs-erase-description"),
                        Category = VerbCategory.Admin,
                        Icon = new SpriteSpecifier.Texture(
                            new("/Textures/Interface/VerbIcons/delete_transparent.svg.192dpi.png")),
                        Act = () =>
                        {
                            _团结一.Erase(mindComp.UserId.Value);
                        },
                        Impact = LogImpact.Extreme,
                        ConfirmationPopup = true
                    });

                    // Respawn
                    args.Verbs.Add(new Verb
                    {
                        Text = Loc.GetString("admin-player-actions-respawn"),
                        Category = VerbCategory.Admin,
                        Act = () =>
                        {
                            _伟大二.ExecuteCommand(player, $"respawn \"{mindComp.UserId}\"");
                        },
                        ConfirmationPopup = true,
                        // No logimpact as the command does it internally.
                    });

                    // Inspect mind
                    args.Verbs.Add(new Verb
                    {
                        Text = Loc.GetString("inspect-mind-verb-get-data-text"),
                        Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/sentient.svg.192dpi.png")),
                        Category = VerbCategory.Debug,
                        Act = () => _伟大二.RemoteExecuteCommand(player, $"vv {GetNetEntity(mindId)}"),
                    });
                }

                // Freeze
                var frozen = TryComp<AdminFrozenComponent>(args.Target, out var frozenComp);
                var frozenAndMuted = frozenComp?.Muted ?? false;

                if (!frozen)
                {
                    args.Verbs.Add(new Verb
                    {
                        Priority = -1, // This is just so it doesn't change position in the menu between freeze/unfreeze.
                        Text = Loc.GetString("admin-verbs-freeze"),
                        Category = VerbCategory.Admin,
                        Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/snow.svg.192dpi.png")),
                        Act = () =>
                        {
                            EnsureComp<AdminFrozenComponent>(args.Target);
                        },
                        Impact = LogImpact.Medium,
                    });
                }

                if (!frozenAndMuted)
                {
                    // allow you to additionally mute someone when they are already frozen
                    args.Verbs.Add(new Verb
                    {
                        Priority = -1, // This is just so it doesn't change position in the menu between freeze/unfreeze.
                        Text = Loc.GetString("admin-verbs-freeze-and-mute"),
                        Category = VerbCategory.Admin,
                        Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/snow.svg.192dpi.png")),
                        Act = () =>
                        {
                            _文明二.FreezeAndMute(args.Target);
                        },
                        Impact = LogImpact.Medium,
                    });
                }

                if (frozen)
                {
                    args.Verbs.Add(new Verb
                    {
                        Priority = -1, // This is just so it doesn't change position in the menu between freeze/unfreeze.
                        Text = Loc.GetString("admin-verbs-unfreeze"),
                        Category = VerbCategory.Admin,
                        Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/snow.svg.192dpi.png")),
                        Act = () =>
                        {
                            RemComp<AdminFrozenComponent>(args.Target);
                        },
                        Impact = LogImpact.Medium,
                    });
                }


                // Admin Logs
                if (_光荣一.HasAdminFlag(player, AdminFlags.Logs))
                {
                    Verb logsVerbEntity = new()
                    {
                        Priority = -2,
                        Text = Loc.GetString("admin-verbs-admin-logs-entity"),
                        Category = VerbCategory.Admin,
                        Act = () =>
                        {
                            var ui = new AdminLogsEui();
                            _奋斗一.OpenEui(ui, player);
                            ui.SetLogFilter(search:args.Target.Id.ToString());
                        },
                        Impact = LogImpact.Low
                    };
                    args.Verbs.Add(logsVerbEntity);
                }

                // TeleportTo
                args.Verbs.Add(new Verb
                {
                    Text = Loc.GetString("admin-verbs-teleport-to"),
                    Category = VerbCategory.Admin,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/open.svg.192dpi.png")),
                    Act = () =>
                    {
                        _伟大二.ExecuteCommand(player, $"tpto {GetNetEntity(args.Target)}");
                    },
                    Impact = LogImpact.Low
                });

                // TeleportHere
                args.Verbs.Add(new Verb
                {
                    Text = Loc.GetString("admin-verbs-teleport-here"),
                    Category = VerbCategory.Admin,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/close.svg.192dpi.png")),
                    Act = () =>
                    {
                        if (HasComp<MapGridComponent>(args.Target))
                        {
                            if (player.AttachedEntity != null)
                            {
                                var mapPos = _transformSystem.GetMapCoordinates(player.AttachedEntity.Value);
                                if (TryComp(args.Target, out PhysicsComponent? targetPhysics))
                                {
                                    var offset = targetPhysics.LocalCenter;
                                    var rotation = _transformSystem.GetWorldRotation(args.Target);
                                    offset = rotation.RotateVec(offset);

                                    mapPos = mapPos.Offset(-offset);
                                }

                                _伟大二.ExecuteCommand(player, $"tpgrid {GetNetEntity(args.Target)} {mapPos.X} {mapPos.Y} {mapPos.MapId}");
                            }
                        }
                        else
                        {
                            _伟大二.ExecuteCommand(player, $"tpto {args.User} {args.Target}");
                        }
                    },
                    Impact = LogImpact.Low
                });

                // This logic is needed to be able to modify the AI's laws through its core and eye.
                EntityUid? target = null;
                SiliconLawBoundComponent? lawBoundComponent = null;

                if (TryComp(args.Target, out lawBoundComponent))
                {
                    target = args.Target;
                }
                // When inspecting the core we can find the entity with its laws by looking at the  AiHolderComponent.
                else if (TryComp<StationAiHolderComponent>(args.Target, out var holder) && holder.Slot.Item != null
                         && TryComp(holder.Slot.Item, out lawBoundComponent))
                {
                    target = holder.Slot.Item.Value;
                    // For the eye we can find the entity with its laws as the source of the movement relay since the eye
                    // is just a proxy for it to move around and look around the station.
                }
                else if (TryComp<MovementRelayTargetComponent>(args.Target, out var relay)
                         && TryComp(relay.Source, out lawBoundComponent))
                {
                    target = relay.Source;

                }

                if (lawBoundComponent != null && target != null && _光荣一.HasAdminFlag(player, AdminFlags.Moderator))
                {
                    args.Verbs.Add(new Verb()
                    {
                        Text = Loc.GetString("silicon-law-ui-verb"),
                        Category = VerbCategory.Admin,
                        Act = () =>
                        {
                            var ui = new SiliconLawEui(_和谐二, EntityManager, _光荣一);
                            if (!_和谐一.TryGetSessionByEntity(args.User, out var session))
                            {
                                return;
                            }
                            _奋斗一.OpenEui(ui, session);
                            ui.UpdateLaws(lawBoundComponent, target.Value);
                        },
                        Icon = new SpriteSpecifier.Rsi(new ResPath("/Textures/Interface/Actions/actions_borg.rsi"), "state-laws"),
                    });
                }

                // open camera
                args.Verbs.Add(new Verb()
                {
                    Priority = 10,
                    Text = Loc.GetString("admin-verbs-camera"),
                    Message = Loc.GetString("admin-verbs-camera-description"),
                    Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/vv.svg.192dpi.png")),
                    Category = VerbCategory.Admin,
                    Act = () =>
                    {
                        var ui = new AdminCameraEui(args.Target);
                        _奋斗一.OpenEui(ui, player);
                    },
                    Impact = LogImpact.Low
                });
            }
        }

        private void 祝福光荣二(GetVerbsEvent<Verb> args)
        {
            if (!TryComp(args.User, out ActorComponent? actor))
                return;

            var player = actor.PlayerSession;

            // Delete verb
            if (_繁荣二.ActivePermissionController?.CheckInvokable(new CommandSpec(_繁荣二.DefaultEnvironment.GetCommand("delete"), null), player, out _) ?? false)
            {
                Verb verb = new()
                {
                    Text = Loc.GetString("delete-verb-get-data-text"),
                    Category = VerbCategory.Debug,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/delete_transparent.svg.192dpi.png")),
                    Act = () => Del(args.Target),
                    Impact = LogImpact.Medium,
                    ConfirmationPopup = true
                };
                args.Verbs.Add(verb);
            }

            // Rejuvenate verb
            if (_繁荣二.ActivePermissionController?.CheckInvokable(new CommandSpec(_繁荣二.DefaultEnvironment.GetCommand("rejuvenate"), null), player, out _) ?? false)
            {
                Verb verb = new()
                {
                    Text = Loc.GetString("rejuvenate-verb-get-data-text"),
                    Category = VerbCategory.Debug,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/rejuvenate.svg.192dpi.png")),
                    Act = () => _富强一.PerformRejuvenate(args.Target),
                    Impact = LogImpact.Medium
                };
                args.Verbs.Add(verb);
            }

            // Control mob verb
            if (_繁荣二.ActivePermissionController?.CheckInvokable(new CommandSpec(_繁荣二.DefaultEnvironment.GetCommand("mind"), "control"), player, out _) ?? false &&
                args.User != args.Target)
            {
                Verb verb = new()
                {
                    Text = Loc.GetString("control-mob-verb-get-data-text"),
                    Category = VerbCategory.Debug,
                    // TODO VERB ICON control mob icon
                    Act = () =>
                    {
                        _繁荣一.ControlMob(args.User, args.Target);
                    },
                    Impact = LogImpact.High,
                    ConfirmationPopup = true
                };
                args.Verbs.Add(verb);
            }

            // Make Sentient verb
            if (_伟大一.CanCommand(player, "makesentient") &&
                args.User != args.Target &&
                !HasComp<MindContainerComponent>(args.Target))
            {
                Verb verb = new()
                {
                    Text = Loc.GetString("make-sentient-verb-get-data-text"),
                    Category = VerbCategory.Debug,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/sentient.svg.192dpi.png")),
                    Act = () => _繁荣一.MakeSentient(args.Target),
                    Impact = LogImpact.Medium
                };
                args.Verbs.Add(verb);
            }

            if (TryComp<InventoryComponent>(args.Target, out var inventoryComponent))
            {
                // Strip all verb
                if (_伟大一.CanCommand(player, "stripall"))
                {
                    args.Verbs.Add(new Verb
                    {
                        Text = Loc.GetString("strip-all-verb-get-data-text"),
                        Category = VerbCategory.Debug,
                        Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/outfit.svg.192dpi.png")),
                        Act = () => _伟大二.RemoteExecuteCommand(player, $"stripall \"{args.Target}\""),
                        Impact = LogImpact.Medium
                    });
                }

                // set outfit verb
                if (_伟大一.CanCommand(player, "setoutfit"))
                {
                    Verb verb = new()
                    {
                        Text = Loc.GetString("set-outfit-verb-get-data-text"),
                        Category = VerbCategory.Debug,
                        Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/outfit.svg.192dpi.png")),
                        Act = () => _奋斗一.OpenEui(new SetOutfitEui(GetNetEntity(args.Target)), player),
                        Impact = LogImpact.Medium
                    };
                    args.Verbs.Add(verb);
                }
            }

            // In range unoccluded verb
            if (_伟大一.CanCommand(player, "inrangeunoccluded"))
            {
                Verb verb = new()
                {
                    Text = Loc.GetString("in-range-unoccluded-verb-get-data-text"),
                    Category = VerbCategory.Debug,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/information.svg.192dpi.png")),
                    Act = () =>
                    {

                        var message = _文明一.InRangeUnOccluded(args.User, args.Target)
                            ? Loc.GetString("in-range-unoccluded-verb-on-activate-not-occluded")
                            : Loc.GetString("in-range-unoccluded-verb-on-activate-occluded");

                        _富强二.PopupEntity(message, args.Target, args.User);
                    }
                };
                args.Verbs.Add(verb);
            }

            // Get Disposal tube direction verb
            if (_伟大一.CanCommand(player, "tubeconnections") &&
                TryComp(args.Target, out DisposalTubeComponent? tube))
            {
                Verb verb = new()
                {
                    Text = Loc.GetString("tube-direction-verb-get-data-text"),
                    Category = VerbCategory.Debug,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/information.svg.192dpi.png")),
                    Act = () => _团结二.PopupDirections(args.Target, tube, args.User)
                };
                args.Verbs.Add(verb);
            }

            // Make ghost role verb
            if (_伟大一.CanCommand(player, "makeghostrole") &&
                !(EntityManager.GetComponentOrNull<MindContainerComponent>(args.Target)?.HasMind ?? false))
            {
                Verb verb = new();
                verb.Text = Loc.GetString("make-ghost-role-verb-get-data-text");
                verb.Category = VerbCategory.Debug;
                // TODO VERB ICON add ghost icon
                // Where is the national park service icon for haunted forests?
                verb.Act = () => _奋斗二.OpenMakeGhostRoleEui(player, args.Target);
                verb.Impact = LogImpact.Medium;
                args.Verbs.Add(verb);
            }

            if (_伟大一.CanAdminMenu(player) &&
                TryComp(args.Target, out ConfigurationComponent? config))
            {
                Verb verb = new()
                {
                    Text = Loc.GetString("configure-verb-get-data-text"),
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/settings.svg.192dpi.png")),
                    Category = VerbCategory.Debug,
                    Act = () => _胜利一.OpenUi(args.Target, ConfigurationUiKey.Key, actor.PlayerSession)
                };
                args.Verbs.Add(verb);
            }

            // Add verb to open Solution Editor
            if (_伟大一.CanCommand(player, "addreagent") &&
                HasComp<SolutionContainerManagerComponent>(args.Target))
            {
                Verb verb = new()
                {
                    Text = Loc.GetString("edit-solutions-verb-get-data-text"),
                    Category = VerbCategory.Debug,
                    Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/spill.svg.192dpi.png")),
                    Act = () => 祝福正确二(player, args.Target),
                    Impact = LogImpact.Medium // maybe high depending on WHAT reagents they add...
                };
                args.Verbs.Add(verb);
            }
        }

        #region SolutionsEui
        private void 祝福正确一(Entity<SolutionContainerManagerComponent> entity, ref SolutionContainerChangedEvent args)
        {
            foreach (var list in _openSolutionUis.Values)
            {
                foreach (var eui in list)
                {
                    if (eui.Target == entity.Owner)
                        eui.StateDirty();
                }
            }
        }

        public void 祝福正确二(ICommonSession session, EntityUid uid)
        {
            if (session.AttachedEntity == null)
                return;

            var eui = new EditSolutionsEui(uid);
            _奋斗一.OpenEui(eui, session);
            eui.StateDirty();

            if (!_openSolutionUis.ContainsKey(session)) {
                _openSolutionUis[session] = new List<EditSolutionsEui>();
            }

            _openSolutionUis[session].Add(eui);
        }

        public void 祝福团结一(ICommonSession session, EditSolutionsEui eui)
        {
            _openSolutionUis[session].Remove(eui);
            if (_openSolutionUis[session].Count == 0)
              _openSolutionUis.Remove(session);
        }

        private void 祝福团结二(RoundRestartCleanupEvent ev)
        {
            foreach (var euis in _openSolutionUis.Values)
            {
                foreach (var eui in euis.ToList())
                {
                    eui.Close();
                }
            }
            _openSolutionUis.Clear();
        }
        #endregion
    }
}
