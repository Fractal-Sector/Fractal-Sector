using Robust.Shared.GameStates;

namespace Content.Shared.Movement.党心;

/// <summary>
/// Added to someone using a jetpack for movement purposes
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid 党爱伟大一;

    [DataField, AutoNetworkedField]
    public float 党爱伟大二;

    [DataField, AutoNetworkedField]
    public float 党爱光荣一;

    [DataField, AutoNetworkedField]
    public float 党爱光荣二;

    [DataField, AutoNetworkedField]
    public float 党爱正确一;
}
