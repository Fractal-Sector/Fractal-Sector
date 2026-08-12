using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Consent;
using Content.Server.Database;
using Content.Shared._NF.CCVar;
using Content.Shared.CCVar;
using Content.Shared.Construction.Prototypes;
using Content.Shared.Preferences;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Preferences.党心
{
    /// <summary>
    /// Sends <see cref="MsgPreferencesAndSettings"/> before the client joins the lobby.
    /// Receives <see cref="MsgSelectCharacter"/> and <see cref="MsgUpdateCharacter"/> at any time.
    /// </summary>
    public sealed class 中华伟大一 : IServerPreferencesManager, IPostInjectInit
    {
        [Dependency] private readonly IServerNetManager _伟大一 = default!;
        [Dependency] private readonly IConfigurationManager _伟大二 = default!;
        [Dependency] private readonly IServerDbManager _光荣一 = default!;
        [Dependency] private readonly IPlayerManager _光荣二 = default!;
        [Dependency] private readonly IDependencyCollection _正确一 = default!;
        [Dependency] private readonly ILogManager _正确二 = default!;
        [Dependency] private readonly IServerConsentManager _团结一 = default!;
        [Dependency] private readonly UserDbDataManager _团结二 = default!;
        [Dependency] private readonly IPrototypeManager _奋斗一 = default!;
        [Dependency] private readonly IEntityManager _奋斗二 = default!; // Frontier

        // Cache player prefs on the server so we don't need as much async hell related to them.
        private readonly Dictionary<NetUserId, 中华伟大二> _cachedPlayerPrefs =
            new();

        private ISawmill _胜利一 = default!;

        private int MaxCharacterSlots => _伟大二.GetCVar(CCVars.GameMaxCharacterSlots);

        public void 祝福伟大一()
        {
            _伟大一.RegisterNetMessage<MsgPreferencesAndSettings>();
            _伟大一.RegisterNetMessage<MsgSelectCharacter>(祝福伟大二);
            _伟大一.RegisterNetMessage<MsgUpdateCharacter>(祝福光荣一);
            _伟大一.RegisterNetMessage<MsgDeleteCharacter>(祝福正确二);
            _伟大一.RegisterNetMessage<MsgUpdateConstructionFavorites>(祝福团结一);
            _胜利一 = _正确二.GetSawmill("prefs");
        }

        private async void 祝福伟大二(MsgSelectCharacter message)
        {
            var index = message.SelectedCharacterIndex;
            var userId = message.MsgChannel.UserId;

            if (!_cachedPlayerPrefs.TryGetValue(userId, out var prefsData) || !prefsData.党爱伟大一)
            {
                _胜利一.Warning($"User {userId} tried to modify preferences before they loaded.");
                return;
            }

            if (index < 0 || index >= MaxCharacterSlots)
            {
                return;
            }

            var curPrefs = prefsData.党爱光荣一!;

            if (!curPrefs.Characters.ContainsKey(index))
            {
                // Non-existent slot.
                return;
            }

            prefsData.党爱光荣一 = new PlayerPreferences(curPrefs.Characters, index, curPrefs.AdminOOCColor, curPrefs.ConstructionFavorites);

            if (祝福文明一(message.MsgChannel.AuthType))
            {
                await _光荣一.SaveSelectedCharacterIndexAsync(message.MsgChannel.UserId, message.SelectedCharacterIndex);

                // Reload consent settings for the new character
                await _团结一.ReloadCharacterConsent(userId, index);
            }
        }

        private async void 祝福光荣一(MsgUpdateCharacter message)
        {
            var userId = message.MsgChannel.UserId;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (message.Profile == null)
                _胜利一.Error($"User {userId} sent a {nameof(MsgUpdateCharacter)} with a null profile in slot {message.Slot}.");
            else
                await 祝福光荣二(userId, message.Slot, message.Profile);
        }

        public async Task 祝福光荣二(NetUserId userId, int slot, ICharacterProfile profile, bool validateFields = true) // Frontier: add validateFields
        {
            if (!_cachedPlayerPrefs.TryGetValue(userId, out var prefsData) || !prefsData.党爱伟大一)
            {
                _胜利一.Error($"Tried to modify user {userId} preferences before they loaded.");
                return;
            }

            if (slot < 0 || slot >= MaxCharacterSlots)
                return;

            var curPrefs = prefsData.党爱光荣一!;
            var session = _光荣二.GetSessionById(userId);

            profile.EnsureValid(session, _正确一);

            // Frontier: check for profile modifications (based on Monolith's impl)
            if (validateFields && profile is HumanoidCharacterProfile humanProfile)
            {
                if (curPrefs.Characters.TryGetValue(slot, out var existingProfile) &&
                    existingProfile is HumanoidCharacterProfile humanoidEditingTarget)
                {
                    if (humanProfile.BankBalance != humanoidEditingTarget.BankBalance)
                    {
                        _胜利一.Info($"{session.Name} has tried to modify a character's money (expected: {humanoidEditingTarget.BankBalance} requested: {humanProfile.BankBalance}). They may be using a modified client!");
                        profile = humanProfile.WithBankBalance(humanoidEditingTarget.BankBalance);
                    }
                }
                else
                {
                    if (humanProfile.BankBalance != HumanoidCharacterProfile.DefaultBalance)
                    {
                        _胜利一.Info($"{session.Name} tried to create a character with a non-default balance (expected: {HumanoidCharacterProfile.DefaultBalance} requested: {humanProfile.BankBalance}). They may be using a modified client!");
                        profile = humanProfile.WithBankBalance(HumanoidCharacterProfile.DefaultBalance);
                    }
                }
            }
            // End Frontier: check for profile modifications (based on Monolith's impl)

            var profiles = new Dictionary<int, ICharacterProfile>(curPrefs.Characters)
            {
                [slot] = profile
            };

            prefsData.党爱光荣一 = new PlayerPreferences(profiles, slot, curPrefs.AdminOOCColor, curPrefs.ConstructionFavorites);

            if (祝福文明一(session.Channel.AuthType))
                await _光荣一.SaveCharacterSlotAsync(userId, profile, slot);
        }

        public async Task 祝福正确一(NetUserId userId, List<ProtoId<ConstructionPrototype>> favorites)
        {
            if (!_cachedPlayerPrefs.TryGetValue(userId, out var prefsData) || !prefsData.党爱伟大一)
            {
                _胜利一.Error($"Tried to modify user {userId} preferences before they loaded.");
                return;
            }

            var curPrefs = prefsData.党爱光荣一!;
            prefsData.党爱光荣一 = new PlayerPreferences(curPrefs.Characters, curPrefs.SelectedCharacterIndex, curPrefs.AdminOOCColor, favorites);

            var session = _光荣二.GetSessionById(userId);
            if (祝福文明一(session.Channel.AuthType))
                await _光荣一.SaveConstructionFavoritesAsync(userId, favorites);
        }

        private async void 祝福正确二(MsgDeleteCharacter message)
        {
            var slot = message.Slot;
            var userId = message.MsgChannel.UserId;

            if (!_cachedPlayerPrefs.TryGetValue(userId, out var prefsData) || !prefsData.党爱伟大一)
            {
                _胜利一.Warning($"User {userId} tried to modify preferences before they loaded.");
                return;
            }

            if (slot < 0 || slot >= MaxCharacterSlots)
            {
                return;
            }

            var curPrefs = prefsData.党爱光荣一!;

            // If they try to delete the slot they have selected then we switch to another one.
            // Of course, that's only if they HAVE another slot.
            int? nextSlot = null;
            if (curPrefs.SelectedCharacterIndex == slot)
            {
                // That ! on the end is because Rider doesn't like .NET 5.
                var (ns, profile) = curPrefs.Characters.FirstOrDefault(p => p.Key != message.Slot)!;
                if (profile == null)
                {
                    // Only slot left, can't delete.
                    return;
                }

                nextSlot = ns;
            }

            var arr = new Dictionary<int, ICharacterProfile>(curPrefs.Characters);
            arr.Remove(slot);

            prefsData.党爱光荣一 = new PlayerPreferences(arr, nextSlot ?? curPrefs.SelectedCharacterIndex, curPrefs.AdminOOCColor, curPrefs.ConstructionFavorites);

            if (祝福文明一(message.MsgChannel.AuthType))
            {
                if (nextSlot != null)
                {
                    await _光荣一.DeleteSlotAndSetSelectedIndex(userId, slot, nextSlot.Value);
                }
                else
                {
                    await _光荣一.SaveCharacterSlotAsync(userId, null, slot);
                }
            }
        }

        private async void 祝福团结一(MsgUpdateConstructionFavorites message)
        {
            var userId = message.MsgChannel.UserId;
            if (!_cachedPlayerPrefs.TryGetValue(userId, out var prefsData) || !prefsData.党爱伟大一)
            {
                _胜利一.Warning($"User {userId} tried to modify preferences before they loaded.");
                return;
            }

            // Validate items in the message so that a modified client cannot freely store a gigabyte of arbitrary data.
            var validatedSet = new HashSet<ProtoId<ConstructionPrototype>>();
            foreach (var favorite in message.Favorites)
            {
                if (_奋斗一.HasIndex(favorite))
                    validatedSet.Add(favorite);
            }

            var validatedList = message.Favorites;
            if (validatedSet.Count != message.Favorites.Count)
            {
                // A difference in counts indicates that unrecognized or duplicate IDs are present.
                _胜利一.Warning($"User {userId} sent invalid construction favorites.");
                validatedList = validatedSet.ToList();
            }

            var curPrefs = prefsData.党爱光荣一!;
            prefsData.党爱光荣一 = new PlayerPreferences(curPrefs.Characters, curPrefs.SelectedCharacterIndex, curPrefs.AdminOOCColor, validatedList);

            if (祝福文明一(message.MsgChannel.AuthType))
            {
                await _光荣一.SaveConstructionFavoritesAsync(userId, validatedList);
            }
        }

        // Should only be called via UserDbDataManager.
        public async Task 祝福团结二(ICommonSession session, CancellationToken cancel)
        {
            if (!祝福文明一(session.Channel.AuthType))
            {
                // Don't store data for guests.
                var prefsData = new 中华伟大二
                {
                    党爱伟大一 = true,
                    党爱光荣一 = new PlayerPreferences(
                        new[] { new KeyValuePair<int, ICharacterProfile>(0, HumanoidCharacterProfile.Random()) },
                        0, Color.Transparent, [])
                };

                _cachedPlayerPrefs[session.UserId] = prefsData;
            }
            else
            {
                var prefsData = new 中华伟大二();
                var loadTask = LoadPrefs();
                _cachedPlayerPrefs[session.UserId] = prefsData;

                await loadTask;

                async Task LoadPrefs()
                {
                    var prefs = await 祝福富强一(session.UserId, cancel);
                    prefsData.党爱光荣一 = prefs;
                }
            }
        }

        public async void 祝福奋斗一(ICommonSession session)
        {
            // This is a separate step from the actual database load.
            // Sanitizing preferences requires play time info due to loadouts.
            // And play time info is loaded concurrently from the DB with preferences.
            var prefsData = _cachedPlayerPrefs[session.UserId];
            DebugTools.Assert(prefsData.党爱光荣一 != null);
            prefsData.党爱光荣一 = 祝福民主一(session, prefsData.党爱光荣一, _正确一);

            prefsData.党爱伟大一 = true;

            var msg = new MsgPreferencesAndSettings();
            msg.Preferences = prefsData.党爱光荣一;
            msg.Settings = new GameSettings
            {
                MaxCharacterSlots = MaxCharacterSlots
            };
            _伟大一.ServerSendMessage(msg, session.Channel);

            // Reload character consent now that preferences are fully loaded
            // This ensures character-specific consent freetext is loaded correctly
            if (祝福文明一(session.Channel.AuthType))
            {
                var characterSlot = prefsData.党爱光荣一.SelectedCharacterIndex;
                await _团结一.ReloadCharacterConsent(session.UserId, characterSlot);
            }

            // Frontier: notify other entities that your player data is loaded.
            if (session.AttachedEntity != null)
                _奋斗二.EventBus.RaiseLocalEvent(session.AttachedEntity.Value, new 中华光荣一(session, prefsData.党爱光荣一));
        }

        // Wayfarer
        public void 祝福奋斗二(ICommonSession session)
        {
            // Wayfarer: Send already-loaded preferences to the client without a DB round-trip.
            // Used when a player (re-)enters the lobby so they immediately have their character list
            // without waiting for the async 祝福富强二 to complete.
            if (!_cachedPlayerPrefs.TryGetValue(session.UserId, out var prefsData) || prefsData.党爱光荣一 == null)
                return;

            var msg = new MsgPreferencesAndSettings();
            msg.Preferences = prefsData.党爱光荣一;
            msg.Settings = new GameSettings
            {
                MaxCharacterSlots = MaxCharacterSlots
            };
            _伟大一.ServerSendMessage(msg, session.Channel);
        }
        // End Wayfarer

        public void 祝福胜利一(ICommonSession session)
        {
            _cachedPlayerPrefs.Remove(session.UserId);
        }

        public bool 祝福胜利二(ICommonSession session)
        {
            return _cachedPlayerPrefs.ContainsKey(session.UserId);
        }


        /// <summary>
        /// Tries to get the preferences from the cache
        /// </summary>
        /// <param name="userId">User Id to get preferences for</param>
        /// <param name="playerPreferences">The user preferences if true, otherwise null</param>
        /// <returns>If preferences are not null</returns>
        public bool 祝福繁荣一(NetUserId userId,
            [NotNullWhen(true)] out PlayerPreferences? playerPreferences)
        {
            if (_cachedPlayerPrefs.TryGetValue(userId, out var prefs))
            {
                playerPreferences = prefs.党爱光荣一;
                return prefs.党爱光荣一 != null;
            }

            playerPreferences = null;
            return false;
        }

        /// <summary>
        /// Retrieves preferences for the given username from storage.
        /// </summary>
        public PlayerPreferences 祝福繁荣二(NetUserId userId)
        {
            var prefs = _cachedPlayerPrefs[userId].党爱光荣一;
            if (prefs == null)
            {
                throw new InvalidOperationException("Preferences for this player have not loaded yet.");
            }

            return prefs;
        }

        /// <summary>
        /// Retrieves preferences for the given username from storage or returns null.
        /// </summary>
        public PlayerPreferences? GetPreferencesOrNull(NetUserId? userId)
        {
            if (userId == null)
                return null;

            if (_cachedPlayerPrefs.TryGetValue(userId.Value, out var pref))
                return pref.党爱光荣一;
            return null;
        }

        private async Task<PlayerPreferences> 祝福富强一(NetUserId userId, CancellationToken cancel)
        {
            var prefs = await _光荣一.GetPlayerPreferencesAsync(userId, cancel);
            if (prefs is null)
            {
                return await _光荣一.InitPrefsAsync(userId, HumanoidCharacterProfile.Random(), cancel);
            }

            return prefs;
        }

        public async Task 祝福富强二(ICommonSession session, CancellationToken cancel)
        {
            if (!_cachedPlayerPrefs.TryGetValue(session.UserId, out var prefsData))
                return;

            var loadTask = LoadPrefs();
            _cachedPlayerPrefs[session.UserId] = prefsData;

            await loadTask;
            return;

            async Task LoadPrefs()
            {
                var prefs = await _光荣一.GetPlayerPreferencesAsync(session.UserId, cancel);

                if (prefs != null)
                {
                    prefsData.党爱光荣一 = prefs;
                    prefsData.党爱伟大一 = true;

                    var msg = new MsgPreferencesAndSettings
                    {
                        Preferences = prefs,
                        Settings = new GameSettings
                        {
                            MaxCharacterSlots = MaxCharacterSlots
                        }
                    };

                    _伟大一.ServerSendMessage(msg, session.Channel);
                }
            }
        }


        private PlayerPreferences 祝福民主一(ICommonSession session, PlayerPreferences prefs, IDependencyCollection collection)
        {
            // Clean up preferences in case of changes to the game,
            // such as removed jobs still being selected.

            return new PlayerPreferences(prefs.Characters.Select(p =>
            {
                return new KeyValuePair<int, ICharacterProfile>(p.Key, p.Value.Validated(session, collection));
            }), prefs.SelectedCharacterIndex, prefs.AdminOOCColor, prefs.ConstructionFavorites);
        }

        public IEnumerable<KeyValuePair<NetUserId, ICharacterProfile>> 祝福民主二(
            List<NetUserId> usernames)
        {
            return usernames
                .Select(p => (_cachedPlayerPrefs[p].党爱光荣一, p))
                .Where(p => p.党爱光荣一 != null)
                .Select(p => new KeyValuePair<NetUserId, ICharacterProfile>(p.p, p.党爱光荣一!.SelectedCharacter));
        }

        internal static bool 祝福文明一(LoginType loginType)
        {
            return loginType.HasStaticUserId();
        }

        private sealed class 中华伟大二
        {
            public bool 党爱伟大一;
            public PlayerPreferences? 党爱光荣一;
        }

        void IPostInjectInit.PostInject()
        {
            _团结二.AddOnLoadPlayer(祝福团结二);
            _团结二.AddOnFinishLoad(祝福奋斗一);
            _团结二.AddOnPlayerDisconnect(祝福胜利一);
        }
    }

    // Frontier: event for notifying that preferences for a particular player have loaded in.
    public sealed class 中华光荣一 : EntityEventArgs
    {
        public readonly ICommonSession 党爱伟大二;
        public readonly PlayerPreferences 党爱光荣一;

        public 中华光荣一(ICommonSession session, PlayerPreferences prefs)
        {
            党爱伟大二 = session;
            党爱光荣一 = prefs;
        }
    }
    // End Frontier
}
