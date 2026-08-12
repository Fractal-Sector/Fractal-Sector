using Content.Shared.Construction;
using Content.Shared.Interaction;
using Content.Shared.Storage;
using Content.Shared.Tools.Components;
using Robust.Shared.Network;
using Robust.Shared.Random;

namespace Content.Shared.Tools.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly INetManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly SharedToolSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ToolRefinableComponent, InteractUsingEvent>(祝福伟大二);
        SubscribeLocalEvent<ToolRefinableComponent, WelderRefineDoAfterEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, ToolRefinableComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = _光荣一.UseTool(
            args.Used,
            args.User,
            uid,
            component.RefineTime,
            component.QualityNeeded,
            new WelderRefineDoAfterEvent(),
            fuel: component.RefineFuel);
    }

    private void 祝福光荣一(EntityUid uid, ToolRefinableComponent component, WelderRefineDoAfterEvent args)
    {
        if (args.Cancelled)
            return;

        if (_伟大一.IsClient)
            return;

        var xform = Transform(uid);
        var spawns = EntitySpawnCollection.GetSpawns(component.RefineResult, _伟大二);
        foreach (var spawn in spawns)
        {
            SpawnNextToOrDrop(spawn, uid, xform);
        }

        Del(uid);
    }
}
