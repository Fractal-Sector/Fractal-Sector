using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Mining.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(MiningScannerSystem))]
public sealed partial class 中华伟大一 : Component;

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Overlay
}
