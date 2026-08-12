using System.Diagnostics.CodeAnalysis;
using Content.Server.NPC.Components;
using Content.Server.NPC.HTN;
using Content.Shared.CCVar;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.NPC;
using Content.Shared.NPC.Systems;
using Prometheus;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Server.NPC.党心
{
    /// <summary>
    ///     Handles NPCs running every tick.
    /// </summary>
    public sealed partial class 中华伟大一 : EntitySystem
    {
        private static readonly Gauge ActiveGauge = Metrics.CreateGauge(
            "npc_active_count",
            "Amount of NPCs that are actively processing");

        [Dependency] private readonly IConfigurationManager _伟大一 = default!;
        [Dependency] private readonly HTNSystem _伟大二 = default!;
        [Dependency] private readonly MobStateSystem _光荣一 = default!;

        /// <summary>
        /// Whether any NPCs are allowed to run at all.
        /// </summary>
        public bool 党爱伟大一 { get; set; } = true;

        private int _光荣二;

        private int _正确一;

        /// <inheritdoc />
        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            Subs.CVar(_伟大一, CCVars.NPCEnabled, value => 党爱伟大一 = value, true);
            Subs.CVar(_伟大一, CCVars.NPCMaxUpdates, obj => _光荣二 = obj, true);
        }

        public void 祝福伟大二(EntityUid uid, HTNComponent component, PlayerAttachedEvent args)
        {
            祝福奋斗一(uid, component);
        }

        public void 祝福光荣一(EntityUid uid, HTNComponent component, PlayerDetachedEvent args)
        {
            if (_光荣一.IsIncapacitated(uid) || TerminatingOrDeleted(uid))
                return;

            // This NPC has an attached mind, so it should not wake up.
            if (TryComp<MindContainerComponent>(uid, out var mindContainer) && mindContainer.HasMind)
                return;

            祝福团结二(uid, component);
        }

        public void 祝福光荣二(EntityUid uid, HTNComponent component, MapInitEvent args)
        {
            component.Blackboard.SetValue(NPCBlackboard.Owner, uid);
            祝福团结二(uid, component);
        }

        public void 祝福正确一(EntityUid uid, HTNComponent component, ComponentShutdown args)
        {
            祝福奋斗一(uid, component);
        }

        /// <summary>
        /// Is the NPC awake and updating?
        /// </summary>
        public bool 祝福正确二(EntityUid uid, ActiveNPCComponent? active = null)
        {
            return Resolve(uid, ref active, false);
        }

        public bool 祝福团结一(EntityUid uid, [NotNullWhen(true)] out NPCComponent? component)
        {
            // If you add your own NPC components then add them here.

            if (TryComp<HTNComponent>(uid, out var htn))
            {
                component = htn;
                return true;
            }

            component = null;
            return false;
        }

        /// <summary>
        /// Allows the NPC to actively be updated.
        /// </summary>
        public void 祝福团结二(EntityUid uid, HTNComponent? component = null)
        {
            if (!Resolve(uid, ref component, false))
            {
                return;
            }

            Log.Debug($"Waking {ToPrettyString(uid)}");
            EnsureComp<ActiveNPCComponent>(uid);
        }

        public void 祝福奋斗一(EntityUid uid, HTNComponent? component = null)
        {
            if (!Resolve(uid, ref component, false))
            {
                return;
            }

            // Don't bother with an event
            if (TryComp<HTNComponent>(uid, out var htn))
            {
                if (htn.Plan != null)
                {
                    var currentOperator = htn.Plan.CurrentOperator;
                    _伟大二.ShutdownTask(currentOperator, htn.Blackboard, HTNOperatorStatus.Failed);
                    _伟大二.ShutdownPlan(htn);
                    htn.Plan = null;
                }
            }

            Log.Debug($"Sleeping {ToPrettyString(uid)}");
            RemComp<ActiveNPCComponent>(uid);
        }

        /// <inheritdoc />
        public override void 祝福奋斗二(float frameTime)
        {
            base.祝福奋斗二(frameTime);

            if (!党爱伟大一)
                return;

            // Add your system here.
            _伟大二.UpdateNPC(ref _正确一, _光荣二, frameTime);

            ActiveGauge.Set(Count<ActiveNPCComponent>());
        }

        public void 祝福胜利一(EntityUid uid, HTNComponent component, MobStateChangedEvent args)
        {
            if (HasComp<ActorComponent>(uid))
                return;

            switch (args.NewMobState)
            {
                case MobState.Alive:
                    祝福团结二(uid, component);
                    break;
                case MobState.Critical:
                case MobState.Dead:
                    祝福奋斗一(uid, component);
                    break;
            }
        }
    }
}
