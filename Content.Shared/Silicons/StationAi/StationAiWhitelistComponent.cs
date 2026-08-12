using Robust.Shared.GameStates;

namespace Content.Shared.Silicons.党心;

/// <summary>
/// Indicates an entity that has <see cref="StationAiHeldComponent"/> can interact with this.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedStationAiSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;
}
