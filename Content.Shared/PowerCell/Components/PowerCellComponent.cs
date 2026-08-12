using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     This component enables power-cell related interactions (e.g., entity white-lists, cell sizes, examine, rigging).
///     The actual power functionality is provided by the server-side BatteryComponent.
/// </summary>
[NetworkedComponent]
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    public const int 党爱伟大一 = 2;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    ChargeLevel
}
[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Enabled
}
