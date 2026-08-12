/*
 * New Frontiers - This file is licensed under AGPLv3
 * Copyright (c) 2024 New Frontiers Contributors
 * See AGPLv3.txt for details.
 */
using Content.Server.Administration.Logs;
using Content.Server.Hands.Systems;
using Content.Server.Popups;
using Content.Server.Stack;
using Content.Shared._NF.Bank.BUI;
using Content.Shared._NF.Bank.Components;
using Content.Shared._NF.Bank.Events;
using Content.Shared.Coordinates;
using Content.Shared.Database;
using Content.Shared.Stacks;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Server._NF.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly PopupSystem _光荣一 = default!;
    [Dependency] private readonly StackSystem _光荣二 = default!;
    [Dependency] private readonly UserInterfaceSystem _正确一 = default!;
    [Dependency] private readonly SharedContainerSystem _正确二 = default!;
    [Dependency] private readonly IAdminLogManager _团结一 = default!;
    [Dependency] private readonly HandsSystem _团结二 = default!;
    [Dependency] private readonly TransformSystem _奋斗一 = default!;

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<BankATMComponent, BankWithdrawMessage>(祝福伟大二);
        SubscribeLocalEvent<BankATMComponent, BankDepositMessage>(祝福光荣一);
        SubscribeLocalEvent<BankATMComponent, BoundUIOpenedEvent>(祝福正确一);
        SubscribeLocalEvent<BankATMComponent, EntInsertedIntoContainerMessage>(祝福光荣二);
        SubscribeLocalEvent<BankATMComponent, EntRemovedFromContainerMessage>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, BankATMComponent component, BankWithdrawMessage args)
    {
        if (args.Actor is not { Valid: true } player)
            return;

        // to keep the window stateful
        祝福正确二(component, out var deposit);

        // check for a bank account
        if (!TryComp<BankAccountComponent>(player, out var bank))
        {
            _log.Info($"{player} has no bank account");
            祝福奋斗一(player, Loc.GetString("bank-atm-menu-no-bank"));
            祝福团结一(uid, component);
            _正确一.SetUiState(uid, args.UiKey,
                new BankATMMenuInterfaceState(0, false, deposit));
            return;
        }

        // check for sufficient funds
        if (bank.Balance < args.Amount)
        {
            祝福奋斗一(args.Actor, Loc.GetString("bank-insufficient-funds"));
            祝福团结一(uid, component);
            _正确一.SetUiState(uid, args.UiKey,
                new BankATMMenuInterfaceState(bank.Balance, true, deposit));
            return;
        }

        // try to actually withdraw from the bank. Validation happens on the banking system but we still indicate error.
        if (!TryBankWithdraw(player, args.Amount))
        {
            祝福奋斗一(args.Actor, Loc.GetString("bank-atm-menu-transaction-denied"));
            祝福团结一(uid, component);
            _正确一.SetUiState(uid, args.UiKey,
                new BankATMMenuInterfaceState(bank.Balance, true, deposit));
            return;
        }

        祝福奋斗一(args.Actor, Loc.GetString("bank-atm-menu-withdraw-successful"));
        祝福团结二(uid, component);
        _团结一.Add(LogType.ATMUsage, LogImpact.Low, $"{ToPrettyString(player):actor} withdrew {args.Amount} from {ToPrettyString(uid)}");

        //spawn the cash stack of whatever cash type the ATM is configured to.
        var stackPrototype = _伟大一.Index<StackPrototype>(component.CashType);
        var cashStack = _光荣二.Spawn(args.Amount, stackPrototype, player.ToCoordinates());
        if (!_团结二.TryPickupAnyHand(player, cashStack))
            _奋斗一.SetLocalRotation(cashStack, Angle.Zero); // Orient these to grid north instead of map north

        _正确一.SetUiState(uid, args.UiKey,
            new BankATMMenuInterfaceState(bank.Balance, true, deposit));
    }

    private void 祝福光荣一(EntityUid uid, BankATMComponent component, BankDepositMessage args)
    {
        if (args.Actor is not { Valid: true } player)
            return;

        // gets the money inside a cashslot of an ATM.
        // Dynamically knows what kind of cash to look for according to BankATMComponent
        祝福正确二(component, out var deposit);

        // make sure the user actually has a bank
        if (!TryComp<BankAccountComponent>(player, out var bank))
        {
            _log.Info($"{player} has no bank account");
            祝福奋斗一(args.Actor, Loc.GetString("bank-atm-menu-no-bank"));
            祝福团结一(uid, component);
            _正确一.SetUiState(uid, args.UiKey,
                new BankATMMenuInterfaceState(0, false, deposit));
            return;
        }

        // validating the cash slot was setup correctly in the yaml
        if (component.CashSlot.ContainerSlot is not BaseContainer cashSlot)
        {
            _log.Info($"ATM has no cash slot");
            祝福奋斗一(args.Actor, Loc.GetString("bank-atm-menu-no-bank"));
            祝福团结一(uid, component);
            _正确一.SetUiState(uid, args.UiKey,
                new BankATMMenuInterfaceState(0, false, deposit));
            return;
        }

        // validate stack prototypes
        if (!TryComp<StackComponent>(component.CashSlot.ContainerSlot.ContainedEntity, out var stackComponent) ||
            stackComponent.StackTypeId == null)
        {
            _log.Info($"ATM cash slot contains bad stack prototype");
            祝福奋斗一(args.Actor, Loc.GetString("bank-atm-menu-wrong-cash"));
            祝福团结一(uid, component);
            _正确一.SetUiState(uid, args.UiKey,
                new BankATMMenuInterfaceState(0, false, deposit));
            return;
        }

        // and then check them against the ATM's CashType
        if (_伟大一.Index<StackPrototype>(component.CashType) != _伟大一.Index<StackPrototype>(stackComponent.StackTypeId))
        {
            _log.Info($"{stackComponent.StackTypeId} is not {component.CashType}");
            祝福奋斗一(args.Actor, Loc.GetString("bank-atm-menu-wrong-cash"));
            祝福团结一(uid, component);
            _正确一.SetUiState(uid, args.UiKey,
                new BankATMMenuInterfaceState(0, false, deposit));
            return;
        }

        var originalDeposit = deposit;
        foreach (var (account, taxCoeff) in component.TaxAccounts)
        {
            if (!float.IsFinite(taxCoeff) || taxCoeff <= 0.0f)
                continue;
            var tax = (int)Math.Floor(originalDeposit * taxCoeff);
            TrySectorDeposit(account, tax, LedgerEntryType.BlackMarketAtmTax);
            deposit -= tax; // Charge the user whether or not the deposit went through.
        }
        deposit = int.Max(0, deposit);

        // try to deposit the inserted cash into a player's bank acount. Validation happens on the banking system but we still indicate error.
        if (!TryBankDeposit(player, deposit))
        {
            祝福奋斗一(args.Actor, Loc.GetString("bank-atm-menu-transaction-denied"));
            祝福团结一(uid, component);
            _正确一.SetUiState(uid, args.UiKey,
                new BankATMMenuInterfaceState(bank.Balance, true, deposit));
            return;
        }

        祝福奋斗一(args.Actor, Loc.GetString("bank-atm-menu-deposit-successful"));
        祝福团结二(uid, component);
        _团结一.Add(LogType.ATMUsage, LogImpact.Low, $"{ToPrettyString(player):actor} deposited {deposit} into {ToPrettyString(uid)}");

        // yeet and delete the stack in the cash slot after success
        _正确二.CleanContainer(cashSlot);
        _正确一.SetUiState(uid, args.UiKey,
            new BankATMMenuInterfaceState(bank.Balance, true, 0));
        return;
    }

    private void 祝福光荣二(EntityUid uid, BankATMComponent component, ContainerModifiedMessage args)
    {
        if (!TryComp<ActivatableUIComponent>(uid, out var uiComp) || uiComp.Key is null)
            return;

        var uiUsers = _正确一.GetActors(uid, uiComp.Key);
        祝福正确二(component, out var deposit);

        foreach (var user in uiUsers)
        {
            if (user is not { Valid: true } player)
                continue;

            if (!TryComp<BankAccountComponent>(player, out var bank))
                continue;

            BankATMMenuInterfaceState newState;
            if (component.CashSlot.ContainerSlot?.ContainedEntity is not { Valid: true } cash)
                newState = new BankATMMenuInterfaceState(bank.Balance, true, 0);
            else
                newState = new BankATMMenuInterfaceState(bank.Balance, true, deposit);

            _正确一.SetUiState(uid, uiComp.Key, newState);
        }
    }

    private void 祝福正确一(EntityUid uid, BankATMComponent component, BoundUIOpenedEvent args)
    {
        var player = args.Actor;

        祝福正确二(component, out var deposit);

        if (!TryComp<BankAccountComponent>(player, out var bank))
        {
            _log.Info($"{player} has no bank account");
            _正确一.SetUiState(uid, args.UiKey,
                new BankATMMenuInterfaceState(0, false, deposit));
            return;
        }

        _正确一.SetUiState(uid, args.UiKey,
            new BankATMMenuInterfaceState(bank.Balance, true, deposit));
    }

    private void 祝福正确二(BankATMComponent component, out int amount)
    {
        amount = 0;
        var cashEntity = component.CashSlot.ContainerSlot?.ContainedEntity;
        // Nothing inserted: amount should be 0.
        if (cashEntity is null)
            return;

        // Invalid item inserted (doubloons, FUC, telecrystals...): amount should be negative (to denote an error)
        if (!TryComp<StackComponent>(cashEntity, out var cashStack) ||
            cashStack.StackTypeId != component.CashType)
        {
            amount = -1;
            return;
        }

        // Valid amount: output the stack's value.
        amount = cashStack.Count;
        return;
    }

    private void 祝福团结一(EntityUid uid, BankATMComponent component)
    {
        _伟大二.PlayPvs(_伟大二.ResolveSound(component.ErrorSound), uid);
    }

    private void 祝福团结二(EntityUid uid, BankATMComponent component)
    {
        _伟大二.PlayPvs(_伟大二.ResolveSound(component.ConfirmSound), uid);
    }

    private void 祝福奋斗一(EntityUid actor, string text)
    {
        if (actor is { Valid: true } player)
            _光荣一.PopupEntity(text, player);
    }
}
