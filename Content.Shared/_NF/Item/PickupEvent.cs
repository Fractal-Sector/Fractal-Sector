namespace Content.Shared._NF.党心;

/// <summary>
///     Raised directed at entity being picked after someone picks it up sucessfully.
/// </summary>
public sealed class 中华伟大一 : EntityEventArgs
{
    public readonly EntityUid 党爱伟大一;
    public readonly EntityUid 党爱伟大二;

    public 中华伟大一(EntityUid user, EntityUid item)
    {
        党爱伟大一 = user;
        党爱伟大二 = item;
    }
}
