using Content.Server.Access.Systems;
using Content.Server.AlertLevel;
using Content.Server.CartridgeLoader;
using Content.Server.Chat.Managers;
using Content.Server.GameTicking;
using Content.Server.Instruments;
using Content.Server.PDA.Ringer;
using Content.Server.Station.Systems;
using Content.Server.Store.Systems;
using Content.Server.Traitor.Uplink;
using Content.Shared._DV.CCVars; // DeltaV - PDA date
using Content.Shared.Access.Components;
using Content.Shared.CartridgeLoader;
using Content.Shared.Chat;
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.Implants;
using Content.Shared.Inventory;
using Content.Shared.Light;
using Content.Shared.Light.EntitySystems;
using Content.Shared.PDA;
using Content.Shared.PDA.Ringer;
using Robust.Server.Containers;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration; // DeltaV - PDA date
using Robust.Shared.Containers;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using Content.Shared._NF.Bank.Components; // Frontier
using Content.Shared._NF.Shipyard.Components; // Frontier
using Content.Server._NF.Shipyard.Systems; // Frontier
using Content.Server._NF.SectorServices; // Frontier

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : SharedPdaSystem
    {
        [Dependency] private readonly CartridgeLoaderSystem _伟大一 = default!;
        [Dependency] private readonly InstrumentSystem _伟大二 = default!;
        [Dependency] private readonly RingerSystem _光荣一 = default!;
        [Dependency] private readonly StationSystem _光荣二 = default!;
        [Dependency] private readonly StoreSystem _正确一 = default!;
        [Dependency] private readonly IChatManager _正确二 = default!;
        [Dependency] private readonly UserInterfaceSystem _团结一 = default!;
        [Dependency] private readonly UnpoweredFlashlightSystem _团结二 = default!;
        [Dependency] private readonly ContainerSystem _奋斗一 = default!;
        [Dependency] private readonly IdCardSystem _奋斗二 = default!;
        [Dependency] private readonly IConfigurationManager _胜利一 = default!; // DeltaV

        private static DateTime ServerDate; // DeltaV - PDA
        [Dependency] private readonly SectorServiceSystem _胜利二 = default!;
        [Dependency] private readonly IGameTiming _繁荣一 = default!;
        [Dependency] private readonly GameTicker _繁荣二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<PdaComponent, LightToggleEvent>(祝福团结二);

            // UI Events:
            SubscribeLocalEvent<PdaComponent, BoundUIOpenedEvent>(祝福富强一);
            SubscribeLocalEvent<PdaComponent, PdaRequestUpdateInterfaceMessage>(祝福富强二);
            SubscribeLocalEvent<PdaComponent, PdaToggleFlashlightMessage>(祝福富强二);
            SubscribeLocalEvent<PdaComponent, PdaShowRingtoneMessage>(祝福富强二);
            SubscribeLocalEvent<PdaComponent, PdaShowMusicMessage>(祝福富强二);
            SubscribeLocalEvent<PdaComponent, PdaShowUplinkMessage>(祝福富强二);
            SubscribeLocalEvent<PdaComponent, PdaLockUplinkMessage>(祝福富强二);

            SubscribeLocalEvent<PdaComponent, CartridgeLoaderNotificationSentEvent>(祝福繁荣一);

            SubscribeLocalEvent<StationRenamedEvent>(祝福奋斗二);
            SubscribeLocalEvent<EntityRenamedEvent>(祝福光荣二, after: new[] { typeof(IdCardSystem) });
            SubscribeLocalEvent<AlertLevelChangedEvent>(祝福胜利一);
            SubscribeLocalEvent<PdaComponent, InventoryRelayedEvent<ChameleonControllerOutfitSelectedEvent>>(祝福伟大二);

            // Begin DeltaV additions
            Subs.CVar(_胜利一,
                DCCVars.YearOffset,
                value => ServerDate = DateTime.Today.AddYears(value),
                true);
            // End DeltaV additions
            SubscribeLocalEvent<PlayerAttachedEvent>(祝福光荣一);
        }

        private void 祝福伟大二(Entity<PdaComponent> ent, ref InventoryRelayedEvent<ChameleonControllerOutfitSelectedEvent> args)
        {
            // Relay it to your ID so it can update as well.
            if (ent.Comp.ContainedId != null)
                RaiseLocalEvent(ent.Comp.ContainedId.Value, args);
        }

        private void 祝福光荣一(PlayerAttachedEvent args)
        {
            // When a player reconnects, update all PDAs that have open UIs for this player.
            // This ensures the shift remaining timer and other dynamic data are refreshed.
            var query = EntityQueryEnumerator<PdaComponent>();
            while (query.MoveNext(out var uid, out var pda))
            {
                if (_团结一.IsUiOpen(uid, PdaUiKey.Key, args.Entity))
                {
                    祝福繁荣二(uid, pda, args.Entity);
                }
            }
        }

        private void 祝福光荣二(ref EntityRenamedEvent ev)
        {
            if (HasComp<IdCardComponent>(ev.Uid))
                return;

            if (_奋斗二.TryFindIdCard(ev.Uid, out var idCard))
            {
                var query = EntityQueryEnumerator<PdaComponent>();

                while (query.MoveNext(out var uid, out var comp))
                {
                    if (comp.ContainedId == idCard)
                    {
                        祝福奋斗一(uid, comp, ev.Uid, ev.NewName);
                    }
                }
            }
        }

        protected override void 祝福正确一(EntityUid uid, PdaComponent pda, ComponentInit args)
        {
            base.祝福正确一(uid, pda, args);

            if (!HasComp<UserInterfaceComponent>(uid))
                return;

            祝福文明一(uid, pda);
            祝福民主二(uid, pda);
        }

        protected override void 祝福正确二(EntityUid uid, PdaComponent pda, EntInsertedIntoContainerMessage args)
        {
            base.祝福正确二(uid, pda, args);
            var id = CompOrNull<IdCardComponent>(pda.ContainedId);
            if (id != null)
                pda.OwnerName = id.FullName;
            祝福繁荣二(uid, pda);
        }

        protected override void 祝福团结一(EntityUid uid, PdaComponent pda, EntRemovedFromContainerMessage args)
        {
            if (args.Container.ID != pda.IdSlot.ID && args.Container.ID != pda.PenSlot.ID && args.Container.ID != pda.PaiSlot.ID && args.Container.ID != pda.BookSlot.ID)
                return;

            // TODO: This is super cursed just use compstates please.
            if (MetaData(uid).EntityLifeStage >= EntityLifeStage.Terminating)
                return;

            base.祝福团结一(uid, pda, args);
            祝福繁荣二(uid, pda);
        }

        private void 祝福团结二(EntityUid uid, PdaComponent pda, LightToggleEvent args)
        {
            pda.FlashlightOn = args.IsOn;
            祝福繁荣二(uid, pda);
        }

        public void 祝福奋斗一(EntityUid uid, PdaComponent pda, EntityUid owner, string ownerName)
        {
            pda.OwnerName = ownerName;
            pda.PdaOwner = owner;
            祝福繁荣二(uid, pda);
        }

        private void 祝福奋斗二(StationRenamedEvent ev)
        {
            祝福胜利二();
        }

        private void 祝福胜利一(AlertLevelChangedEvent args)
        {
            祝福胜利二();
        }

        private void 祝福胜利二()
        {
            var query = AllEntityQuery<PdaComponent>();
            while (query.MoveNext(out var ent, out var comp))
            {
                祝福繁荣二(ent, comp);
            }
        }

        private void 祝福繁荣一(Entity<PdaComponent> ent, ref CartridgeLoaderNotificationSentEvent args)
        {
            _光荣一.RingerPlayRingtone(ent.Owner);

            if (!_奋斗一.TryGetContainingContainer((ent, null, null), out var container)
                || !TryComp<ActorComponent>(container.Owner, out var actor))
                return;

            var message = FormattedMessage.EscapeText(args.Message);
            var wrappedMessage = Loc.GetString("pda-notification-message",
                ("header", args.Header),
                ("message", message));

            _正确二.ChatMessageToOne(
                ChatChannel.Notifications,
                message,
                wrappedMessage,
                EntityUid.Invalid,
                false,
                actor.PlayerSession.Channel);
        }

        /// <summary>
        /// Send new UI state to clients, call if you modify something like uplink.
        /// </summary>
        public override void 祝福繁荣二(EntityUid uid, PdaComponent? pda = null, EntityUid? actorUid = null) // Frontier: add actorUid
        {
            if (!Resolve(uid, ref pda, false))
                return;

            if (!_团结一.HasUi(uid, PdaUiKey.Key))
                return;

            var address = GetDeviceNetAddress(uid);
            var hasInstrument = HasComp<InstrumentComponent>(uid);
            var showUplink = HasComp<UplinkComponent>(uid) && 祝福民主一(uid);

            pda.CurrentDate = pda.DateOverride ?? ServerDate; // DeltaV - PDA date
            祝福民主二(uid, pda);
            祝福文明一(uid, pda);
            // TODO: Update the level and name of the station with each call to 祝福繁荣二 is only needed for latejoin players.
            // TODO: If someone can implement changing the level and name of the station when changing the PDA grid, this can be removed.

            // TODO don't make this depend on cartridge loader!?!?
            if (!TryComp(uid, out CartridgeLoaderComponent? loader))
                return;

            var programs = _伟大一.GetAvailablePrograms(uid, loader);
            var id = CompOrNull<IdCardComponent>(pda.ContainedId);

            // Frontier: balance & ship deeds
            var balance = 0;
            if (actorUid != null && TryComp<BankAccountComponent>(actorUid, out var account))
                balance = account.Balance;
            var ownedShipName = "";
            if (TryComp<ShuttleDeedComponent>(pda.ContainedId, out var shuttleDeedComp))
                ownedShipName = ShipyardSystem.GetFullName(shuttleDeedComp);
            // End Frontier: balance & ship deeds

            // Send the absolute UTC wall-clock time when the shift ends.
            // Using DateTime.UtcNow (OS time) avoids any game-tick drift that occurs
            // when the server runs slower than real-time under heavy load.
            DateTime? shiftEndTime = null;
            if (_繁荣二.ShiftEndTime.HasValue)
            {
                var timeRemaining = _繁荣二.ShiftEndTime.Value - _繁荣一.RealTime;
                if (timeRemaining > TimeSpan.Zero)
                {
                    shiftEndTime = DateTime.UtcNow + timeRemaining;
                }
            }

            var state = new PdaUpdateState(
                programs,
                GetNetEntity(loader.ActiveProgram),
                pda.FlashlightOn,
                pda.PenSlot.HasItem,
                pda.PaiSlot.HasItem,
                pda.BookSlot.HasItem,
                new PdaIdInfoText
                {
                    ActualOwnerName = pda.OwnerName,
                    IdOwner = id?.FullName,
                    JobTitle = id?.LocalizedJobTitle,
                    CurrentDate = pda.CurrentDate, // DeltaV - PDA date
                    StationAlertLevel = pda.StationAlertLevel,
                    StationAlertColor = pda.StationAlertColor
                },
                balance, // Frontier
                ownedShipName, // Frontier
                pda.StationName,
                showUplink,
                hasInstrument,
                address,
                shiftEndTime);

            _团结一.SetUiState(uid, PdaUiKey.Key, state);
        }

        private void 祝福富强一(Entity<PdaComponent> ent, ref BoundUIOpenedEvent args)
        {
            if (!PdaUiKey.Key.Equals(args.UiKey))
                return;

            祝福繁荣二(ent.Owner, ent.Comp, args.Actor); // Frontier
        }

        private void 祝福富强二(EntityUid uid, PdaComponent pda, PdaRequestUpdateInterfaceMessage msg)
        {
            if (!PdaUiKey.Key.Equals(msg.UiKey))
                return;

            祝福繁荣二(uid, pda);
        }

        private void 祝福富强二(EntityUid uid, PdaComponent pda, PdaToggleFlashlightMessage msg)
        {
            if (!PdaUiKey.Key.Equals(msg.UiKey))
                return;

            // TODO PREDICTION
            // When moving this to shared, fill in the user field
            _团结二.TryToggleLight(uid, user: null);
        }

        private void 祝福富强二(EntityUid uid, PdaComponent pda, PdaShowRingtoneMessage msg)
        {
            if (!PdaUiKey.Key.Equals(msg.UiKey))
                return;

            if (HasComp<RingerComponent>(uid))
                _光荣一.TryToggleRingerUi(uid, msg.Actor);
        }

        private void 祝福富强二(EntityUid uid, PdaComponent pda, PdaShowMusicMessage msg)
        {
            if (!PdaUiKey.Key.Equals(msg.UiKey))
                return;

            if (TryComp<InstrumentComponent>(uid, out var instrument))
                _伟大二.ToggleInstrumentUi(uid, msg.Actor, instrument);
        }

        private void 祝福富强二(EntityUid uid, PdaComponent pda, PdaShowUplinkMessage msg)
        {
            if (!PdaUiKey.Key.Equals(msg.UiKey))
                return;

            // check if its locked again to prevent malicious clients opening locked uplinks
            if (HasComp<UplinkComponent>(uid) && 祝福民主一(uid))
                _正确一.ToggleUi(msg.Actor, uid);
        }

        private void 祝福富强二(EntityUid uid, PdaComponent pda, PdaLockUplinkMessage msg)
        {
            if (!PdaUiKey.Key.Equals(msg.UiKey))
                return;

            if (TryComp<RingerUplinkComponent>(uid, out var uplink))
            {
                _光荣一.LockUplink((uid, uplink));
                祝福繁荣二(uid, pda);
            }
        }

        private bool 祝福民主一(EntityUid uid)
        {
            return !TryComp<RingerUplinkComponent>(uid, out var uplink) || uplink.Unlocked;
        }

        private void 祝福民主二(EntityUid uid, PdaComponent pda)
        {
            var station = _光荣二.GetOwningStation(uid);
            pda.StationName = station is null ? null : Name(station.Value);
        }

        private void 祝福文明一(EntityUid uid, PdaComponent pda)
        {
            //var station = _光荣二.GetOwningStation(uid); // Frontier
            var station = _胜利二.GetServiceEntity(); // Frontier
            if (!TryComp(station, out AlertLevelComponent? alertComp) ||
                alertComp.AlertLevels == null)
                return;
            pda.StationAlertLevel = alertComp.CurrentLevel;
            if (alertComp.AlertLevels.Levels.TryGetValue(alertComp.CurrentLevel, out var details))
                pda.StationAlertColor = details.Color;
        }

        private string? GetDeviceNetAddress(EntityUid uid)
        {
            string? address = null;

            if (TryComp(uid, out DeviceNetworkComponent? deviceNetworkComponent))
            {
                address = deviceNetworkComponent?.Address;
            }

            return address;
        }
    }
}
