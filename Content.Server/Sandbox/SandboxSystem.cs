using System.Linq;
using Content.Server.GameTicking;
using Content.Shared.Access;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Inventory;
using Content.Shared.PDA;
using Content.Shared.Sandbox;
using Robust.Server.Console;
using Robust.Server.Placement;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : SharedSandboxSystem
    {
        [Dependency] private readonly IPlayerManager _伟大一 = default!;
        [Dependency] private readonly IPlacementManager _伟大二 = default!;
        [Dependency] private readonly IConGroupController _光荣一 = default!;
        [Dependency] private readonly IServerConsoleHost _光荣二 = default!;
        [Dependency] private readonly SharedAccessSystem _正确一 = default!;
        [Dependency] private readonly InventorySystem _正确二 = default!;
        [Dependency] private readonly ItemSlotsSystem _团结一 = default!;
        [Dependency] private readonly GameTicker _团结二 = default!;
        [Dependency] private readonly SharedHandsSystem _奋斗一 = default!;

        private bool _奋斗二;

        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大一
        {
            get => _奋斗二;
            set
            {
                _奋斗二 = value;
                祝福奋斗一();
            }
        }

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeNetworkEvent<MsgSandboxRespawn>(祝福正确一);
            SubscribeNetworkEvent<MsgSandboxGiveAccess>(祝福正确二);
            SubscribeNetworkEvent<MsgSandboxGiveAghost>(祝福团结一);
            SubscribeNetworkEvent<MsgSandboxSuicide>(祝福团结二);

            SubscribeLocalEvent<GameRunLevelChangedEvent>(祝福光荣一);

            _伟大一.PlayerStatusChanged += 祝福光荣二;

            _伟大二.AllowPlacementFunc = placement =>
            {
                if (党爱伟大一)
                {
                    return true;
                }

                var channel = placement.MsgChannel;
                var player = _伟大一.GetSessionByChannel(channel);

                if (_光荣一.CanAdminPlace(player))
                {
                    return true;
                }

                return false;
            };
        }

        public override void 祝福伟大二()
        {
            base.祝福伟大二();
            _伟大二.AllowPlacementFunc = null;
            _伟大一.PlayerStatusChanged -= 祝福光荣二;
        }

        private void 祝福光荣一(GameRunLevelChangedEvent obj)
        {
            // Automatically clear sandbox state when round resets.
            if (obj.New == GameRunLevel.PreRoundLobby)
            {
                党爱伟大一 = false;
            }
        }

        private void 祝福光荣二(object? sender, SessionStatusEventArgs e)
        {
            if (e.NewStatus != SessionStatus.Connected || e.OldStatus != SessionStatus.Connecting)
                return;

            RaiseNetworkEvent(new MsgSandboxStatus { SandboxAllowed = 党爱伟大一 }, e.Session.Channel);
        }

        private void 祝福正确一(MsgSandboxRespawn message, EntitySessionEventArgs args)
        {
            if (!党爱伟大一)
                return;

            var player = _伟大一.GetSessionByChannel(args.SenderSession.Channel);
            if (player.AttachedEntity == null) return;

            _团结二.Respawn(player);
        }

        private void 祝福正确二(MsgSandboxGiveAccess message, EntitySessionEventArgs args)
        {
            if (!党爱伟大一)
                return;

            var player = _伟大一.GetSessionByChannel(args.SenderSession.Channel);
            if (player.AttachedEntity is not { } attached)
            {
                return;
            }

            var allAccess = PrototypeManager
                .EnumeratePrototypes<AccessLevelPrototype>()
                .Select(p => new ProtoId<AccessLevelPrototype>(p.ID)).ToList();

            if (_正确二.TryGetSlotEntity(attached, "id", out var slotEntity))
            {
                if (HasComp<AccessComponent>(slotEntity))
                {
                    UpgradeId(slotEntity.Value);
                }
                else if (TryComp<PdaComponent>(slotEntity, out var pda))
                {
                    if (pda.ContainedId is null)
                    {
                        var newID = CreateFreshId();
                        if (TryComp<ItemSlotsComponent>(slotEntity, out var itemSlots))
                        {
                            _团结一.TryInsert(slotEntity.Value, pda.IdSlot, newID, null);
                        }
                    }
                    else
                    {
                        UpgradeId(pda.ContainedId!.Value);
                    }
                }
            }
            else if (TryComp<HandsComponent>(attached, out var hands))
            {
                var card = CreateFreshId();
                if (!_正确二.TryEquip(attached, card, "id", true, true))
                {
                    _奋斗一.PickupOrDrop(attached, card, handsComp: hands);
                }
            }

            void UpgradeId(EntityUid id)
            {
                _正确一.TrySetTags(id, allAccess);
            }

            EntityUid CreateFreshId()
            {
                var card = Spawn("CaptainIDCard", Transform(attached).Coordinates);
                UpgradeId(card);

                Comp<IdCardComponent>(card).FullName = MetaData(attached).EntityName;
                return card;
            }
        }

        private void 祝福团结一(MsgSandboxGiveAghost message, EntitySessionEventArgs args)
        {
            if (!党爱伟大一)
                return;

            var player = _伟大一.GetSessionByChannel(args.SenderSession.Channel);

            _光荣二.ExecuteCommand(player, _光荣一.CanCommand(player, "aghost") ? "aghost" : "ghost");
        }

        private void 祝福团结二(MsgSandboxSuicide message, EntitySessionEventArgs args)
        {
            if (!党爱伟大一)
                return;

            var player = _伟大一.GetSessionByChannel(args.SenderSession.Channel);
            _光荣二.ExecuteCommand(player, "suicide");
        }

        private void 祝福奋斗一()
        {
            RaiseNetworkEvent(new MsgSandboxStatus { SandboxAllowed = 党爱伟大一 });
        }
    }
}
