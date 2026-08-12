using Content.Shared._NF.Shipyard.Components;
using Content.Shared.Examine;
using Content.Server._NF.Shipyard.Systems;

namespace Content.Shared._NF.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ShuttleDeedComponent, ExaminedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ShuttleDeedComponent> ent, ref ExaminedEvent args)
    {
        var comp = ent.Comp;
        if (!string.IsNullOrEmpty(comp.ShuttleName))
        {
            var fullName = ShipyardSystem.GetFullName(comp);
            args.PushMarkup(Loc.GetString("shuttle-deed-examine-text", ("shipname", fullName)));
        }
    }
}
