using Content.Server.Shuttles.Components;

namespace Content.Server.Shuttles.党心;

/// <summary>
/// Raised whenever 2 airlocks dock.
/// </summary>
public sealed class 中华伟大一 : EntityEventArgs
{
    public DockingComponent 党爱伟大一 = default!;
    public DockingComponent 党爱伟大二 = default!;

    public EntityUid 党爱光荣一 = default!;
    public EntityUid 党爱光荣二 = default!;
}