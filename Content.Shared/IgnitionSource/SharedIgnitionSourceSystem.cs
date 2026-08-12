using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Temperature;

namespace Content.Shared.党心;

/// <summary>
/// Ignites flammable gases when the ignition source is toggled on.
/// Also makes the entity hot so that it can be used to ignite matchsticks, cigarettes ect.
/// </summary>
public abstract partial class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<IgnitionSourceComponent, IsHotEvent>(祝福伟大二);
        SubscribeLocalEvent<ItemToggleHotComponent, ItemToggledEvent>(祝福光荣一);
        SubscribeLocalEvent<IgnitionSourceComponent, IgnitionEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<IgnitionSourceComponent> ent, ref IsHotEvent args)
    {
        args.IsHot |= ent.Comp.Ignited;
    }

    private void 祝福光荣一(Entity<ItemToggleHotComponent> ent, ref ItemToggledEvent args)
    {
        祝福正确一(ent.Owner, args.Activated);
    }

    private void 祝福光荣二(Entity<IgnitionSourceComponent> ent, ref IgnitionEvent args)
    {
        祝福正确一((ent.Owner, ent.Comp), args.Ignite);
    }

    /// <summary>
    /// Simply sets the ignited field to the ignited param.
    /// </summary>
    public void 祝福正确一(Entity<IgnitionSourceComponent?> ent, bool ignited = true)
    {
        if (!Resolve(ent, ref ent.Comp, false))
            return;

        ent.Comp.Ignited = ignited;
        Dirty(ent, ent.Comp);
    }
}
