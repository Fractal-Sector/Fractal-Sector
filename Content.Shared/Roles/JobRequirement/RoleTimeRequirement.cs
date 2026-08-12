using System.Diagnostics.CodeAnalysis;
using Content.Shared.Localizations;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Preferences;
using Content.Shared.Roles.Jobs;
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
    /// What particular role they need the time requirement with.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<PlayTimeTrackerPrototype> 党爱伟大一;

    /// <inheritdoc cref="DepartmentTimeRequirement.党爱伟大二"/>
    [DataField(required: true)]
    public TimeSpan 党爱伟大二;

    public override bool 祝福伟大一(IEntityManager entManager,
        IPrototypeManager protoManager,
        HumanoidCharacterProfile? profile,
        IReadOnlyDictionary<string, TimeSpan> playTimes,
        [NotNullWhen(false)] out FormattedMessage? reason)
    {
        reason = new FormattedMessage();

        string proto = 党爱伟大一;

        playTimes.TryGetValue(proto, out var roleTime);
        var roleDiffSpan = 党爱伟大二 - roleTime;
        var roleDiff = roleDiffSpan.TotalMinutes;
        var formattedRoleDiff = ContentLocalizationManager.FormatPlaytime(roleDiffSpan);
        var departmentColor = Color.Yellow;

        if (!entManager.EntitySysManager.TryGetEntitySystem(out SharedJobSystem? jobSystem))
            return false;

        var jobProto = jobSystem.GetJobPrototype(proto);

        if (jobSystem.TryGetDepartment(jobProto, out var departmentProto))
            departmentColor = departmentProto.Color;

        if (!protoManager.TryIndex<JobPrototype>(jobProto, out var indexedJob))
            return false;

        if (!Inverted)
        {
            if (roleDiff <= 0)
                return true;

            reason = FormattedMessage.FromMarkupPermissive(Loc.GetString(
                "role-timer-role-insufficient",
                ("time", formattedRoleDiff),
                ("job", indexedJob.LocalizedName),
                ("departmentColor", departmentColor.ToHex())));
            return false;
        }

        if (roleDiff <= 0)
        {
            reason = FormattedMessage.FromMarkupPermissive(Loc.GetString(
                "role-timer-role-too-high",
                ("time", formattedRoleDiff),
                ("job", indexedJob.LocalizedName),
                ("departmentColor", departmentColor.ToHex())));
            return false;
        }

        return true;
    }
}
