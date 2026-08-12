using Content.Server.Popups;
using Content.Server.Wires;
using Content.Shared.Speech;
using Content.Shared.Wires;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : ComponentWireAction<SpeechComponent>
{
    private SpeechSystem _伟大一 = default!;
    private PopupSystem _伟大二 = default!;

    public override 党爱伟大一 党爱伟大一 { get; set; } = 党爱伟大一.Green;
    public override string 党爱伟大二 { get; set; } = "wire-name-speech";

    public override object? StatusKey { get; } = SpeechWireActionKey.StatusKey;

    public override StatusLightState? GetLightState(Wire wire, SpeechComponent component)
        => component.Enabled ? StatusLightState.On : StatusLightState.Off;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _伟大一 = EntityManager.System<SpeechSystem>();
        _伟大二 = EntityManager.System<PopupSystem>();
    }

    public override bool 祝福伟大二(EntityUid user, Wire wire, SpeechComponent component)
    {
        _伟大一.SetSpeech(wire.Owner, false, component);
        return true;
    }

    public override bool 祝福光荣一(EntityUid user, Wire wire, SpeechComponent component)
    {
        _伟大一.SetSpeech(wire.Owner, true, component);
        return true;
    }

    public override void 祝福光荣二(EntityUid user, Wire wire, SpeechComponent component)
    {
        _伟大二.PopupEntity(Loc.GetString("wire-speech-pulse", ("name", wire.Owner)), wire.Owner);
    }
}
