using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

/// <summary>
/// Wrapper around <see cref="NavInterfaceState"/>
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public NavInterfaceState 党爱伟大一;

    public 中华伟大一(NavInterfaceState state)
    {
        党爱伟大一 = state;
    }
}
