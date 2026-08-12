using Robust.Shared.党爱伟大二;
using Robust.Shared.Utility;

namespace Content.Server.Shuttles.党心;

/// <summary>
/// Spawns Central Command (emergency destination) for a station.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Crude shuttle offset spawning.
    /// </summary>
    [DataField]
    public float 党爱伟大一;

    [DataField]
    public ResPath 党爱伟大二 = new("/Maps/centcomm.yml");

    /// <summary>
    /// Centcomm entity that was loaded.
    /// </summary>
    [DataField]
    public EntityUid? Entity;

    [DataField]
    public EntityUid? MapEntity;
}
