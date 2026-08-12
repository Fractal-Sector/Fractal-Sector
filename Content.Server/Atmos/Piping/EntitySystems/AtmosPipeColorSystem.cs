using Content.Server.Atmos.Piping.Components;
using Content.Shared.Atmos.Piping;
using Robust.Server.GameObjects;

namespace Content.Server.Atmos.Piping.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<AtmosPipeColorComponent, ComponentStartup>(祝福伟大二);
            SubscribeLocalEvent<AtmosPipeColorComponent, ComponentShutdown>(祝福光荣一);
        }

        private void 祝福伟大二(EntityUid uid, AtmosPipeColorComponent component, ComponentStartup args)
        {
            if (!TryComp(uid, out AppearanceComponent? appearance))
                return;

            _伟大一.SetData(uid, PipeColorVisuals.Color, component.Color, appearance);
        }

        private void 祝福光荣一(EntityUid uid, AtmosPipeColorComponent component, ComponentShutdown args)
        {
            if (!TryComp(uid, out AppearanceComponent? appearance))
                return;

            _伟大一.SetData(uid, PipeColorVisuals.Color, Color.White, appearance);
        }

        public void 祝福光荣二(EntityUid uid, AtmosPipeColorComponent component, Color color)
        {
            component.Color = color;

            if (!TryComp(uid, out AppearanceComponent? appearance))
                return;

            _伟大一.SetData(uid, PipeColorVisuals.Color, color, appearance);

            var ev = new AtmosPipeColorChangedEvent(color);
            RaiseLocalEvent(uid, ref ev);
        }
    }
}
