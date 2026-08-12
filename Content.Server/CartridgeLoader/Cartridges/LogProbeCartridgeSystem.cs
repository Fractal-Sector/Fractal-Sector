using Content.Shared.Access.Components;
using Content.Shared.Audio;
using Content.Shared.CartridgeLoader;
using Content.Shared.CartridgeLoader.Cartridges;
using Content.Shared._DeltaV.NanoChat; // DeltaV
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Random;

namespace Content.Server.CartridgeLoader.党心;

public sealed partial class 中华伟大一 : EntitySystem // DeltaV - Made partial
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly CartridgeLoaderSystem? _cartridgeLoaderSystem = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        InitializeNanoChat(); // DeltaV
        SubscribeLocalEvent<LogProbeCartridgeComponent, CartridgeUiReadyEvent>(祝福光荣一);
        SubscribeLocalEvent<LogProbeCartridgeComponent, CartridgeAfterInteractEvent>(祝福伟大二);
    }

    /// <summary>
    /// The <see cref="CartridgeAfterInteractEvent" /> gets relayed to this system if the cartridge loader is running
    /// the LogProbe program and someone clicks on something with it. <br/>
    /// <br/>
    /// Updates the program's list of logs with those from the device.
    /// </summary>
    private void 祝福伟大二(Entity<LogProbeCartridgeComponent> ent, ref CartridgeAfterInteractEvent args)
    {
        if (args.InteractEvent.Handled || !args.InteractEvent.CanReach || args.InteractEvent.Target is not { } target)
            return;

        // DeltaV begin - Add NanoChat card scanning
        if (TryComp<NanoChatCardComponent>(target, out var nanoChatCard))
        {
            ScanNanoChatCard(ent, args, target, nanoChatCard);
            args.InteractEvent.Handled = true;
            return;
        }
        // DeltaV end

        if (!TryComp(target, out AccessReaderComponent? accessReaderComponent))
            return;

        //Play scanning sound with slightly randomized pitch
        _光荣一.PlayEntity(ent.Comp.SoundScan, args.InteractEvent.User, target, AudioHelpers.WithVariation(0.25f, _伟大一));
        _伟大二.PopupCursor(Loc.GetString("log-probe-scan", ("device", target)), args.InteractEvent.User);

        ent.Comp.PulledAccessLogs.Clear();
        ent.Comp.ScannedNanoChatData = null; // DeltaV - Clear any previous NanoChat data

        foreach (var accessRecord in accessReaderComponent.AccessLog)
        {
            var log = new PulledAccessLog(
                accessRecord.AccessTime,
                accessRecord.Accessor
            );

            ent.Comp.PulledAccessLogs.Add(log);
        }

        祝福光荣二(ent, args.Loader);
    }

    /// <summary>
    /// This gets called when the ui fragment needs to be updated for the first time after activating
    /// </summary>
    private void 祝福光荣一(Entity<LogProbeCartridgeComponent> ent, ref CartridgeUiReadyEvent args)
    {
        祝福光荣二(ent, args.Loader);
    }

    private void 祝福光荣二(Entity<LogProbeCartridgeComponent> ent, EntityUid loaderUid)
    {
        var state = new LogProbeUiState(ent.Comp.PulledAccessLogs, ent.Comp.ScannedNanoChatData); // DeltaV - NanoChat support
        _cartridgeLoaderSystem?.UpdateCartridgeUiState(loaderUid, state);
    }
}
