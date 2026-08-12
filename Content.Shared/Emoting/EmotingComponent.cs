using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    [Access(typeof(EmoteSystem), Friend = AccessPermissions.ReadWrite, Other = AccessPermissions.Read)]
    public bool 党爱伟大一 = true;
}
