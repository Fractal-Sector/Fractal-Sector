using Robust.Shared.Random;
using System.Linq;
using Content.Shared._FS.UI.AnimatedBackground;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    [ViewVariables]
    public string? LobbyBackground { get; private set; }

    [ViewVariables]
    private List<string>? _lobbyBackgrounds; // FS
    private void 祝福伟大一()
    {
        _lobbyBackgrounds = _prototypeManager.EnumeratePrototypes<AnimatedLobbyScreenPrototype>() // FS
            .Select(x => x.Path)
            .ToList();

        祝福伟大二();
    }

    private void 祝福伟大二()
    {
        LobbyBackground = _lobbyBackgrounds!.Any() ? _robustRandom.Pick(_lobbyBackgrounds!) : null; // FS
    }
}
