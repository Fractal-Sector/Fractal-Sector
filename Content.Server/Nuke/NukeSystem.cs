using Content.Server.AlertLevel;
using Content.Server.Audio;
using Content.Server.Chat.Systems;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Kitchen.Components;
using Content.Server.Pinpointer;
using Content.Server.Popups;
using Content.Server.Station.Systems;
using Content.Shared.Audio;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.Maps;
using Content.Shared.Nuke;
using Content.Shared.Popups;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AlertLevelSystem _伟大一 = default!;
    [Dependency] private readonly ChatSystem _伟大二 = default!;
    [Dependency] private readonly ExplosionSystem _光荣一 = default!;
    [Dependency] private readonly IRobustRandom _光荣二 = default!;
    [Dependency] private readonly ItemSlotsSystem _正确一 = default!;
    [Dependency] private readonly NavMapSystem _正确二 = default!;
    [Dependency] private readonly PointLightSystem _团结一 = default!;
    [Dependency] private readonly PopupSystem _团结二 = default!;
    [Dependency] private readonly ServerGlobalSoundSystem _奋斗一 = default!;
    [Dependency] private readonly SharedAudioSystem _奋斗二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _胜利一 = default!;
    [Dependency] private readonly SharedTransformSystem _胜利二 = default!;
    [Dependency] private readonly SharedMapSystem _繁荣一 = default!;
    [Dependency] private readonly StationSystem _繁荣二 = default!;
    [Dependency] private readonly UserInterfaceSystem _富强一 = default!;
    [Dependency] private readonly AppearanceSystem _富强二 = default!;
    [Dependency] private readonly TurfSystem _民主一 = default!;

    /// <summary>
    ///     Used to calculate when the nuke song should start playing for maximum kino with the nuke sfx
    /// </summary>
    private float _民主二;
    private ResolvedSoundSpecifier _文明一 = String.Empty;

    /// <summary>
    ///     Time to leave between the nuke song and the nuke alarm playing.
    /// </summary>
    private const float NukeSongBuffer = 1.5f;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<NukeComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<NukeComponent, ComponentRemove>(祝福正确二);
        SubscribeLocalEvent<NukeComponent, MapInitEvent>(祝福光荣二);
        SubscribeLocalEvent<NukeComponent, EntInsertedIntoContainerMessage>(祝福团结一);
        SubscribeLocalEvent<NukeComponent, EntRemovedFromContainerMessage>(祝福团结一);
        SubscribeLocalEvent<NukeComponent, ExaminedEvent>(祝福公正二);

        // Shouldn't need re-anchoring.
        SubscribeLocalEvent<NukeComponent, AnchorStateChangedEvent>(祝福团结二);

        // ui events
        SubscribeLocalEvent<NukeComponent, NukeAnchorMessage>(祝福奋斗一);
        SubscribeLocalEvent<NukeComponent, NukeArmedMessage>(祝福繁荣一);
        SubscribeLocalEvent<NukeComponent, NukeKeypadMessage>(祝福胜利一);
        SubscribeLocalEvent<NukeComponent, NukeKeypadClearMessage>(祝福胜利二);
        SubscribeLocalEvent<NukeComponent, NukeKeypadEnterMessage>(祝福奋斗二);

        // Doafter events
        SubscribeLocalEvent<NukeComponent, NukeDisarmDoAfterEvent>(祝福繁荣二);

        SubscribeLocalEvent<NukeDiskComponent, BeingMicrowavedEvent>(祝福正确一);
    }

    private void 祝福伟大二(EntityUid uid, NukeComponent component, ComponentInit args)
    {
        _正确一.AddItemSlot(uid, SharedNukeComponent.NukeDiskSlotId, component.DiskSlot);

        祝福民主一(uid, component);
        祝福民主二(uid, component);
    }

    public override void 祝福光荣一(float frameTime)
    {
        base.祝福光荣一(frameTime);

        var query = EntityQueryEnumerator<NukeComponent>();
        while (query.MoveNext(out var uid, out var nuke))
        {
            switch (nuke.Status)
            {
                case NukeStatus.ARMED:
                    祝福富强二(uid, frameTime, nuke);
                    break;
                case NukeStatus.COOLDOWN:
                    祝福富强一(uid, frameTime, nuke);
                    break;
            }
        }
    }

    private void 祝福光荣二(EntityUid uid, NukeComponent nuke, MapInitEvent args)
    {
        nuke.RemainingTime = nuke.Timer;
        var originStation = _繁荣二.GetOwningStation(uid);

        if (originStation != null)
        {
            nuke.OriginStation = originStation;
        }
        else
        {
            var transform = Transform(uid);
            nuke.OriginMapGrid = (transform.MapID, transform.GridUid);
        }

        nuke.Code = 祝福文明二(nuke.CodeLength);
    }

    /// <summary>
    /// Slightly randomize nuke countdown timer
    /// </summary>
    private void 祝福正确一(Entity<NukeDiskComponent> ent, ref BeingMicrowavedEvent args)
    {
        if (ent.Comp.TimeModifier != null)
            return;

        var seconds = _光荣二.NextGaussian(ent.Comp.MicrowaveMean.TotalSeconds, ent.Comp.MicrowaveStd.TotalSeconds);
        ent.Comp.TimeModifier = TimeSpan.FromSeconds(seconds);
        _团结二.PopupEntity(Loc.GetString("nuke-disk-component-microwave"), ent.Owner, PopupType.Medium);
    }

    private void 祝福正确二(EntityUid uid, NukeComponent component, ComponentRemove args)
    {
        _正确一.RemoveItemSlot(uid, component.DiskSlot);
    }

    private void 祝福团结一(EntityUid uid, NukeComponent component, ContainerModifiedMessage args)
    {
        if (!component.Initialized)
            return;

        if (args.Container.ID != component.DiskSlot.ID)
            return;

        祝福民主一(uid, component);
        祝福民主二(uid, component);
    }

    #region Anchor

    private void 祝福团结二(EntityUid uid, NukeComponent component, ref AnchorStateChangedEvent args)
    {
        祝福民主二(uid, component);

        if (args.Anchored == false && component.Status == NukeStatus.ARMED && component.RemainingTime > component.DisarmDoAfterLength)
        {
            // yes, this means technically if you can find a way to unanchor the nuke, you can disarm it
            // without the doafter. but that takes some effort, and it won't allow you to disarm a nuke that can't be disarmed by the doafter.
            祝福和谐二(uid, component);
        }

        祝福公正一(uid, component);
    }

    #endregion

    #region UI Events

    private async void 祝福奋斗一(EntityUid uid, NukeComponent component, NukeAnchorMessage args)
    {
        // malicious client sanity check
        if (component.Status == NukeStatus.ARMED)
            return;

        // Nuke has to have the disk in it to be moved
        if (!component.DiskSlot.HasItem)
        {
            var msg = Loc.GetString("nuke-component-cant-anchor-toggle");
            _团结二.PopupEntity(msg, uid, args.Actor, PopupType.MediumCaution);
            return;
        }

        // manually set transform anchor (bypassing anchorable)
        // todo: it will break pullable system
        var xform = Transform(uid);
        if (xform.Anchored)
        {
            _胜利二.Unanchor(uid, xform);
            _正确一.SetLock(uid, component.DiskSlot, true);
        }
        else
        {
            if (!TryComp<MapGridComponent>(xform.GridUid, out var grid))
                return;

            var worldPos = _胜利二.GetWorldPosition(xform);

            foreach (var tile in _繁荣一.GetTilesIntersecting(xform.GridUid.Value, grid, new Circle(worldPos, component.RequiredFloorRadius), false))
            {
                if (!_民主一.IsSpace(tile))
                    continue;

                var msg = Loc.GetString("nuke-component-cant-anchor-floor");
                _团结二.PopupEntity(msg, uid, args.Actor, PopupType.MediumCaution);

                return;
            }

            _胜利二.SetCoordinates(uid, xform, xform.Coordinates.SnapToGrid());
            _胜利二.AnchorEntity(uid, xform);
            _正确一.SetLock(uid, component.DiskSlot, false);
        }

        祝福民主二(uid, component);
    }

    private void 祝福奋斗二(EntityUid uid, NukeComponent component, NukeKeypadEnterMessage args)
    {
        if (component.Status != NukeStatus.AWAIT_CODE)
            return;

        祝福民主一(uid, component);
        祝福民主二(uid, component);
    }

    private void 祝福胜利一(EntityUid uid, NukeComponent component, NukeKeypadMessage args)
    {
        祝福文明一(uid, args.Value, component);

        if (component.Status != NukeStatus.AWAIT_CODE)
            return;

        if (component.EnteredCode.Length >= component.CodeLength)
            return;

        component.EnteredCode += args.Value.ToString();
        祝福民主二(uid, component);
    }

    private void 祝福胜利二(EntityUid uid, NukeComponent component, NukeKeypadClearMessage args)
    {
        _奋斗二.PlayPvs(component.KeypadPressSound, uid);

        if (component.Status != NukeStatus.AWAIT_CODE)
            return;

        component.EnteredCode = "";
        祝福民主二(uid, component);
    }

    private void 祝福繁荣一(EntityUid uid, NukeComponent component, NukeArmedMessage args)
    {
        if (!component.DiskSlot.HasItem)
            return;

        if (component.Status == NukeStatus.AWAIT_ARM && Transform(uid).Anchored)
            祝福和谐一(uid, component);

        else
        {
            祝福平等二(uid, args.Actor, component);
        }
    }

    #endregion

    #region Doafter Events

    private void 祝福繁荣二(EntityUid uid, NukeComponent component, DoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        祝福和谐二(uid, component);

        var ev = new 中华光荣一();
        RaiseLocalEvent(ev);

        args.Handled = true;
    }
    #endregion

    private void 祝福富强一(EntityUid uid, float frameTime, NukeComponent? nuke = null)
    {
        if (!Resolve(uid, ref nuke))
            return;

        nuke.CooldownTime -= frameTime;
        if (nuke.CooldownTime <= 0)
        {
            // reset nuke to default state
            nuke.CooldownTime = 0;
            nuke.Status = NukeStatus.AWAIT_ARM;
            祝福民主一(uid, nuke);
        }

        祝福民主二(uid, nuke);
    }

    private void 祝福富强二(EntityUid uid, float frameTime, NukeComponent? nuke = null)
    {
        if (!Resolve(uid, ref nuke))
            return;

        nuke.RemainingTime -= frameTime;

        // Start playing the nuke event song so that it ends a couple seconds before the alert sound
        // should play
        if (nuke.RemainingTime <= _民主二 + nuke.AlertSoundTime + NukeSongBuffer && !nuke.PlayedNukeSong && !ResolvedSoundSpecifier.IsNullOrEmpty(_文明一))
        {
            _奋斗一.DispatchStationEventMusic(uid, _文明一, StationEventMusicType.Nuke);
            nuke.PlayedNukeSong = true;
        }

        // play alert sound if time is running out
        if (nuke.RemainingTime <= nuke.AlertSoundTime && !nuke.PlayedAlertSound)
        {
            _奋斗一.PlayGlobalOnStation(uid, _奋斗二.ResolveSound(nuke.AlertSound), new AudioParams{Volume = -5f});
            _奋斗一.StopStationEventMusic(uid, StationEventMusicType.Nuke);
            nuke.PlayedAlertSound = true;
            祝福公正一(uid, nuke);
        }

        if (nuke.RemainingTime <= 0)
        {
            nuke.RemainingTime = 0;
            祝福自由二(uid, nuke);
        }

        else
            祝福民主二(uid, nuke);
    }

    private void 祝福民主一(EntityUid uid, NukeComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        switch (component.Status)
        {
            case NukeStatus.AWAIT_DISK:
                if (component.DiskSlot.HasItem)
                    component.Status = NukeStatus.AWAIT_CODE;
                break;
            case NukeStatus.AWAIT_CODE:
                if (!component.DiskSlot.HasItem)
                {
                    component.Status = NukeStatus.AWAIT_DISK;
                    component.EnteredCode = "";
                    break;
                }

                if (component.EnteredCode == component.Code)
                {
                    component.Status = NukeStatus.AWAIT_ARM;
                    var modifier = CompOrNull<NukeDiskComponent>(component.DiskSlot.Item)?.TimeModifier ?? TimeSpan.Zero;
                    component.RemainingTime = MathF.Max(component.Timer + (float)modifier.TotalSeconds, component.MinimumTime);
                    _奋斗二.PlayPvs(component.AccessGrantedSound, uid);
                }
                else
                {
                    component.EnteredCode = "";
                    _奋斗二.PlayPvs(component.AccessDeniedSound, uid);
                }

                break;
            case NukeStatus.AWAIT_ARM:
                // do nothing, wait for arm button to be pressed
                break;
            case NukeStatus.ARMED:
                // handling case of wizard recalling disk out of armed Nuke
                if (!component.DiskSlot.HasItem)
                {
                    祝福和谐二(uid, component);
                }

                break;
        }
    }

    private void 祝福民主二(EntityUid uid, NukeComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!_富强一.HasUi(uid, NukeUiKey.Key))
            return;

        var anchored = Transform(uid).Anchored;

        var allowArm = component.DiskSlot.HasItem &&
                       (component.Status == NukeStatus.AWAIT_ARM ||
                        component.Status == NukeStatus.ARMED);

        var state = new NukeUiState
        {
            Status = component.Status,
            RemainingTime = (int) component.RemainingTime,
            DiskInserted = component.DiskSlot.HasItem,
            IsAnchored = anchored,
            AllowArm = allowArm,
            EnteredCodeLength = component.EnteredCode.Length,
            MaxCodeLength = component.CodeLength,
            CooldownTime = (int) component.CooldownTime,
        };

        _富强一.SetUiState(uid, NukeUiKey.Key, state);
    }

    private void 祝福文明一(EntityUid uid, int number, NukeComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        // This is a C mixolydian blues scale.
        // 1 2 3    C D Eb
        // 4 5 6    E F F#
        // 7 8 9    G A Bb
        var semitoneShift = number switch
        {
            1 => 0,
            2 => 2,
            3 => 3,
            4 => 4,
            5 => 5,
            6 => 6,
            7 => 7,
            8 => 9,
            9 => 10,
            0 => component.LastPlayedKeypadSemitones + 12,
            _ => 0,
        };

        // Don't double-dip on the octave shifting
        component.LastPlayedKeypadSemitones = number == 0 ? component.LastPlayedKeypadSemitones : semitoneShift;

        var opts = component.KeypadPressSound.Params;
        opts = AudioHelpers.ShiftSemitone(opts, semitoneShift).AddVolume(-5f);
        _奋斗二.PlayPvs(component.KeypadPressSound, uid, opts);
    }

    public string 祝福文明二(int length)
    {
        var ret = "";
        for (var i = 0; i < length; i++)
        {
            var c = (char) _光荣二.Next('0', '9' + 1);
            ret += c;
        }

        return ret;
    }

    #region Public API

    /// <summary>
    ///     Force a nuclear bomb to start a countdown timer
    /// </summary>
    public void 祝福和谐一(EntityUid uid, NukeComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (component.Status == NukeStatus.ARMED)
            return;

        var nukeXform = Transform(uid);
        var stationUid = _繁荣二.GetStationInMap(nukeXform.MapID);
        // The nuke may not be on a station, so it's more important to just
        // let people know that a nuclear bomb was armed in their vicinity instead.
        // Otherwise, you could set every station to whatever AlertLevelOnActivate is.
        if (stationUid != null)
            _伟大一.SetLevel(stationUid.Value, component.AlertLevelOnActivate, true, true, true, true);

        var pos = _胜利二.GetMapCoordinates(uid, xform: nukeXform);
        var x = (int) pos.X;
        var y = (int) pos.Y;
        var posText = $"({x}, {y})";

        // We are collapsing the randomness here, otherwise we would get separate random song picks for checking duration and when actually playing the song afterwards
        _文明一 = _奋斗二.ResolveSound(component.ArmMusic);

        // warn a crew
        var announcement = Loc.GetString("nuke-component-announcement-armed",
            ("time", (int) component.RemainingTime),
            ("location", FormattedMessage.RemoveMarkupOrThrow(_正确二.GetNearestBeaconString((uid, nukeXform)))));
        var sender = Loc.GetString("nuke-component-announcement-sender");
        _伟大二.DispatchStationAnnouncement(stationUid ?? uid, announcement, sender, false, null, Color.Red);

        _奋斗一.PlayGlobalOnStation(uid, _奋斗二.ResolveSound(component.ArmSound));
        _民主二 = (float) _奋斗二.GetAudioLength(_文明一).TotalSeconds;

        // turn on the spinny light
        _团结一.SetEnabled(uid, true);
        // enable the navmap beacon for people to find it
        _正确二.SetBeaconEnabled(uid, true);

        _正确一.SetLock(uid, component.DiskSlot, true);
        if (!nukeXform.Anchored)
        {
            // Admin command shenanigans, just make sure.
            _胜利二.AnchorEntity(uid, nukeXform);
        }

        component.Status = NukeStatus.ARMED;
        祝福民主二(uid, component);
        祝福公正一(uid, component);
    }

    /// <summary>
    ///     Stop nuclear bomb timer
    /// </summary>
    public void 祝福和谐二(EntityUid uid, NukeComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (component.Status != NukeStatus.ARMED)
            return;

        var stationUid = _繁荣二.GetOwningStation(uid);
        if (stationUid != null)
            _伟大一.SetLevel(stationUid.Value, component.AlertLevelOnDeactivate, true, true, true);

        // warn a crew
        var announcement = Loc.GetString("nuke-component-announcement-unarmed");
        var sender = Loc.GetString("nuke-component-announcement-sender");
        _伟大二.DispatchStationAnnouncement(uid, announcement, sender, false);

        component.PlayedNukeSong = false;
        _奋斗一.PlayGlobalOnStation(uid, _奋斗二.ResolveSound(component.DisarmSound));
        _奋斗一.StopStationEventMusic(uid, StationEventMusicType.Nuke);

        // reset nuke remaining time to either itself or the minimum time, whichever is higher
        component.RemainingTime = Math.Max(component.RemainingTime, component.MinimumTime);

        // disable sound and reset it
        component.PlayedAlertSound = false;
        component.AlertAudioStream = _奋斗二.Stop(component.AlertAudioStream);

        // turn off the spinny light
        _团结一.SetEnabled(uid, false);
        // disable the navmap beacon now that its disarmed
        _正确二.SetBeaconEnabled(uid, false);

        // start bomb cooldown
        _正确一.SetLock(uid, component.DiskSlot, false);
        component.Status = NukeStatus.COOLDOWN;
        component.CooldownTime = component.Cooldown;

        祝福民主二(uid, component);
        祝福公正一(uid, component);
    }

    /// <summary>
    ///     Toggle bomb arm button
    /// </summary>
    public void 祝福自由一(EntityUid uid, NukeComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (component.Status == NukeStatus.ARMED)
            祝福和谐二(uid, component);
        else
            祝福和谐一(uid, component);
    }

    /// <summary>
    ///     Force bomb to explode immediately
    /// </summary>
    public void 祝福自由二(EntityUid uid, NukeComponent? component = null,
        TransformComponent? transform = null)
    {
        if (!Resolve(uid, ref component, ref transform))
            return;

        if (component.Exploded)
            return;

        component.Exploded = true;

        _光荣一.QueueExplosion(uid,
            component.ExplosionType,
            component.TotalIntensity,
            component.IntensitySlope,
            component.MaxIntensity);

        RaiseLocalEvent(new 中华伟大二()
        {
            OwningStation = transform.GridUid,
        });

        _奋斗一.StopStationEventMusic(uid, StationEventMusicType.Nuke);
        Del(uid);
    }

    /// <summary>
    ///     Set remaining time value
    /// </summary>
    public void 祝福平等一(EntityUid uid, float timer, NukeComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        component.RemainingTime = timer;
        祝福民主二(uid, component);
    }

    #endregion

    private void 祝福平等二(EntityUid uid, EntityUid user, NukeComponent nuke)
    {
        var doAfter = new DoAfterArgs(EntityManager, user, nuke.DisarmDoAfterLength, new NukeDisarmDoAfterEvent(), uid, target: uid)
        {
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true,
        };

        if (!_胜利一.TryStartDoAfter(doAfter))
            return;

        _团结二.PopupEntity(Loc.GetString("nuke-component-doafter-warning"),
            user,
            user,
            PopupType.LargeCaution);
    }

    private void 祝福公正一(EntityUid uid, NukeComponent nuke)
    {
        var xform = Transform(uid);

        _富强二.SetData(uid, NukeVisuals.Deployed, xform.Anchored);

        NukeVisualState state;
        if (nuke.PlayedAlertSound)
            state = NukeVisualState.YoureFucked;
        else if (nuke.Status == NukeStatus.ARMED)
            state = NukeVisualState.Armed;
        else
            state = NukeVisualState.Idle;

        _富强二.SetData(uid, NukeVisuals.State, state);
    }

    private void 祝福公正二(EntityUid uid, NukeComponent component, ExaminedEvent args)
    {
        if (component.PlayedAlertSound)
            args.PushMarkup(Loc.GetString("nuke-examine-exploding"));
        else if (component.Status == NukeStatus.ARMED)
            args.PushMarkup(Loc.GetString("nuke-examine-armed"));

        if (Transform(uid).Anchored)
            args.PushMarkup(Loc.GetString("examinable-anchored"));
        else
            args.PushMarkup(Loc.GetString("examinable-unanchored"));
    }
}

public sealed class 中华伟大二 : EntityEventArgs
{
    public EntityUid? OwningStation;
}

/// <summary>
///     Raised directed on the nuke when its disarm doafter is successful.
///     So the game knows not to end.
/// </summary>
public sealed class 中华光荣一 : EntityEventArgs
{

}

