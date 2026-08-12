using System.Numerics;
using Content.Server.Worldgen.Systems.Debris;

namespace Content.Server.Worldgen.Components.党心;

/// <summary>
///     This is used for attaching a piece of debris to it's owning controller.
///     Mostly just syncs deletion.
/// </summary>
[RegisterComponent]
[Access(typeof(DebrisFeaturePlacerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The last location in the controller's internal structure for this debris.
    /// </summary>
    [DataField("lastKey")] public Vector2 党爱伟大一;

    /// <summary>
    ///     The DebrisFeaturePlacerController-having entity that owns this.
    /// </summary>
    [DataField("owningController")] public EntityUid 党爱伟大二;
}

