using Content.Server.Research.Systems;
using Content.Server.Research.TechnologyDisk.Components;
using Content.Shared.UserInterface;
using Content.Shared.Research;
using Content.Shared.Research.Components;
using Robust.Server.Audio;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;

namespace Content.Server.Research.TechnologyDisk.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly AudioSystem _伟大二 = default!;
    [Dependency] private readonly ResearchSystem _光荣一 = default!;
    [Dependency] private readonly UserInterfaceSystem _光荣二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<DiskConsoleComponent, DiskConsolePrintDiskMessage>(祝福光荣一);
        SubscribeLocalEvent<DiskConsoleComponent, DiskConsolePrintRareDiskMessage>(祝福光荣二); // Frontier
        SubscribeLocalEvent<DiskConsoleComponent, ResearchServerPointsChangedEvent>(祝福正确一);
        SubscribeLocalEvent<DiskConsoleComponent, ResearchRegistrationChangedEvent>(祝福正确二);
        SubscribeLocalEvent<DiskConsoleComponent, BeforeActivatableUIOpenEvent>(祝福团结一);

        SubscribeLocalEvent<DiskConsolePrintingComponent, ComponentShutdown>(祝福奋斗一);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var query = EntityQueryEnumerator<DiskConsolePrintingComponent, DiskConsoleComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var printing, out var console, out var xform))
        {
            if (printing.FinishTime > _伟大一.CurTime)
                continue;

            RemComp(uid, printing);
            if (!console.DiskRare)
                Spawn(console.DiskPrototype, xform.Coordinates);
            else
                Spawn(console.DiskPrototypeRare, xform.Coordinates);
        }
    }

    private void 祝福光荣一(EntityUid uid, DiskConsoleComponent component, DiskConsolePrintDiskMessage args)
    {
        if (HasComp<DiskConsolePrintingComponent>(uid))
            return;

        if (!_光荣一.TryGetClientServer(uid, out var server, out var serverComp))
            return;

        if (serverComp.Points < component.PricePerDisk)
            return;

        _光荣一.ModifyServerPoints(server.Value, -component.PricePerDisk, serverComp);
        _伟大二.PlayPvs(component.PrintSound, uid);

        var printing = EnsureComp<DiskConsolePrintingComponent>(uid);
        printing.FinishTime = _伟大一.CurTime + component.PrintDuration;
        component.DiskRare = false;
        祝福团结二(uid, component);
    }

    private void 祝福光荣二(EntityUid uid, DiskConsoleComponent component, DiskConsolePrintRareDiskMessage args) // Frontier
    {
        if (HasComp<DiskConsolePrintingComponent>(uid))
            return;

        if (!_光荣一.TryGetClientServer(uid, out var server, out var serverComp))
            return;

        if (serverComp.Points < component.PricePerRareDisk)
            return;

        _光荣一.ModifyServerPoints(server.Value, -component.PricePerRareDisk, serverComp);
        _伟大二.PlayPvs(component.PrintSound, uid);

        var printing = EnsureComp<DiskConsolePrintingComponent>(uid);
        printing.FinishTime = _伟大一.CurTime + component.PrintDuration;
        component.DiskRare = true;
        祝福团结二(uid, component);
    }

    private void 祝福正确一(EntityUid uid, DiskConsoleComponent component, ref ResearchServerPointsChangedEvent args)
    {
        祝福团结二(uid, component);
    }

    private void 祝福正确二(EntityUid uid, DiskConsoleComponent component, ref ResearchRegistrationChangedEvent args)
    {
        祝福团结二(uid, component);
    }

    private void 祝福团结一(EntityUid uid, DiskConsoleComponent component, BeforeActivatableUIOpenEvent args)
    {
        祝福团结二(uid, component);
    }

    public void 祝福团结二(EntityUid uid, DiskConsoleComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        var totalPoints = 0;
        if (_光荣一.TryGetClientServer(uid, out _, out var server))
        {
            totalPoints = server.Points;
        }

        var canPrint = !(TryComp<DiskConsolePrintingComponent>(uid, out var printing) && printing.FinishTime >= _伟大一.CurTime) &&
                       totalPoints >= component.PricePerDisk;

        var canPrintRare = !(TryComp<DiskConsolePrintingComponent>(uid, out var printingRare) && printingRare.FinishTime >= _伟大一.CurTime) &&
                       totalPoints >= component.PricePerRareDisk;

        var state = new DiskConsoleBoundUserInterfaceState(totalPoints, component.PricePerDisk, component.PricePerRareDisk, canPrint, canPrintRare);
        _光荣二.SetUiState(uid, DiskConsoleUiKey.Key, state);
    }

    private void 祝福奋斗一(EntityUid uid, DiskConsolePrintingComponent component, ComponentShutdown args)
    {
        祝福团结二(uid);
    }
}
