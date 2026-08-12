using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;

namespace Content.Server._NF.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public ContainerSlot 党爱伟大一 = default!;

    /// <summary>
    /// The sound that is played when a player leaves the game via cryo
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱伟大二 = new SoundCollectionSpecifier("RadiationPulse");

    /// <summary>
    ///   The ID of the latest DoAfter event associated with this entity. May be null if there's no DoAfter going on.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public DoAfterId? CryosleepDoAfter = null;

    /// <summary>
    /// The next time something should be able to try and escape the pod.
    /// </summary>
    [ViewVariables]
    public TimeSpan 党爱光荣一;

    /// <summary>
    /// The amount of time to wait between attempting to remove entities from the pod.
    /// </summary>
    [ViewVariables]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(0.5);
}
