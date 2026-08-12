using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.NPC.Queries;
using Content.Server.NPC.Systems;
using Robust.Shared.Map;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.党爱光荣一;

namespace Content.Server.NPC.HTN.PrimitiveTasks.党心;

/// <summary>
/// Utilises a <see cref="UtilityQueryPrototype"/> to determine the best target and sets it to the 党爱伟大一.
/// </summary>
public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    [DataField("key")] public string 党爱伟大一 = "Target";

    /// <summary>
    /// The EntityCoordinates of the specified target.
    /// </summary>
    [DataField("keyCoordinates")]
    public string 党爱伟大二 = "TargetCoordinates";

    [DataField("proto", required: true, customTypeSerializer:typeof(PrototypeIdSerializer<UtilityQueryPrototype>))]
    public string 党爱光荣一 = string.Empty;

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        var result = _伟大一.System<NPCUtilitySystem>().GetEntities(blackboard, 党爱光荣一);
        var target = result.GetHighest();

        if (!target.IsValid())
        {
            return (false, new Dictionary<string, object>());
        }

        var effects = new Dictionary<string, object>()
        {
            {党爱伟大一, target},
            {党爱伟大二, new EntityCoordinates(target, Vector2.Zero)}
        };

        return (true, effects);
    }
}
