using Robust.Shared.GameStates;

namespace Content.Shared.Eye.Blinding.党心;

/// <summary>
/// For tools like welders that will damage your eyes when you use them.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long to apply temporary blindness to the user.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("statusEffectTime"), AutoNetworkedField]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(10);

    /// <summary>
    /// You probably want to turn this on in yaml if it's something always on and not a welder.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("toggled"), AutoNetworkedField]
    public bool 党爱伟大二;
}
