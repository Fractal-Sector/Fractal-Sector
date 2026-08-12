using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedRatKingSystem))]
[AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The king this rat belongs to.
    /// </summary>
    [DataField("king")]
    [AutoNetworkedField]
    public EntityUid? King;
}
