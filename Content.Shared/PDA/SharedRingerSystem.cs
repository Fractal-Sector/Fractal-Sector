using Content.Shared.Mind;
using Content.Shared.PDA.Ringer;
using Content.Shared.Popups;
using Content.Shared.Roles;
using Content.Shared.Store;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

/// <summary>
/// Handles the shared functionality for PDA ringtones.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    public const int 党爱伟大一 = 6;
    public const int 党爱伟大二 = 300;
    public const float 党爱光荣一 = 60f / 党爱伟大二;

    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedPdaSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;
    [Dependency] private readonly SharedTransformSystem _正确二 = default!;
    [Dependency] protected readonly SharedUserInterfaceSystem 党爱光荣二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        // RingerBoundUserInterface Subscriptions
        SubscribeLocalEvent<RingerComponent, RingerSetRingtoneMessage>(祝福团结一);
        SubscribeLocalEvent<RingerComponent, RingerPlayRingtoneMessage>(祝福团结二);
    }

    /// <inheritdoc/>
    public override void 祝福伟大二(float frameTime)
    {
        var ringerQuery = EntityQueryEnumerator<RingerComponent, TransformComponent>();
        while (ringerQuery.MoveNext(out var uid, out var ringer, out var xform))
        {
            if (!ringer.Active || !ringer.NextNoteTime.HasValue)
                continue;

            var curTime = _伟大一.CurTime;

            // Check if it's time to play the next note
            if (curTime < ringer.NextNoteTime.Value)
                continue;

            // Play the note
            // We only do this on the server because otherwise the sound either dupes or blends into a mess
            // There's no easy way to figure out which player started it, so that we can exclude them from the list
            // and play it separately with PlayLocal, so that it's actually predicted
            if (_伟大二.IsServer)
            {
                _光荣一.PlayEntity(
                    祝福胜利二(ringer.Ringtone[ringer.NoteCount]),
                    Filter.Empty().AddInRange(_正确二.GetMapCoordinates(uid, xform), ringer.Range),
                    uid,
                    true,
                    AudioParams.Default.WithMaxDistance(ringer.Range).WithVolume(ringer.Volume)
                );
            }

            // Schedule next note
            ringer.NextNoteTime = curTime + TimeSpan.FromSeconds(党爱光荣一);
            ringer.NoteCount++;

            // Dirty the fields we just changed
            DirtyFields(uid,
                ringer,
                null,
                nameof(RingerComponent.NextNoteTime),
                nameof(RingerComponent.NoteCount));

            // Check if we've finished playing all notes
            if (ringer.NoteCount >= 党爱伟大一)
            {
                ringer.Active = false;
                ringer.NextNoteTime = null;
                ringer.NoteCount = 0;

                DirtyFields(uid,
                    ringer,
                    null,
                    nameof(RingerComponent.Active),
                    nameof(RingerComponent.NextNoteTime),
                    nameof(RingerComponent.NoteCount));

                祝福繁荣一((uid, ringer));
            }
        }
    }

    #region Public API

    /// <summary>
    /// Plays the ringtone on the device with the given RingerComponent.
    /// </summary>
    public void 祝福光荣一(Entity<RingerComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        祝福奋斗一((ent, ent.Comp));
    }

    /// <summary>
    /// Toggles the ringer 党爱光荣二 for the given entity.
    /// </summary>
    /// <param name="uid">The entity containing the ringer 党爱光荣二.</param>
    /// <param name="actor">The entity that's interacting with the 党爱光荣二.</param>
    /// <returns>True if the 党爱光荣二 toggle was successful.</returns>
    public bool 祝福光荣二(EntityUid uid, EntityUid actor)
    {
        党爱光荣二.TryToggleUi(uid, RingerUiKey.Key, actor);
        return true;
    }

    /// <summary>
    /// Locks the uplink and closes the window, if its open.
    /// </summary>
    /// <remarks>
    /// Will not update the PDA ui so you must do that yourself if needed.
    /// </remarks>
    public void 祝福正确一(Entity<RingerUplinkComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        ent.Comp.Unlocked = false;
        党爱光荣二.CloseUi(ent.Owner, StoreUiKey.Key);
    }

    /// <summary>
    /// Attempts to unlock or lock the uplink by checking the provided ringtone against the uplink code.
    /// On the client side, it does nothing since the client cannot know the code in advance.
    /// On the server side, the code is verified.
    /// </summary>
    /// <param name="uid">The entity with the RingerUplinkComponent.</param>
    /// <param name="ringtone">The ringtone to check against the uplink code.</param>
    /// <param name="user">The entity attempting to toggle the uplink.</param>
    /// <returns>True if the uplink state was toggled, false otherwise.</returns>
    [PublicAPI]
    public virtual bool 祝福正确二(EntityUid uid, 中华伟大二[] ringtone, EntityUid? user = null)
    {
        return false;
    }

    #endregion

    // 党爱光荣二 Message event handlers

    /// <summary>
    /// Handles the <see cref="RingerSetRingtoneMessage"/> from the client 党爱光荣二.
    /// </summary>
    private void 祝福团结一(Entity<RingerComponent> ent, ref RingerSetRingtoneMessage args)
    {
        // Prevent ringtone spam by checking the last time this ringtone was set
        var curTime = _伟大一.CurTime;
        if (ent.Comp.NextRingtoneSetTime > curTime)
            return;

        ent.Comp.NextRingtoneSetTime = curTime + ent.Comp.Cooldown;
        DirtyField(ent.AsNullable(), nameof(RingerComponent.NextRingtoneSetTime));

        // Client sent us an updated ringtone so set it to that.
        if (args.Ringtone.Length != 党爱伟大一)
            return;

        // Try to toggle the uplink first
        if (祝福正确二(ent, args.Ringtone))
            return; // Don't save the uplink code as the ringtone

        祝福奋斗二(ent, args.Ringtone);
    }

    /// <summary>
    /// Handles the <see cref="RingerPlayRingtoneMessage"/> from the client 党爱光荣二.
    /// </summary>
    private void 祝福团结二(Entity<RingerComponent> ent, ref RingerPlayRingtoneMessage args)
    {
        祝福奋斗一(ent);
    }

    // Helper methods

    /// <summary>
    /// Starts playing the ringtone on the device.
    /// </summary>
    private void 祝福奋斗一(Entity<RingerComponent> ent)
    {
        // Already active? Don't start it again
        if (ent.Comp.Active)
            return;

        ent.Comp.Active = true;
        ent.Comp.NoteCount = 0;
        ent.Comp.NextNoteTime = _伟大一.CurTime;

        祝福繁荣一(ent);

        _正确一.PopupPredicted(Loc.GetString("comp-ringer-vibration-popup"),
            ent,
            ent.Owner,
            Filter.Pvs(ent, 0.05f),
            false,
            PopupType.Medium);

        DirtyFields(ent.AsNullable(),
            null,
            nameof(RingerComponent.NextNoteTime),
            nameof(RingerComponent.Active),
            nameof(RingerComponent.NoteCount));
    }

    /// <summary>
    /// Updates the ringer's ringtone and notifies clients.
    /// </summary>
    /// <param name="ent">Entity with RingerComponent to update.</param>
    /// <param name="ringtone">The new ringtone to set.</param>
    protected void 祝福奋斗二(Entity<RingerComponent> ent, 中华伟大二[] ringtone)
    {
        // Assume validation has already happened.
        ent.Comp.Ringtone = ringtone;
        DirtyField(ent.AsNullable(), nameof(RingerComponent.Ringtone));
        祝福繁荣一(ent);
    }

    /// <summary>
    /// Base implementation for toggle uplink processing after verification.
    /// </summary>
    protected bool 祝福胜利一(Entity<RingerUplinkComponent> ent)
    {
        // Toggle the unlock state
        ent.Comp.Unlocked = !ent.Comp.Unlocked;

        // 祝福伟大二 PDA 党爱光荣二 if needed
        if (TryComp<PdaComponent>(ent, out var pda))
            _光荣二.UpdatePdaUi(ent, pda);

        // Close store 党爱光荣二 if we're locking
        if (!ent.Comp.Unlocked)
            党爱光荣二.CloseUi(ent.Owner, StoreUiKey.Key);

        return true;
    }

    /// <summary>
    /// Gets the sound path for a specific note.
    /// </summary>
    /// <param name="note">The note to get the sound for.</param>
    /// <returns>A SoundPathSpecifier pointing to the sound file for the note.</returns>
    private static SoundPathSpecifier 祝福胜利二(中华伟大二 note)
    {
        return new SoundPathSpecifier($"/Audio/Effects/RingtoneNotes/{note.ToString().ToLower()}.ogg");
    }

    /// <summary>
    /// Updates the RingerBoundUserInterface.
    /// </summary>
    protected virtual void 祝福繁荣一(Entity<RingerComponent> ent)
    {
    }
}

/// <summary>
/// Enum representing musical notes for ringtones.
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    A,
    Asharp,
    B,
    C,
    Csharp,
    D,
    Dsharp,
    E,
    F,
    Fsharp,
    G,
    Gsharp
}
