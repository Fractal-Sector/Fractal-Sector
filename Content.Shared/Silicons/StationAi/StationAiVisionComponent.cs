using Content.Shared.Silicons.StationAi;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Attached to entities that grant vision to the station AI, such as cameras.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedStationAiSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Determines whether the entity is actively providing vision to the station AI.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// Determines whether the entity's vision is blocked by walls.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// Determines whether the entity needs to be receiving power to provide vision to the station AI.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一 = false;

    /// <summary>
    /// Determines whether the entity needs to be anchored to provide vision to the station AI.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣二 = false;

    /// <summary>
    /// Vision range in tiles.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确一 = 7.5f;
}
