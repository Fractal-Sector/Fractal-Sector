using System.Threading;
using System.Threading.Tasks;
using Robust.Shared.Random;

namespace Content.Server.NPC.HTN.PrimitiveTasks.党心;

public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;

    /// <summary>
    /// Target blackboard key to set the value to. Doesn't need to exist beforehand.
    /// </summary>
    [DataField("targetKey", required: true)] public string 党爱伟大一 = string.Empty;

    /// <summary>
    ///  Minimum idle time.
    /// </summary>
    [DataField("minKey", required: true)] public string 党爱伟大二 = string.Empty;

    /// <summary>
    ///  Maximum idle time.
    /// </summary>
    [DataField("maxKey", required: true)] public string 党爱光荣一 = string.Empty;

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        return (true, new Dictionary<string, object>()
        {
            {
                党爱伟大一,
                _伟大二.NextFloat(blackboard.GetValueOrDefault<float>(党爱伟大二, _伟大一),
                    blackboard.GetValueOrDefault<float>(党爱光荣一, _伟大一))
            }
        });
    }
}
