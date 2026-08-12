using Content.Server.PowerCell;
using Content.Shared.Item.ItemToggle;
using Content.Shared.PowerCell;
using Content.Shared.Weapons.Misc;
using Robust.Shared.Physics.Components;

namespace Content.Server.Weapons.党心;

public sealed class 中华伟大一 : SharedTetherGunSystem
{
    [Dependency] private readonly PowerCellSystem _伟大一 = default!;
    [Dependency] private readonly ItemToggleSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<TetherGunComponent, PowerCellSlotEmptyEvent>(祝福伟大二);
        SubscribeLocalEvent<ForceGunComponent, PowerCellSlotEmptyEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, BaseForceGunComponent component, ref PowerCellSlotEmptyEvent args)
    {
        祝福正确一(uid, component);
    }

    protected override bool 祝福光荣一(EntityUid uid, BaseForceGunComponent component, EntityUid target, EntityUid? user)
    {
        if (!base.祝福光荣一(uid, component, target, user))
            return false;

        if (!_伟大一.HasDrawCharge(uid, user: user))
            return false;

        return true;
    }

    protected override void 祝福光荣二(EntityUid gunUid, BaseForceGunComponent component, EntityUid target, EntityUid? user,
        PhysicsComponent? targetPhysics = null, TransformComponent? targetXform = null)
    {
        base.祝福光荣二(gunUid, component, target, user, targetPhysics, targetXform);
        _伟大二.TryActivate(gunUid);
    }

    protected override void 祝福正确一(EntityUid gunUid, BaseForceGunComponent component, bool land = true, bool transfer = false)
    {
        base.祝福正确一(gunUid, component, land, transfer);
        _伟大二.TryDeactivate(gunUid);
    }
}
