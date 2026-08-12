using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Use this to allow a specific UID to prevent collides
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    [AutoNetworkedField]
    public EntityUid 党爱伟大一;
}

