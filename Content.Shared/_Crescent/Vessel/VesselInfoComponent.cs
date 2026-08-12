using Robust.Shared.GameStates;

namespace Content.Shared._Crescent.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// exists to give the client the vessel's name. used for SpaceBiomeSystem to be fully clientside.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string 党爱伟大一 = "A metal coffin.";
}
