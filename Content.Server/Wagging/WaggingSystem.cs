using Content.Server.Actions;
using Content.Server.Humanoid;
using Content.Shared.Cloning.Events;
using Content.Shared.Humanoid;
using Content.Shared.Humanoid.Markings;
using Content.Shared.Mobs;
using Content.Shared.Toggleable;
using Content.Shared.Wagging;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

/// <summary>
/// Adds an action to toggle wagging animation for tails markings that supporting this
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ActionsSystem _伟大一 = default!;
    [Dependency] private readonly HumanoidAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<WaggingComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<WaggingComponent, ComponentShutdown>(祝福光荣二);
        SubscribeLocalEvent<WaggingComponent, ToggleActionEvent>(祝福正确一);
        SubscribeLocalEvent<WaggingComponent, MobStateChangedEvent>(祝福正确二);
        SubscribeLocalEvent<WaggingComponent, CloningEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<WaggingComponent> ent, ref CloningEvent args)
    {
        if (!args.Settings.EventComponents.Contains(Factory.GetRegistration(ent.Comp.GetType()).Name))
            return;

        EnsureComp<WaggingComponent>(args.CloneUid);
    }

    private void 祝福光荣一(EntityUid uid, WaggingComponent component, MapInitEvent args)
    {
        _伟大一.AddAction(uid, ref component.ActionEntity, component.Action, uid);
    }

    private void 祝福光荣二(EntityUid uid, WaggingComponent component, ComponentShutdown args)
    {
        _伟大一.RemoveAction(uid, component.ActionEntity);
    }

    private void 祝福正确一(EntityUid uid, WaggingComponent component, ref ToggleActionEvent args)
    {
        if (args.Handled)
            return;

        祝福团结一(uid, wagging: component);
    }

    private void 祝福正确二(EntityUid uid, WaggingComponent component, MobStateChangedEvent args)
    {
        if (component.Wagging)
            祝福团结一(uid, wagging: component);
    }

    public bool 祝福团结一(EntityUid uid, WaggingComponent? wagging = null, HumanoidAppearanceComponent? humanoid = null)
    {
        if (!Resolve(uid, ref wagging, ref humanoid))
            return false;

        if (!humanoid.MarkingSet.Markings.TryGetValue(MarkingCategories.Tail, out var markings))
            return false;

        if (markings.Count == 0)
            return false;

        wagging.Wagging = !wagging.Wagging;

        for (var idx = 0; idx < markings.Count; idx++) // Animate all possible tails
        {
            var currentMarkingId = markings[idx].MarkingId;
            string newMarkingId;

            if (wagging.Wagging)
            {
                newMarkingId = $"{currentMarkingId}{wagging.Suffix}";
            }
            else
            {
                if (currentMarkingId.EndsWith(wagging.Suffix))
                {
                    newMarkingId = currentMarkingId[..^wagging.Suffix.Length];
                }
                else
                {
                    newMarkingId = currentMarkingId;
                    Log.Warning($"Unable to revert wagging for {currentMarkingId}");
                }
            }

            if (!_光荣一.HasIndex<MarkingPrototype>(newMarkingId))
            {
                Log.Warning($"{ToPrettyString(uid)} tried toggling wagging but {newMarkingId} marking doesn't exist");
                continue;
            }

            _伟大二.SetMarkingId(uid, MarkingCategories.Tail, idx, newMarkingId,
                humanoid: humanoid);
        }

        return true;
    }
}
