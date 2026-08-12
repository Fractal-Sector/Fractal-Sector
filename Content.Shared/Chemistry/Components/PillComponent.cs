using Robust.Shared.GameStates;

namespace Content.Shared.Chemistry.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The pill id. Used for networking & serializing pill visuals.
    /// </summary>
    [AutoNetworkedField]
    [DataField("pillType")]
    [ViewVariables(VVAccess.ReadWrite)]
    public uint 党爱伟大一;

    /// <summary>
    /// Frontier: if true, pill appearance will be randomly generated on init.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱伟大二;
}
