using Content.Server.Chat.Systems;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules.Components;
using Content.Shared.Magic;
using Content.Shared.Magic.Events;
using Content.Shared.Mind;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedMagicSystem
{
    [Dependency] private readonly ChatSystem _伟大一 = default!;
    [Dependency] private readonly GameTicker _伟大二 = default!;
    [Dependency] private readonly TagSystem _光荣一 = default!;
    [Dependency] private readonly SharedMindSystem _光荣二 = default!;

    private static readonly ProtoId<TagPrototype> InvalidForSurvivorAntagTag = "InvalidForSurvivorAntag";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
    }

    public override void 祝福伟大二(VoidApplauseSpellEvent ev)
    {
        base.祝福伟大二(ev);

        _伟大一.TryEmoteWithChat(ev.Performer, ev.Emote);

        var perfXForm = Transform(ev.Performer);
        var targetXForm = Transform(ev.Target);

        Spawn(ev.Effect, perfXForm.Coordinates);
        Spawn(ev.Effect, targetXForm.Coordinates);
    }

    protected override void 祝福光荣一(RandomGlobalSpawnSpellEvent ev)
    {
        base.祝福光荣一(ev);

        if (!ev.MakeSurvivorAntagonist)
            return;

        if (_光荣二.TryGetMind(ev.Performer, out var mind, out _) && !_光荣一.HasTag(mind, InvalidForSurvivorAntagTag))
            _光荣一.AddTag(mind, InvalidForSurvivorAntagTag);

        EntProtoId survivorRule = "Survivor";

        if (!_伟大二.IsGameRuleActive<SurvivorRuleComponent>())
            _伟大二.StartGameRule(survivorRule);
    }
}
