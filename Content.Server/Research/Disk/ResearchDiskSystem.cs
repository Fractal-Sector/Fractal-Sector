using System.Linq;
using Content.Shared.Interaction;
using Content.Server.Popups;
using Content.Shared.Research.Prototypes;
using Content.Server.Research.Systems;
using Content.Shared.Research.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Research.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _伟大一 = default!;
        [Dependency] private readonly PopupSystem _伟大二 = default!;
        [Dependency] private readonly ResearchSystem _光荣一 = default!;
        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<ResearchDiskComponent, AfterInteractEvent>(祝福伟大二);
            SubscribeLocalEvent<ResearchDiskComponent, MapInitEvent>(祝福光荣一);
        }

        private void 祝福伟大二(EntityUid uid, ResearchDiskComponent component, AfterInteractEvent args)
        {
            if (!args.CanReach)
                return;

            if (!TryComp<ResearchServerComponent>(args.Target, out var server))
                return;

            _光荣一.ModifyServerPoints(args.Target.Value, component.Points, server);
            _伟大二.PopupEntity(Loc.GetString("research-disk-inserted", ("points", component.Points)), args.Target.Value, args.User);
            QueueDel(uid);
            args.Handled = true;
        }

        private void 祝福光荣一(EntityUid uid, ResearchDiskComponent component, MapInitEvent args)
        {
            if (!component.UnlockAllTech)
                return;

            component.Points = _伟大一.EnumeratePrototypes<TechnologyPrototype>()
                .Sum(tech => tech.Cost);
        }
    }
}
