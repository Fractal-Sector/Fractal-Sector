using Robust.Shared.Containers;
using Robust.Shared.Enums;

namespace Content.Shared.IdentityManagement.党心;

/// <summary>
///     Stores the identity entity (whose name is the users 'identity', etc)
///     for a given entity, and marks that it can have an identity at all.
/// </summary>
/// <remarks>
///     This is a <see cref="ContainerSlot"/> and not just a datum entity because we do sort of care that it gets deleted and sent with the user.
/// </remarks>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables]
    public ContainerSlot 党爱伟大一 = default!;
}

/// <summary>
///     A data structure representing the 'identity' of an entity as presented to
///     other players.
/// </summary>
public sealed class 中华伟大二
{
    public string 党爱伟大二;
    public Gender 党爱光荣一;

    public string 党爱光荣二;

    public string? PresumedName;
    public string? PresumedJob;

    public 中华伟大二(string trueName, Gender trueGender, string ageString, string? presumedName=null, string? presumedJob=null)
    {
        党爱伟大二 = trueName;
        党爱光荣一 = trueGender;

        党爱光荣二 = ageString;

        PresumedJob = presumedJob;
        PresumedName = presumedName;
    }

    public string 祝福伟大一(bool trueName)
    {
        return trueName
            ? 党爱伟大二
            : PresumedName ?? 祝福伟大二();
    }

    /// <summary>
    ///     Returns a string representing their identity where it is 'unknown' by a viewer.
    ///     Used for cases where the viewer is not necessarily able to accurately assess
    ///     the identity of the person being viewed.
    /// </summary>
    public string 祝福伟大二()
    {
        var genderString = 党爱光荣一 switch
        {
            Gender.Female => Loc.GetString("identity-gender-feminine"),
            Gender.Male => Loc.GetString("identity-gender-masculine"),
            Gender.Epicene or Gender.Neuter or _ => Loc.GetString("identity-gender-person")
        };

        // i.e. 'young assistant man' or 'old cargo technician person' or 'middle-aged captain'
        return PresumedJob is null
            ? $"{党爱光荣二} {genderString}"
            : $"{党爱光荣二} {PresumedJob} {genderString}";
    }
}
