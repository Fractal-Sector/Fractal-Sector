using System.Linq;
using Content.Shared._FS.VoiceBark.Components;
using Content.Shared.Inventory;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared._FS.VoiceBark.Systems;

/// <summary>
/// Shared logic for the bark-voice feature: resolving a character's voice
/// settings into playback data per spoken "letter". Actual sound playback
/// differs between server (dead broadcast path kept for parity) and client
/// (driven off the chat log) - see the derived systems.
/// </summary>
public abstract class SharedVoiceBarkSystem : EntitySystem
{
    private static readonly char[] LongPauseChars = ['.', ',', '?', '!'];
    private static readonly char[] SkipChars = [' ', '\n', '\r', '\t'];

    // Cyrillic uppercase consonants - used to lower pitch/volume/pause on
    // consonant-ish beeps. Purely a phonetic flavor tweak, not tied to the
    // client's UI locale.
    private static readonly char[] Soglasnoy =
        ['Б', 'В', 'Г', 'Д', 'Ж', 'З', 'Й', 'К', 'Л', 'М', 'Н', 'П', 'Р', 'С', 'Т', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ'];

    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ApplyVoiceBarkProtoComponent, ComponentInit>(OnApplyVoiceBarkInit);
    }

    private void OnApplyVoiceBarkInit(Entity<ApplyVoiceBarkProtoComponent> ent, ref ComponentInit args)
    {
        ApplyVoiceBark(ent.Owner, ent.Comp.VoiceProto, ent.Comp.PercentageApplyData);
        RemComp(ent.Owner, ent.Comp);
    }

    public List<VoiceBarkPrototype> GetVoiceList(ProtoId<VoiceBarkListPrototype>? id = null)
    {
        var list = new List<VoiceBarkPrototype>();

        if (!_prototypeManager.TryIndex(id ?? "Default", out var listProto))
            return list;

        foreach (var voice in listProto.VoiceList)
        {
            if (_prototypeManager.TryIndex(voice, out var prototype))
                list.Add(prototype);
        }

        return list;
    }

    public void ApplyVoiceBark(EntityUid uid, ProtoId<VoiceBarkPrototype> protoId, VoiceBarkPercentageApplyData? data = null)
    {
        if (!_prototypeManager.TryIndex(protoId, out var prototype))
            return;

        ApplyVoiceBark(uid, prototype.BarkSound, prototype.ClampData, data);
    }

    public void ApplyVoiceBark(EntityUid uid, SoundSpecifier barkSound, VoiceBarkClampData clampData, VoiceBarkPercentageApplyData? data = null)
    {
        var voiceData = VoiceBarkVoiceData.WithClampingValue(
            barkSound,
            clampData,
            data ?? VoiceBarkPercentageApplyData.Default);

        var hadComp = HasComp<VoiceBarkComponent>(uid);
        var comp = EnsureComp<VoiceBarkComponent>(uid);
        comp.VoiceData = voiceData;

        if (hadComp)
            Dirty(uid, comp);
    }

    public List<VoiceBarkData> GenBarkData(VoiceBarkVoiceData data, string text, bool isWhisper) =>
        text.Select(currChar => GenBarkData(data, currChar, isWhisper)).ToList();

    public VoiceBarkData GenBarkData(VoiceBarkVoiceData data, char currChar, bool isWhisper)
    {
        var currBark = new VoiceBarkData(
            data.PitchAverage,
            data.VolumeAverage,
            data.PauseAverage);

        if (SkipChars.Contains(currChar))
            currBark.Enabled = false;

        if (LongPauseChars.Contains(currChar))
        {
            currBark.Pause *= 1.2f;
            currBark.Enabled = false;
        }

        if (isWhisper)
            currBark.Volume -= SharedAudioSystem.GainToVolume(4f);

        if (Soglasnoy.Contains(currChar))
        {
            currBark.Pitch -= 0.2f;
            currBark.Volume -= SharedAudioSystem.GainToVolume(4f);
            currBark.Pause *= 0.8f;
        }

        currBark.Pitch += System.Random.Shared.NextFloat(-data.PitchVariance, data.PitchVariance);

        return currBark;
    }

    public void Bark(Entity<VoiceBarkComponent> entity, string text, bool isWhisper)
    {
        var ev = new TransformSpeakerVoiceBarkEvent(entity, entity.Comp.VoiceData);
        RaiseLocalEvent(entity, ev);
        Bark(entity, GenBarkData(ev.VoiceData, text, isWhisper));
    }

    public abstract void Bark(Entity<VoiceBarkComponent> entity, List<VoiceBarkData> barks);
}

/// <summary>
/// Lets worn clothing (masks, helmets, voice changers) intercept/modify the
/// bark voice data before it's used, mirroring how vanilla speech-transform
/// events work.
/// </summary>
public sealed class TransformSpeakerVoiceBarkEvent(EntityUid sender, VoiceBarkVoiceData voiceData) : EntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags TargetSlots => SlotFlags.WITHOUT_POCKET;

    public EntityUid Sender = sender;
    public VoiceBarkVoiceData VoiceData = voiceData;
}
