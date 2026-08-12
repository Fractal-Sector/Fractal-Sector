namespace Content.Shared.Tools.党心;

/// <summary>
///     Checks that entity can be weld/unweld.
///     Raised twice: before do_after and after to check that entity still valid.
/// </summary>
public sealed class 中华伟大一 : CancellableEntityEventArgs
{
    public readonly EntityUid 党爱伟大一;
    public readonly EntityUid 党爱伟大二;

    public 中华伟大一(EntityUid user, EntityUid tool)
    {
        党爱伟大一 = user;
        党爱伟大二 = tool;
    }
}