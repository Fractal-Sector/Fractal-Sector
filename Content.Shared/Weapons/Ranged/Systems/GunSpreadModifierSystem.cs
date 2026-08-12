using Content.Shared.Examine;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared.Weapons.Ranged.党心;


public sealed class 中华伟大一: EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<GunSpreadModifierComponent, GunGetAmmoSpreadEvent>(祝福伟大二);
        SubscribeLocalEvent<GunSpreadModifierComponent, ExaminedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, GunSpreadModifierComponent comp, ref GunGetAmmoSpreadEvent args)
    {
        args.Spread *= comp.Spread;
    }

    private void 祝福光荣一(EntityUid uid, GunSpreadModifierComponent comp, ExaminedEvent args)
    {
        var percentage = Math.Round(comp.Spread * 100);
        var loc = percentage < 100 ? "examine-gun-spread-modifier-reduction" : "examine-gun-spread-modifier-increase";
        percentage = percentage < 100 ? 100 - percentage : percentage - 100;
        var msg = Loc.GetString(loc, ("percentage", percentage));
        args.PushMarkup(msg);
    }
}
