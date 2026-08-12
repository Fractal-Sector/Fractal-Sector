using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[NetworkedComponent, EntityCategory("Spawner")]
public abstract partial class 中华伟大一 : Component
{
    [DataField("state")]
    public 中华伟大二 State = 中华伟大二.Charging;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Charging,
    AlmostFinished,
    Finished,
}
