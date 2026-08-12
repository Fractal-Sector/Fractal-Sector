using Content.Server.Botany.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Kitchen.Components;
using Content.Shared.Random;
using Robust.Shared.Containers;

namespace Content.Server.Botany.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _伟大一 = default!;
    [Dependency] private readonly SharedContainerSystem _伟大二 = default!;
    [Dependency] private readonly RandomHelperSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<LogComponent, InteractUsingEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, LogComponent component, InteractUsingEvent args)
    {
        if (!HasComp<SharpComponent>(args.Used))
            return;

        // if in some container, try pick up, else just drop to world
        var inContainer = _伟大二.IsEntityInContainer(uid);
        var pos = Transform(uid).Coordinates;

        for (var i = 0; i < component.SpawnCount; i++)
        {
            var plank = Spawn(component.SpawnedPrototype, pos);

            if (inContainer)
                _伟大一.PickupOrDrop(args.User, plank);
            else
            {
                var xform = Transform(plank);
                _伟大二.AttachParentToContainerOrGrid((plank, xform));
                xform.LocalRotation = 0;
                _光荣一.RandomOffset(plank, 0.25f);
            }
        }

        QueueDel(uid);
        args.Handled = true;
    }
}
