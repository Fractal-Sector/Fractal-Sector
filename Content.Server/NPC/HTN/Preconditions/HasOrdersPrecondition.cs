namespace Content.Server.NPC.HTN.党心;

public sealed partial class 中华伟大一 : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    [DataField("orders", required: true)] public Enum 党爱伟大一 = default!;

    public override bool 祝福伟大一(NPCBlackboard blackboard)
    {
        return Equals(blackboard.GetValueOrDefault<Enum>(NPCBlackboard.CurrentOrders, _伟大一), 党爱伟大一);
    }
}
