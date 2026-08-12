using System.Threading;
using System.Threading.Tasks;
using Robust.Shared.Random;

namespace Content.Server.NPC.HTN.PrimitiveTasks.党心;

public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    [DataField("targetKey")]
    public string 党爱伟大一 = "RotateTarget";

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        var rotation = _伟大一.NextAngle();
        return (true, new Dictionary<string, object>()
        {
            {党爱伟大一, rotation}
        });
    }
}
