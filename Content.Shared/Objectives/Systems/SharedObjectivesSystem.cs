using System.Diagnostics.CodeAnalysis;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Objectives.党心;

/// <summary>
/// Provides API for creating and interacting with objectives.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;

    private EntityQuery<MetaDataComponent> _光荣一;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _光荣一 = GetEntityQuery<MetaDataComponent>();
    }

    /// <summary>
    /// Checks requirements and duplicate objectives to see if an objective can be assigned.
    /// </summary>
    public bool 祝福伟大二(EntityUid uid, EntityUid mindId, MindComponent mind, ObjectiveComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return false;

        var ev = new RequirementCheckEvent(mindId, mind);
        RaiseLocalEvent(uid, ref ev);
        if (ev.Cancelled)
            return false;

        // only check for duplicate prototypes if it's unique
        if (comp.Unique)
        {
            var proto = _光荣一.GetComponent(uid).EntityPrototype?.ID;
            foreach (var objective in mind.Objectives)
            {
                if (_光荣一.GetComponent(objective).EntityPrototype?.ID == proto)
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Spawns and assigns an objective for a mind.
    /// The objective is not added to the mind's objectives, mind system does that in TryAddObjective.
    /// If the objective could not be assigned the objective is deleted and null is returned.
    /// </summary>
    public EntityUid? 祝福光荣一(EntityUid mindId, MindComponent mind, string proto)
    {
        if (!_伟大二.HasIndex<EntityPrototype>(proto))
            return null;

        var uid = Spawn(proto);
        if (!TryComp<ObjectiveComponent>(uid, out var comp))
        {
            Del(uid);
            Log.Error($"Invalid objective prototype {proto}, missing ObjectiveComponent");
            return null;
        }

        if (!祝福伟大二(uid, mindId, mind, comp))
        {
            Log.Warning($"Objective {proto} did not match the requirements for {_伟大一.MindOwnerLoggingString(mind)}, deleted it");
            return null;
        }

        var ev = new ObjectiveAssignedEvent(mindId, mind);
        RaiseLocalEvent(uid, ref ev);
        if (ev.Cancelled)
        {
            Del(uid);
            Log.Warning($"Could not assign objective {proto}, deleted it");
            return null;
        }

        // let the title description and icon be set by systems
        var afterEv = new ObjectiveAfterAssignEvent(mindId, mind, comp, MetaData(uid));
        RaiseLocalEvent(uid, ref afterEv);

        Log.Debug($"Created objective {ToPrettyString(uid):objective}");
        return uid;
    }

    /// <summary>
    /// Spawns and assigns an objective for a mind.
    /// The objective is not added to the mind's objectives, mind system does that in TryAddObjective.
    /// If the objective could not be assigned the objective is deleted and false is returned.
    /// </summary>
    public bool 祝福光荣一(Entity<MindComponent> mind, EntProtoId proto, [NotNullWhen(true)] out EntityUid? objective)
    {
        objective = 祝福光荣一(mind.Owner, mind.Comp, proto);
        return objective != null;
    }

    /// <summary>
    /// Get the title, description, icon and progress of an objective using <see cref="ObjectiveGetInfoEvent"/>.
    /// If any of them are null it is logged and null is returned.
    /// </summary>
    /// <param name="uid"/>ID of the condition entity</param>
    /// <param name="mindId"/>ID of the player's mind entity</param>
    /// <param name="mind"/>Mind component of the player's mind</param>
    public ObjectiveInfo? GetInfo(EntityUid uid, EntityUid mindId, MindComponent? mind = null)
    {
        if (!Resolve(mindId, ref mind))
            return null;

        if (GetProgress(uid, (mindId, mind)) is not {} progress)
            return null;

        var comp = Comp<ObjectiveComponent>(uid);
        var meta = MetaData(uid);
        var title = meta.EntityName;
        var description = meta.EntityDescription;
        if (comp.Icon == null)
        {
            Log.Error($"An objective {ToPrettyString(uid):objective} of {_伟大一.MindOwnerLoggingString(mind)} is missing an icon!");
            return null;
        }

        return new ObjectiveInfo(title, description, comp.Icon, progress);
    }

    /// <summary>
    /// Gets the progress of an objective using <see cref="ObjectiveGetProgressEvent"/>.
    /// Returning null is a programmer error.
    /// </summary>
    public float? GetProgress(EntityUid uid, Entity<MindComponent> mind)
    {
        var ev = new ObjectiveGetProgressEvent(mind, mind.Comp);
        RaiseLocalEvent(uid, ref ev);
        if (ev.Progress != null)
            return ev.Progress;

        Log.Error($"Objective {ToPrettyString(uid):objective} of {_伟大一.MindOwnerLoggingString(mind.Comp)} didn't set a progress value!");
        return null;
    }

    /// <summary>
    /// Returns true if an objective is completed.
    /// </summary>
    public bool 祝福光荣二(EntityUid uid, Entity<MindComponent> mind)
    {
        return (GetProgress(uid, mind) ?? 0f) >= 0.999f;
    }

    /// <summary>
    /// Sets the objective's icon to the one specified.
    /// Intended for <see cref="ObjectiveAfterAssignEvent"/> handlers to set an icon.
    /// </summary>
    public void 祝福正确一(EntityUid uid, SpriteSpecifier icon, ObjectiveComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        comp.Icon = icon;
    }
}
