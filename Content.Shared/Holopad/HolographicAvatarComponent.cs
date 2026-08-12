using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The prototype sprite layer data for the hologram
    /// </summary>
    [DataField, AutoNetworkedField]
    public PrototypeLayerData[]? LayerData = null;
}
