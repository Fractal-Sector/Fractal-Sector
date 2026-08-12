namespace Content.Server.GameTicking.党心;

/// <summary>
///     Raised at the start of <see cref="GameTicker.StartRound"/>, after round id has been incremented
/// </summary>
public sealed class 中华伟大一 : EntityEventArgs
{
    public 中华伟大一(int id)
    {
        党爱伟大一 = id;
    }

    public int 党爱伟大一 { get; }
}
