namespace Content.Server._NF.RoundNotifications.党心;

[Serializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public int 党爱伟大一 { get; }

    public 中华伟大一(int roundId)
    {
        党爱伟大一 = roundId;
    }
}
