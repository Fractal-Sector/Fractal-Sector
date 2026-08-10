using Content.Shared._FS.VoiceBark;
using Content.Shared._FS.VoiceBark.Components;
using Content.Shared._FS.VoiceBark.Systems;
using Robust.Server.GameObjects;
using Robust.Shared.Console;
using Robust.Shared.Player;

namespace Content.Server._FS.VoiceBark;

/// <summary>
/// Server-side half of the bark-voice feature. Ordinary chat speech is
/// voiced client-side off the chat log (see the client VoiceBarkSystem) -
/// the broadcast below isn't invoked for normal play, it exists for parity
/// with WWDP and as a hook for a future NPC/admin-triggered bark.
/// </summary>
public sealed class VoiceBarkSystem : SharedVoiceBarkSystem
{
    [Dependency] private readonly TransformSystem _transformSystem = default!;

    public override void Bark(Entity<VoiceBarkComponent> entity, List<VoiceBarkData> barks)
    {
        var mapPos = _transformSystem.GetMapCoordinates(entity.Owner);
        var filter = Filter.Empty().AddInRange(mapPos, 16f);
        RaiseNetworkEvent(new EntityVoiceBarkEvent(GetNetEntity(entity), barks), filter);
    }
}

public sealed class AddBarkCommand : IConsoleCommand
{
    public string Command => "addbark";
    public string Description => "Assign a bark voice to an entity.";
    public string Help => Command + " <uid> <voicePrototype>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();

        if (args.Length < 2)
        {
            shell.WriteError("Not enough arguments.");
            return;
        }

        if (!entMan.TryParseNetEntity(args[0], out var attachedEnt))
        {
            shell.WriteError("Could not find entity " + args[0]);
            return;
        }

        entMan.System<VoiceBarkSystem>().ApplyVoiceBark(attachedEnt.Value, args[1]);
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
            return CompletionResult.FromHint("<uid>");
        if (args.Length == 2)
            return CompletionResult.FromHintOptions(CompletionHelper.PrototypeIDs<VoiceBarkPrototype>(), "voice prototype");

        return CompletionResult.Empty;
    }
}
