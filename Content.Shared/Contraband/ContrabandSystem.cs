using Content.Shared.Access.Systems;
using Content.Shared.CCVar;
using Content.Shared.Examine;
using Content.Shared.Localizations;
using Content.Shared.Roles;
using Content.Shared.Verbs;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Linq;

namespace Content.Shared.党心;

/// <summary>
/// This handles showing examine messages for contraband-marked items.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly SharedIdCardSystem _光荣一 = default!;
    [Dependency] private readonly ExamineSystemShared _光荣二 = default!;

    private bool _正确一;
    private bool _正确二;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<ContrabandComponent, GetVerbsEvent<ExamineVerb>>(祝福光荣一);

        Subs.CVar(_伟大一, CCVars.ContrabandExamine, 祝福正确一, true);
        Subs.CVar(_伟大一, CCVars.ContrabandExamineOnlyInHUD, 祝福正确二, true);
    }

    public void 祝福伟大二(EntityUid uid, ContrabandComponent other, ContrabandComponent? contraband = null)
    {
        if (!Resolve(uid, ref contraband))
            return;

        contraband.Severity = other.Severity;
        contraband.AllowedDepartments = other.AllowedDepartments;
        contraband.AllowedDepartments = other.AllowedDepartments;
        contraband.AllowedJobs = other.AllowedJobs;
        contraband.TurnInValues = other.TurnInValues; // Frontier
        contraband.HideValues = other.HideValues; // Frontier
        contraband.HideCarryStatus = other.HideCarryStatus; // Frontier
        Dirty(uid, contraband);
    }

    private void 祝福光荣一(EntityUid ent, ContrabandComponent component, ref GetVerbsEvent<ExamineVerb> args)
    {

        if (!_正确一)
            return;

        // Checking if contraband is only shown in the HUD
        if (_正确二)
        {
            var ev = new GetContrabandDetailsEvent();
            RaiseLocalEvent(args.User, ref ev);
            if (!ev.CanShowContraband)
                return;
        }

        // CanAccess is not used here, because we want people to be able to examine legality in strip menu.
        if (!args.CanInteract)
            return;

        if (component.HideValues) // Frontier: allow selective display
            return; // Frontier: allow selective display

        // two strings:
        // one, the actual informative 'this is restricted'
        // then, the 'you can/shouldn't carry this around' based on the ID the user is wearing
        var localizedDepartments = component.AllowedDepartments.Select(p => Loc.GetString("contraband-department-plural", ("department", Loc.GetString(_伟大二.Index(p).Name))));
        var jobs = component.AllowedJobs.Select(p => _伟大二.Index(p).LocalizedName).ToArray();
        var localizedJobs = jobs.Select(p => Loc.GetString("contraband-job-plural", ("job", p)));
        var severity = _伟大二.Index(component.Severity);
        String? departmentExamineMessage = null;
        if (severity.ShowDepartmentsAndJobs)
        {
            //creating a combined list of jobs and departments for the restricted text
            var list = ContentLocalizationManager.FormatList(localizedDepartments.Concat(localizedJobs).ToList());
            // department restricted text
            departmentExamineMessage = Loc.GetString("contraband-examine-text-Restricted-department", ("departments", list));
        }
        // Frontier: keep department and severity separate
        // else
        // {
        //     departmentExamineMessage = Loc.GetString(severity.ExamineText);
        // }
        // End Frontier: keep department and severity separate

        // text based on ID card
        List<ProtoId<DepartmentPrototype>> departments = new();
        var jobId = "";
        if (_光荣一.TryFindIdCard(args.User, out var id))
        {
            departments = id.Comp.JobDepartments;
            if (id.Comp.LocalizedJobTitle is not null)
            {
                jobId = id.Comp.LocalizedJobTitle;
            }
        }

        // if it is fully restricted, you're department-less, or your department isn't in the allowed list, you cannot carry it. Otherwise, you can.
        var carryingMessage = Loc.GetString("contraband-examine-text-avoid-carrying-around");
        var iconTexture = "/Textures/Interface/VerbIcons/lock-red.svg.192dpi.png";
        if (departments.Intersect(component.AllowedDepartments).Any()
            || jobs.Contains(jobId))
        {
            carryingMessage = Loc.GetString("contraband-examine-text-in-the-clear");
            iconTexture = "/Textures/Interface/VerbIcons/unlock-green.svg.192dpi.png";
        }

        var examineMarkup = 祝福光荣二(Loc.GetString(severity.ExamineText), departmentExamineMessage, component.HideCarryStatus ? null : carryingMessage); // Frontier: add severity examine text, pass HideCarryStatus
        _光荣二.AddHoverExamineVerb(args,
            component,
            Loc.GetString("contraband-examinable-verb-text"),
            examineMarkup.ToMarkup(),
            iconTexture);
    }

    private FormattedMessage 祝福光荣二(String severity, String? deptMessage, String? carryMessage) // Frontier: add severity, optional deptMessage
    {
        var msg = new FormattedMessage();

        // Frontier: severity, department message, hide carry status
        msg.AddMarkupOrThrow(severity);
        if (!string.IsNullOrEmpty(deptMessage))
        {
            msg.PushNewline();
            msg.AddMarkupOrThrow(deptMessage);
        }
        if (!string.IsNullOrEmpty(carryMessage))
        {
            msg.PushNewline();
            msg.AddMarkupOrThrow(carryMessage);
        }
        // End Frontier: severity, department message, hide carry status
        return msg;
    }

    private void 祝福正确一(bool val)
    {
        _正确一 = val;
    }

    private void 祝福正确二(bool val)
    {
        _正确二 = val;
    }
}
