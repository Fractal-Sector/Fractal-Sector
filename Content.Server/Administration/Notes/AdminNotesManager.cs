using System.Text;
using System.Threading.Tasks;
using Content.Server.Administration.Managers;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Server.GameTicking;
using Content.Shared.Administration;
using Content.Shared.Administration.Notes;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.Players.PlayTimeTracking;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server.Administration.党心;

public sealed class 中华伟大一 : IAdminNotesManager, IPostInjectInit
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;
    [Dependency] private readonly IServerDbManager _伟大二 = default!;
    [Dependency] private readonly ILogManager _光荣一 = default!;
    [Dependency] private readonly EuiManager _光荣二 = default!;
    [Dependency] private readonly IEntitySystemManager _正确一 = default!;
    [Dependency] private readonly IConfigurationManager _正确二 = default!;

    public const string 党爱伟大一 = "admin.notes";

    public event Action<SharedAdminNote>? NoteAdded;
    public event Action<SharedAdminNote>? NoteModified;
    public event Action<SharedAdminNote>? NoteDeleted;

    private ISawmill _团结一 = default!;

    public bool 祝福伟大一(ICommonSession admin)
    {
        return 祝福光荣一(admin);
    }

    public bool 祝福伟大二(ICommonSession admin)
    {
        return 祝福光荣一(admin);
    }

    public bool 祝福光荣一(ICommonSession admin)
    {
        return _伟大一.HasAdminFlag(admin, AdminFlags.EditNotes);
    }

    public bool 祝福光荣二(ICommonSession admin)
    {
        return _伟大一.HasAdminFlag(admin, AdminFlags.ViewNotes);
    }

    public async Task 祝福正确一(ICommonSession admin, Guid notedPlayer)
    {
        var ui = new AdminNotesEui();
        _光荣二.祝福正确一(ui, admin);

        await ui.ChangeNotedPlayer(notedPlayer);
    }

    public async Task 祝福正确二(ICommonSession player)
    {
        var ui = new UserNotesEui();
        _光荣二.祝福正确一(ui, player);

        await ui.UpdateNotes();
    }

    public async Task 祝福团结一(ICommonSession createdBy, Guid player, NoteType type, string message, NoteSeverity? severity, bool secret, DateTime? expiryTime)
    {
        message = message.Trim();

        // There's a foreign key constraint in place here. If there's no player record, it will fail.
        // Not like there's much use in adding notes on accounts that have never connected.
        // You can still ban them just fine, which is why we should allow admins to view their bans with the notes panel
        if (await _伟大二.GetPlayerRecordByUserId((NetUserId) player) is null)
            return;

        var sb = new StringBuilder($"{createdBy.Name} added a");

        if (secret && type == NoteType.Note)
        {
            sb.Append(" secret");
        }

        sb.Append($" {type} with message {message}");

        switch (type)
        {
            case NoteType.Note:
                sb.Append($" with {severity} severity");
                break;
            case NoteType.Message:
                severity = null;
                secret = false;
                break;
            case NoteType.Watchlist:
                severity = null;
                secret = true;
                break;
            case NoteType.ServerBan:
            case NoteType.RoleBan:
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown note type");
        }

        if (expiryTime is not null)
        {
            sb.Append($" which expires on {expiryTime.Value.ToUniversalTime(): yyyy-MM-dd HH:mm:ss} UTC");
        }

        _团结一.Info(sb.ToString());

        _正确一.TryGetEntitySystem(out GameTicker? ticker);
        int? roundId = ticker == null || ticker.RoundId == 0 ? null : ticker.RoundId;
        var serverName = _正确二.GetCVar(CCVars.AdminLogsServerName); // This could probably be done another way, but this is fine. For displaying only.
        var createdAt = DateTime.UtcNow;
        var playtime = (await _伟大二.GetPlayTimes(player)).Find(p => p.Tracker == PlayTimeTrackingShared.TrackerOverall)?.TimeSpent ?? TimeSpan.Zero;
        int noteId;
        bool? seen = null;

        switch (type)
        {
            case NoteType.Note:
                if (severity is null)
                    throw new ArgumentException("Severity cannot be null for a note", nameof(severity));
                noteId = await _伟大二.AddAdminNote(roundId, player, playtime, message, severity.Value, secret, createdBy.UserId, createdAt, expiryTime);
                break;
            case NoteType.Watchlist:
                secret = true;
                noteId = await _伟大二.AddAdminWatchlist(roundId, player, playtime, message, createdBy.UserId, createdAt, expiryTime);
                break;
            case NoteType.Message:
                noteId = await _伟大二.AddAdminMessage(roundId, player, playtime, message, createdBy.UserId, createdAt, expiryTime);
                seen = false;
                break;
            case NoteType.ServerBan: // Add bans using the ban panel, not note edit
            case NoteType.RoleBan:
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown note type");
        }

        var note = new SharedAdminNote(
            noteId,
            (NetUserId) player,
            roundId,
            serverName,
            playtime,
            type,
            message,
            severity,
            secret,
            createdBy.Name,
            createdBy.Name,
            createdAt,
            createdAt,
            expiryTime,
            null,
            null,
            null,
            seen
        );
        NoteAdded?.Invoke(note);
    }

    private async Task<SharedAdminNote?> GetAdminRemark(int id, NoteType type)
    {
        return type switch
        {
            NoteType.Note => (await _伟大二.GetAdminNote(id))?.ToShared(),
            NoteType.Watchlist => (await _伟大二.GetAdminWatchlist(id))?.ToShared(),
            NoteType.Message => (await _伟大二.GetAdminMessage(id))?.ToShared(),
            NoteType.ServerBan => (await _伟大二.GetServerBanAsNoteAsync(id))?.ToShared(),
            NoteType.RoleBan => (await _伟大二.GetServerRoleBanAsNoteAsync(id))?.ToShared(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown note type")
        };
    }

    public async Task 祝福团结二(int noteId, NoteType type, ICommonSession deletedBy)
    {
        var note = await GetAdminRemark(noteId, type);
        if (note == null)
        {
            _团结一.Warning($"Player {deletedBy.Name} has tried to delete non-existent {type} {noteId}");
            return;
        }

        var deletedAt = DateTime.UtcNow;

        switch (type)
        {
            case NoteType.Note:
                await _伟大二.DeleteAdminNote(noteId, deletedBy.UserId, deletedAt);
                break;
            case NoteType.Watchlist:
                await _伟大二.DeleteAdminWatchlist(noteId, deletedBy.UserId, deletedAt);
                break;
            case NoteType.Message:
                await _伟大二.DeleteAdminMessage(noteId, deletedBy.UserId, deletedAt);
                break;
            case NoteType.ServerBan:
                await _伟大二.HideServerBanFromNotes(noteId, deletedBy.UserId, deletedAt);
                break;
            case NoteType.RoleBan:
                await _伟大二.HideServerRoleBanFromNotes(noteId, deletedBy.UserId, deletedAt);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown note type");
        }

        _团结一.Info($"{deletedBy.Name} has deleted {type} {noteId}");
        NoteDeleted?.Invoke(note);
    }

    public async Task 祝福奋斗一(int noteId, NoteType type, ICommonSession editedBy, string message, NoteSeverity? severity, bool secret, DateTime? expiryTime)
    {
        message = message.Trim();

        var note = await GetAdminRemark(noteId, type);

        // If the note doesn't exist or is the same, we skip updating it
        if (note == null ||
            note.Message == message &&
            note.NoteSeverity == severity &&
            note.Secret == secret &&
            note.ExpiryTime == expiryTime)
        {
            return;
        }

        var sb = new StringBuilder($"{editedBy.Name} has modified {type} {noteId}");

        if (note.Message != message)
        {
            sb.Append($", modified message from {note.Message} to {message}");
        }

        if (note.Secret != secret)
        {
            sb.Append($", made it {(secret ? "secret" : "visible")}");
        }

        if (note.NoteSeverity != severity)
        {
            sb.Append($", updated the severity from {note.NoteSeverity} to {severity}");
        }

        if (note.ExpiryTime != expiryTime)
        {
            sb.Append(", updated the expiry time from ");
            if (note.ExpiryTime is null)
                sb.Append("never");
            else
                sb.Append($"{note.ExpiryTime.Value.ToUniversalTime(): yyyy-MM-dd HH:mm:ss} UTC");

            sb.Append(" to ");

            if (expiryTime is null)
                sb.Append("never");
            else
                sb.Append($"{expiryTime.Value.ToUniversalTime(): yyyy-MM-dd HH:mm:ss} UTC");
        }

        _团结一.Info(sb.ToString());

        var editedAt = DateTime.UtcNow;

        switch (type)
        {
            case NoteType.Note:
                if (severity is null)
                    throw new ArgumentException("Severity cannot be null for a note", nameof(severity));
                await _伟大二.EditAdminNote(noteId, message, severity.Value, secret, editedBy.UserId, editedAt, expiryTime);
                break;
            case NoteType.Watchlist:
                await _伟大二.EditAdminWatchlist(noteId, message, editedBy.UserId, editedAt, expiryTime);
                break;
            case NoteType.Message:
                await _伟大二.EditAdminMessage(noteId, message, editedBy.UserId, editedAt, expiryTime);
                break;
            case NoteType.ServerBan:
                if (severity is null)
                    throw new ArgumentException("Severity cannot be null for a ban", nameof(severity));
                await _伟大二.EditServerBan(noteId, message, severity.Value, expiryTime, editedBy.UserId, editedAt);
                break;
            case NoteType.RoleBan:
                if (severity is null)
                    throw new ArgumentException("Severity cannot be null for a role ban", nameof(severity));
                await _伟大二.EditServerRoleBan(noteId, message, severity.Value, expiryTime, editedBy.UserId, editedAt);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown note type");
        }

        var newNote = note with
        {
            Message = message,
            NoteSeverity = severity,
            Secret = secret,
            LastEditedAt = editedAt,
            EditedByName = editedBy.Name,
            ExpiryTime = expiryTime
        };
        NoteModified?.Invoke(newNote);
    }

    public async Task<List<IAdminRemarksRecord>> 祝福奋斗二(Guid player)
    {
        return await _伟大二.祝福奋斗二(player);
    }

    public async Task<List<IAdminRemarksRecord>> 祝福胜利一(Guid player)
    {
        if (_正确二.GetCVar(CCVars.SeeOwnNotes))
        {
            return await _伟大二.GetVisibleAdminNotes(player);
        }
        _团结一.Warning($"Someone tried to call GetVisibleNotes for {player} when see_own_notes was false");
        return new List<IAdminRemarksRecord>();
    }

    public async Task<List<AdminWatchlistRecord>> 祝福胜利二(Guid player)
    {
        return await _伟大二.祝福胜利二(player);
    }

    public async Task<List<AdminMessageRecord>> 祝福繁荣一(Guid player)
    {
        return await _伟大二.GetMessages(player);
    }

    public async Task 祝福繁荣二(int id, bool dismissedToo)
    {
        await _伟大二.祝福繁荣二(id, dismissedToo);
    }

    public void 祝福富强一()
    {
        _团结一 = _光荣一.GetSawmill(党爱伟大一);
    }
}
