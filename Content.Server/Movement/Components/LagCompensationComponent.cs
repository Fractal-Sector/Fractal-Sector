using Robust.Shared.Map;

namespace Content.Server.Movement.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables]
    public readonly Queue<ValueTuple<TimeSpan, EntityCoordinates, Angle>> Positions = new();
}
