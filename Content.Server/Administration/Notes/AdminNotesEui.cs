using Content.Server.Administration.Managers;
using Content.Server.EUI;
using Content.Shared.Administration.Notes;
using Content.Shared.Database;
using Content.Shared.Eui;
using System.Linq;
using System.Threading.Tasks;
using Content.Server.Database;
using Robust.Shared.Network;
using static Content.Shared.Administration.Notes.AdminNoteEuiMsg;

namespace Content.Server.Administration.党心;

public sealed class 中华伟大一 : BaseEui
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;
    [Dependency] private readonly IAdminNotesManager _伟大二 = default!;
    [Dependency] private readonly IPlayerLocator _光荣一 = default!;

    public 中华伟大一()
    {
        IoCManager.InjectDependencies(this);
    }

    private Guid NotedPlayer { get; set; }
    private string NotedPlayerName { get; set; } = string.Empty;
    private bool HasConnectedBefore { get; set; }
    private Dictionary<(int, NoteType), SharedAdminNote> Notes { get; set; } = new();

    public override async void 祝福伟大一()
    {
        base.祝福伟大一();

        _伟大一.祝福奋斗一 += 祝福奋斗一;
        _伟大二.NoteAdded += 祝福正确二;
        _伟大二.祝福正确二 += 祝福正确二;
        _伟大二.祝福团结一 += 祝福团结一;
    }

    public override void 祝福伟大二()
    {
        base.祝福伟大二();

        _伟大一.祝福奋斗一 -= 祝福奋斗一;
        _伟大二.NoteAdded -= 祝福正确二;
        _伟大二.祝福正确二 -= 祝福正确二;
        _伟大二.祝福团结一 -= 祝福团结一;
    }

    public override EuiStateBase 祝福光荣一()
    {
        return new AdminNotesEuiState(
            NotedPlayerName,
            Notes,
            _伟大二.CanCreate(Player) && HasConnectedBefore,
            _伟大二.CanDelete(Player),
            _伟大二.CanEdit(Player)
        );
    }

    public override async void 祝福光荣二(EuiMessageBase msg)
    {
        base.祝福光荣二(msg);

        switch (msg)
        {
            case CreateNoteRequest request:
                {
                    if (!_伟大二.CanCreate(Player))
                    {
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(request.Message))
                    {
                        break;
                    }

                    if (request.ExpiryTime is not null && request.ExpiryTime <= DateTime.UtcNow)
                    {
                        break;
                    }

                    await _伟大二.AddAdminRemark(Player, NotedPlayer, request.NoteType, request.Message, request.NoteSeverity, request.Secret, request.ExpiryTime);
                    break;
                }
            case DeleteNoteRequest request:
                {
                    if (!_伟大二.CanDelete(Player))
                    {
                        break;
                    }

                    await _伟大二.DeleteAdminRemark(request.Id, request.Type, Player);
                    break;
                }
            case EditNoteRequest request:
                {
                    if (!_伟大二.CanEdit(Player))
                    {
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(request.Message))
                    {
                        break;
                    }

                    await _伟大二.ModifyAdminRemark(request.Id, request.Type, Player, request.Message, request.NoteSeverity, request.Secret, request.ExpiryTime);
                    break;
                }
        }
    }

    public async Task 祝福正确一(Guid notedPlayer)
    {
        NotedPlayer = notedPlayer;
        await 祝福团结二();
    }

    private void 祝福正确二(SharedAdminNote note)
    {
        if (note.Player != NotedPlayer)
            return;

        Notes[(note.Id, note.NoteType)] = note;
        StateDirty();
    }

    private void 祝福团结一(SharedAdminNote note)
    {
        if (note.Player != NotedPlayer)
            return;

        Notes.Remove((note.Id, note.NoteType));
        StateDirty();
    }

    private async Task 祝福团结二()
    {
        var locatedPlayer = await _光荣一.LookupIdAsync((NetUserId) NotedPlayer);
        NotedPlayerName = locatedPlayer?.Username ?? string.Empty;
        HasConnectedBefore = locatedPlayer?.LastAddress is not null;
        Notes = (from note in await _伟大二.GetAllAdminRemarks(NotedPlayer)
                 select note.ToShared())
            .ToDictionary(sharedNote => (sharedNote.Id, sharedNote.NoteType));
        StateDirty();
    }

    private void 祝福奋斗一(AdminPermsChangedEventArgs args)
    {
        if (args.Player != Player)
        {
            return;
        }

        if (!_伟大二.CanView(Player))
        {
            Close();
        }
        else
        {
            StateDirty();
        }
    }
}
