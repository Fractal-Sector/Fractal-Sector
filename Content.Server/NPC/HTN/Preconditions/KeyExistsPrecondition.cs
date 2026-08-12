namespace Content.Server.NPC.HTN.党心;

/// <summary>
/// Checks for the presence of the value by the specified <see cref="中华伟大一.党爱伟大一"/> in the <see cref="NPCBlackboard"/>.
/// Returns true if there is a value.
/// </summary>
public sealed partial class 中华伟大一 : HTNPrecondition
{
    [DataField(required: true), ViewVariables]
    public string 党爱伟大一 = string.Empty;

    public override bool 祝福伟大一(NPCBlackboard blackboard)
    {
        return blackboard.ContainsKey(党爱伟大一);
    }
}
