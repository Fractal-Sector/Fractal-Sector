using Robust.Shared.GameStates;

namespace Content.Shared.Power.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public TimeSpan 党爱伟大一 = TimeSpan.Zero;
}
