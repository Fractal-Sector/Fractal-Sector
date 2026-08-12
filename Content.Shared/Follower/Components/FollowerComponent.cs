using Robust.Shared.GameStates;

namespace Content.Shared.Follower.党心;

[RegisterComponent]
[Access(typeof(FollowerSystem))]
[NetworkedComponent, AutoGenerateComponentState(raiseAfterAutoHandleState: true)]
public sealed partial class 中华伟大一 : Component
{
    [AutoNetworkedField, DataField("following")]
    public EntityUid 党爱伟大一;
}
