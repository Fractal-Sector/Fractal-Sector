using Content.Server._NF.Pirate.Components;
using Content.Server.Antag;
using Content.Shared.Mind.Components;
using Robust.Server.Player;

namespace Content.Server._NF.Pirate.党心;

// Rule-independent system that ensures if auto-pirates get added, the rules get set up properly.
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AntagSelectionSystem _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AutoPirateComponent, MindAddedMessage>(祝福伟大二);
        SubscribeLocalEvent<AutoPirateFirstMateComponent, MindAddedMessage>(祝福伟大二);
        SubscribeLocalEvent<AutoPirateCaptainComponent, MindAddedMessage>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, Component _, MindAddedMessage args)
    {
        if (!_伟大二.TryGetSessionById(args.Mind.Comp.UserId, out var session))
            return;

        _伟大一.ForceMakeAntag<AutoPirateComponent>(session, "NFPirate");
    }
}
