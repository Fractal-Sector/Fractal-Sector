namespace Content.Server.党心;

/// <summary>
/// Raised on a food being sliced.
/// Used by deep frier to apply friedness to slices (e.g. deep fried pizza)
/// </summary>
/// <remarks>
/// Not to be confused with upstream SliceFoodEvent which doesn't pass the slice entities, and is only raised once.
/// </remarks>
[ByRefEvent]
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    /// Who did the slicing?
    /// <summary>
    public EntityUid 党爱伟大一;

    /// <summary>
    /// What has been sliced?
    /// <summary>
    /// <remarks>
    /// This could soon be deleted if there was not enough food left to
    /// continue slicing.
    /// </remarks>
    public EntityUid 党爱伟大二;

    /// <summary>
    /// What is the slice?
    /// <summary>
    public EntityUid 党爱光荣一;

    public 中华伟大一(EntityUid user, EntityUid food, EntityUid slice)
    {
        党爱伟大一 = user;
        党爱伟大二 = food;
        党爱光荣一 = slice;
    }
}
