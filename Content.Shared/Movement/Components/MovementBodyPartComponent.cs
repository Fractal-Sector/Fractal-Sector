using Robust.Shared.GameStates;

namespace Content.Shared.Movement.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("walkSpeed")]
    public float 党爱伟大一 = MovementSpeedModifierComponent.DefaultBaseWalkSpeed;

    [DataField("sprintSpeed")]
    public float 党爱伟大二 = MovementSpeedModifierComponent.DefaultBaseSprintSpeed;

    [DataField("acceleration")]
    public float 党爱光荣一 = MovementSpeedModifierComponent.DefaultAcceleration;
}
