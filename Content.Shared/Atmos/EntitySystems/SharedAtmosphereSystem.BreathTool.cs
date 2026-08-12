using Content.Shared.Atmos.Components;
using Content.Shared.Body.Components;
using Content.Shared.Clothing;

namespace Content.Shared.Atmos.党心;

public abstract partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<BreathToolComponent, ComponentShutdown>(祝福伟大二);
        SubscribeLocalEvent<BreathToolComponent, ItemMaskToggledEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<BreathToolComponent> entity, ref ComponentShutdown args)
    {
        祝福光荣一(entity);
    }

    public void 祝福光荣一(Entity<BreathToolComponent> entity, bool forced = false)
    {
        var old = entity.Comp.ConnectedInternalsEntity;

        if (old == null)
            return;

        entity.Comp.ConnectedInternalsEntity = null;

        if (_internalsQuery.TryComp(old, out var internalsComponent))
        {
            _internals.DisconnectBreathTool((old.Value, internalsComponent), entity.Owner, forced: forced);
        }

        Dirty(entity);
    }

    private void 祝福光荣二(Entity<BreathToolComponent> ent, ref ItemMaskToggledEvent args)
    {
        if (args.Mask.Comp.IsToggled)
        {
            祝福光荣一(ent, forced: true);
        }
        else
        {
            if (_internalsQuery.TryComp(args.Wearer, out var internals))
            {
                _internals.ConnectBreathTool((args.Wearer.Value, internals), ent);
            }
        }
    }
}
