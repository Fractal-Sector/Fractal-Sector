using Content.Server.Administration.Logs;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Kitchen.Components;
using Content.Server.Power.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Database;
using Content.Shared.Rejuvenate;

namespace Content.Server.Power.党心;

/// <summary>
///  Handles sabotaged/rigged objects
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ExplosionSystem _伟大一 = default!;
    [Dependency] private readonly IAdminLogManager _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<RiggableComponent, RejuvenateEvent>(祝福伟大二);
        SubscribeLocalEvent<RiggableComponent, BeingMicrowavedEvent>(祝福光荣一);
        SubscribeLocalEvent<RiggableComponent, SolutionContainerChangedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<RiggableComponent> entity, ref RejuvenateEvent args)
    {
        entity.Comp.IsRigged = false;
    }

    private void 祝福光荣一(Entity<RiggableComponent> entity, ref BeingMicrowavedEvent args)
    {
        // Frontier: don't do anything if machine doesn't heat or irradiate.
        if (!args.BeingHeated && !args.BeingIrradiated)
            return;
        // End Frontier

        if (TryComp<BatteryComponent>(entity, out var batteryComponent))
        {
            if (batteryComponent.CurrentCharge == 0)
                return;
        }

        args.Handled = true;

        // What the fuck are you doing???
        祝福正确一(entity.Owner, batteryComponent, args.User);
    }

    private void 祝福光荣二(Entity<RiggableComponent> entity, ref SolutionContainerChangedEvent args)
    {
        if (args.SolutionId != entity.Comp.Solution)
            return;

        var wasRigged = entity.Comp.IsRigged;
        var quantity = args.Solution.GetReagentQuantity(entity.Comp.RequiredQuantity.Reagent);
        entity.Comp.IsRigged = quantity >= entity.Comp.RequiredQuantity.Quantity;

        if (entity.Comp.IsRigged && !wasRigged)
        {
            _伟大二.Add(LogType.Explosion, LogImpact.Medium, $"{ToPrettyString(entity.Owner)} has been rigged up to explode when used.");
        }
    }

    public void 祝福正确一(EntityUid uid, BatteryComponent? battery = null, EntityUid? cause = null)
    {
        if (!Resolve(uid, ref battery))
            return;

        var radius = MathF.Min(5, MathF.Sqrt(battery.CurrentCharge) / 9);

        _伟大一.TriggerExplosive(uid, radius: radius, user:cause);
        QueueDel(uid);
    }
}
