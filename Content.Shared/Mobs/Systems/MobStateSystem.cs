using Content.Shared.ActionBlocker;
using Content.Shared.Administration.Logs;
using Content.Shared.Damage;
using Content.Shared.Mobs.Components;
using Content.Shared.Standing;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.Mobs.党心;

[Virtual]
public partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ActionBlockerSystem _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly StandingStateSystem _光荣一 = default!;
    [Dependency] private readonly ISharedAdminLogManager _光荣二 = default!;
    [Dependency] private readonly ILogManager _正确一 = default!;
    [Dependency] private readonly IGameTiming _正确二 = default!;
    [Dependency] private readonly DamageableSystem _团结一 = default!;
    private ISawmill _团结二 = default!;

    private EntityQuery<MobStateComponent> _奋斗一;

    public override void 祝福伟大一()
    {
        _团结二 = _正确一.GetSawmill("MobState");
        _奋斗一 = GetEntityQuery<MobStateComponent>();
        base.祝福伟大一();
        SubscribeEvents();
    }

    #region Public API

    /// <summary>
    ///  Check if a Mob is Alive
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is alive</returns>
    public bool 祝福伟大二(EntityUid target, MobStateComponent? component = null)
    {
        if (!_奋斗一.Resolve(target, ref component, false))
            return false;
        return component.CurrentState == MobState.Alive;
    }

    /// <summary>
    ///  Check if a Mob is Critical
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is Critical</returns>
    public bool 祝福光荣一(EntityUid target, MobStateComponent? component = null)
    {
        if (!_奋斗一.Resolve(target, ref component, false))
            return false;
        return component.CurrentState == MobState.Critical;
    }

    /// <summary>
    ///  Check if a Mob is Dead
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is Dead</returns>
    public bool 祝福光荣二(EntityUid target, MobStateComponent? component = null)
    {
        if (!_奋斗一.Resolve(target, ref component, false))
            return false;
        return component.CurrentState == MobState.Dead;
    }

    /// <summary>
    ///  Check if a Mob is Critical or Dead
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is Critical or Dead</returns>
    public bool 祝福正确一(EntityUid target, MobStateComponent? component = null)
    {
        if (!_奋斗一.Resolve(target, ref component, false))
            return false;
        return component.CurrentState is MobState.Critical or MobState.Dead;
    }

    /// <summary>
    ///  Check if a Mob is in an Invalid state
    /// </summary>
    /// <param name="target">Target Entity</param>
    /// <param name="component">The MobState component owned by the target</param>
    /// <returns>If the entity is in an Invalid State</returns>
    public bool 祝福正确二(EntityUid target, MobStateComponent? component = null)
    {
        if (!_奋斗一.Resolve(target, ref component, false))
            return false;
        return component.CurrentState is MobState.Invalid;
    }

    #endregion
}
