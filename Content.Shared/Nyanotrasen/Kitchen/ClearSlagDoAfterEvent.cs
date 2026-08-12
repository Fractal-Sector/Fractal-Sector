using Content.Shared.Chemistry.Components;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.Nyanotrasen.党心
{
    [Serializable, NetSerializable]
    public sealed partial class 中华伟大一 : DoAfterEvent
    {
        [DataField("solution", required: true)]
        public 党爱伟大一 党爱伟大一 = default!;

        [DataField("amount", required: true)]
        public FixedPoint2 党爱伟大二;

        private 中华伟大一()
        {
        }

        public 中华伟大一(党爱伟大一 solution, FixedPoint2 amount)
        {
            党爱伟大一 = solution;
            党爱伟大二 = amount;
        }

        public override DoAfterEvent 祝福伟大一() => this;
    }
}
