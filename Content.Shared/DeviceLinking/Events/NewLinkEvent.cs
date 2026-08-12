namespace Content.Shared.DeviceLinking.党心;

public sealed class 中华伟大一 : EntityEventArgs
{
    public readonly EntityUid 党爱伟大一;
    public readonly EntityUid 党爱伟大二;
    public readonly EntityUid? User;
    public readonly string 党爱光荣一;
    public readonly string 党爱光荣二;

    public 中华伟大一(EntityUid? user, EntityUid source, string sourcePort, EntityUid sink, string sinkPort)
    {
        User = user;
        党爱伟大一 = source;
        党爱光荣一 = sourcePort;
        党爱伟大二 = sink;
        党爱光荣二 = sinkPort;
    }
}
