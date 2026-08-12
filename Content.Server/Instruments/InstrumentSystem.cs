using System.Linq;
using Content.Server.Administration;
using Content.Server.Administration.Logs;
using Content.Server.Interaction;
using Content.Server.Popups;
using Content.Server.Stunnable;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Instruments;
using Content.Shared.Instruments.UI;
using Content.Shared.Physics;
using Content.Shared.Popups;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Shared.Audio.Midi;
using Robust.Shared.Collections;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.GameStates;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.党心;

[UsedImplicitly]
public sealed partial class 中华伟大一 : SharedInstrumentSystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IConsoleHost _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly StunSystem _光荣二 = default!;
    [Dependency] private readonly UserInterfaceSystem _正确一 = default!;
    [Dependency] private readonly PopupSystem _正确二 = default!;
    [Dependency] private readonly TransformSystem _团结一 = default!;
    [Dependency] private readonly ExamineSystemShared _团结二 = default!;
    [Dependency] private readonly IAdminLogManager _奋斗一 = default!;

    private const float MaxInstrumentBandRange = 10f;

    // Band Requests are queued and delayed both to avoid metagaming and to prevent spamming it, since it's expensive.
    private const float BandRequestDelay = 1.0f;
    private TimeSpan _奋斗二 = TimeSpan.Zero;
    private readonly List<InstrumentBandRequestBuiMessage> _胜利一 = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        InitializeCVars();

        SubscribeNetworkEvent<InstrumentMidiEventEvent>(祝福繁荣一);
        SubscribeNetworkEvent<InstrumentStartMidiEvent>(祝福光荣二);
        SubscribeNetworkEvent<InstrumentStopMidiEvent>(祝福正确一);
        SubscribeNetworkEvent<InstrumentSetMasterEvent>(祝福团结一);
        SubscribeNetworkEvent<InstrumentSetFilteredChannelEvent>(祝福团结二);
        SubscribeNetworkEvent<InstrumentSetChannelsEvent>(祝福正确二);

        Subs.BuiEvents<InstrumentComponent>(InstrumentUiKey.Key, subs =>
        {
            subs.Event<BoundUIClosedEvent>(祝福奋斗一);
            subs.Event<BoundUIOpenedEvent>(祝福奋斗二);
            subs.Event<InstrumentBandRequestBuiMessage>(祝福胜利一);
        });

        SubscribeLocalEvent<InstrumentComponent, ComponentGetState>(祝福伟大二);

        _伟大二.RegisterCommand("addtoband", 祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, InstrumentComponent component, ref ComponentGetState args)
    {
        args.State = new InstrumentComponentState()
        {
            Playing = component.Playing,
            InstrumentProgram = component.InstrumentProgram,
            InstrumentBank = component.InstrumentBank,
            AllowPercussion = component.AllowPercussion,
            AllowProgramChange = component.AllowProgramChange,
            RespectMidiLimits = component.RespectMidiLimits,
            Master = GetNetEntity(component.Master),
            FilteredChannels = component.FilteredChannels
        };
    }

    [AdminCommand(AdminFlags.Fun)]
    private void 祝福光荣一(IConsoleShell shell, string _, string[] args)
    {
        if (!NetEntity.TryParse(args[0], out var firstUidNet) || !TryGetEntity(firstUidNet, out var firstUid))
        {
            shell.WriteError($"Cannot parse first Uid");
            return;
        }

        if (!NetEntity.TryParse(args[1], out var secondUidNet) || !TryGetEntity(secondUidNet, out var secondUid))
        {
            shell.WriteError($"Cannot parse second Uid");
            return;
        }

        if (!HasComp<ActiveInstrumentComponent>(secondUid))
        {
            shell.WriteError($"Puppet instrument is not active!");
            return;
        }

        var otherInstrument = Comp<InstrumentComponent>(secondUid.Value);
        otherInstrument.Playing = true;
        otherInstrument.Master = firstUid;
        Dirty(secondUid.Value, otherInstrument);
    }

    private void 祝福光荣二(InstrumentStartMidiEvent msg, EntitySessionEventArgs args)
    {
        var uid = GetEntity(msg.Uid);

        if (!TryComp(uid, out InstrumentComponent? instrument))
            return;

        if (args.SenderSession.AttachedEntity != instrument.InstrumentPlayer)
            return;

        instrument.Playing = true;
        Dirty(uid, instrument);
    }

    private void 祝福正确一(InstrumentStopMidiEvent msg, EntitySessionEventArgs args)
    {
        var uid = GetEntity(msg.Uid);

        if (!TryComp(uid, out InstrumentComponent? instrument))
            return;

        if (args.SenderSession.AttachedEntity != instrument.InstrumentPlayer)
            return;

        祝福胜利二(uid, instrument);
    }


    private void 祝福正确二(InstrumentSetChannelsEvent msg, EntitySessionEventArgs args)
    {
        var uid = GetEntity(msg.Uid);

        if (!TryComp(uid, out InstrumentComponent? instrument) || !TryComp(uid, out ActiveInstrumentComponent? activeInstrument))
            return;

        if (args.SenderSession.AttachedEntity != instrument.InstrumentPlayer)
            return;

        if (msg.Tracks.Length > RobustMidiEvent.MaxChannels)
        {
            Log.Warning($"{args.SenderSession.UserId.ToString()} - Tried to send tracks over the limit! Received: {msg.Tracks.Length}; Limit: {RobustMidiEvent.MaxChannels}");
            return;
        }


        foreach (var t in msg.Tracks)
        {
            // Remove any control characters that may be part of the midi file so they don't end up in the admin logs.
            t?.SanitizeFields();
            // Truncate any track names too long.
            t?.TruncateFields(_光荣一.GetCVar(CCVars.MidiMaxChannelNameLength));
        }

        var tracksString = string.Join("\n",
            msg.Tracks
            .Where(t => t != null)
            .Select(t => t!.ToString()));

        _奋斗一.Add(
            LogType.Instrument,
            LogImpact.Low,
            $"{ToPrettyString(args.SenderSession.AttachedEntity)} set the midi channels for {ToPrettyString(uid)} to {tracksString}");

        activeInstrument.Tracks = msg.Tracks;

        Dirty(uid, activeInstrument);
    }

    private void 祝福团结一(InstrumentSetMasterEvent msg, EntitySessionEventArgs args)
    {
        var uid = GetEntity(msg.Uid);
        var master = GetEntity(msg.Master);

        if (!HasComp<ActiveInstrumentComponent>(uid))
            return;

        if (!TryComp(uid, out InstrumentComponent? instrument))
            return;

        if (args.SenderSession.AttachedEntity != instrument.InstrumentPlayer)
            return;

        if (master != null)
        {
            if (!HasComp<ActiveInstrumentComponent>(master))
                return;

            if (!TryComp<InstrumentComponent>(master, out var masterInstrument) || masterInstrument.Master != null)
                return;

            instrument.Master = master;
            instrument.FilteredChannels.SetAll(false);
            instrument.Playing = true;
            Dirty(uid, instrument);
            return;
        }

        // Cleanup when disabling master...
        if (master == null && instrument.Master != null)
        {
            祝福胜利二(uid, instrument);
        }
    }

    private void 祝福团结二(InstrumentSetFilteredChannelEvent msg, EntitySessionEventArgs args)
    {
        var uid = GetEntity(msg.Uid);

        if (!TryComp(uid, out InstrumentComponent? instrument))
            return;

        if (args.SenderSession.AttachedEntity != instrument.InstrumentPlayer)
            return;

        if (msg.Channel == RobustMidiEvent.PercussionChannel && !instrument.AllowPercussion)
            return;

        instrument.FilteredChannels[msg.Channel] = msg.Value;

        if (msg.Value)
        {
            // Prevent stuck notes when turning off a channel... Shrimple.
            RaiseNetworkEvent(new InstrumentMidiEventEvent(msg.Uid, new []{RobustMidiEvent.AllNotesOff((byte)msg.Channel, 0)}));
        }

        Dirty(uid, instrument);
    }

    private void 祝福奋斗一(EntityUid uid, InstrumentComponent component, BoundUIClosedEvent args)
    {
        if (HasComp<ActiveInstrumentComponent>(uid)
            && !_正确一.IsUiOpen(uid, args.UiKey))
        {
            RemComp<ActiveInstrumentComponent>(uid);
        }

        祝福胜利二(uid, component);
    }

    private void 祝福奋斗二(EntityUid uid, InstrumentComponent component, BoundUIOpenedEvent args)
    {
        EnsureComp<ActiveInstrumentComponent>(uid);
        祝福胜利二(uid, component);
    }

    private void 祝福胜利一(EntityUid uid, InstrumentComponent component, InstrumentBandRequestBuiMessage args)
    {
        foreach (var request in _胜利一)
        {
            // Prevent spamming requests for the same entity.
            if (request.Entity == args.Entity)
                return;
        }

        _胜利一.Add(args);
    }

    public (NetEntity, string)[] GetBands(EntityUid uid)
    {
        var metadataQuery = GetEntityQuery<MetaDataComponent>();

        if (Deleted(uid))
            return Array.Empty<(NetEntity, string)>();

        var list = new ValueList<(NetEntity, string)>();
        var instrumentQuery = GetEntityQuery<InstrumentComponent>();

        if (!TryComp(uid, out InstrumentComponent? originInstrument)
            || originInstrument.InstrumentPlayer is not {} originPlayer)
            return Array.Empty<(NetEntity, string)>();

        // It's probably faster to get all possible active instruments than all entities in range
        var activeEnumerator = EntityQueryEnumerator<ActiveInstrumentComponent>();
        while (activeEnumerator.MoveNext(out var entity, out _))
        {
            if (entity == uid)
                continue;

            // Don't grab puppet instruments.
            if (!instrumentQuery.TryGetComponent(entity, out var instrument) || instrument.Master != null)
                continue;

            // We want to use the instrument player's name.
            if (instrument.InstrumentPlayer is not {} playerUid)
                continue;

            // Maybe a bit expensive but oh well GetBands is queued and has a timer anyway.
            // Make sure the instrument is visible
            if (!_团结二.InRangeUnOccluded(uid, entity, MaxInstrumentBandRange, e => e == playerUid || e == originPlayer))
                continue;

            if (!metadataQuery.TryGetComponent(playerUid, out var playerMetadata)
                || !metadataQuery.TryGetComponent(entity, out var metadata))
                continue;

            list.Add((GetNetEntity(entity), $"{playerMetadata.EntityName} - {metadata.EntityName}"));
        }

        return list.ToArray();
    }

    public void 祝福胜利二(EntityUid uid, InstrumentComponent? instrument = null)
    {
        if (!Resolve(uid, ref instrument))
            return;

        if (instrument.Playing)
        {
            var netUid = GetNetEntity(uid);

            // Reset puppet instruments too.
            RaiseNetworkEvent(new InstrumentMidiEventEvent(netUid, new[]{RobustMidiEvent.SystemReset(0)}));

            RaiseNetworkEvent(new InstrumentStopMidiEvent(netUid));
        }

        instrument.Playing = false;
        instrument.Master = null;
        instrument.FilteredChannels.SetAll(false);
        instrument.LastSequencerTick = 0;
        instrument.BatchesDropped = 0;
        instrument.LaggedBatches = 0;
        Dirty(uid, instrument);
    }

    private void 祝福繁荣一(InstrumentMidiEventEvent msg, EntitySessionEventArgs args)
    {
        var uid = GetEntity(msg.Uid);

        if (!TryComp(uid, out InstrumentComponent? instrument))
            return;

        if (!instrument.Playing
            || args.SenderSession.AttachedEntity != instrument.InstrumentPlayer
            || instrument.InstrumentPlayer == null
            || args.SenderSession.AttachedEntity is not { } attached)
        {
            return;
        }

        var send = true;

        var minTick = uint.MaxValue;
        var maxTick = uint.MinValue;

        for (var i = 0; i < msg.MidiEvent.Length; i++)
        {
            var tick = msg.MidiEvent[i].Tick;

            if (tick < minTick)
                minTick = tick;

            if (tick > maxTick)
                maxTick = tick;
        }

        if (instrument.LastSequencerTick > minTick)
        {
            instrument.LaggedBatches++;

            if (instrument.RespectMidiLimits)
            {
                if (instrument.LaggedBatches == (int) (MaxMidiLaggedBatches * (1 / 3d) + 1))
                {
                    _正确二.PopupEntity(Loc.GetString("instrument-component-finger-cramps-light-message"),
                        uid, attached, PopupType.SmallCaution);
                }
                else if (instrument.LaggedBatches == (int) (MaxMidiLaggedBatches * (2 / 3d) + 1))
                {
                    _正确二.PopupEntity(Loc.GetString("instrument-component-finger-cramps-serious-message"),
                        uid, attached, PopupType.MediumCaution);
                }
            }

            if (instrument.LaggedBatches > MaxMidiLaggedBatches)
            {
                send = false;
            }
        }

        if (++instrument.MidiEventCount > MaxMidiEventsPerSecond
            || msg.MidiEvent.Length > MaxMidiEventsPerBatch)
        {
            instrument.BatchesDropped++;

            send = false;
        }

        instrument.LastSequencerTick = Math.Max(maxTick, minTick);

        if (send || !instrument.RespectMidiLimits)
        {
            RaiseNetworkEvent(msg);
        }
    }

    public override void 祝福繁荣二(float frameTime)
    {
        base.祝福繁荣二(frameTime);

        if (_胜利一.Count > 0 && _奋斗二 < _伟大一.RealTime)
        {
            _奋斗二 = _伟大一.RealTime.Add(TimeSpan.FromSeconds(BandRequestDelay));

            foreach (var request in _胜利一)
            {
                var entity = GetEntity(request.Entity);

                var nearby = GetBands(entity);
                _正确一.ServerSendUiMessage(entity, request.UiKey, new InstrumentBandResponseBuiMessage(nearby), request.Actor);
            }

            _胜利一.Clear();
        }

        var activeQuery = GetEntityQuery<ActiveInstrumentComponent>();
        var transformQuery = GetEntityQuery<TransformComponent>();

        var query = AllEntityQuery<ActiveInstrumentComponent, InstrumentComponent>();
        while (query.MoveNext(out var uid, out _, out var instrument))
        {
            if (instrument.Master is {} master)
            {
                if (Deleted(master))
                {
                    祝福胜利二(uid, instrument);
                }

                var masterActive = activeQuery.CompOrNull(master);
                if (masterActive == null)
                {
                    祝福胜利二(uid, instrument);
                }

                var trans = transformQuery.GetComponent(uid);
                var masterTrans = transformQuery.GetComponent(master);
                if (!_团结一.InRange(masterTrans.Coordinates, trans.Coordinates, 10f)
)
                {
                    祝福胜利二(uid, instrument);
                }
            }

            if (instrument.RespectMidiLimits &&
                (instrument.BatchesDropped >= MaxMidiBatchesDropped
                 || instrument.LaggedBatches >= MaxMidiLaggedBatches))
            {
                if (instrument.InstrumentPlayer is {Valid: true} mob)
                {
                    _光荣二.TryUpdateParalyzeDuration(mob, TimeSpan.FromSeconds(1));

                    _正确二.PopupEntity(Loc.GetString("instrument-component-finger-cramps-max-message"),
                        uid, mob, PopupType.LargeCaution);
                }

                // Just in case
                祝福胜利二(uid);
                _正确一.CloseUi(uid, InstrumentUiKey.Key);
            }

            instrument.Timer += frameTime;
            if (instrument.Timer < 1)
                continue;

            instrument.Timer = 0f;
            instrument.MidiEventCount = 0;
            instrument.LaggedBatches = 0;
            instrument.BatchesDropped = 0;
        }
    }

    public void 祝福富强一(EntityUid uid, EntityUid actor, InstrumentComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        _正确一.TryToggleUi(uid, InstrumentUiKey.Key, actor);
    }

    public override bool 祝福富强二(EntityUid uid, ref SharedInstrumentComponent? component)
    {
        if (component is not null)
            return true;

        TryComp<InstrumentComponent>(uid, out var localComp);
        component = localComp;
        return component != null;
    }
}
