using Content.Client.UserInterface.Systems.Chat;
using Content.Shared._FS.CCVar;
using Content.Shared._FS.VoiceBark;
using Content.Shared._FS.VoiceBark.Components;
using Content.Shared._FS.VoiceBark.Systems;
using Content.Shared.Chat;
using Robust.Client.Audio;
using Robust.Client.UserInterface;
using Robust.Shared.Audio;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Client._FS.VoiceBark;

/// <summary>
/// Client-side bark playback. Ordinary chat speech doesn't need a server
/// broadcast - every client that already receives a normal ChatMessage
/// (via vanilla chat replication) independently generates and plays its own
/// bark audio for the speaker, using that speaker's networked
/// VoiceBarkComponent for pitch/volume/pause.
/// </summary>
public sealed class VoiceBarkSystem : SharedVoiceBarkSystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly AudioSystem _audio = default!;
    [Dependency] private readonly IUserInterfaceManager _uiManager = default!;

    private bool _enabled;
    private float _volume;
    private int _maxBarkCount;

    public override void Initialize()
    {
        base.Initialize();
        UpdatesOutsidePrediction = true;

        _uiManager.GetUIController<ChatUIController>().MessageAdded += OnMessageAdded;

        _cfg.OnValueChanged(FSCVars.VoiceBarkVolume, OnVolumeChanged, true);
        _cfg.OnValueChanged(FSCVars.VoiceBarkType, OnVoiceTypeChanged, true);
        _cfg.OnValueChanged(FSCVars.VoiceBarkLimit, OnLimitChanged, true);
        SubscribeNetworkEvent<EntityVoiceBarkEvent>(OnEntityBark);
    }

    public override void Shutdown()
    {
        _uiManager.GetUIController<ChatUIController>().MessageAdded -= OnMessageAdded;
        _cfg.UnsubValueChanged(FSCVars.VoiceBarkVolume, OnVolumeChanged);
        _cfg.UnsubValueChanged(FSCVars.VoiceBarkType, OnVoiceTypeChanged);
        _cfg.UnsubValueChanged(FSCVars.VoiceBarkLimit, OnLimitChanged);
    }

    private void OnMessageAdded(ChatMessage message)
    {
        if (message.Channel is not (ChatChannel.Local or ChatChannel.Whisper))
            return;

        var ent = GetEntity(message.SenderEntity);
        if (!TryComp<VoiceBarkComponent>(ent, out var comp))
            return;

        Bark(new(ent, comp), message.Message, message.Channel == ChatChannel.Whisper);
    }

    private void OnVoiceTypeChanged(CharacterVoiceType voice) => _enabled = voice == CharacterVoiceType.Bark;
    private void OnVolumeChanged(float volume) => _volume = volume;
    private void OnLimitChanged(int count) => _maxBarkCount = count;

    private void OnEntityBark(EntityVoiceBarkEvent ev)
    {
        var ent = GetEntity(ev.Entity);
        if (!TryComp<VoiceBarkComponent>(ent, out var comp))
            return;

        Bark(new(ent, comp), ev.Barks);
    }

    public override void Bark(Entity<VoiceBarkComponent> entity, List<VoiceBarkData> barks)
    {
        if (!_enabled)
            return;

        if (TryComp<VoiceBarkQueueComponent>(entity, out var queue))
            RemComp(entity, queue);

        if (_maxBarkCount > 0 && barks.Count > _maxBarkCount)
            barks.RemoveRange(_maxBarkCount, barks.Count - _maxBarkCount);

        queue = AddComp<VoiceBarkQueueComponent>(entity);
        queue.Barks = new(barks);
        queue.ResolvedSound = entity.Comp.VoiceData.BarkSound;
    }

    private void PlayToken(EntityUid entity, SoundSpecifier soundSpecifier, VoiceBarkData currentBark)
    {
        if (!currentBark.Enabled)
            return;

        _audio.PlayEntity(
            soundSpecifier,
            Filter.Local(),
            entity,
            true,
            new AudioParams()
                .WithPitchScale(currentBark.Pitch)
                .WithVolume(currentBark.Volume * _volume));
    }

    public override void Update(float frameTime)
    {
        if (!_gameTiming.IsFirstTimePredicted)
            return;

        var query = EntityQueryEnumerator<VoiceBarkQueueComponent>();
        while (query.MoveNext(out var uid, out var queue))
        {
            if (queue.CurrentBark is null)
            {
                if (!queue.Barks.TryDequeue(out var barkData))
                {
                    RemComp(uid, queue);
                    continue;
                }

                queue.CurrentBark = barkData;
            }

            if (queue.CurrentBark.Value.Pause <= queue.BarkTime)
            {
                queue.BarkTime = 0;
                PlayToken(uid, queue.ResolvedSound, queue.CurrentBark.Value);
                queue.CurrentBark = null;
            }
            else
            {
                queue.BarkTime += frameTime;
            }
        }
    }
}
