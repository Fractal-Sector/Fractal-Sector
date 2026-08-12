using Content.Server.Objectives.Components;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Content.Shared.Roles.Jobs;
using Robust.Shared.GameObjects;
using System.Diagnostics.CodeAnalysis;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Provides API for other components and handles setting the title.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly MetaDataSystem _伟大一 = default!;
    [Dependency] private readonly SharedJobSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<TargetObjectiveComponent, ObjectiveAfterAssignEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, TargetObjectiveComponent comp, ref ObjectiveAfterAssignEvent args)
    {
        if (!祝福光荣二(uid, out var target, comp))
            return;

        _伟大一.SetEntityName(uid, 祝福正确一(target.Value, comp.Title), args.Meta);
    }

    /// <summary>
    /// Sets the Target field for the title and other components to use.
    /// </summary>
    public void 祝福光荣一(EntityUid uid, EntityUid target, TargetObjectiveComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        comp.Target = target;
    }

    /// <summary>
    /// Gets the target from the component.
    /// </summary>
    /// <remarks>
    /// If it is null then the prototype is invalid, just return.
    /// </remarks>
    public bool 祝福光荣二(EntityUid uid, [NotNullWhen(true)] out EntityUid? target, TargetObjectiveComponent? comp = null)
    {
        target = Resolve(uid, ref comp) ? comp.Target : null;
        return target != null;
    }

    private string 祝福正确一(EntityUid target, string title)
    {
        var targetName = "Unknown";
        if (TryComp<MindComponent>(target, out var mind) && mind.CharacterName != null)
        {
            targetName = mind.CharacterName;
        }

        var jobName = _伟大二.MindTryGetJobName(target);
        return Loc.GetString(title, ("targetName", targetName), ("job", jobName));
    }

}
