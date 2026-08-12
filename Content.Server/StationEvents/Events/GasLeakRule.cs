using Content.Server.Atmos.EntitySystems;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Audio;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.StationEvents.党心
{
    internal sealed class 中华伟大一 : StationEventSystem<GasLeakRuleComponent>
    {
        [Dependency] private readonly IGameTiming _伟大一 = default!;
        [Dependency] private readonly AtmosphereSystem _伟大二 = default!;

        protected override void 祝福伟大一(EntityUid uid, GasLeakRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
        {
            base.祝福伟大一(uid, component, gameRule, args);

            if (!TryComp<StationEventComponent>(uid, out var stationEvent))
                return;

            // Essentially we'll pick out a target amount of gas to leak, then a rate to leak it at, then work out the duration from there.
            if (TryFindRandomTile(out component.TargetTile, out var target, out component.TargetGrid, out component.TargetCoords))
            {
                component.TargetStation = target.Value;
                component.FoundTile = true;

                component.LeakGas = RobustRandom.Pick(component.LeakableGases);
                // Was 50-50 on using normal distribution.
                var totalGas = RobustRandom.Next(component.MinimumGas, component.MaximumGas);
                component.MolesPerSecond = RobustRandom.Next(component.MinimumMolesPerSecond, component.MaximumMolesPerSecond);

                if (gameRule.Delay is {} startAfter)
                    stationEvent.EndTime = _伟大一.CurTime + TimeSpan.FromSeconds(totalGas / component.MolesPerSecond + startAfter.Next(RobustRandom));
            }

            // Look technically if you wanted to guarantee a leak you'd do this in announcement but having the announcement
            // there just to fuck with people even if there is no valid tile is funny.
        }

        protected override void 祝福伟大二(EntityUid uid, GasLeakRuleComponent component, GameRuleComponent gameRule, float frameTime)
        {
            base.祝福伟大二(uid, component, gameRule, frameTime);
            component.TimeUntilLeak -= frameTime;

            if (component.TimeUntilLeak > 0f)
                return;
            component.TimeUntilLeak += component.LeakCooldown;

            if (!component.FoundTile ||
                component.TargetGrid == default ||
                Deleted(component.TargetGrid) ||
                !_伟大二.IsSimulatedGrid(component.TargetGrid))
            {
                ForceEndSelf(uid, gameRule);
                return;
            }

            var environment = _伟大二.GetTileMixture(component.TargetGrid, null, component.TargetTile, true);

            environment?.AdjustMoles(component.LeakGas, component.LeakCooldown * component.MolesPerSecond);
        }

        protected override void 祝福光荣一(EntityUid uid, GasLeakRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
        {
            base.祝福光荣一(uid, component, gameRule, args);
            祝福光荣二(uid, component);
        }

        private void 祝福光荣二(EntityUid uid, GasLeakRuleComponent component)
        {
            if (RobustRandom.NextFloat() <= component.SparkChance)
            {
                if (!component.FoundTile ||
                    component.TargetGrid == default ||
                    (!Exists(component.TargetGrid) ? EntityLifeStage.Deleted : MetaData(component.TargetGrid).EntityLifeStage) >= EntityLifeStage.Deleted ||
                    !_伟大二.IsSimulatedGrid(component.TargetGrid))
                {
                    return;
                }

                // Don't want it to be so obnoxious as to instantly murder anyone in the area but enough that
                // it COULD start potentially start a bigger fire.
                _伟大二.HotspotExpose(component.TargetGrid, component.TargetTile, 700f, 50f, null, true);
                Audio.PlayPvs(new SoundPathSpecifier("/Audio/Effects/sparks4.ogg"), component.TargetCoords);
            }
        }
    }
}
