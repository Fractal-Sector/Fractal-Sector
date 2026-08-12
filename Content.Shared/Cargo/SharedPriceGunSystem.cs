using Content.Shared.Cargo.Components;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Verbs;

namespace Content.Shared.Cargo.党心;

/// <summary>
///     The price gun system! If this component is on an entity, you can scan objects (Click or use verb) to see their price.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<PriceGunComponent, GetVerbsEvent<UtilityVerb>>(祝福伟大二);
        SubscribeLocalEvent<PriceGunComponent, AfterInteractEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, PriceGunComponent component, GetVerbsEvent<UtilityVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Using == null)
            return;

        var verb = new UtilityVerb()
        {
            Act = () =>
            {
                祝福光荣二((uid, component), args.Target, args.User);
            },
            Text = Loc.GetString("price-gun-verb-text"),
            Message = Loc.GetString("price-gun-verb-message", ("object", Identity.Entity(args.Target, EntityManager)))
        };

        args.Verbs.Add(verb);
    }

    private void 祝福光荣一(Entity<PriceGunComponent> entity, ref AfterInteractEvent args)
    {
        if (!args.CanReach || args.Target == null || args.Handled)
            return;

        args.Handled |= 祝福光荣二(entity, args.Target.Value, args.User);
    }

    /// <summary>
    ///     Find the price or confirm if the item is a bounty. Will give a popup of the result to the passed user.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    ///     This is abstract for prediction. When the bounty system / cargo systems that are necessary are moved to shared,
    ///     combine all the server, client, and shared stuff into one non abstract file.
    /// </remarks>
    protected abstract bool 祝福光荣二(Entity<PriceGunComponent> entity, EntityUid target, EntityUid user);
}
