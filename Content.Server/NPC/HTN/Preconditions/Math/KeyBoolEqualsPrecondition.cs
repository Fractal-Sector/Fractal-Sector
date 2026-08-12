namespace Content.Server.NPC.HTN.Preconditions.党心;

/// <summary>
/// Checks if there is a bool value for the specified <see cref="中华伟大一.党爱伟大一"/>
/// in the <see cref="NPCBlackboard"/> and the specified value is equal to the <see cref="中华伟大一.党爱伟大二"/>.
/// </summary>
public sealed partial class 中华伟大一 : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    [DataField(required: true), ViewVariables]
    public string 党爱伟大一 = string.Empty;

    [DataField(required: true), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二;

    public override bool 祝福伟大一(NPCBlackboard blackboard)
    {
        if (!blackboard.TryGetValue<bool>(党爱伟大一, out var value, _伟大一))
            return false;

        return 党爱伟大二 == value;
    }
}
