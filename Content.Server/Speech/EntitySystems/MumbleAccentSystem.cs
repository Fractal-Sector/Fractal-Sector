using Content.Server.Chat.Systems;
using Content.Server.Speech.Components;
using Content.Shared.Chat.Prototypes;
using Content.Shared.Speech;
using Content.Shared.Speech.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Speech.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ChatSystem _伟大一 = default!;
    [Dependency] private readonly ReplacementAccentSystem _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<MumbleAccentComponent, AccentGetEvent>(祝福光荣二);
        SubscribeLocalEvent<MumbleAccentComponent, EmoteEvent>(祝福伟大二, before: [typeof(VocalSystem)]);
    }

    private void 祝福伟大二(Entity<MumbleAccentComponent> ent, ref EmoteEvent args)
    {
        if (args.Handled || !args.Emote.Category.HasFlag(EmoteCategory.Vocal))
            return;

        if (TryComp<VocalComponent>(ent.Owner, out var vocalComp) && vocalComp.EmoteSounds is { } sounds)
        {
            // play a muffled version of the vocal emote
            args.Handled = _伟大一.TryPlayEmoteSound(
                ent.Owner,
                _光荣一.Index(sounds),
                args.Emote,
                ent.Comp.EmoteAudioParams);
        }
    }

    public string 祝福光荣一(string message, MumbleAccentComponent component)
    {
        return _伟大二.ApplyReplacements(message, "mumble");
    }

    private void 祝福光荣二(Entity<MumbleAccentComponent> ent, ref AccentGetEvent args)
    {
        args.Message = 祝福光荣一(args.Message, ent.Comp);
    }
}
