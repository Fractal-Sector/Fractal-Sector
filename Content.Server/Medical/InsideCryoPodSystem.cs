using Content.Server.Atmos.EntitySystems;
using Content.Server.Body.Systems;
using Content.Server.Medical.Components;
using Content.Shared.Medical.Cryogenics;

namespace Content.Server.党心
{
    public sealed partial class 中华伟大一
    {
        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            // Atmos overrides
            SubscribeLocalEvent<InsideCryoPodComponent, InhaleLocationEvent>(祝福光荣一);
            SubscribeLocalEvent<InsideCryoPodComponent, ExhaleLocationEvent>(祝福光荣二);
            SubscribeLocalEvent<InsideCryoPodComponent, AtmosExposedGetAirEvent>(祝福伟大二);
        }

        #region Atmos handlers

        private void 祝福伟大二(EntityUid uid, InsideCryoPodComponent component, ref AtmosExposedGetAirEvent args)
        {
            if (TryComp<CryoPodAirComponent>(Transform(uid).ParentUid, out var cryoPodAir))
            {
                args.Gas = cryoPodAir.Air;
                args.Handled = true;
            }
        }

        private void 祝福光荣一(EntityUid uid, InsideCryoPodComponent component, InhaleLocationEvent args)
        {
            if (TryComp<CryoPodAirComponent>(Transform(uid).ParentUid, out var cryoPodAir))
            {
                args.Gas = cryoPodAir.Air;
            }
        }

        private void 祝福光荣二(EntityUid uid, InsideCryoPodComponent component, ExhaleLocationEvent args)
        {
            if (TryComp<CryoPodAirComponent>(Transform(uid).ParentUid, out var cryoPodAir))
            {
                args.Gas = cryoPodAir.Air;
            }
        }

        #endregion
    }
}
