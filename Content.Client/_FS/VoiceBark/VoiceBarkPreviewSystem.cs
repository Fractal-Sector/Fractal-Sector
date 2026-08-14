using System.Linq;
using Content.Shared._FS.VoiceBark;
using Robust.Client.Audio;
using Robust.Client.Player;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Client._FS.VoiceBark;

/// <summary>
/// Client-only, entity-less playback used to preview a bark voice from the
/// character editor - plays globally (just for the local player) rather
/// than attached to any entity.
/// </summary>
public sealed class VoiceBarkPreviewSystem : EntitySystem
{
    [Dependency] private readonly VoiceBarkSystem _voiceBarkSystem = default!;
    [Dependency] private readonly AudioSystem _audio = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    private Queue<(VoiceBarkData, SoundSpecifier)> _barks = new();
    private (VoiceBarkData, SoundSpecifier)? _currentBark;
    private float _barkTime;

    public void PlayGlobal(ProtoId<VoiceBarkPrototype> protoId, string text, VoiceBarkPercentageApplyData? data = null)
    {
        if (!_prototypeManager.TryIndex(protoId, out var prototype))
            return;

        PlayGlobal(prototype, text, data);
    }

    public void PlayGlobal(VoiceBarkPrototype prototype, string text, VoiceBarkPercentageApplyData? data = null)
    {
        var voiceData = VoiceBarkVoiceData.WithClampingValue(prototype.BarkSound, prototype.ClampData, data ?? VoiceBarkPercentageApplyData.Default);
        PlayGlobal(voiceData, _voiceBarkSystem.GenBarkData(voiceData, text, false));
    }

    public void PlayGlobal(VoiceBarkVoiceData barkVoiceData, List<VoiceBarkData> barks)
    {
        _barks = new(barks.Select(p => (p, barkVoiceData.BarkSound)));
        _barkTime = 0;
        _currentBark = null;
    }

    public override void Update(float frameTime)
    {
        if (!_gameTiming.IsFirstTimePredicted)
            return;

        if (_playerManager.LocalSession is null)
            return;

        if (_currentBark is null)
        {
            if (!_barks.TryDequeue(out var barkData))
            {
                _barkTime = 0;
                return;
            }

            _currentBark = barkData;
        }

        if (_currentBark.Value.Item1.Pause <= _barkTime)
        {
            _barkTime = 0;

            if (!_currentBark.Value.Item1.Enabled)
            {
                _currentBark = null;
                return;
            }

            _audio.PlayGlobal(
                _currentBark.Value.Item2,
                _playerManager.LocalSession,
                new AudioParams()
                    .WithPitchScale(_currentBark.Value.Item1.Pitch)
                    .WithVolume(_currentBark.Value.Item1.Volume));
            _currentBark = null;
        }
        else
        {
            _barkTime += frameTime;
        }
    }
}
