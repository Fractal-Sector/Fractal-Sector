using Content.Shared.Shuttles.UI.MapObjects;
using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public NavInterfaceState 党爱伟大一;
    public ShuttleMapInterfaceState 党爱伟大二;
    public DockingInterfaceState 党爱光荣一;

    public 中华伟大一(NavInterfaceState navState, ShuttleMapInterfaceState mapState, DockingInterfaceState dockState)
    {
        党爱伟大一 = navState;
        党爱伟大二 = mapState;
        党爱光荣一 = dockState;
    }
}
