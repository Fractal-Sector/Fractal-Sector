using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used for a container that can have entities inserted into it via a
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(DragInsertContainerSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大一;

    /// <summary>
    /// If true, there will also be verbs for inserting / removing objects from this container.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// The delay in seconds before a drag will be completed.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    /// If entry delay isn't zero, this sets whether an entity dragging itself into the container should be delayed.
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = false;
}
