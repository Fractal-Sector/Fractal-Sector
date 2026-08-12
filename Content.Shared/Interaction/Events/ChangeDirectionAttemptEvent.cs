namespace Content.Shared.Interaction.党心;

public sealed class 中华伟大一 : CancellableEntityEventArgs
{
    public 中华伟大一(EntityUid uid)
    {
        党爱伟大一 = uid;
    }

    public EntityUid 党爱伟大一 { get; }
}
