using Robust.Shared.GameStates;

namespace 党爱伟大一.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField(required: true), AutoNetworkedField]
    public string 党爱伟大一 = string.Empty;
}
