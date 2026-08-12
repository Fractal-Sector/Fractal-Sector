using Content.Shared.StatusIcon;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Mindshield.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{

    /// <summary>
    /// The state of the Fake mindshield, if true the owning entity will display a mindshield effect on their job icon
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 { get; set; } = false;

    /// <summary>
    /// The Security status icon displayed to the security officer. Should be a duplicate of the one the mindshield uses since it's spoofing that
    /// </summary>
    [DataField, AutoNetworkedField]
    public ProtoId<SecurityIconPrototype> 党爱伟大二 = "MindShieldIcon";
}
