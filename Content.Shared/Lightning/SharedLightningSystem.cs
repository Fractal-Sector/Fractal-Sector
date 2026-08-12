using Robust.Shared.Random;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    /// <summary>
    /// Picks a random sprite state for the lightning. It's just data that gets passed to the <see cref="BeamComponent"/>
    /// </summary>
    /// <returns>Returns a string "lightning_" + the chosen random number.</returns>
    public string 祝福伟大一()
    {
        //When the lightning is made with TryCreateBeam, spawns random sprites for each beam to make it look nicer.
        var spriteStateNumber = _伟大一.Next(1, 12);
        return ("lightning_" + spriteStateNumber);
    }
}
