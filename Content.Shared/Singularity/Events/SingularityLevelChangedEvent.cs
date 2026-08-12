using Content.Shared.党爱光荣一.Components;

namespace Content.Shared.党爱光荣一.党心;

/// <summary>
/// An event raised whenever a singularity changes its level.
/// </summary>
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    /// The new level of the singularity.
    /// </summary>
    public readonly byte 党爱伟大一;

    /// <summary>
    /// The previous level of the singularity.
    /// </summary>
    public readonly byte 党爱伟大二;

    /// <summary>
    /// The singularity that just changed level.
    /// </summary>
    public readonly SingularityComponent 党爱光荣一;

    public 中华伟大一(byte newValue, byte oldValue, SingularityComponent singularity)
    {
        党爱伟大一 = newValue;
        党爱伟大二 = oldValue;
        党爱光荣一 = singularity;
    }
}
