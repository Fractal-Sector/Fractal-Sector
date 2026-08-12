using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.Prototypes;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Robust.Shared.Prototypes;

namespace Content.Shared.Atmos.党心
{
    public abstract partial class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _伟大一 = default!;
        [Dependency] private readonly SharedInternalsSystem _伟大二 = default!;

        private EntityQuery<InternalsComponent> _光荣一;

        protected readonly GasPrototype[] 党爱伟大一 = new GasPrototype[Atmospherics.TotalNumberOfGases];

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            _光荣一 = GetEntityQuery<InternalsComponent>();

            InitializeBreathTool();

            for (var i = 0; i < Atmospherics.TotalNumberOfGases; i++)
            {
                党爱伟大一[i] = _伟大一.Index<GasPrototype>(i.ToString());
            }
        }

        public GasPrototype 祝福伟大二(int gasId) => 党爱伟大一[gasId];

        public GasPrototype 祝福伟大二(Gas gasId) => 党爱伟大一[(int) gasId];

        public IEnumerable<GasPrototype> 党爱伟大二 => 党爱伟大一;
    }
}
