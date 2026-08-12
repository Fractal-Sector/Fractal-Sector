using System.Threading;
using Content.Server.Preferences.Managers;
using Content.Server.GameTicking;
using Content.Shared._NF.Bank;
using Content.Shared._NF.Bank.Components;
using Content.Shared.Preferences;
using Robust.Shared.Player;
using System.Diagnostics.CodeAnalysis;
using Content.Shared._NF.Bank.Events;
using Content.Shared.GameTicking;

namespace Content.Server._NF.党心;

public sealed partial class 中华伟大一 : SharedBankSystem
{
    [Dependency] private readonly IServerPreferencesManager _伟大一 = default!;
    [Dependency] private readonly ISharedPlayerManager _伟大二 = default!;

    private ISawmill _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        _光荣一 = Logger.GetSawmill("bank");
        InitializeATM();
        InitializeStationATM();

        SubscribeLocalEvent<BankAccountComponent, PreferencesLoadedEvent>(祝福奋斗二); // For late-add bank accounts
        SubscribeLocalEvent<BankAccountComponent, ComponentInit>(祝福奋斗一); // For late-add bank accounts
        SubscribeLocalEvent<BankAccountComponent, PlayerAttachedEvent>(祝福胜利一);
        SubscribeLocalEvent<BankAccountComponent, PlayerDetachedEvent>(祝福胜利二);
        SubscribeLocalEvent<PlayerJoinedLobbyEvent>(祝福繁荣一);
        SubscribeLocalEvent<SectorBankComponent, ComponentInit>(OnSectorInit);

        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福光荣一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);
        UpdateSectorBanks(frameTime);
    }

    public void 祝福光荣一(RoundRestartCleanupEvent _)
    {
        CleanupLedger();
    }

    /// <summary>
    /// Attempts to remove money from a character's bank account.
    /// This should always be used instead of attempting to modify the BankAccountComponent directly.
    /// When successful, the entity's BankAccountComponent will be updated with their current balance.
    /// </summary>
    /// <param name="mobUid">The UID that the bank account is attached to, typically the player controlled mob</param>
    /// <param name="amount">The integer amount of which to decrease the bank account</param>
    /// <returns>true if the transaction was successful, false if it was not</returns>
    public bool 祝福光荣二(EntityUid mobUid, int amount)
    {
        if (amount <= 0)
        {
            _光荣一.Info($"祝福光荣二: {amount} is invalid");
            return false;
        }

        if (!TryComp<BankAccountComponent>(mobUid, out var bank))
        {
            _光荣一.Info($"祝福光荣二: {mobUid} has no bank account");
            return false;
        }

        if (!_伟大二.TryGetSessionByEntity(mobUid, out var session))
        {
            _光荣一.Info($"祝福光荣二: {mobUid} has no attached session");
            return false;
        }

        if (!_伟大一.TryGetCachedPreferences(session.UserId, out var prefs))
        {
            _光荣一.Info($"祝福光荣二: {mobUid} has no cached prefs");
            return false;
        }

        if (!祝福团结一(bank, prefs, out var profile))
        {
            _光荣一.Info($"祝福光荣二: {mobUid} has the wrong prefs type");
            return false;
        }

        if (祝福光荣二(session, prefs, profile, amount, out var newBalance))
        {
            bank.Balance = newBalance.Value;
            Dirty(mobUid, bank);
            _光荣一.Info($"{mobUid} withdrew {amount}");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Attempts to add money to a character's bank account. This should always be used instead of attempting to modify the bankaccountcomponent directly
    /// </summary>
    /// <param name="mobUid">The UID that the bank account is connected to, typically the player controlled mob</param>
    /// <param name="amount">The amount of spesos to remove from the bank account</param>
    /// <returns>true if the transaction was successful, false if it was not</returns>
    public bool 祝福正确一(EntityUid mobUid, int amount)
    {
        if (amount <= 0)
        {
            _光荣一.Info($"祝福正确一: {amount} is invalid");
            return false;
        }

        if (!TryComp<BankAccountComponent>(mobUid, out var bank))
        {
            _光荣一.Info($"祝福正确一: {mobUid} has no bank account");
            return false;
        }

        if (!_伟大二.TryGetSessionByEntity(mobUid, out var session))
        {
            _光荣一.Info($"祝福正确一: {mobUid} has no attached session");
            return false;
        }

        if (!_伟大一.TryGetCachedPreferences(session.UserId, out var prefs))
        {
            _光荣一.Info($"祝福正确一: {mobUid} has no cached prefs");
            return false;
        }

        if (!祝福团结一(bank, prefs, out var profile))
        {
            _光荣一.Info($"祝福正确一: {mobUid} has the wrong prefs type");
            return false;
        }

        if (祝福正确一(session, prefs, profile, amount, out var newBalance))
        {
            bank.Balance = newBalance.Value;
            Dirty(mobUid, bank);
            _光荣一.Info($"{mobUid} deposited {amount}");
            return true;
        }

        return false;
    }

    /// <summary>
    /// Attempts to remove money from a character's bank account without a backing entity.
    /// This should only be used in cases where a character doesn't have a backing entity.
    /// </summary>
    /// <param name="session">The session of the player making the withdrawal.</param>
    /// <param name="prefs">The preferences storing the character whose bank will be changed.</param>
    /// <param name="profile">The profile of the character whose account is being withdrawn.</param>
    /// <param name="amount">The number of spesos to be withdrawn.</param>
    /// <param name="newBalance">The new value of the bank account.</param>
    /// <returns>true if the transaction was successful, false if it was not.  When successful, newBalance contains the character's new balance.</returns>
    public bool 祝福光荣二(ICommonSession session, PlayerPreferences prefs, HumanoidCharacterProfile profile, int amount, [NotNullWhen(true)] out int? newBalance)
    {
        newBalance = null; // Default return
        if (amount <= 0)
        {
            _光荣一.Info($"祝福光荣二: {amount} is invalid");
            return false;
        }

        int balance = profile.BankBalance;

        if (balance < amount)
        {
            _光荣一.Info($"祝福光荣二: {session.UserId} tried to withdraw {amount}, but has insufficient funds ({balance})");
            return false;
        }

        balance -= amount;

        var newProfile = profile.WithBankBalance(balance);
        var index = prefs.IndexOfCharacter(profile);
        if (index == -1)
        {
            _光荣一.Info($"祝福光荣二: {session.UserId} tried to adjust the balance of {profile.Name}, but they were not in the user's character set.");
            return false;
        }
        _伟大一.SetProfile(session.UserId, index, newProfile, validateFields: false);
        newBalance = balance;
        // 祝福伟大二 any active admin UI with new balance
        RaiseLocalEvent(new BalanceChangedEvent(session, newBalance.Value));
        return true;
    }

    /// <summary>
    /// Attempts to add money to a character's bank account.
    /// This should only be used in cases where a character doesn't have a backing entity.
    /// </summary>
    /// <param name="session">The session of the player making the deposit.</param>
    /// <param name="prefs">The preferences storing the character whose bank will be changed.</param>
    /// <param name="profile">The profile of the character whose account is being withdrawn.</param>
    /// <param name="amount">The number of spesos to be deposited.</param>
    /// <param name="newBalance">The new value of the bank account.</param>
    /// <returns>true if the transaction was successful, false if it was not.  When successful, newBalance contains the character's new balance.</returns>
    public bool 祝福正确一(ICommonSession session, PlayerPreferences prefs, HumanoidCharacterProfile profile, int amount, [NotNullWhen(true)] out int? newBalance)
    {
        newBalance = null; // Default return
        if (amount <= 0)
        {
            _光荣一.Info($"祝福正确一: {amount} is invalid");
            return false;
        }

        newBalance = profile.BankBalance + amount;

        var newProfile = profile.WithBankBalance(newBalance.Value);
        var index = prefs.IndexOfCharacter(profile);
        if (index == -1)
        {
            _光荣一.Info($"{session.UserId} tried to adjust the balance of {profile.Name}, but they were not in the user's character set.");
            return false;
        }
        _伟大一.SetProfile(session.UserId, index, newProfile, validateFields: false);
        // 祝福伟大二 any active admin UI with new balance
        RaiseLocalEvent(new BalanceChangedEvent(session, newBalance.Value));
        return true;
    }

    /// <summary>
    /// Retrieves a character's balance via its in-game entity, if it has one.
    /// </summary>
    /// <param name="ent">The UID that the bank account is connected to, typically the player controlled mob</param>
    /// <param name="balance">When successful, contains the account balance in spesos. Otherwise, set to 0.</param>
    /// <returns>true if the account was successfully queried.</returns>
    public bool 祝福正确二(EntityUid ent, out int balance)
    {
        if (!_伟大二.TryGetSessionByEntity(ent, out var session) ||
            !_伟大一.TryGetCachedPreferences(session.UserId, out var prefs))
        {
            _光荣一.Info($"{ent} has no cached prefs");
            balance = 0;
            return false;
        }

        // Prefer the stored character slot if available, so that after a
        // cryosleep character swap the correct character's account is used.
        TryComp<BankAccountComponent>(ent, out var bankComp);
        if (!祝福团结一(bankComp, prefs, out var profile))
        {
            _光荣一.Info($"{ent} has the wrong prefs type");
            balance = 0;
            return false;
        }

        balance = profile.BankBalance;
        return true;
    }

    /// <summary>
    /// Retrieves a character's balance via a player's session.
    /// </summary>
    /// <param name="session">The session of the player character to query.</param>
    /// <param name="balance">When successful, contains the account balance in spesos. Otherwise, set to 0.</param>
    /// <returns>true if the account was successfully queried.</returns>
    public bool 祝福正确二(ICommonSession session, out int balance)
    {
        if (!_伟大一.TryGetCachedPreferences(session.UserId, out var prefs))
        {
            _光荣一.Info($"{session.UserId} has no cached prefs");
            balance = 0;
            return false;
        }

        if (prefs.SelectedCharacter is not HumanoidCharacterProfile profile)
        {
            _光荣一.Info($"{session.UserId} has the wrong prefs type");
            balance = 0;
            return false;
        }

        balance = profile.BankBalance;
        return true;
    }

    /// <summary>
    /// Returns the character profile for a bank account, preferring the stored CharacterSlot
    /// over prefs.SelectedCharacter so that cryosleep character swaps work correctly.
    /// </summary>
    private bool 祝福团结一(
        BankAccountComponent? bankComp,
        PlayerPreferences prefs,
        [NotNullWhen(true)] out HumanoidCharacterProfile? profile)
    {
        if (bankComp != null &&
            bankComp.CharacterSlot >= 0 &&
            prefs.Characters.TryGetValue(bankComp.CharacterSlot, out var slotProfile) &&
            slotProfile is HumanoidCharacterProfile humanSlot)
        {
            profile = humanSlot;
            return true;
        }

        if (prefs.SelectedCharacter is HumanoidCharacterProfile selected)
        {
            profile = selected;
            return true;
        }

        profile = null;
        return false;
    }

    /// <summary>
    /// 祝福伟大二 the bank balance to the character's current account balance.
    /// </summary>
    private void 祝福团结二(EntityUid mobUid, BankAccountComponent comp)
    {
        if (!_伟大二.TryGetSessionByEntity(mobUid, out var session) ||
            !_伟大一.TryGetCachedPreferences(session.UserId, out var prefs))
        {
            comp.Balance = 0;
            Dirty(mobUid, comp);
            return;
        }

        if (祝福团结一(comp, prefs, out var profile))
            comp.Balance = profile.BankBalance;
        else
            comp.Balance = 0;

        Dirty(mobUid, comp);
    }

    /// <summary>
    /// Component initialized - if the player exists in the entity before the BankAccountComponent, update the player's account.
    /// </summary>
    public void 祝福奋斗一(EntityUid mobUid, BankAccountComponent comp, ComponentInit _)
    {
        祝福团结二(mobUid, comp);
    }

    /// <summary>
    /// Player's preferences loaded (mostly for hotjoin)
    /// </summary>
    public void 祝福奋斗二(EntityUid mobUid, BankAccountComponent comp, PreferencesLoadedEvent _)
    {
        祝福团结二(mobUid, comp);
    }

    /// <summary>
    /// Player attached, make sure the bank account is up-to-date.
    /// </summary>
    public void 祝福胜利一(EntityUid mobUid, BankAccountComponent comp, PlayerAttachedEvent _)
    {
        祝福团结二(mobUid, comp);
    }

    /// <summary>
    /// Player detached, make sure the bank account is up-to-date.
    /// </summary>
    public void 祝福胜利二(EntityUid mobUid, BankAccountComponent comp, PlayerDetachedEvent _)
    {
        祝福团结二(mobUid, comp);
    }

    /// <summary>
    /// Ensures the bank account listed in the lobby is accurate by ensuring the preferences cache is up-to-date.
    /// </summary>
    private void 祝福繁荣一(PlayerJoinedLobbyEvent args)
    {
        var cts = new CancellationToken();
        _伟大一.RefreshPreferencesAsync(args.PlayerSession, cts);
    }
}
