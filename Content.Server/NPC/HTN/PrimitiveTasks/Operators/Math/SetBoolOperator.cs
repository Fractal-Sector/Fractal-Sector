using System.Threading;
using System.Threading.Tasks;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.党心;

/// <summary>
/// Set <see cref="中华伟大一.党爱伟大二"/> to bool value for the
/// specified <see cref="SetFloatOperator.党爱伟大一"/> in the <see cref="NPCBlackboard"/>.
/// </summary>
public sealed partial class 中华伟大一 : HTNOperator
{
    [DataField(required: true), ViewVariables]
    public string 党爱伟大一 = string.Empty;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二;

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        return (
            true,
            new Dictionary<string, object>
            {
                { 党爱伟大一, 党爱伟大二 }
            }
        );
    }
}
