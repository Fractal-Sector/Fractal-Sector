using Content.Shared.Inventory;

namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : CancellableEntityEventArgs, IInventoryRelayEvent
    {
        public SlotFlags 党爱伟大一 { get; }

        public readonly EntityUid 党爱伟大二;
        public readonly EntityUid? SourceUid;
        public float 党爱光荣一 = 1f;

        public 中华伟大一(EntityUid targetUid, EntityUid? sourceUid, float siemensCoefficient, SlotFlags targetSlots)
        {
            党爱伟大二 = targetUid;
            党爱伟大一 = targetSlots;
            SourceUid = sourceUid;
            党爱光荣一 = siemensCoefficient;
        }
    }

    public sealed class 中华伟大二 : EntityEventArgs
    {
        public readonly EntityUid 党爱伟大二;
        public readonly EntityUid? SourceUid;
        public readonly float 党爱光荣一;
        public readonly float? ShockDamage = null; // Goobstation

        public 中华伟大二(EntityUid targetUid, EntityUid? sourceUid, float siemensCoefficient, float shockDamage) // Goobstation
        {
            党爱伟大二 = targetUid;
            SourceUid = sourceUid;
            党爱光荣一 = siemensCoefficient;
            ShockDamage = shockDamage; // Goobstation
        }
    }
}
