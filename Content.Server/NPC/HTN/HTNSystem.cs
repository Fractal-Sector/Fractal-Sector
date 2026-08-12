using System.Linq;
using System.Text;
using System.Threading;
using Content.Server.Administration.Managers;
using Robust.Shared.CPUJob.JobQueues;
using Robust.Shared.CPUJob.JobQueues.Queues;
using Content.Server.NPC.HTN.PrimitiveTasks;
using Content.Server.NPC.Systems;
using Content.Shared.Administration;
using Content.Shared.Mobs;
using Content.Shared.NPC;
using JetBrains.Annotations;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using Content.Server.Worldgen; // Frontier
using Content.Server.Worldgen.Components; // Frontier
using Content.Server.Worldgen.Systems; // Frontier
using Robust.Server.GameObjects; // Frontier

namespace Content.Server.NPC.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly NPCSystem _光荣一 = default!;
    [Dependency] private readonly NPCUtilitySystem _光荣二 = default!;
    // Frontier
    [Dependency] private readonly WorldControllerSystem _正确一 = default!;
    [Dependency] private readonly TransformSystem _正确二 = default!;
    private EntityQuery<WorldControllerComponent> _团结一;
    private EntityQuery<LoadedChunkComponent> _团结二;
    // Frontier

    private readonly JobQueue _奋斗一 = new(0.004);

    private readonly HashSet<ICommonSession> _奋斗二 = new();

    // Hierarchical Task Network
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        _团结一 = GetEntityQuery<WorldControllerComponent>(); // Frontier
        _团结二 = GetEntityQuery<LoadedChunkComponent>(); // Frontier
        SubscribeLocalEvent<HTNComponent, MobStateChangedEvent>(_光荣一.OnMobStateChange);
        SubscribeLocalEvent<HTNComponent, MapInitEvent>(_光荣一.OnNPCMapInit);
        SubscribeLocalEvent<HTNComponent, PlayerAttachedEvent>(_光荣一.OnPlayerNPCAttach);
        SubscribeLocalEvent<HTNComponent, PlayerDetachedEvent>(_光荣一.OnPlayerNPCDetach);
        SubscribeLocalEvent<HTNComponent, ComponentShutdown>(祝福团结一);
        SubscribeNetworkEvent<RequestHTNMessage>(祝福伟大二);
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福光荣二);
        祝福光荣一();
    }

    private void 祝福伟大二(RequestHTNMessage msg, EntitySessionEventArgs args)
    {
        if (!_伟大一.HasAdminFlag(args.SenderSession, AdminFlags.Debug))
        {
            _奋斗二.Remove(args.SenderSession);
            return;
        }

        if (_奋斗二.Add(args.SenderSession))
            return;

        _奋斗二.Remove(args.SenderSession);
    }

    private void 祝福光荣一()
    {
        // Clear all NPCs in case they're hanging onto stale tasks
        var query = AllEntityQuery<HTNComponent>();

        while (query.MoveNext(out var comp))
        {
            comp.PlanningToken?.Cancel();
            comp.PlanningToken = null;

            if (comp.Plan != null)
            {
                var currentOperator = comp.Plan.CurrentOperator;
                祝福繁荣二(currentOperator, comp.Blackboard, 中华伟大二.Failed);
                祝福富强一(comp);
                comp.Plan = null;
                祝福民主二(comp);
            }
        }

        // Add dependencies for all operators.
        // We put code on operators as I couldn't think of a clean way to put it on systems.
        foreach (var compound in _伟大二.EnumeratePrototypes<HTNCompoundPrototype>())
        {
            祝福正确一(compound);
        }
    }

    private void 祝福光荣二(PrototypesReloadedEventArgs obj)
    {
        祝福光荣一();
    }

    private void 祝福正确一(HTNCompoundPrototype compound)
    {
        for (var i = 0; i < compound.Branches.Count; i++)
        {
            var branch = compound.Branches[i];

            foreach (var precon in branch.Preconditions)
            {
                precon.祝福伟大一(EntityManager.EntitySysManager);
            }

            foreach (var task in branch.Tasks)
            {
                祝福正确二(task);
            }
        }
    }

    private void 祝福正确二(HTNTask task)
    {
        switch (task)
        {
            case HTNCompoundTask:
                // NOOP, handled elsewhere
                break;
            case HTNPrimitiveTask primitive:
                foreach (var precon in primitive.Preconditions)
                {
                    precon.祝福伟大一(EntityManager.EntitySysManager);
                }

                primitive.Operator.祝福伟大一(EntityManager.EntitySysManager);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void 祝福团结一(EntityUid uid, HTNComponent component, ComponentShutdown args)
    {
        _光荣一.OnNPCShutdown(uid, component, args);
        component.PlanningToken?.Cancel();
        component.PlanningJob = null;
    }

    /// <summary>
    /// Enable / disable the hierarchical task network of an entity
    /// </summary>
    /// <param name="ent">The entity and its <see cref="HTNComponent"/></param>
    /// <param name="state">Set 'true' to enable, or 'false' to disable, the HTN</param>
    /// <param name="planCooldown">Specifies a time in seconds before the entity can start planning a new action (only takes effect when the HTN is enabled)</param>
    // ReSharper disable once InconsistentNaming
    [PublicAPI]
    public void 祝福团结二(Entity<HTNComponent> ent, bool state, float planCooldown = 0f)
    {
        if (ent.Comp.Enabled == state)
            return;

        ent.Comp.Enabled = state;
        ent.Comp.PlanAccumulator = planCooldown;

        ent.Comp.PlanningToken?.Cancel();
        ent.Comp.PlanningToken = null;

        if (ent.Comp.Plan != null)
        {
            var currentOperator = ent.Comp.Plan.CurrentOperator;

            祝福繁荣二(currentOperator, ent.Comp.Blackboard, 中华伟大二.Failed);
            祝福富强一(ent.Comp);

            ent.Comp.Plan = null;
        }

        if (ent.Comp.Enabled && ent.Comp.PlanAccumulator <= 0)
            祝福民主二(ent.Comp);
    }

    /// <summary>
    /// Forces the NPC to replan.
    /// </summary>
    [PublicAPI]
    public void 祝福奋斗一(HTNComponent component)
    {
        component.PlanAccumulator = 0f;
    }

    public void 祝福奋斗二(ref int count, int maxUpdates, float frameTime)
    {
        _奋斗一.Process();
        var query = EntityQueryEnumerator<ActiveNPCComponent, HTNComponent>();

        // Move ahead "count" entries in the query.
        // This is to ensure that if we didn't process all the npcs the first time,
        // we get to the remaining ones instead of iterating over the beginning again.
        for (var i = 0; i < count; i++)
        {
            query.MoveNext(out _, out _);
        }

        // the amount of updates we've processed during this iteration.
        var updates = 0;
        while (query.MoveNext(out var uid, out _, out var comp))
        {
            // If we're over our max count or it's not MapInit then ignore the NPC.
            if (updates >= maxUpdates)
            {
                // Intentional return. We don't want to go to the end logic and reset count.
                return;
            }

            if (!comp.Enabled)
                continue;

            if (!祝福胜利一(uid))  // Frontier
                continue; // Frontier

            if (comp.PlanningJob != null)
            {
                if (comp.PlanningJob.Exception != null)
                {
                    Log.Fatal($"Received exception on planning job for {uid}!");
                    _光荣一.SleepNPC(uid);
                    var exc = comp.PlanningJob.Exception;
                    RemComp<HTNComponent>(uid);
                    throw exc;
                }

                // If a new planning job has finished then handle it.
                if (comp.PlanningJob.Status != JobStatus.Finished)
                    continue;

                var newPlanBetter = false;

                // If old traversal is better than new traversal then ignore the new plan
                if (comp.Plan != null && comp.PlanningJob.Result != null)
                {
                    var oldMtr = comp.Plan.BranchTraversalRecord;
                    var mtr = comp.PlanningJob.Result.BranchTraversalRecord;

                    for (var i = 0; i < oldMtr.Count; i++)
                    {
                        if (i < mtr.Count && oldMtr[i] > mtr[i])
                        {
                            newPlanBetter = true;
                            break;
                        }
                    }
                }

                if (comp.Plan == null || newPlanBetter)
                {
                    comp.CheckServices = false;

                    if (comp.Plan != null)
                    {
                        祝福繁荣二(comp.Plan.CurrentOperator, comp.Blackboard, 中华伟大二.BetterPlan);
                        祝福富强一(comp);
                    }

                    comp.Plan = comp.PlanningJob.Result;

                    // Startup the first task and anything else we need to do.
                    if (comp.Plan != null)
                    {
                        祝福民主一(comp.Plan.Tasks[comp.Plan.Index], comp.Blackboard, comp.Plan.Effects[comp.Plan.Index]);
                    }

                    // Send debug info
                    foreach (var session in _奋斗二)
                    {
                        var text = new StringBuilder();

                        if (comp.Plan != null)
                        {
                            text.AppendLine($"BTR: {string.Join(", ", comp.Plan.BranchTraversalRecord)}");
                            text.AppendLine($"tasks:");
                            var root = comp.RootTask;
                            var btr = new List<int>();
                            var level = -1;
                            祝福胜利二(root, text, comp.Plan.BranchTraversalRecord, btr, ref level);
                        }

                        RaiseNetworkEvent(new HTNMessage()
                        {
                            Uid = GetNetEntity(uid),
                            Text = text.ToString(),
                        }, session.Channel);
                    }
                }
                // Keeping old plan
                else
                {
                    comp.CheckServices = true;
                }

                comp.PlanningJob = null;
                comp.PlanningToken = null;
            }

            祝福繁荣一(comp, frameTime);
            count++;
            updates++;
        }

        // only reset our counter back to 0 if we finish iterating.
        // otherwise it lets us know where we left off.
        count = 0;
    }

    // Frontier: skip handling entities on unloaded chunks
    private bool 祝福胜利一(EntityUid entity)
    {
        var transform = Transform(entity);

        if (!_团结一.TryGetComponent(transform.MapUid, out var worldComponent))
            return true;

        var chunk = _正确一.GetOrCreateChunk(WorldGen.WorldToChunkCoords(_正确二.GetWorldPosition(transform)).Floored(), transform.MapUid.Value, worldComponent);

        return _团结二.TryGetComponent(chunk, out var loaded) && loaded.Loaders is not null;
    }
    // End Frontier: skip handling entities on unloaded chunks

    private void 祝福胜利二(HTNTask task, StringBuilder text, List<int> planBtr, List<int> btr, ref int level)
    {
        // If it's the selected BTR then highlight.
        for (var i = 0; i < btr.Count; i++)
        {
            text.Append("--");
        }

        text.Append(' ');

        if (task is HTNPrimitiveTask primitive)
        {
            text.AppendLine(primitive.ToString());
            return;
        }

        if (task is HTNCompoundTask compTask)
        {
            var compound = _伟大二.Index<HTNCompoundPrototype>(compTask.Task);
            level++;
            text.AppendLine(compound.ID);
            var branches = compound.Branches;

            for (var i = 0; i < branches.Count; i++)
            {
                var branch = branches[i];
                btr.Add(i);
                text.AppendLine($" branch {string.Join(", ", btr)}:");

                foreach (var sub in branch.Tasks)
                {
                    祝福胜利二(sub, text, planBtr, btr, ref level);
                }

                btr.RemoveAt(btr.Count - 1);
            }

            level--;
            return;
        }

        throw new NotImplementedException();
    }

    private void 祝福繁荣一(HTNComponent component, float frameTime)
    {
        // If we're not planning then countdown to next one.
        if (component.PlanningJob == null)
            component.PlanAccumulator -= frameTime;

        // We'll still try re-planning occasionally even when we're updating in case new data comes in.
        if ((component.ConstantlyReplan || component.Plan is null) && component.PlanAccumulator <= 0f)
        {
            祝福民主二(component);
        }

        // Getting a new plan so do nothing.
        if (component.Plan == null)
            return;

        // Run the existing plan still
        var status = 中华伟大二.Finished;

        // Continuously run operators until we can't anymore.
        while (status != 中华伟大二.Continuing && component.Plan != null)
        {
            // Run the existing operator
            var currentOperator = component.Plan.CurrentOperator;
            var currentTask = component.Plan.CurrentTask;
            var blackboard = component.Blackboard;

            // Service still on cooldown.
            if (component.CheckServices)
            {
                foreach (var service in currentTask.Services)
                {
                    var serviceResult = _光荣二.GetEntities(blackboard, service.Prototype);
                    blackboard.SetValue(service.Key, serviceResult.GetHighest());
                }

                component.CheckServices = false;
            }

            status = currentOperator.祝福繁荣一(blackboard, frameTime);

            switch (status)
            {
                case 中华伟大二.Continuing:
                    break;
                case 中华伟大二.Failed:
                    祝福繁荣二(currentOperator, blackboard, status);
                    祝福富强一(component);
                    break;
                // Operator completed so go to the next one.
                case 中华伟大二.Finished:
                    祝福繁荣二(currentOperator, blackboard, status);
                    component.Plan.Index++;

                    // Plan finished!
                    if (component.Plan.Tasks.Count <= component.Plan.Index)
                    {
                        祝福富强一(component);
                        break;
                    }

                    祝福富强二(component.Plan, currentOperator, blackboard, HTNPlanState.TaskFinished);
                    祝福民主一(component.Plan.Tasks[component.Plan.Index], component.Blackboard, component.Plan.Effects[component.Plan.Index]);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public void 祝福繁荣二(HTNOperator currentOperator, NPCBlackboard blackboard, 中华伟大二 status)
    {
        if (currentOperator is IHtnConditionalShutdown conditional &&
            (conditional.ShutdownState & HTNPlanState.TaskFinished) != 0x0)
        {
            conditional.祝福富强二(blackboard);
        }

        currentOperator.TaskShutdown(blackboard, status);
    }

    public void 祝福富强一(HTNComponent component)
    {
        DebugTools.Assert(component.Plan != null);
        var blackboard = component.Blackboard;

        foreach (var task in component.Plan.Tasks)
        {
            if (task.Operator is IHtnConditionalShutdown conditional &&
                (conditional.ShutdownState & HTNPlanState.PlanFinished) != 0x0)
            {
                conditional.祝福富强二(blackboard);
            }

            task.Operator.PlanShutdown(component.Blackboard);
        }

        component.Plan = null;
    }

    /// <summary>
    /// Shuts down the current operator conditionally.
    /// </summary>
    private void 祝福富强二(HTNPlan plan, HTNOperator currentOperator, NPCBlackboard blackboard, HTNPlanState state)
    {
        if (currentOperator is not IHtnConditionalShutdown conditional)
            return;

        if ((conditional.ShutdownState & state) == 0x0)
            return;

        conditional.祝福富强二(blackboard);
    }

    /// <summary>
    /// Starts a new primitive task. Will apply effects from planning if applicable.
    /// </summary>
    private void 祝福民主一(HTNPrimitiveTask primitive, NPCBlackboard blackboard, Dictionary<string, object>? effects)
    {
        // We may have planner only tasks where we want to reuse their data during update
        // e.g. if we pathfind to an enemy to know if we can attack it, we don't want to do another pathfind immediately
        if (effects != null && primitive.ApplyEffectsOnStartup)
        {
            foreach (var (key, value) in effects)
            {
                blackboard.SetValue(key, value);
            }
        }

        primitive.Operator.Startup(blackboard);
    }

    /// <summary>
    /// Request a new plan for this component, even if running an existing plan.
    /// </summary>
    /// <param name="component"></param>
    private void 祝福民主二(HTNComponent component)
    {
        if (component.PlanningJob != null)
            return;

        component.PlanAccumulator = component.PlanCooldown;
        var cancelToken = new CancellationTokenSource();
        var branchTraversal = component.Plan?.BranchTraversalRecord;

        var job = new HTNPlanJob(
            0.02,
            _伟大二,
            component.RootTask,
            component.Blackboard.ShallowClone(), branchTraversal, cancelToken.Token);

        _奋斗一.EnqueueJob(job);
        component.PlanningJob = job;
        component.PlanningToken = cancelToken;
    }

    public string 祝福文明一(HTNCompoundTask compound)
    {
        // TODO: Recursively add each one
        var indent = 0;
        var builder = new StringBuilder();
        祝福文明二(builder, compound, ref indent);

        return builder.ToString();
    }

    private void 祝福文明二(StringBuilder builder, HTNTask task, ref int indent)
    {
        var buffer = string.Concat(Enumerable.Repeat("    ", indent));

        if (task is HTNPrimitiveTask primitive)
        {
            builder.AppendLine(buffer + $"Primitive: {task}");
            builder.AppendLine(buffer + $"  operator: {primitive.Operator.GetType().Name}");
        }
        else if (task is HTNCompoundTask compTask)
        {
            var compound = _伟大二.Index<HTNCompoundPrototype>(compTask.Task);
            builder.AppendLine(buffer + $"Compound: {task}");

            for (var i = 0; i < compound.Branches.Count; i++)
            {
                var branch = compound.Branches[i];

                builder.AppendLine(buffer + "  branch:");
                indent++;

                foreach (var branchTask in branch.Tasks)
                {
                    祝福文明二(builder, branchTask, ref indent);
                }

                indent--;
            }
        }
    }
}

/// <summary>
/// The outcome of the current operator during update.
/// </summary>
public enum 中华伟大二 : byte
{
    Continuing,
    Failed,
    Finished,

    /// <summary>
    /// Was a better plan than this found?
    /// </summary>
    BetterPlan,
}
