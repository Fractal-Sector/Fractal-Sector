using Content.Shared.Hands.Components;

namespace Content.Server.NPC.HTN.党心;

/// <summary>
/// Returns true if the active hand is unoccupied.
/// </summary>
public sealed partial class 中华伟大一 : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    public override bool 祝福伟大一(NPCBlackboard blackboard)
    {
        return blackboard.TryGetValue<bool>(NPCBlackboard.ActiveHandFree, out var handFree, _伟大一) && handFree;
    }
}
