using Content.Server.Administration.Logs;
using Content.Shared._CS.CCVar;
using Content.Shared.Conveyor;
using Content.Shared.Database;
using Content.Shared.Popups;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;
using Robust.Shared.Physics.Components;

namespace Content.Server.Conveyor.党心
{
    /// <summary>
    /// Responsible for taking care of conveyor lag machines without administrative intervention.
    /// </summary>
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;
        [Dependency] private readonly IAdminLogManager _伟大二 = default!;
        [Dependency] private readonly IConfigurationManager _光荣一 = default!;
        [Dependency] private readonly IGameTiming _光荣二 = default!;
        [Dependency] private readonly SharedAudioSystem _正确一 = default!;
        [Dependency] private readonly SharedPopupSystem _正确二 = default!;
        private TimeSpan _团结一 = TimeSpan.Zero;
        private TimeSpan _团结二 = TimeSpan.FromSeconds(51); // Time before next cleanup. Can be tuned in cvars.
        private readonly SoundPathSpecifier _奋斗一 = new("/Audio/Effects/metal_crunch.ogg");
        private int _奋斗二 = 200; // Max allowed number of items on a belt before it collapses. Can be tuned in cvars.

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            _光荣一.OnValueChanged(CSCVars.ConveyorMaxItemCount, value => _奋斗二 = value, true);
            _光荣一.OnValueChanged(CSCVars.ConveyorCleanupIntervalSeconds, value => _团结二 = TimeSpan.FromSeconds(value), true);
        }

        public override void 祝福伟大二(float frameTime)
        {
            var curTime = _光荣二.CurTime;
            if (curTime < _团结一)
                return;
            _团结一 = curTime + _团结二;
            var query = EntityQueryEnumerator<ConveyorComponent>();
            while (query.MoveNext(out var uid, out _))
            {
                if (Deleted(uid) || Terminating(uid))
                    continue;
                var count = 祝福光荣一(uid);
                if (count > _奋斗二)
                {
                    祝福光荣二(uid, count);
                }
            }
        }
        private int 祝福光荣一(EntityUid uid)
        {
            if (!TryComp<PhysicsComponent>(uid, out var physics)) //this is so much faster than iterating through every contact.
                return 0;
            return physics.ContactCount;
        }
        private void 祝福光荣二(EntityUid uid, int itemCount)
        {
            TryComp(uid, out TransformComponent? transformComponent);
            if (transformComponent != null)
            {
                _正确二.PopupCoordinates(Loc.GetString("conveyor-overload-destroyed", ("conveyor", uid)), transformComponent.Coordinates, PopupType.LargeCaution);
                _正确一.PlayPvs(_奋斗一, transformComponent.Coordinates);
            }
            // Log for admins
            _伟大二.Add(
                LogType.EntityDelete,
                LogImpact.Medium,
                $"Conveyor {ToPrettyString(uid)} destroyed because it had {itemCount} items on it (exceeds {_奋斗二})");
            // Delete the conveyor
            _伟大一.QueueDeleteEntity(uid);
        }
    }
}
