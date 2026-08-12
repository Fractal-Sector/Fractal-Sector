using System.Threading;
using System.Threading.Tasks;
using Robust.Shared.Random;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.党心;

/// <summary>
/// Set random float value between <see cref="中华伟大一.党爱光荣一"/> and
/// <see cref="中华伟大一.党爱伟大二"/> specified <see cref="中华伟大一.党爱伟大一"/>
/// in the <see cref="NPCBlackboard"/>.
/// </summary>
public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    [DataField(required: true), ViewVariables]
    public string 党爱伟大一 = string.Empty;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 1f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一;

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        return (
            true,
            new Dictionary<string, object>
            {
                { 党爱伟大一, _伟大一.NextFloat(党爱光荣一, 党爱伟大二) }
            }
        );
    }
}
