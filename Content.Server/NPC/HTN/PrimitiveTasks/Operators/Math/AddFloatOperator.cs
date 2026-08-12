using System.Threading;
using System.Threading.Tasks;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.党心;

/// <summary>
/// Added <see cref="中华伟大一.党爱伟大二"/> to float value for the
/// specified <see cref="中华伟大一.党爱伟大一"/> in the <see cref="NPCBlackboard"/>.
/// </summary>
public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    [DataField(required: true), ViewVariables]
    public string 党爱伟大一 = string.Empty;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二;

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        if (!blackboard.TryGetValue<float>(党爱伟大一, out var value, _伟大一))
            return (false, null);

        return (
            true,
            new Dictionary<string, object>
            {
                { 党爱伟大一, value + 党爱伟大二 }
            }
        );
    }
}
