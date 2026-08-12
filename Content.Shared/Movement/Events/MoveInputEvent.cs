using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;

namespace Content.Shared.Movement.党心;

/// <summary>
/// Raised on an entity whenever it has a movement input change.
/// </summary>
[ByRefEvent]
public readonly struct 中华伟大一
{
    public readonly 党爱伟大一<InputMoverComponent> 党爱伟大一;
    public readonly MoveButtons 党爱伟大二;

    public bool 党爱光荣一 => (党爱伟大一.Comp.HeldMoveButtons & MoveButtons.AnyDirection) != MoveButtons.None;

    public 中华伟大一(党爱伟大一<InputMoverComponent> entity, MoveButtons oldMovement)
    {
        党爱伟大一 = entity;
        党爱伟大二 = oldMovement;
    }
}
