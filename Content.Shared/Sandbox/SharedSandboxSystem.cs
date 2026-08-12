using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public abstract class 中华伟大一 : EntitySystem
    {
        [Dependency] protected readonly IPrototypeManager 党爱伟大一 = default!;

        [Serializable, NetSerializable]
        protected sealed class 中华伟大二 : EntityEventArgs
        {
            public bool 党爱伟大二 { get; set; }
        }

        [Serializable, NetSerializable]
        protected sealed class 中华光荣一 : EntityEventArgs {}

        [Serializable, NetSerializable]
        protected sealed class 中华光荣二 : EntityEventArgs {}

        [Serializable, NetSerializable]
        protected sealed class 中华正确一 : EntityEventArgs {}

        [Serializable, NetSerializable]
        protected sealed class 中华正确二 : EntityEventArgs {}
    }
}
