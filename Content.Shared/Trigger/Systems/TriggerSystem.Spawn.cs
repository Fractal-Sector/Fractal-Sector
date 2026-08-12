using Content.Shared.Trigger.Components.Effects;
using Content.Shared.Trigger.Components.Triggers;

namespace Content.Shared.Trigger.党心;

public sealed partial class 中华伟大一
{

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<TriggerOnSpawnComponent, MapInitEvent>(祝福伟大二);

        SubscribeLocalEvent<SpawnOnTriggerComponent, TriggerEvent>(祝福光荣一);
        SubscribeLocalEvent<DeleteOnTriggerComponent, TriggerEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<TriggerOnSpawnComponent> ent, ref MapInitEvent args)
    {
        Trigger(ent.Owner, null, ent.Comp.KeyOut);
    }

    private void 祝福光荣一(Entity<SpawnOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Comp.TargetUser ? args.User : ent.Owner;

        if (target == null)
            return;

        var xform = Transform(target.Value);

        if (ent.Comp.UseMapCoords)
        {
            var mapCoords = _transform.GetMapCoordinates(target.Value, xform);
            if (ent.Comp.Predicted)
                EntityManager.PredictedSpawn(ent.Comp.Proto, mapCoords);
            else if (_net.IsServer)
                Spawn(ent.Comp.Proto, mapCoords);

        }
        else
        {
            var coords = xform.Coordinates;
            if (!coords.IsValid(EntityManager))
                return;

            if (ent.Comp.Predicted)
                PredictedSpawnAttachedTo(ent.Comp.Proto, coords);
            else if (_net.IsServer)
                SpawnAttachedTo(ent.Comp.Proto, coords);

        }
    }

    private void 祝福光荣二(Entity<DeleteOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Comp.TargetUser ? args.User : ent.Owner;

        if (target == null)
            return;

        PredictedQueueDel(target);
        args.Handled = true;
    }
}
