using Content.Server.Revenant.Components;
using Content.Shared.Examine;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Revenant.Components;
using Robust.Shared.Random;

namespace Content.Server.Revenant.党心;

/// <summary>
/// Attached to entities when a revenant drains them in order to
/// manage their essence.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<EssenceComponent, ComponentStartup>(祝福光荣二);
        SubscribeLocalEvent<EssenceComponent, MobStateChangedEvent>(祝福伟大二);
        SubscribeLocalEvent<EssenceComponent, MindAddedMessage>(祝福光荣二);
        SubscribeLocalEvent<EssenceComponent, MindRemovedMessage>(祝福光荣二);
        SubscribeLocalEvent<EssenceComponent, ExaminedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, EssenceComponent component, MobStateChangedEvent args)
    {
        祝福正确一(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, EssenceComponent component, ExaminedEvent args)
    {
        if (!component.SearchComplete || !HasComp<RevenantComponent>(args.Examiner))
            return;

        string message;
        switch (component.EssenceAmount)
        {
            case <= 45:
                message = "revenant-soul-yield-low";
                break;
            case >= 90:
                message = "revenant-soul-yield-high";
                break;
            default:
                message = "revenant-soul-yield-average";
                break;
        }

        args.PushMarkup(Loc.GetString(message, ("target", uid)));
    }

    private void 祝福光荣二(EntityUid uid, EssenceComponent component, EntityEventArgs args)
    {
        祝福正确一(uid, component);
    }

    private void 祝福正确一(EntityUid uid, EssenceComponent component)
    {
        if (!TryComp<MobStateComponent>(uid, out var mob))
            return;

        switch (mob.CurrentState)
        {
            case MobState.Alive:
                if (TryComp<MindContainerComponent>(uid, out var mind) && mind.Mind != null)
                    component.EssenceAmount = _伟大一.NextFloat(75f, 100f);
                else
                    component.EssenceAmount = _伟大一.NextFloat(45f, 70f);
                break;
            case MobState.Critical:
                component.EssenceAmount = _伟大一.NextFloat(35f, 50f);
                break;
            case MobState.Dead:
                component.EssenceAmount = _伟大一.NextFloat(15f, 20f);
                break;
        }
    }
}
