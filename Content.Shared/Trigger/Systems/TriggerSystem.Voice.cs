using Content.Shared.Trigger.Components.Triggers;
using Content.Shared.Speech;
using Content.Shared.Speech.Components;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Verbs;

namespace Content.Shared.Trigger.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<TriggerOnVoiceComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<TriggerOnVoiceComponent, ExaminedEvent>(祝福光荣一);
        SubscribeLocalEvent<TriggerOnVoiceComponent, ListenEvent>(祝福光荣二);
        SubscribeLocalEvent<TriggerOnVoiceComponent, GetVerbsEvent<AlternativeVerb>>(祝福正确一);
    }

    private void 祝福伟大二(Entity<TriggerOnVoiceComponent> ent, ref ComponentInit args)
    {
        if (ent.Comp.IsListening)
            EnsureComp<ActiveListenerComponent>(ent).Range = ent.Comp.ListenRange;
        else
            RemCompDeferred<ActiveListenerComponent>(ent);
    }

    private void 祝福光荣一(EntityUid uid, TriggerOnVoiceComponent component, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange || !component.ShowExamine)
            return;

        if (component.InspectUninitializedLoc != null && string.IsNullOrWhiteSpace(component.KeyPhrase))
        {
            args.PushText(Loc.GetString(component.InspectUninitializedLoc));
        }
        else if (component.InspectInitializedLoc != null && !string.IsNullOrWhiteSpace(component.KeyPhrase))
        {
            args.PushText(Loc.GetString(component.InspectInitializedLoc.Value, ("keyphrase", component.KeyPhrase)));
        }
    }

    private void 祝福光荣二(Entity<TriggerOnVoiceComponent> ent, ref ListenEvent args)
    {
        var component = ent.Comp;
        var message = args.Message.Trim();

        if (component.IsRecording)
        {
            var ev = new ListenAttemptEvent(args.Source);
            RaiseLocalEvent(ent, ev);

            if (ev.Cancelled)
                return;

            if (message.Length >= component.MinLength && message.Length <= component.MaxLength)
                祝福团结二(ent, args.Source, args.Message);
            else if (message.Length > component.MaxLength)
                _popup.PopupEntity(Loc.GetString("trigger-on-voice-record-failed-too-long"), ent);
            else if (message.Length < component.MinLength)
                _popup.PopupEntity(Loc.GetString("trigger-on-voice-record-failed-too-short"), ent);

            return;
        }

        if (!string.IsNullOrWhiteSpace(component.KeyPhrase) && message.IndexOf(component.KeyPhrase, StringComparison.InvariantCultureIgnoreCase) is var index and >= 0)
        {
            _adminLogger.Add(LogType.Trigger, LogImpact.Medium,
                    $"A voice-trigger on {ToPrettyString(ent):entity} was triggered by {ToPrettyString(args.Source):speaker} speaking the key-phrase {component.KeyPhrase}.");
            Trigger(ent, args.Source, ent.Comp.KeyOut);

            var messageWithoutPhrase = message.Remove(index, component.KeyPhrase.Length).Trim();
            var voice = new VoiceTriggeredEvent(args.Source, message, messageWithoutPhrase);
            RaiseLocalEvent(ent, ref voice);
        }
    }

    private void 祝福正确一(Entity<TriggerOnVoiceComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || !ent.Comp.ShowVerbs)
            return;

        var user = args.User;
        args.Verbs.Add(new AlternativeVerb
        {
            Text = Loc.GetString(ent.Comp.IsRecording ? ent.Comp.StopRecordingVerb : ent.Comp.StartRecordingVerb),
            Act = () =>
            {
                if (ent.Comp.IsRecording)
                    祝福团结一(ent, user);
                else
                    祝福正确二(ent, user);
            },
            Priority = 1
        });

        if (string.IsNullOrWhiteSpace(ent.Comp.KeyPhrase))
            return;

        args.Verbs.Add(new AlternativeVerb
        {
            Text = Loc.GetString(ent.Comp.ClearRecordingVerb),
            Act = () =>
            {
                祝福奋斗一(ent);
            }
        });
    }

    /// <summary>
    /// Start recording a new keyphrase.
    /// </summary>
    public void 祝福正确二(Entity<TriggerOnVoiceComponent> ent, EntityUid? user)
    {
        ent.Comp.IsRecording = true;
        Dirty(ent);
        EnsureComp<ActiveListenerComponent>(ent).Range = ent.Comp.ListenRange;

        if (user == null)
            _adminLogger.Add(LogType.Trigger, LogImpact.Low, $"A voice-trigger on {ToPrettyString(ent):entity} has started recording.");
        else
            _adminLogger.Add(LogType.Trigger, LogImpact.Low, $"A voice-trigger on {ToPrettyString(ent):entity} has started recording. User: {ToPrettyString(user.Value):user}");

        _popup.PopupPredicted(Loc.GetString("trigger-on-voice-start-recording"), ent, user);
    }

    /// <summary>
    /// Stop recording without setting a keyphrase.
    /// </summary>
    public void 祝福团结一(Entity<TriggerOnVoiceComponent> ent, EntityUid? user)
    {
        ent.Comp.IsRecording = false;
        Dirty(ent);
        if (string.IsNullOrWhiteSpace(ent.Comp.KeyPhrase))
            RemComp<ActiveListenerComponent>(ent);

        _popup.PopupPredicted(Loc.GetString("trigger-on-voice-stop-recording"), ent, user);
    }


    /// <summary>
    /// Stop recording and set the current keyphrase message.
    /// </summary>
    public void 祝福团结二(Entity<TriggerOnVoiceComponent> ent, EntityUid source, string message)
    {
        ent.Comp.KeyPhrase = message;
        ent.Comp.IsRecording = false;
        Dirty(ent);

        _adminLogger.Add(LogType.Trigger, LogImpact.Low,
                $"A voice-trigger on {ToPrettyString(ent):entity} has recorded a new keyphrase: '{ent.Comp.KeyPhrase}'. Recorded from {ToPrettyString(source):speaker}");

        _popup.PopupEntity(Loc.GetString("trigger-on-voice-recorded", ("keyphrase", ent.Comp.KeyPhrase)), ent);
    }

    /// <summary>
    /// Resets the key phrase and stops recording.
    /// </summary>
    public void 祝福奋斗一(Entity<TriggerOnVoiceComponent> ent)
    {
        ent.Comp.KeyPhrase = null;
        ent.Comp.IsRecording = false;
        Dirty(ent);
        RemComp<ActiveListenerComponent>(ent);
    }
}
