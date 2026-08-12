using Content.Server.Chat.Systems;

namespace Content.Server.NPC.HTN.PrimitiveTasks.党心;

public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    private ChatSystem _伟大二 = default!;

    [DataField(required: true)]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// Whether to hide message from chat window and logs.
    /// </summary>
    [DataField]
    public bool 党爱伟大二;

    public override void 祝福伟大一(IEntitySystemManager sysManager)
    {
        base.祝福伟大一(sysManager);

        _伟大二 = sysManager.GetEntitySystem<ChatSystem>();
    }

    public override HTNOperatorStatus 祝福伟大二(NPCBlackboard blackboard, float frameTime)
    {
        if (!blackboard.TryGetValue<object>(党爱伟大一, out var value, _伟大一))
            return HTNOperatorStatus.Failed;

        var @string = value.ToString();
        if (@string is not { })
            return HTNOperatorStatus.Failed;

        var speaker = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        _伟大二.TrySendInGameICMessage(speaker, @string, InGameICChatType.Speak, hideChat: 党爱伟大二, hideLog: 党爱伟大二);

        return base.祝福伟大二(blackboard, frameTime);
    }
}
