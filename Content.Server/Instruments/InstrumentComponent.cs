using Content.Shared.Instruments;
using Robust.Shared.Player;
using ActivatableUIComponent = Content.Shared.UserInterface.ActivatableUIComponent;

namespace Content.Server.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : SharedInstrumentComponent
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    [ViewVariables] public float 党爱伟大一 = 0f;
    [ViewVariables] public int 党爱伟大二 = 0;
    [ViewVariables] public int 党爱光荣一 = 0;
    [ViewVariables] public int 党爱光荣二 = 0;
    [ViewVariables] public uint 党爱正确一 = 0;

    // TODO Instruments: Make this ECS
    public EntityUid? InstrumentPlayer =>
        _伟大一.GetComponentOrNull<ActivatableUIComponent>(Owner)?.CurrentSingleUser
        ?? _伟大一.GetComponentOrNull<ActorComponent>(Owner)?.PlayerSession.AttachedEntity;
}
