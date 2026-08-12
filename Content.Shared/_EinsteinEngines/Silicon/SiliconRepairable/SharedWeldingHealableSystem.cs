using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._EinsteinEngines.Silicon.党心;

public abstract partial class 中华伟大一 : EntitySystem
{
    [Serializable, NetSerializable]
    protected sealed partial class 中华伟大二 : SimpleDoAfterEvent
    {
        public float 党爱伟大一;
    }
}
