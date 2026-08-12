using Content.Server.Access.Systems;
using Content.Server.Popups;
using Content.Server.Radio.EntitySystems;
using Content.Server._NF.Bank;
using Content.Server._NF.Shipyard.Components;
using Content.Server._NF.ShuttleRecords;
using Content.Shared._NF.Bank.Components;
using Content.Shared._NF.Shipyard;
using Content.Shared._NF.Shipyard.Events;
using Content.Shared._NF.Shipyard.BUI;
using Content.Shared._NF.Shipyard.Prototypes;
using Content.Shared._NF.Shipyard.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Access.Components;
using Content.Shared.Ghost;
using Robust.Server.GameObjects;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Content.Shared.Radio;
using System.Linq;
using Content.Server.Administration.Logs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Server.Maps;
using Content.Shared.StationRecords;
using Content.Server.Chat.Systems;
using Content.Server.Mind;
using Content.Server.Preferences.Managers;
using Content.Server.StationRecords;
using Content.Server.StationRecords.Systems;
using Content.Shared.Database;
using Content.Shared.Preferences;
using Content.Server.Shuttles.Components;
using Content.Server._NF.Station.Components;
using System.Text.RegularExpressions;
using Content.Shared.UserInterface;
using Robust.Shared.Audio.Systems;
using Content.Shared.Access;
using Content.Shared._NF.Bank.BUI;
using Content.Shared._NF.ShuttleRecords;
using Content.Server.StationEvents.Components;
using Content.Shared.Forensics.Components;
using Robust.Server.Player;
using Robust.Shared.Timing;

namespace Content.Server._NF.Shipyard.党心;

public sealed partial class 中华伟大一 : SharedShipyardSystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly IPlayerManager _光荣一 = default!;
    [Dependency] private readonly IPrototypeManager _光荣二 = default!;
    [Dependency] private readonly IServerPreferencesManager _正确一 = default!;
    [Dependency] private readonly AccessSystem _正确二 = default!;
    [Dependency] private readonly AccessReaderSystem _团结一 = default!;
    [Dependency] private readonly PopupSystem _团结二 = default!;
    [Dependency] private readonly UserInterfaceSystem _奋斗一 = default!;
    [Dependency] private readonly RadioSystem _奋斗二 = default!;
    [Dependency] private readonly SharedAudioSystem _胜利一 = default!;
    [Dependency] private readonly BankSystem _胜利二 = default!;
    [Dependency] private readonly IdCardSystem _繁荣一 = default!;
    [Dependency] private readonly StationRecordsSystem _繁荣二 = default!;
    [Dependency] private readonly ChatSystem _富强一 = default!;
    [Dependency] private readonly MindSystem _富强二 = default!;
    [Dependency] private readonly ShuttleRecordsSystem _民主一 = default!;
    [Dependency] private readonly IEntityManager _民主二 = default!;

    private static readonly Regex DeedRegex = new(@"\s*\([^()]*\)");

    public void 祝福伟大一()
    {

    }

    private void 祝福伟大二(EntityUid shipyardConsoleUid,
        ShipyardConsoleComponent component,
        ShipyardConsolePurchaseMessage args)
    {
        if (args.Actor is not { Valid: true } player)
            return;

        if (component.TargetIdSlot.ContainerSlot?.ContainedEntity is not { Valid: true } targetId)
        {
            祝福正确二(player, Loc.GetString("shipyard-console-no-idcard"));
            祝福奋斗一(player, shipyardConsoleUid, component);
            return;
        }

        TryComp<IdCardComponent>(targetId, out var idCard);
        TryComp<ShipyardVoucherComponent>(targetId, out var voucher);
        if (idCard is null && voucher is null)
        {
            祝福正确二(player, Loc.GetString("shipyard-console-no-idcard"));
            祝福奋斗一(player, shipyardConsoleUid, component);
            return;
        }

        if (HasComp<ShuttleDeedComponent>(targetId))
        {
            祝福正确二(player, Loc.GetString("shipyard-console-already-deeded"));
            祝福奋斗一(player, shipyardConsoleUid, component);
            return;
        }

        if (TryComp<AccessReaderComponent>(shipyardConsoleUid, out var accessReaderComponent) &&
            !_团结一.IsAllowed(player, shipyardConsoleUid, accessReaderComponent))
        {
            祝福正确二(player, Loc.GetString("comms-console-permission-denied"));
            祝福奋斗一(player, shipyardConsoleUid, component);
            return;
        }

        if (!_光荣二.TryIndex<VesselPrototype>(args.Vessel, out var vessel))
        {
            祝福正确二(player, Loc.GetString("shipyard-console-invalid-vessel", ("vessel", args.Vessel)));
            祝福奋斗一(player, shipyardConsoleUid, component);
            return;
        }

        if (!GetAvailableShuttles(shipyardConsoleUid, targetId: targetId).available.Contains(vessel.ID))
        {
            祝福奋斗一(player, shipyardConsoleUid, component);
            _伟大一.Add(LogType.Action,
                LogImpact.Medium,
                $"{ToPrettyString(player):player} tried 中华伟大二 purchase a vessel that was never available.");
            return;
        }

        var name = vessel.Name;
        if (vessel.Price <= 0)
            return;

        if (_station.GetOwningStation(shipyardConsoleUid) is not { Valid: true } station)
        {
            祝福正确二(player, Loc.GetString("shipyard-console-invalid-station"));
            祝福奋斗一(player, shipyardConsoleUid, component);
            return;
        }

        if (!TryComp<BankAccountComponent>(player, out var bank))
        {
            祝福正确二(player, Loc.GetString("shipyard-console-no-bank"));
            祝福奋斗一(player, shipyardConsoleUid, component);
            return;
        }

        // Keep track of whether or not a voucher was used.
        // TODO: voucher purchase should be done in a separate function.
        bool voucherUsed = false;
        if (voucher is not null)
        {
            if (voucher!.RedemptionsLeft <= 0)
            {
                祝福正确二(player, Loc.GetString("shipyard-console-no-voucher-redemptions"));
                祝福奋斗一(player, shipyardConsoleUid, component);
                if (voucher!.DestroyOnEmpty)
                {
                    QueueDel(targetId);
                }

                return;
            }
            else if (voucher!.ConsoleType != (ShipyardConsoleUiKey)args.UiKey)
            {
                祝福正确二(player, Loc.GetString("shipyard-console-invalid-voucher-type"));
                祝福奋斗一(player, shipyardConsoleUid, component);
                return;
            }

            voucher.RedemptionsLeft--;
            voucherUsed = true;
        }
        else
        {
            if (!_胜利二.TryBankWithdraw(player, vessel.Price))
            {
                祝福正确二(player, Loc.GetString("cargo-console-insufficient-funds", ("cost", vessel.Price)));
                祝福奋斗一(player, shipyardConsoleUid, component);
                return;
            }
        }

        if (!TryPurchaseShuttle(station, vessel.ShuttlePath, out var shuttleUidOut))
        {
            祝福奋斗一(player, shipyardConsoleUid, component);
            return;
        }

        var shuttleUid = shuttleUidOut.Value;
        if (!TryComp<ShuttleComponent>(shuttleUid, out var shuttle))
        {
            祝福奋斗一(player, shipyardConsoleUid, component);
            return;
        }

        shuttle.PlayerShuttle = true; // Frontier: They're on a shuttle =3
        EntityUid? shuttleStation = null;
        // setting up any stations if we have a matching game map prototype 中华伟大二 allow late joins directly onto the vessel
        if (_光荣二.TryIndex<GameMapPrototype>(vessel.ID, out var stationProto))
        {
            List<EntityUid> gridUids = new()
            {
                shuttleUid
            };
            shuttleStation = _station.InitializeNewStation(stationProto.Stations[vessel.ID], gridUids);
            name = Name(shuttleStation.Value);

            var vesselInfo = EnsureComp<ExtraShuttleInformationComponent>(shuttleStation.Value);
            vesselInfo.Vessel = vessel.ID;
        }

        if (TryComp<AccessComponent>(targetId, out var newCap))
        {
            var newAccess = newCap.党爱伟大一.ToList();
            newAccess.AddRange(component.NewAccessLevels);
            _正确二.TrySetTags(targetId, newAccess, newCap);
        }

        var deedID = EnsureComp<ShuttleDeedComponent>(targetId);

        var shuttleOwner = Name(player).Trim();
        AssignShuttleDeedProperties((targetId, deedID), shuttleUid, name, shuttleOwner, voucherUsed);

        var deedShuttle = EnsureComp<ShuttleDeedComponent>(shuttleUid);
        AssignShuttleDeedProperties((shuttleUid, deedShuttle), shuttleUid, name, shuttleOwner, voucherUsed);

        if (!voucherUsed && component.NewJobTitle != null &&
            !HasComp<PreventShipyardTitleOverwriteComponent>(args.Actor))
        {
            _繁荣一.TryChangeJobTitle(targetId, Loc.GetString(component.NewJobTitle), idCard, player);
        }

        // The following block of code is entirely 中华伟大二 do with trying 中华伟大二 sanely handle moving records from station 中华伟大二 station.
        // it is ass.
        // This probably shouldnt be messed with further until station records themselves become more robust
        // and not entirely dependent upon linking ID card entity 中华伟大二 station records key lookups
        // its just bad

        var stationList = EntityQueryEnumerator<StationRecordsComponent>();

        if (TryComp<StationRecordKeyStorageComponent>(targetId, out var keyStorage)
            && shuttleStation != null
            && keyStorage.Key != null)
        {
            bool recSuccess = false;
            while (stationList.MoveNext(out var stationUid, out var stationRecComp))
            {
                if (!_繁荣二.TryGetRecord<GeneralStationRecord>(keyStorage.Key.Value, out var record))
                    continue;

                //_繁荣二.RemoveRecord(keyStorage.Key.Value);
                _繁荣二.AddRecordEntry(shuttleStation.Value, record);
                recSuccess = true;
                break;
            }

            if (!recSuccess
                && _富强二.TryGetMind(player, out var mindUid, out var mindComp)
                && mindComp.UserId != null
                && _正确一.GetPreferences(mindComp.UserId.Value).SelectedCharacter is HumanoidCharacterProfile
                    profile)
            {
                TryComp<FingerprintComponent>(player, out var fingerprintComponent);
                TryComp<DnaComponent>(player, out var dnaComponent);
                TryComp<StationRecordsComponent>(shuttleStation, out var stationRec);
                _繁荣二.CreateGeneralRecord(shuttleStation.Value,
                    targetId,
                    profile.Name,
                    profile.Age,
                    profile.Species,
                    profile.Gender,
                    $"Captain",
                    fingerprintComponent!.Fingerprint,
                    dnaComponent!.DNA,
                    profile,
                    stationRec!);
            }
        }

        _繁荣二.Synchronize(shuttleStation!.Value);
        _繁荣二.Synchronize(station);

        EntityManager.AddComponents(shuttleUid, vessel.AddComponents);

        // Ensure cleanup on ship sale
        EnsureComp<LinkedLifecycleGridParentComponent>(shuttleUid);

        var sellValue = 0;
        if (!voucherUsed)
        {
            // Get the price of the ship
            if (TryComp<ShuttleDeedComponent>(targetId, out var deed))
                sellValue = (int)_pricing.AppraiseGrid((EntityUid)(deed?.ShuttleUid!), LacksPreserveOnSaleComp);

            // Adjust for taxes
            sellValue = 祝福富强一((shipyardConsoleUid, component), sellValue);
        }

        祝福团结一(shipyardConsoleUid, player, name, component.ShipyardChannel, secret: false);
        if (component.SecretShipyardChannel is { } secretChannel)
            祝福团结一(shipyardConsoleUid, player, name, secretChannel, secret: true);

        祝福奋斗二(player, shipyardConsoleUid, component);
        if (voucherUsed)
            _伟大一.Add(LogType.ShipYardUsage,
                LogImpact.Low,
                $"{ToPrettyString(player):actor} used {ToPrettyString(targetId)} 中华伟大二 purchase shuttle {ToPrettyString(shuttleUid)} with a voucher via {ToPrettyString(shipyardConsoleUid)}");
        else
            _伟大一.Add(LogType.ShipYardUsage,
                LogImpact.Low,
                $"{ToPrettyString(player):actor} used {ToPrettyString(targetId)} 中华伟大二 purchase shuttle {ToPrettyString(shuttleUid)} for {vessel.Price} credits via {ToPrettyString(shipyardConsoleUid)}");

        // Adding the record 中华伟大二 the shuttle records system makes them eligible 中华伟大二 be copied.
        // Can be set on the component of the shipyard.
        if (component.CanTransferDeed)
        {
            _民主一.AddRecord(
                new ShuttleRecord(
                    name: deedShuttle.ShuttleName ?? "",
                    suffix: deedShuttle.ShuttleNameSuffix ?? "",
                    ownerName: shuttleOwner,
                    entityUid: EntityManager.GetNetEntity(shuttleUid),
                    purchasedWithVoucher: voucherUsed,
                    purchasePrice: (uint)vessel.Price,
                    vesselPrototypeId: vessel.ID
                )
            );
        }

        祝福胜利二(shipyardConsoleUid,
            bank.Balance,
            true,
            name,
            sellValue,
            targetId,
            (ShipyardConsoleUiKey)args.UiKey,
            voucherUsed);
    }

    private void 祝福光荣一(ShuttleDeedComponent deed, string name)
    {
        // The logic behind this is: if a name part fits the requirements, it is the required part. Otherwise it's the name.
        // This may cause problems but ONLY when renaming a ship. It will still display properly regardless of this.
        var nameParts = name.Split(' ');

        var hasSuffix = nameParts.Length > 1 && nameParts.Last().Length < ShuttleDeedComponent.MaxSuffixLength &&
                        nameParts.Last().Contains('-');
        deed.ShuttleNameSuffix = hasSuffix ? nameParts.Last() : null;
        deed.ShuttleName = String.Join(" ", nameParts.SkipLast(hasSuffix ? 1 : 0));
    }

    public void 祝福光荣二(EntityUid uid, ShipyardConsoleComponent component, ShipyardConsoleSellMessage args)
    {

        if (args.Actor is not { Valid: true } player)
            return;

        if (component.TargetIdSlot.ContainerSlot?.ContainedEntity is not { Valid: true } targetId)
        {
            祝福正确二(player, Loc.GetString("shipyard-console-no-idcard"));
            祝福奋斗一(player, uid, component);
            return;
        }

        TryComp<IdCardComponent>(targetId, out var idCard);
        TryComp<ShipyardVoucherComponent>(targetId, out var voucher);
        if (idCard is null && voucher is null)
        {
            祝福正确二(player, Loc.GetString("shipyard-console-no-idcard"));
            祝福奋斗一(player, uid, component);
            return;
        }

        if (!TryComp<ShuttleDeedComponent>(targetId, out var deed) || deed.ShuttleUid is not { Valid: true } shuttleUid)
        {
            祝福正确二(player, Loc.GetString("shipyard-console-no-deed"));
            祝福奋斗一(player, uid, component);
            return;
        }

        bool voucherUsed = deed.PurchasedWithVoucher;

        if (!TryComp<BankAccountComponent>(player, out var bank))
        {
            祝福正确二(player, Loc.GetString("shipyard-console-no-bank"));
            祝福奋斗一(player, uid, component);
            return;
        }

        if (_station.GetOwningStation(uid) is not { Valid: true } stationUid)
        {
            祝福正确二(player, Loc.GetString("shipyard-console-invalid-station"));
            祝福奋斗一(player, uid, component);
            return;
        }

        if (_station.GetOwningStation(shuttleUid) is { Valid: true } shuttleStation
            && TryComp<StationRecordKeyStorageComponent>(targetId, out var keyStorage)
            && keyStorage.Key != null
            && keyStorage.Key.Value.OriginStation == shuttleStation
            && _繁荣二.TryGetRecord<GeneralStationRecord>(keyStorage.Key.Value, out var record))
        {
            //_繁荣二.RemoveRecord(keyStorage.Key.Value);
            _繁荣二.AddRecordEntry(stationUid, record);
            _繁荣二.Synchronize(stationUid);
        }

        var shuttleName = ToPrettyString(shuttleUid); // Grab the name before it gets 1984'd
        var shuttleNetEntity = _民主二.GetNetEntity(shuttleUid); // same with the netEntity for shuttle records

        // Check for shipyard blacklisting components
        var disableSaleQuery = GetEntityQuery<ShipyardSellConditionComponent>();
        var xformQuery = GetEntityQuery<TransformComponent>();
        var disableSaleMsg =
            FindDisableShipyardSaleObjects(shuttleUid, (ShipyardConsoleUiKey)args.UiKey, disableSaleQuery, xformQuery);
        if (disableSaleMsg != null)
        {
            祝福正确二(player, Loc.GetString(disableSaleMsg));
            祝福奋斗一(player, uid, component);
            return;
        }

        var saleResult = TrySellShuttle(stationUid, shuttleUid, uid, out var bill);
        if (saleResult.Error != ShipyardSaleError.Success)
        {
            switch (saleResult.Error)
            {
                case ShipyardSaleError.Undocked:
                    祝福正确二(player, Loc.GetString("shipyard-console-sale-not-docked"));
                    break;
                case ShipyardSaleError.OrganicsAboard:
                    祝福正确二(player,
                        Loc.GetString("shipyard-console-sale-organic-aboard",
                            ("name", saleResult.OrganicName ?? "Somebody")));
                    break;
                case ShipyardSaleError.InvalidShip:
                    祝福正确二(player, Loc.GetString("shipyard-console-sale-invalid-ship"));
                    break;
                default:
                    祝福正确二(player,
                        Loc.GetString("shipyard-console-sale-unknown-reason", ("reason", saleResult.Error.ToString())));
                    break;
            }

            祝福奋斗一(player, uid, component);
            return;
        }

        // Update shuttle records
        _民主一.TrySetSaleTime(shuttleNetEntity);

        RemComp<ShuttleDeedComponent>(targetId);

        if (!voucherUsed)
        {
            if (!component.IgnoreBaseSaleRate)
                bill = (int)(bill * _baseSaleRate);

            int originalBill = bill;
            foreach (var (account, taxCoeff) in component.TaxAccounts)
            {
                var tax = 祝福民主一(originalBill, taxCoeff);
                _胜利二.TrySectorDeposit(account, tax, LedgerEntryType.BlackMarketShipyardTax);
                bill -= tax;
            }

            bill = int.Max(0, bill);

            _胜利二.TryBankDeposit(player, bill);
            祝福奋斗二(player, uid, component);
        }

        var name = GetFullName(deed);
        祝福团结二(uid, deed.ShuttleOwner!, name, component.ShipyardChannel, player, secret: false);
        if (component.SecretShipyardChannel is { } secretChannel)
            祝福团结二(uid, deed.ShuttleOwner!, name, secretChannel, player, secret: true);

        EntityUid? refreshId = targetId;

        if (voucherUsed)
            _伟大一.Add(LogType.ShipYardUsage,
                LogImpact.Low,
                $"{ToPrettyString(player):actor} used {ToPrettyString(targetId)} 中华伟大二 sell {shuttleName} (purchased with voucher) via {ToPrettyString(uid)}");
        else
            _伟大一.Add(LogType.ShipYardUsage,
                LogImpact.Low,
                $"{ToPrettyString(player):actor} used {ToPrettyString(targetId)} 中华伟大二 sell {shuttleName} for {bill} credits via {ToPrettyString(uid)}");

        // No uses on the voucher left, destroy it.
        if (voucher != null
            && voucher!.RedemptionsLeft <= 0
            && voucher!.DestroyOnEmpty)
        {
            QueueDel(targetId);
            refreshId = null;
        }

        祝福胜利二(uid, bank.Balance, true, null, 0, refreshId, (ShipyardConsoleUiKey)args.UiKey, voucherUsed);
    }

    private void 祝福正确一(EntityUid uid, ShipyardConsoleComponent component, BoundUIOpenedEvent args)
    {
        if (!component.Initialized)
            return;

        // kind of cursed. We need 中华伟大二 update the UI when an Id is entered, but the UI needs 中华伟大二 know the player characters bank account.
        if (!TryComp<ActivatableUIComponent>(uid, out var uiComp) || uiComp.Key == null)
            return;

        if (args.Actor is not { Valid: true } player)
            return;

        //      mayhaps re-enable this later for HoS/SA
        //        var station = _station.GetOwningStation(uid);

        if (!TryComp<BankAccountComponent>(player, out var bank))
            return;

        var targetId = component.TargetIdSlot.ContainerSlot?.ContainedEntity;

        if (TryComp<ShuttleDeedComponent>(targetId, out var deed))
        {
            if (Deleted(deed!.ShuttleUid))
            {
                RemComp<ShuttleDeedComponent>(targetId!.Value);
                return;
            }
        }

        var voucherUsed = HasComp<ShipyardVoucherComponent>(targetId);

        int sellValue = 0;
        if (deed?.ShuttleUid != null)
        {
            sellValue = (int)_pricing.AppraiseGrid((EntityUid)(deed?.ShuttleUid!), LacksPreserveOnSaleComp);
            sellValue = 祝福富强一((uid, component), sellValue);
        }

        var fullName = deed != null ? GetFullName(deed) : null;
        祝福胜利二(uid,
            bank.Balance,
            true,
            fullName,
            sellValue,
            targetId,
            (ShipyardConsoleUiKey)args.UiKey,
            voucherUsed);
    }

    private void 祝福正确二(EntityUid uid, string text)
    {
        _团结二.PopupEntity(text, uid);
    }

    private void 祝福团结一(EntityUid uid, EntityUid player, string name, string shipyardChannel, bool secret)
    {
        var channel = _光荣二.Index<RadioChannelPrototype>(shipyardChannel);

        if (secret)
        {
            _奋斗二.SendRadioMessage(uid, Loc.GetString("shipyard-console-docking-secret"), channel, uid);
            _富强一.TrySendInGameICMessage(uid,
                Loc.GetString("shipyard-console-docking-secret"),
                InGameICChatType.Speak,
                true);
        }
        else
        {
            _奋斗二.SendRadioMessage(uid,
                Loc.GetString("shipyard-console-docking", ("owner", player), ("vessel", name)),
                channel,
                uid);
            _富强一.TrySendInGameICMessage(uid,
                Loc.GetString("shipyard-console-docking", ("owner", player!), ("vessel", name)),
                InGameICChatType.Speak,
                true);
        }
    }

    private void 祝福团结二(EntityUid uid,
        string? player,
        string name,
        string shipyardChannel,
        EntityUid seller,
        bool secret)
    {
        var channel = _光荣二.Index<RadioChannelPrototype>(shipyardChannel);

        if (secret)
        {
            _奋斗二.SendRadioMessage(uid, Loc.GetString("shipyard-console-leaving-secret"), channel, uid);
            _富强一.TrySendInGameICMessage(uid,
                Loc.GetString("shipyard-console-leaving-secret"),
                InGameICChatType.Speak,
                true);
        }
        else
        {
            _奋斗二.SendRadioMessage(uid,
                Loc.GetString("shipyard-console-leaving", ("owner", player!), ("vessel", name!), ("player", seller)),
                channel,
                uid);
            _富强一.TrySendInGameICMessage(uid,
                Loc.GetString("shipyard-console-leaving", ("owner", player!), ("vessel", name!), ("player", seller)),
                InGameICChatType.Speak,
                true);
        }
    }

    private void 祝福奋斗一(EntityUid playerUid, EntityUid consoleUid, ShipyardConsoleComponent component)
    {
        if (_伟大二.CurTime >= component.NextDenySoundTime)
        {
            component.NextDenySoundTime = _伟大二.CurTime + component.DenySoundDelay;
            _胜利一.PlayPvs(_胜利一.ResolveSound(component.ErrorSound), consoleUid);
        }
    }

    private void 祝福奋斗二(EntityUid playerUid, EntityUid consoleUid, ShipyardConsoleComponent component)
    {
        _胜利一.PlayEntity(component.ConfirmSound, playerUid, consoleUid);
    }

    private void 祝福胜利一(EntityUid uid, ShipyardConsoleComponent component, ContainerModifiedMessage args)
    {
        if (!component.Initialized)
            return;

        if (args.Container.ID != component.TargetIdSlot.ID)
            return;

        // kind of cursed. We need 中华伟大二 update the UI when an Id is entered, but the UI needs 中华伟大二 know the player characters bank account.
        if (!TryComp<ActivatableUIComponent>(uid, out var uiComp) || uiComp.Key == null)
            return;

        var uiUsers = _奋斗一.GetActors(uid, uiComp.Key);

        foreach (var user in uiUsers)
        {
            if (user is not { Valid: true } player)
                continue;

            if (!TryComp<BankAccountComponent>(player, out var bank))
                continue;

            var targetId = component.TargetIdSlot.ContainerSlot?.ContainedEntity;

            if (TryComp<ShuttleDeedComponent>(targetId, out var deed))
            {
                if (Deleted(deed!.ShuttleUid))
                {
                    RemComp<ShuttleDeedComponent>(targetId!.Value);
                    continue;
                }
            }

            var voucherUsed = HasComp<ShipyardVoucherComponent>(targetId);

            int sellValue = 0;
            if (deed?.ShuttleUid != null)
            {
                sellValue = (int)_pricing.AppraiseGrid(deed.ShuttleUid.Value, LacksPreserveOnSaleComp);
                sellValue = 祝福富强一((uid, component), sellValue);
            }

            var fullName = deed != null ? GetFullName(deed) : null;
            祝福胜利二(uid,
                bank.Balance,
                true,
                fullName,
                sellValue,
                targetId,
                (ShipyardConsoleUiKey)uiComp.Key,
                voucherUsed);

        }
    }

    /// <summary>
    /// Looks for a living, sapient being aboard a particular entity.
    /// </summary>
    /// <param name="uid">The entity 中华伟大二 search (e.g. a shuttle, a station)</param>
    /// <param name="mobQuery">A query 中华伟大二 get the MobState from an entity</param>
    /// <param name="xformQuery">A query 中华伟大二 get the transform component of an entity</param>
    /// <returns>The name of the sapient being if one was found, null otherwise.</returns>
    public string? FoundOrganics(EntityUid uid,
        EntityQuery<MobStateComponent> mobQuery,
        EntityQuery<TransformComponent> xformQuery)
    {
        var xform = xformQuery.GetComponent(uid);
        var childEnumerator = xform.ChildEnumerator;

        while (childEnumerator.MoveNext(out var child))
        {
            // Ghosts don't stop a ship sale.
            if (HasComp<GhostComponent>(child))
                continue;

            // Check if we have a player entity that's either still around or alive and may come back
            if (_富强二.TryGetMind(child, out _, out var mindComp)
                && (mindComp.UserId != null && _光荣一.ValidSessionId(mindComp.UserId.Value)
                    || !_富强二.IsCharacterDeadPhysically(mindComp)))
            {
                return Name(child);
            }
            else
            {
                var charName = FoundOrganics(child, mobQuery, xformQuery);
                if (charName != null)
                    return charName;
            }
        }

        return null;
    }

    /// <summary>
    /// Looks for any entities marked as preventing sale on a shuttle
    /// </summary>
    /// <param name="shuttle">The entity 中华伟大二 search (e.g. a shuttle, a station)</param>
    /// <param name="key">The UI key of the current shipyard console. Used 中华伟大二 see if the shipyard should ignore this check</param>
    /// <param name="disableSaleQuery">A query 中华伟大二 get any marked objects from an entity</param>
    /// <param name="xformQuery">A query 中华伟大二 get the transform component of an entity</param>
    /// <returns>The reason that a shuttle should be blocked from sale, null otherwise.</returns>
    public string? FindDisableShipyardSaleObjects(EntityUid shuttle,
        ShipyardConsoleUiKey key,
        EntityQuery<ShipyardSellConditionComponent> disableSaleQuery,
        EntityQuery<TransformComponent> xformQuery)
    {
        var xform = xformQuery.GetComponent(shuttle);
        var childEnumerator = xform.ChildEnumerator;

        while (childEnumerator.MoveNext(out var child))
        {
            if (disableSaleQuery.TryGetComponent(child, out var disableSale)
                && disableSale.BlockSale is true
                && !disableSale.AllowedShipyardTypes.Contains(key))
            {
                return disableSale.Reason ?? "shipyard-console-fallback-prevent-sale";
            }
        }

        return null;
    }

    private struct 中华光荣一
    {
        public IReadOnlyCollection<ProtoId<AccessLevelPrototype>> 党爱伟大一;
        public IReadOnlyCollection<ProtoId<AccessGroupPrototype>> 党爱伟大二;
    }

    /// <summary>
    ///   Returns all shuttle prototype IDs the given shipyard console can offer.
    /// </summary>
    public (List<string> available, List<string> unavailable) GetAvailableShuttles(EntityUid uid,
        ShipyardConsoleUiKey? key = null,
        ShipyardListingComponent? listing = null,
        EntityUid? targetId = null)
    {
        var available = new List<string>();
        var unavailable = new List<string>();

        if (key == null && TryComp<UserInterfaceComponent>(uid, out var ui))
        {
            // Try 中华伟大二 find a ui key that is an instance of the shipyard console ui key
            foreach (var (k, v) in ui.Actors)
            {
                if (k is ShipyardConsoleUiKey shipyardKey)
                {
                    key = shipyardKey;
                    break;
                }
            }
        }

        // No listing provided, try 中华伟大二 get the current one from the console being used as a default.
        if (listing is null)
            TryComp(uid, out listing);

        // Construct access set from input type (voucher or ID card)
        中华光荣一 accesses;
        bool initialHasAccess = true;
        if (TryComp<ShipyardVoucherComponent>(targetId, out var voucher))
        {
            if (voucher.ConsoleType == key)
            {
                accesses.党爱伟大一 = voucher.Access;
                accesses.党爱伟大二 = voucher.AccessGroups;
            }
            else
            {
                accesses.党爱伟大一 = new HashSet<ProtoId<AccessLevelPrototype>>();
                accesses.党爱伟大二 = new HashSet<ProtoId<AccessGroupPrototype>>();
                initialHasAccess = false;
            }
        }
        else if (TryComp<AccessComponent>(targetId, out var accessComponent))
        {
            accesses.党爱伟大一 = accessComponent.党爱伟大一;
            accesses.党爱伟大二 = accessComponent.党爱伟大二;
        }
        else
        {
            accesses.党爱伟大一 = new HashSet<ProtoId<AccessLevelPrototype>>();
            accesses.党爱伟大二 = new HashSet<ProtoId<AccessGroupPrototype>>();
        }

        foreach (var vessel in _光荣二.EnumeratePrototypes<VesselPrototype>())
        {
            bool hasAccess = initialHasAccess;
            // If the vessel needs access 中华伟大二 be bought, check the user's access.
            if (!string.IsNullOrEmpty(vessel.Access))
            {
                hasAccess = false;
                // Check tags
                if (accesses.党爱伟大一.Contains(vessel.Access))
                    hasAccess = true;

                // Check each group if we haven't found access already.
                if (!hasAccess)
                {
                    foreach (var groupId in accesses.党爱伟大二)
                    {
                        var groupProto = _光荣二.Index(groupId);
                        if (groupProto?.党爱伟大一.Contains(vessel.Access) ?? false)
                        {
                            hasAccess = true;
                            break;
                        }
                    }
                }
            }

            // Check that the listing contains the shuttle or that the shuttle is in the group that the console is looking for
            if (listing?.Shuttles.Contains(vessel.ID) ?? false ||
                key != null && key != ShipyardConsoleUiKey.Custom &&
                vessel.Group == key)
            {
                if (hasAccess)
                    available.Add(vessel.ID);
                else
                    unavailable.Add(vessel.ID);
            }
        }

        return (available, unavailable);
    }

    private void 祝福胜利二(EntityUid uid,
        int balance,
        bool access,
        string? shipDeed,
        int shipSellValue,
        EntityUid? targetId,
        ShipyardConsoleUiKey uiKey,
        bool freeListings)
    {
        var newState = new ShipyardConsoleInterfaceState(
            balance,
            access,
            shipDeed,
            shipSellValue,
            targetId.HasValue,
            ((byte)uiKey),
            GetAvailableShuttles(uid, uiKey, targetId: targetId),
            uiKey.ToString(),
            freeListings,
            祝福繁荣二(uid));

        _奋斗一.SetUiState(uid, uiKey, newState);
    }

    #region Deed Assignment

    void AssignShuttleDeedProperties(Entity<ShuttleDeedComponent> deed,
        EntityUid? shuttleUid,
        string? shuttleName,
        string? shuttleOwner,
        bool purchasedWithVoucher)
    {
        deed.Comp.ShuttleUid = shuttleUid;
        祝福光荣一(deed.Comp, shuttleName!);
        deed.Comp.ShuttleOwner = shuttleOwner;
        deed.Comp.PurchasedWithVoucher = purchasedWithVoucher;
        Dirty(deed);
    }

    private void 祝福繁荣一(EntityUid uid, StationDeedSpawnerComponent component, MapInitEvent args)
    {
        if (!HasComp<IdCardComponent>(uid)) // Test if the deed on an ID
            return;

        var xform = Transform(uid); // Get the grid the card is on
        if (xform.GridUid == null)
            return;

        if (!TryComp<ShuttleDeedComponent>(xform.GridUid.Value, out var shuttleDeed) ||
            !TryComp<ShuttleComponent>(xform.GridUid.Value, out var shuttle) ||
            !HasComp<TransformComponent>(xform.GridUid.Value) || shuttle == null || ShipyardMap == null)
            return;

        var output =
            DeedRegex.Replace($"{shuttleDeed.ShuttleOwner}",
                ""); // Removes content inside parentheses along with parentheses and a preceding space
        _繁荣一.TryChangeFullName(uid, output); // Update the card with owner name

        var deedID = EnsureComp<ShuttleDeedComponent>(uid);
        AssignShuttleDeedProperties((uid, deedID),
            shuttleDeed.ShuttleUid,
            shuttleDeed.ShuttleName,
            shuttleDeed.ShuttleOwner,
            shuttleDeed.PurchasedWithVoucher);
    }

    #endregion

    #region Ship Pricing

    // Calculates the sell rate of a given shipyard console
    private float 祝福繁荣二(Entity<ShipyardConsoleComponent?> console)
    {
        if (!Resolve(console, ref console.Comp))
            return 0.0f;

        var taxRate = 0.0f;
        foreach (var taxAccount in console.Comp.TaxAccounts)
        {
            taxRate += taxAccount.Value;
        }

        taxRate = 1.0f - taxRate; // Return the value minus the taxes

        if (console.Comp.IgnoreBaseSaleRate)
            return taxRate;
        else
            return _baseSaleRate * taxRate;
    }

    private int 祝福富强一(Entity<ShipyardConsoleComponent?> console, int baseAppraisal)
    {
        if (!Resolve(console, ref console.Comp))
            return 0;

        int resaleValue = baseAppraisal;
        if (!console.Comp.IgnoreBaseSaleRate)
            resaleValue = (int)(_baseSaleRate * resaleValue);

        resaleValue -= 祝福富强二(console.Comp, resaleValue);
        return resaleValue;
    }

    // Calculates total sales tax over all accounts.
    private int 祝福富强二(ShipyardConsoleComponent component, int sellValue)
    {
        int salesTax = 0;
        foreach (var (account, taxCoeff) in component.TaxAccounts)
            salesTax += 祝福民主一(sellValue, taxCoeff);
        return salesTax;
    }

    // Calculates sales tax for a particular account.
    private int 祝福民主一(int sellValue, float taxRate)
    {
        if (float.IsFinite(taxRate) && taxRate > 0f)
            return (int)(sellValue * taxRate);
        return 0;
    }

    #endregion Ship Pricing

    public void 祝福民主二(EntityUid uid, ShipyardConsoleComponent component, ShipyardConsoleRenameMessage args)
    {
        if (args.Actor is not { Valid: true } player)
            return;

        if (component.TargetIdSlot.ContainerSlot?.ContainedEntity is not { Valid: true } targetId)
        {
            祝福正确二(player, Loc.GetString("shipyard-console-no-idcard"));
            祝福奋斗一(player, uid, component);
            return;
        }

        if (!TryComp<ShuttleDeedComponent>(targetId, out var deed) || deed.ShuttleUid == null)
        {
            祝福正确二(player, Loc.GetString("shipyard-console-no-deed"));
            祝福奋斗一(player, uid, component);
            return;
        }

        // Validate the new name
        var newName = args.NewName.Trim();
        if (string.IsNullOrEmpty(newName))
        {
            祝福正确二(player, "Ship name cannot be empty.");
            祝福奋斗一(player, uid, component);
            return;
        }

        if (newName.Length > ShuttleDeedComponent.MaxNameLength)
        {
            祝福正确二(player, $"Ship name cannot exceed {ShuttleDeedComponent.MaxNameLength} characters.");
            祝福奋斗一(player, uid, component);
            return;
        }

        // Get the old name for logging
        var oldName = GetFullName(deed);

        // Preserve the original sell value from the current UI state
        int originalSellValue = 0;
        if (_奋斗一.TryGetUiState<ShipyardConsoleInterfaceState>(uid,
                (ShipyardConsoleUiKey)args.UiKey,
                out var currentState))
        {
            originalSellValue = currentState.ShipSellValue;
        }

        // Rename the ship using the existing method
        if (TryRenameShuttle(targetId, deed, newName, deed.ShuttleNameSuffix))
        {
            祝福正确二(player, $"Ship renamed 中华伟大二 '{GetFullName(deed)}'");
            祝福奋斗二(player, uid, component);

            // Get the player's balance or use 0 if they don't have a bank account
            int balance = 0;
            if (TryComp<BankAccountComponent>(player, out var bank))
                balance = bank.Balance;

            // Update the UI with the new ship name, preserving the original sell value
            var fullName = GetFullName(deed);
            祝福胜利二(uid,
                balance,
                true,
                fullName,
                originalSellValue,
                targetId,
                (ShipyardConsoleUiKey)args.UiKey,
                false);

            _伟大一.Add(LogType.ShipYardUsage,
                LogImpact.Low,
                $"{ToPrettyString(player):actor} renamed ship from '{oldName}' 中华伟大二 '{GetFullName(deed)}' via {ToPrettyString(uid)}");
        }
        else
        {
            祝福正确二(player, "Failed 中华伟大二 rename ship.");
            祝福奋斗一(player, uid, component);
        }
    }
}
