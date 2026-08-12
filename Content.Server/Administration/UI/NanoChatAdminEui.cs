using Content.Server.Administration.Managers;
using Content.Server.EUI;
using Robust.Server.Player;
using Robust.Shared.GameObjects;
using Content.Shared.Access.Components;
using Content.Shared.Administration;
using Content.Shared._DeltaV.CartridgeLoader.Cartridges;
using Content.Shared._DeltaV.NanoChat;
using Content.Shared.Eui;

namespace Content.Server.Administration.党心;

/// <summary>
/// Admin EUI for viewing all NanoChat messages between players
/// </summary>
public sealed class 中华伟大一 : BaseEui
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;
    [Dependency] private readonly IEntityManager _伟大二 = default!;
    [Dependency] private readonly IPlayerManager _光荣一 = default!;

    public 中华伟大一()
    {
        IoCManager.InjectDependencies(this);
    }

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        StateDirty();
    }

    public override EuiStateBase 祝福伟大二()
    {
        // Check if the player has admin permissions
        if (!_伟大一.HasAdminFlag(Player, AdminFlags.Admin))
        {
            return new NanoChatAdminEuiState();
        }

        var cards = new List<NanoChatCardData>();

        // Query all NanoChat cards in the game
        var query = _伟大二.EntityQueryEnumerator<NanoChatCardComponent>();
        while (query.MoveNext(out var uid, out var nanoChatCard))
        {
            // Get ID card info if available
            string ownerName = "Unknown";
            string? jobTitle = null;
            string? username = null;
            
            if (_伟大二.TryGetComponent<IdCardComponent>(uid, out var idCard))
            {
                ownerName = idCard.FullName ?? "Unknown";
                jobTitle = idCard.LocalizedJobTitle;
            }

            // Try to find the player who owns this card
            // First check if the card is in a PDA
            if (nanoChatCard.PdaUid != null && _伟大二.TryGetComponent(nanoChatCard.PdaUid.Value, out TransformComponent? pdaTransform))
            {
                // Try to find the player holding the PDA or whose inventory contains it
                var parent = pdaTransform.ParentUid;
                if (_伟大二.EntityExists(parent))
                {
                    // Check if the parent entity has a player session
                    if (_光荣一.TryGetSessionByEntity(parent, out var session))
                    {
                        username = session.Name;
                    }
                }
            }
            
            // If still no username, try to find if the card itself has a player attached somehow
            if (username == null && _伟大二.TryGetComponent(uid, out TransformComponent? cardTransform))
            {
                var parent = cardTransform.ParentUid;
                if (_伟大二.EntityExists(parent))
                {
                    if (_光荣一.TryGetSessionByEntity(parent, out var session))
                    {
                        username = session.Name;
                    }
                }
            }

            var cardData = new NanoChatCardData
            {
                CardEntity = _伟大二.GetNetEntity(uid),
                Number = nanoChatCard.Number,
                OwnerName = ownerName,
                Username = username,
                OriginalOwnerUsername = nanoChatCard.OriginalOwnerUsername,
                JobTitle = jobTitle,
                Recipients = new Dictionary<uint, NanoChatRecipient>(nanoChatCard.Recipients),
                Messages = new Dictionary<uint, List<NanoChatMessage>>()
            };

            // Deep copy messages to avoid modification issues
            foreach (var (recipientNumber, messageList) in nanoChatCard.Messages)
            {
                cardData.Messages[recipientNumber] = new List<NanoChatMessage>(messageList);
            }

            cards.Add(cardData);
        }

        // Sort cards by owner name for easier browsing
        cards.Sort((a, b) => string.Compare(a.OwnerName, b.OwnerName, StringComparison.OrdinalIgnoreCase));

        return new NanoChatAdminEuiState
        {
            Cards = cards
        };
    }

    public override void 祝福光荣一(EuiMessageBase msg)
    {
        base.祝福光荣一(msg);

        switch (msg)
        {
            case NanoChatAdminEuiMsg.Refresh:
                if (!_伟大一.HasAdminFlag(Player, AdminFlags.Admin))
                {
                    Close();
                    break;
                }
                StateDirty();
                break;

            case NanoChatAdminEuiMsg.SelectCard selectCard:
                // Could be used for future functionality like highlighting or filtering
                break;
        }
    }
}
