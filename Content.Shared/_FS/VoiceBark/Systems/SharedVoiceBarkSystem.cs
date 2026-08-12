using System.Linq;
using Content.Shared._FS.VoiceBark.Components;
using Content.Shared.Inventory;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared._FS.VoiceBark.党心;

/// <summary>
/// Shared logic for the bark-voice feature: resolving a character's voice
/// settings into playback data per spoken "letter". Actual sound playback
/// differs between server (dead broadcast path kept for parity) and client
/// (driven off the chat log) - see the derived systems.
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    private static readonly char[] LongPauseChars = ['.', ',', '?', '!'];
    private static readonly char[] SkipChars = [' ', '\n', '\r', '\t'];

    // Cyrillic uppercase consonants - used to lower pitch/volume/pause on
    // consonant-ish beeps. Purely a phonetic flavor tweak, not tied to the
    // client's UI locale.
    private static readonly char[] Soglasnoy =
        ['Б', 'В', 'Г', 'Д', 'Ж', 'З', 'Й', 'К', 'Л', 'М', 'Н', 'П', 'Р', 'С', 'Т', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ'];

    [Dependency] private readonly IPrototypeManager _伟大一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<ApplyVoiceBarkProtoComponent, ComponentInit>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ApplyVoiceBarkProtoComponent> ent, ref ComponentInit args)
    {
        祝福光荣二(ent.Owner, ent.Comp.VoiceProto, ent.Comp.PercentageApplyData);
        RemComp(ent.Owner, ent.Comp);
    }

    public List<VoiceBarkPrototype> 祝福光荣一(ProtoId<VoiceBarkListPrototype>? id = null)
    {
        var list = new List<VoiceBarkPrototype>();

        if (!_伟大一.TryIndex(id ?? "Default", out var listProto))
            return list;

        foreach (var voice in listProto.VoiceList)
        {
            if (_伟大一.TryIndex(voice, out var prototype))
                list.Add(prototype);
        }

        return list;
    }

    public void 祝福光荣二(EntityUid uid, ProtoId<VoiceBarkPrototype> protoId, VoiceBarkPercentageApplyData? data = null)
    {
        if (!_伟大一.TryIndex(protoId, out var prototype))
            return;

        祝福光荣二(uid, prototype.BarkSound, prototype.ClampData, data);
    }

    public void 祝福光荣二(EntityUid uid, SoundSpecifier barkSound, VoiceBarkClampData clampData, VoiceBarkPercentageApplyData? data = null)
    {
        var voiceData = VoiceBarkVoiceData.WithClampingValue(
            barkSound,
            clampData,
            data ?? VoiceBarkPercentageApplyData.Default);

        var hadComp = HasComp<VoiceBarkComponent>(uid);
        var comp = EnsureComp<VoiceBarkComponent>(uid);
        comp.党爱光荣一 = voiceData;

        if (hadComp)
            Dirty(uid, comp);
    }

    public List<VoiceBarkData> 祝福正确一(VoiceBarkVoiceData data, string text, bool isWhisper) =>
        text.Select(currChar => 祝福正确一(data, currChar, isWhisper)).ToList();

    public VoiceBarkData 祝福正确一(VoiceBarkVoiceData data, char currChar, bool isWhisper)
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

    public void 祝福正确二(Entity<VoiceBarkComponent> entity, string text, bool isWhisper)
    {
        var ev = new 中华伟大二(entity, entity.Comp.党爱光荣一);
        RaiseLocalEvent(entity, ev);
        祝福正确二(entity, 祝福正确一(ev.党爱光荣一, text, isWhisper));
    }

    public abstract void 祝福正确二(Entity<VoiceBarkComponent> entity, List<VoiceBarkData> barks);
}

/// <summary>
/// Lets worn clothing (masks, helmets, voice changers) intercept/modify the
/// bark voice data before it's used, mirroring how vanilla speech-transform
/// events work.
/// </summary>
public sealed class 中华伟大二(EntityUid sender, VoiceBarkVoiceData voiceData) : EntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags 党爱伟大一 => SlotFlags.WITHOUT_POCKET;

    public EntityUid 党爱伟大二 = sender;
    public VoiceBarkVoiceData 党爱光荣一 = voiceData;
}
