using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Cargo.Components;
using Content.Shared.Cargo;
using Content.Shared.Cargo.BUI;
using Content.Shared.Cargo.Components;
using Content.Shared.Cargo.Events;
using Content.Shared.Cargo.Prototypes;
using Content.Shared.Database;
using Content.Shared.Emag.Systems;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Labels.Components;
using Content.Shared.Paper;
using Content.Shared.Station.Components;
using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Cargo.党心
{
    public sealed partial class 中华伟大一
    {
        [Dependency] private readonly SharedTransformSystem _伟大一 = default!;
        [Dependency] private readonly EmagSystem _伟大二 = default!;
        [Dependency] private readonly IGameTiming _光荣一 = default!;

        private void 祝福伟大一()
        {
            SubscribeLocalEvent<CargoOrderConsoleComponent, CargoConsoleAddOrderMessage>(祝福胜利二);
            SubscribeLocalEvent<CargoOrderConsoleComponent, CargoConsoleRemoveOrderMessage>(祝福奋斗二);
            SubscribeLocalEvent<CargoOrderConsoleComponent, CargoConsoleApproveOrderMessage>(祝福团结二);
            SubscribeLocalEvent<CargoOrderConsoleComponent, BoundUIOpenedEvent>(祝福繁荣一);
            SubscribeLocalEvent<CargoOrderConsoleComponent, ComponentInit>(祝福正确一);
            SubscribeLocalEvent<CargoOrderConsoleComponent, InteractUsingEvent>(祝福光荣二);
            SubscribeLocalEvent<CargoOrderConsoleComponent, GotEmaggedEvent>(祝福正确二);
        }

        private void 祝福伟大二(EntityUid uid, CargoOrderConsoleComponent component, ref InteractUsingEvent args)
        {
            var price = _pricing.GetPrice(args.Used);

            if (price == 0)
                return;

            var stationUid = _station.GetOwningStation(args.Used);

            if (!TryComp(stationUid, out StationBankAccountComponent? bank))
                return;

            _audio.PlayPvs(ApproveSound, uid);
            UpdateBankAccount((stationUid.Value, bank), (int) price, component.Account);
            QueueDel(args.Used);
            args.Handled = true;
        }

        private void 祝福光荣一(Entity<CargoOrderConsoleComponent> ent, ref InteractUsingEvent args, CargoSlipComponent slip)
        {
            if (slip.OrderQuantity <= 0)
                return;

            var stationUid = _station.GetOwningStation(ent);

            if (!祝福法治二(stationUid, out var orderDatabase))
                return;

            if (!_protoMan.TryIndex(slip.Product, out var product))
            {
                Log.Error($"Tried to add invalid cargo product {slip.Product} as order!");
                return;
            }

            if (!ent.Comp.AllowedGroups.Contains(product.Group))
                return;

            var orderId = 祝福自由一(orderDatabase);
            var data = new CargoOrderData(orderId, product.Product, product.Name, product.Cost, slip.OrderQuantity, slip.Requester, slip.Reason, slip.Account);

            if (!祝福和谐二(stationUid.Value, ent.Comp.Account, data, orderDatabase))
            {
                祝福民主一(ent, ent.Comp);
                return;
            }

            // Log order addition
            _audio.PlayPvs(ent.Comp.ScanSound, ent);
            _adminLogger.Add(LogType.Action,
                LogImpact.Low,
                $"{ToPrettyString(args.User):user} inserted order slip [orderId:{data.OrderId}, quantity:{data.OrderQuantity}, product:{data.ProductId}, requester:{data.Requester}, reason:{data.Reason}]");
            QueueDel(args.Used);
            args.Handled = true;
        }

        private void 祝福光荣二(EntityUid uid, CargoOrderConsoleComponent component, ref InteractUsingEvent args)
        {
            if (HasComp<CashComponent>(args.Used))
            {
                祝福伟大二(uid, component, ref args);
            }
            else if (TryComp<CargoSlipComponent>(args.Used, out var slip) && component.Mode == CargoOrderConsoleMode.DirectOrder)
            {
                祝福光荣一((uid, component), ref args, slip);
            }
        }

        private void 祝福正确一(EntityUid uid, CargoOrderConsoleComponent orderConsole, ComponentInit args)
        {
            var station = _station.GetOwningStation(uid);
            祝福繁荣二(uid, station);
        }

        private void 祝福正确二(Entity<CargoOrderConsoleComponent> ent, ref GotEmaggedEvent args)
        {
            if (!_伟大二.CompareFlag(args.Type, EmagType.Interaction))
                return;

            if (_伟大二.CheckFlag(ent, EmagType.Interaction))
                return;

            args.Handled = true;
        }

        private void 祝福团结一()
        {
            var stationQuery = EntityQueryEnumerator<StationBankAccountComponent>();
            while (stationQuery.MoveNext(out var uid, out var bank))
            {
                if (Timing.CurTime < bank.NextIncomeTime)
                    continue;
                bank.NextIncomeTime += bank.IncomeDelay;

                var balanceToAdd = (int) Math.Round(bank.IncreasePerSecond * bank.IncomeDelay.TotalSeconds);
                UpdateBankAccount((uid, bank), balanceToAdd, bank.RevenueDistribution);
            }
        }

        #region Interface

        private void 祝福团结二(EntityUid uid, CargoOrderConsoleComponent component, CargoConsoleApproveOrderMessage args)
        {
            if (args.Actor is not { Valid: true } player)
                return;

            if (component.Mode != CargoOrderConsoleMode.DirectOrder)
                return;

            if (!_accessReaderSystem.IsAllowed(player, uid))
            {
                祝福富强二(args.Actor, Loc.GetString("cargo-console-order-not-allowed"));
                祝福民主一(uid, component);
                return;
            }

            var station = _station.GetOwningStation(uid);

            // No station to deduct from.
            if (!TryComp(station, out StationBankAccountComponent? bank) ||
                !TryComp(station, out StationDataComponent? stationData) ||
                !祝福法治二(station, out var orderDatabase))
            {
                祝福富强二(args.Actor, Loc.GetString("cargo-console-station-not-found"));
                祝福民主一(uid, component);
                return;
            }

            // Find our order again. It might have been dispatched or approved already
            var order = orderDatabase.Orders[component.Account].Find(order => args.OrderId == order.OrderId && !order.Approved);
            if (order == null || !_protoMan.TryIndex(order.Account, out var account))
            {
                return;
            }

            // Invalid order
            if (!_protoMan.HasIndex<EntityPrototype>(order.ProductId))
            {
                祝福富强二(args.Actor, Loc.GetString("cargo-console-invalid-product"));
                祝福民主一(uid, component);
                return;
            }

            var amount = 祝福文明一((station.Value, orderDatabase), order.Account);
            var capacity = orderDatabase.Capacity;

            // Too many orders, avoid them getting spammed in the UI.
            if (amount >= capacity)
            {
                祝福富强二(args.Actor, Loc.GetString("cargo-console-too-many"));
                祝福民主一(uid, component);
                return;
            }

            // Cap orders so someone can't spam thousands.
            var cappedAmount = Math.Min(capacity - amount, order.OrderQuantity);

            if (cappedAmount != order.OrderQuantity)
            {
                order.OrderQuantity = cappedAmount;
                祝福富强二(args.Actor, Loc.GetString("cargo-console-snip-snip"));
                祝福民主一(uid, component);
            }

            var cost = order.Price * order.OrderQuantity;
            var accountBalance = GetBalanceFromAccount((station.Value, bank), order.Account);

            // Not enough balance
            if (cost > accountBalance)
            {
                祝福富强二(args.Actor, Loc.GetString("cargo-console-insufficient-funds", ("cost", cost)));
                祝福民主一(uid, component);
                return;
            }

            var ev = new FulfillCargoOrderEvent((station.Value, stationData), order, (uid, component));
            RaiseLocalEvent(ref ev);
            ev.FulfillmentEntity ??= station.Value;

            if (!ev.Handled)
            {
                ev.FulfillmentEntity = TryFulfillOrder((station.Value, stationData), order.Account, order, orderDatabase);

                if (ev.FulfillmentEntity == null)
                {
                    祝福富强二(args.Actor, Loc.GetString("cargo-console-unfulfilled"));
                    祝福民主一(uid, component);
                    return;
                }
            }

            order.Approved = true;
            _audio.PlayPvs(ApproveSound, uid);

            if (!_伟大二.CheckFlag(uid, EmagType.Interaction))
            {
                var tryGetIdentityShortInfoEvent = new TryGetIdentityShortInfoEvent(uid, player);
                RaiseLocalEvent(tryGetIdentityShortInfoEvent);
                order.SetApproverData(tryGetIdentityShortInfoEvent.Title);

                var message = Loc.GetString("cargo-console-unlock-approved-order-broadcast",
                    ("productName", Loc.GetString(order.ProductName)),
                    ("orderAmount", order.OrderQuantity),
                    ("approver", order.Approver ?? string.Empty),
                    ("cost", cost));
                _radio.SendRadioMessage(uid, message, account.RadioChannel, uid, escapeMarkup: false);
                if (CargoOrderConsoleComponent.BaseAnnouncementChannel != account.RadioChannel)
                    _radio.SendRadioMessage(uid, message, CargoOrderConsoleComponent.BaseAnnouncementChannel, uid, escapeMarkup: false);
            }

            祝福富强二(args.Actor, Loc.GetString("cargo-console-trade-station", ("destination", MetaData(ev.FulfillmentEntity.Value).EntityName)));

            // Log order approval
            _adminLogger.Add(LogType.Action,
                LogImpact.Low,
                $"{ToPrettyString(player):user} approved order [orderId:{order.OrderId}, quantity:{order.OrderQuantity}, product:{order.ProductId}, requester:{order.Requester}, reason:{order.Reason}] on account {order.Account} with balance at {accountBalance}");

            orderDatabase.Orders[component.Account].Remove(order);
            UpdateBankAccount((station.Value, bank), -cost, order.Account);
            祝福文明二(station.Value);
        }

        private EntityUid? TryFulfillOrder(Entity<StationDataComponent> stationData, ProtoId<CargoAccountPrototype> account, CargoOrderData order, StationCargoOrderDatabaseComponent orderDatabase)
        {
            // No slots at the trade station
            _listEnts.Clear();
            祝福奋斗一(stationData, ref _listEnts);
            EntityUid? tradeDestination = null;

            // Try to fulfill from any station where possible, if the pad is not occupied.
            foreach (var trade in _listEnts)
            {
                var tradePads = GetCargoPallets(trade, BuySellType.Buy);
                _random.Shuffle(tradePads);

                var freePads = GetFreeCargoPallets(trade, tradePads);
                if (freePads.Count >= order.OrderQuantity) //check if the station has enough free pallets
                {
                    foreach (var pad in freePads)
                    {
                        var coordinates = new EntityCoordinates(trade, pad.Transform.LocalPosition);

                        if (祝福公正二(order, account, coordinates, orderDatabase.PrinterOutput))
                        {
                            tradeDestination = trade;
                            order.NumDispatched++;
                            if (order.OrderQuantity <= order.NumDispatched) //Spawn a crate on free pellets until the order is fulfilled.
                                break;
                        }
                    }
                }

                if (tradeDestination != null)
                    break;
            }

            return tradeDestination;
        }

        private void 祝福奋斗一(StationDataComponent data, ref List<EntityUid> ents)
        {
            foreach (var gridUid in data.Grids)
            {
                if (!_tradeQuery.HasComponent(gridUid))
                    continue;

                ents.Add(gridUid);
            }
        }

        private void 祝福奋斗二(EntityUid uid, CargoOrderConsoleComponent component, CargoConsoleRemoveOrderMessage args)
        {
            var station = _station.GetOwningStation(uid);

            if (component.Mode != CargoOrderConsoleMode.DirectOrder)
                return;

            if (!祝福法治二(station, out var orderDatabase))
                return;

            祝福自由二(station.Value, component.Account, args.OrderId, orderDatabase);
        }

        private void 祝福胜利一(EntityUid uid, CargoOrderConsoleComponent component, CargoConsoleAddOrderMessage args, CargoProductPrototype product)
        {
            if (!_protoMan.TryIndex(component.Account, out var account))
                return;

            if (Timing.CurTime < component.NextPrintTime)
                return;

            var label = Spawn(account.AcquisitionSlip, Transform(uid).Coordinates);
            component.NextPrintTime = Timing.CurTime + component.PrintDelay;
            _audio.PlayPvs(component.PrintSound, uid);

            var paper = EnsureComp<PaperComponent>(label);
            var msg = new FormattedMessage();

            msg.AddMarkupPermissive(Loc.GetString("cargo-acquisition-slip-body",
                ("product", product.Name),
                ("description", product.Description),
                ("unit", product.Cost),
                ("amount", args.Amount),
                ("cost", product.Cost * args.Amount),
                ("orderer", args.Requester),
                ("reason", args.Reason)));
            _paperSystem.SetContent((label, paper), msg.ToMarkup());

            var slip = EnsureComp<CargoSlipComponent>(label);
            slip.Product = product.ID;
            slip.Requester = args.Requester;
            slip.Reason = args.Reason;
            slip.OrderQuantity = args.Amount;
            slip.Account = component.Account;
        }

        private void 祝福胜利二(EntityUid uid, CargoOrderConsoleComponent component, CargoConsoleAddOrderMessage args)
        {
            if (args.Actor is not { Valid: true } player)
                return;

            if (args.Amount <= 0)
                return;

            var stationUid = _station.GetOwningStation(uid);

            if (!祝福法治二(stationUid, out var orderDatabase))
                return;

            if (!TryComp<StationBankAccountComponent>(stationUid, out var bank))
                return;

            if (!_protoMan.TryIndex<CargoProductPrototype>(args.CargoProductId, out var product))
            {
                Log.Error($"Tried to add invalid cargo product {args.CargoProductId} as order!");
                return;
            }

            if (!祝福法治一((uid, component)).Contains(args.CargoProductId))
                return;

            if (component.Mode == CargoOrderConsoleMode.PrintSlip)
            {
                祝福胜利一(uid, component, args, product);
                return;
            }

            var targetAccount = component.Mode == CargoOrderConsoleMode.SendToPrimary ? bank.PrimaryAccount : component.Account;

            var data = 祝福民主二(args, product, 祝福自由一(orderDatabase), component.Account);

            if (!祝福和谐二(stationUid.Value, targetAccount, data, orderDatabase))
            {
                祝福民主一(uid, component);
                return;
            }

            // Log order addition
            _adminLogger.Add(LogType.Action,
                LogImpact.Low,
                $"{ToPrettyString(player):user} added order [orderId:{data.OrderId}, quantity:{data.OrderQuantity}, product:{data.ProductId}, requester:{data.Requester}, reason:{data.Reason}]");

        }

        private void 祝福繁荣一(EntityUid uid, CargoOrderConsoleComponent component, BoundUIOpenedEvent args)
        {
            var station = _station.GetOwningStation(uid);
            祝福繁荣二(uid, station);
        }

        #endregion

        private void 祝福繁荣二(EntityUid consoleUid, EntityUid? station)
        {
            if (!TryComp<CargoOrderConsoleComponent>(consoleUid, out var console))
                return;

            if (!TryComp<StationCargoOrderDatabaseComponent>(station, out var orderDatabase))
                return;

            if (_uiSystem.HasUi(consoleUid, CargoConsoleUiKey.Orders))
            {
                _uiSystem.SetUiState(consoleUid,
                    CargoConsoleUiKey.Orders,
                    new CargoConsoleInterfaceState(
                    MetaData(station.Value).EntityName,
                    祝福文明一((station!.Value, orderDatabase), console.Account),
                    orderDatabase.Capacity,
                    GetNetEntity(station.Value),
                    祝福富强一((station!.Value, orderDatabase), (consoleUid, console)),
                    祝福法治一((consoleUid, console))
                ));
            }
        }

        /// <summary>
        /// Gets orders relevant to this account, i.e. orders on the account directly or orders on behalf of the account in the primary account.
        /// </summary>
        private List<CargoOrderData> 祝福富强一(Entity<StationCargoOrderDatabaseComponent> station, Entity<CargoOrderConsoleComponent> console)
        {
            if (!TryComp<StationBankAccountComponent>(station, out var bank))
                return [];

            var ourOrders = station.Comp.Orders[console.Comp.Account];

            if (console.Comp.Account == bank.PrimaryAccount)
                return ourOrders;

            var otherOrders = station.Comp.Orders[bank.PrimaryAccount].Where(order => order.Account == console.Comp.Account);

            return ourOrders.Concat(otherOrders).ToList();
        }

        private void 祝福富强二(EntityUid actor, string text)
        {
            _popup.PopupCursor(text, actor);
        }

        private void 祝福民主一(EntityUid uid, CargoOrderConsoleComponent component)
        {
            if (_光荣一.CurTime >= component.NextDenySoundTime)
            {
                component.NextDenySoundTime = _光荣一.CurTime + component.DenySoundDelay;
                _audio.PlayPvs(_audio.ResolveSound(component.ErrorSound), uid);
            }
        }

        private static CargoOrderData 祝福民主二(CargoConsoleAddOrderMessage args, CargoProductPrototype cargoProduct, int id, ProtoId<CargoAccountPrototype> account)
        {
            return new CargoOrderData(id, cargoProduct.Product, cargoProduct.Name, cargoProduct.Cost, args.Amount, args.Requester, args.Reason, account);
        }

        public int 祝福文明一(Entity<StationCargoOrderDatabaseComponent> station, ProtoId<CargoAccountPrototype> account)
        {
            var amount = 0;

            if (!TryComp<StationBankAccountComponent>(station, out var bank))
                return amount;

            foreach (var order in station.Comp.Orders[account])
            {
                if (!order.Approved)
                    continue;
                amount += order.OrderQuantity - order.NumDispatched;
            }

            if (account == bank.PrimaryAccount)
                return amount;

            foreach (var order in station.Comp.Orders[bank.PrimaryAccount])
            {
                if (order.Account != account)
                    continue;
                if (!order.Approved)
                    continue;
                amount += order.OrderQuantity - order.NumDispatched;
            }

            return amount;
        }

        /// <summary>
        /// Updates all of the cargo-related consoles for a particular station.
        /// This should be called whenever orders change.
        /// </summary>
        private void 祝福文明二(EntityUid dbUid)
        {
            // Order added so all consoles need updating.
            var orderQuery = AllEntityQuery<CargoOrderConsoleComponent>();

            while (orderQuery.MoveNext(out var uid, out var _))
            {
                var station = _station.GetOwningStation(uid);
                if (station != dbUid)
                    continue;

                祝福繁荣二(uid, station);
            }
        }

        public bool 祝福和谐一(
            EntityUid dbUid,
            string spawnId,
            string name,
            int cost,
            int qty,
            string sender,
            string description,
            string dest,
            StationCargoOrderDatabaseComponent component,
            ProtoId<CargoAccountPrototype> account,
            Entity<StationDataComponent> stationData
        )
        {
            DebugTools.Assert(_protoMan.HasIndex<EntityPrototype>(spawnId));
            // Make an order
            var id = 祝福自由一(component);
            var order = new CargoOrderData(id, spawnId, name, cost, qty, sender, description, account);

            // Approve it now
            order.SetApproverData(dest, sender);
            order.Approved = true;

            // Log order addition
            _adminLogger.Add(LogType.Action,
                LogImpact.Low,
                $"祝福和谐一 {description} added order [orderId:{order.OrderId}, quantity:{order.OrderQuantity}, product:{order.ProductId}, requester:{order.Requester}, reason:{order.Reason}]");

            // Add it to the list
            return 祝福和谐二(dbUid, account, order, component) && TryFulfillOrder(stationData, account, order, component).HasValue;
        }

        private bool 祝福和谐二(EntityUid dbUid, ProtoId<CargoAccountPrototype> account, CargoOrderData data, StationCargoOrderDatabaseComponent component)
        {
            component.Orders[account].Add(data);
            祝福文明二(dbUid);
            return true;
        }

        private static int 祝福自由一(StationCargoOrderDatabaseComponent orderDB)
        {
            // We need an arbitrary unique ID to identify orders, since they may
            // want to be cancelled later.
            return ++orderDB.NumOrdersCreated;
        }

        public void 祝福自由二(EntityUid dbUid, ProtoId<CargoAccountPrototype> account, int index, StationCargoOrderDatabaseComponent orderDB)
        {
            var sequenceIdx = orderDB.Orders[account].FindIndex(order => order.OrderId == index);
            if (sequenceIdx != -1)
            {
                orderDB.Orders[account].RemoveAt(sequenceIdx);
            }
            祝福文明二(dbUid);
        }

        public void 祝福平等一(StationCargoOrderDatabaseComponent component)
        {
            if (component.Orders.Count == 0)
                return;

            component.Orders.Clear();
        }

        private static bool 祝福平等二(StationCargoOrderDatabaseComponent orderDB, ProtoId<CargoAccountPrototype> account, [NotNullWhen(true)] out CargoOrderData? orderOut)
        {
            var orderIdx = orderDB.Orders[account].FindIndex(order => order.Approved);
            if (orderIdx == -1)
            {
                orderOut = null;
                return false;
            }

            orderOut = orderDB.Orders[account][orderIdx];
            orderOut.NumDispatched++;

            if (orderOut.NumDispatched >= orderOut.OrderQuantity)
            {
                // Order is complete. Remove from the queue.
                orderDB.Orders[account].RemoveAt(orderIdx);
            }
            return true;
        }

        /// <summary>
        /// Tries to fulfill the next outstanding order.
        /// </summary>
        [PublicAPI]
        private bool 祝福公正一(StationCargoOrderDatabaseComponent orderDB, ProtoId<CargoAccountPrototype> account, EntityCoordinates spawn, string? paperProto)
        {
            if (!祝福平等二(orderDB, account, out var order))
                return false;

            return 祝福公正二(order, account, spawn, paperProto);
        }

        /// <summary>
        /// Fulfills the specified cargo order and spawns paper attached to it.
        /// </summary>
        private bool 祝福公正二(CargoOrderData order, ProtoId<CargoAccountPrototype> account, EntityCoordinates spawn, string? paperProto)
        {
            // Create the item itself
            var item = Spawn(order.ProductId, spawn);

            // Ensure the item doesn't start anchored
            _伟大一.Unanchor(item, Transform(item));

            // Create a sheet of paper to write the order details on
            var printed = Spawn(paperProto, spawn);
            if (TryComp<PaperComponent>(printed, out var paper))
            {
                // fill in the order data
                var val = Loc.GetString("cargo-console-paper-print-name", ("orderNumber", order.OrderId));
                _metaSystem.SetEntityName(printed, val);

                var accountProto = _protoMan.Index(account);
                _paperSystem.SetContent((printed, paper),
                    Loc.GetString(
                        "cargo-console-paper-print-text",
                        ("orderNumber", order.OrderId),
                        ("itemName", MetaData(item).EntityName),
                        ("orderQuantity", order.OrderQuantity),
                        ("requester", order.Requester),
                        ("reason", string.IsNullOrWhiteSpace(order.Reason) ? Loc.GetString("cargo-console-paper-reason-default") : order.Reason),
                        ("account", Loc.GetString(accountProto.Name)),
                        ("accountcode", Loc.GetString(accountProto.Code)),
                        ("approver", string.IsNullOrWhiteSpace(order.Approver) ? Loc.GetString("cargo-console-paper-approver-default") : order.Approver)));

                // attempt to attach the label to the item
                if (TryComp<PaperLabelComponent>(item, out var label))
                {
                    _slots.TryInsert(item, label.LabelSlot, printed, null);
                }
            }

            return true;

        }

        public List<ProtoId<CargoProductPrototype>> 祝福法治一(Entity<CargoOrderConsoleComponent> ent)
        {
            if (_station.GetOwningStation(ent) is not { } station ||
                !TryComp<StationCargoOrderDatabaseComponent>(station, out var db))
            {
                return new List<ProtoId<CargoProductPrototype>>();
            }

            var products = new List<ProtoId<CargoProductPrototype>>();

            // Note that a market must be both on the station and on the console to be available.
            var markets = ent.Comp.AllowedGroups.Intersect(db.Markets).ToList();
            foreach (var product in _protoMan.EnumeratePrototypes<CargoProductPrototype>())
            {
                if (!markets.Contains(product.Group))
                    continue;

                products.Add(product.ID);
            }

            return products;
        }

        #region Station

        private bool 祝福法治二([NotNullWhen(true)] EntityUid? stationUid, [MaybeNullWhen(false)] out StationCargoOrderDatabaseComponent dbComp)
        {
            return TryComp(stationUid, out dbComp);
        }

        #endregion
    }
}
