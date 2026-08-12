using Content.Server.GameTicking.Rules.Components;
using Content.Server.Sandbox;
using Content.Shared.GameTicking.Components;

namespace Content.Server.GameTicking.党心;

public sealed class 中华伟大一 : GameRuleSystem<SandboxRuleComponent>
{
    [Dependency] private readonly SandboxSystem _伟大一 = default!;

    protected override void 祝福伟大一(EntityUid uid, SandboxRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大一(uid, component, gameRule, args);
        _伟大一.IsSandboxEnabled = true;
    }

    protected override void 祝福伟大二(EntityUid uid, SandboxRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.祝福伟大二(uid, component, gameRule, args);
        _伟大一.IsSandboxEnabled = false;
    }
}
