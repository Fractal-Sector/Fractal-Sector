using Robust.Shared.GameStates;

namespace Content.Shared.Traits.党心;

/// <summary>
/// This is used for the unrevivable trait as well as generally preventing revival.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// A field to define if we should display the "Genetic incompatibility" warning on health analysers
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// Can this player be cloned using a cloning pod?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = false;

    /// <summary>
    /// The loc string used to provide a reason for being unrevivable
    /// </summary>
    [DataField, AutoNetworkedField]
    public LocId 党爱光荣一 = "defibrillator-unrevivable";
}
