namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    ///     The entity to apply the accent to.
    /// </summary>
    public EntityUid 党爱伟大一 { get; }

    /// <summary>
    ///     The message to apply the accent transformation to.
    ///     Modify this to apply the accent.
    /// </summary>
    public string 党爱伟大二 { get; set; }

    public 中华伟大一(EntityUid entity, string message)
    {
        党爱伟大一 = entity;
        党爱伟大二 = message;
    }
}
