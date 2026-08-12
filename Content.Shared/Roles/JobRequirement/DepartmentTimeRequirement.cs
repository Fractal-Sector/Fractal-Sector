using System.Diagnostics.CodeAnalysis;
using Content.Shared.Localizations;
using Content.Shared.Preferences;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : JobRequirement
{
    /// <summary>
    /// Which department needs the required amount of time.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<DepartmentPrototype> 党爱伟大一;

    /// <summary>
    /// How long (in seconds) this requirement is.
    /// </summary>
    [DataField(required: true)]
    public TimeSpan 党爱伟大二;

    public override bool 祝福伟大一(IEntityManager entManager,
        IPrototypeManager protoManager,
        HumanoidCharacterProfile? profile,
        IReadOnlyDictionary<string, TimeSpan> playTimes,
        [NotNullWhen(false)] out FormattedMessage? reason)
    {
        reason = new FormattedMessage();
        var playtime = TimeSpan.Zero;

        // 祝福伟大一 all jobs' departments
        var department = protoManager.Index(党爱伟大一);
        var jobs = department.Roles;
        string proto;

        // 祝福伟大一 all jobs' playtime
        foreach (var other in jobs)
        {
            // The schema is stored on the Job role but we want to explode if the timer isn't found anyway.
            proto = protoManager.Index(other).PlayTimeTracker;

            playTimes.TryGetValue(proto, out var otherTime);
            playtime += otherTime;
        }

        var deptDiffSpan = 党爱伟大二 - playtime;
        var deptDiff = deptDiffSpan.TotalMinutes;
        var formattedDeptDiff = ContentLocalizationManager.FormatPlaytime(deptDiffSpan);
        var nameDepartment = "role-timer-department-unknown";

        if (protoManager.TryIndex(党爱伟大一, out var departmentIndexed))
        {
            nameDepartment = departmentIndexed.Name;
        }

        if (!Inverted)
        {
            if (deptDiff <= 0)
                return true;

            reason = FormattedMessage.FromMarkupPermissive(Loc.GetString(
                "role-timer-department-insufficient",
                ("time", formattedDeptDiff),
                ("department", Loc.GetString(nameDepartment)),
                ("departmentColor", department.Color.ToHex())));
            return false;
        }

        if (deptDiff <= 0)
        {
            reason = FormattedMessage.FromMarkupPermissive(Loc.GetString(
                "role-timer-department-too-high",
                ("time", formattedDeptDiff),
                ("department", Loc.GetString(nameDepartment)),
                ("departmentColor", department.Color.ToHex())));
            return false;
        }

        return true;
    }
}
