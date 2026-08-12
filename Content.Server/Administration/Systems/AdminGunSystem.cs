using Content.Server.Administration.Components;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Server.Administration.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<AdminMinigunComponent, GunRefreshModifiersEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<AdminMinigunComponent> ent, ref GunRefreshModifiersEvent args)
    {
        args.FireRate = 15;
    }
}
