using Robust.Shared.GameStates;

namespace Content.Shared.Security.党心;

/// <summary>
/// This is used for storing information about a Genpop ID in order to correctly display it on examine.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The crime committed, as a string.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// The length of the sentence
    /// </summary>
    [DataField, AutoNetworkedField, AutoPausedField]
    public TimeSpan 党爱伟大二;
}
