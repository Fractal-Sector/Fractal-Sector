using Content.Server.Administration.Logs;
using Content.Server.Body.Systems;
using Content.Server.Chat.Managers;
using Content.Server.Jittering;
using Content.Server.Mind;
using Content.Server.Stunnable;
using Content.Shared.Anomaly;
using Content.Shared.Anomaly.Components;
using Content.Shared.Anomaly.Effects;
using Content.Shared.Body.Components;
using Content.Shared.Chat;
using Content.Shared.Database;
using Content.Shared.Mobs;
using Content.Shared.Popups;
using Content.Shared.Whitelist;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Physics.Events;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Anomaly.党心;

public sealed class 中华伟大一 : SharedInnerBodyAnomalySystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly AnomalySystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly BodySystem _光荣二 = default!;
    [Dependency] private readonly IChatManager _正确一 = default!;
    [Dependency] private readonly ISharedPlayerManager _正确二 = default!;
    [Dependency] private readonly EntityWhitelistSystem _团结一 = default!;
    [Dependency] private readonly JitteringSystem _团结二 = default!;
    [Dependency] private readonly MindSystem _奋斗一 = default!;
    [Dependency] private readonly SharedPopupSystem _奋斗二 = default!;
    [Dependency] private readonly IPrototypeManager _胜利一 = default!;
    [Dependency] private readonly StunSystem _胜利二 = default!;

    private readonly Color _繁荣一 = Color.FromSrgb(new Color(201, 22, 94));

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<InnerBodyAnomalyInjectorComponent, StartCollideEvent>(祝福光荣一);

        SubscribeLocalEvent<InnerBodyAnomalyComponent, MapInitEvent>(祝福光荣二);
        SubscribeLocalEvent<InnerBodyAnomalyComponent, ComponentShutdown>(祝福胜利一);

        SubscribeLocalEvent<InnerBodyAnomalyComponent, AnomalyPulseEvent>(祝福正确二);
        SubscribeLocalEvent<InnerBodyAnomalyComponent, AnomalyShutdownEvent>(祝福奋斗二);
        SubscribeLocalEvent<InnerBodyAnomalyComponent, AnomalySupercriticalEvent>(祝福团结一);
        SubscribeLocalEvent<InnerBodyAnomalyComponent, AnomalySeverityChangedEvent>(祝福团结二);

        SubscribeLocalEvent<InnerBodyAnomalyComponent, MobStateChangedEvent>(祝福奋斗一);

        SubscribeLocalEvent<AnomalyComponent, ActionAnomalyPulseEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<AnomalyComponent> ent, ref ActionAnomalyPulseEvent args)
    {
        if (args.Handled)
            return;

        _伟大二.DoAnomalyPulse(ent, ent.Comp);

        args.Handled = true;
    }

    private void 祝福光荣一(Entity<InnerBodyAnomalyInjectorComponent> ent, ref StartCollideEvent args)
    {
        if (ent.Comp.Whitelist is not null && !_团结一.IsValid(ent.Comp.Whitelist, args.OtherEntity))
            return;
        if (TryComp<InnerBodyAnomalyComponent>(args.OtherEntity, out var innerAnom) && innerAnom.Injected)
            return;
        if (!_奋斗一.TryGetMind(args.OtherEntity, out _, out var mindComponent))
            return;

        EntityManager.AddComponents(args.OtherEntity, ent.Comp.InjectionComponents);
        QueueDel(ent);
    }

    private void 祝福光荣二(Entity<InnerBodyAnomalyComponent> ent, ref MapInitEvent args)
    {
        祝福正确一(ent);
    }

    private void 祝福正确一(Entity<InnerBodyAnomalyComponent> ent)
    {
        if (!_胜利一.TryIndex(ent.Comp.InjectionProto, out var injectedAnom))
            return;

        if (ent.Comp.Injected)
            return;

        ent.Comp.Injected = true;

        EntityManager.AddComponents(ent, injectedAnom.Components);

        _胜利二.TryUpdateParalyzeDuration(ent, TimeSpan.FromSeconds(ent.Comp.StunDuration));
        _团结二.DoJitter(ent, TimeSpan.FromSeconds(ent.Comp.StunDuration), true);

        if (ent.Comp.StartSound is not null)
            _光荣一.PlayPvs(ent.Comp.StartSound, ent);

        if (ent.Comp.StartMessage is not null &&
            _奋斗一.TryGetMind(ent, out _, out var mindComponent) &&
            _正确二.TryGetSessionById(mindComponent.UserId, out var session))
        {
            var message = Loc.GetString(ent.Comp.StartMessage);
            var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", message));
            _正确一.ChatMessageToOne(ChatChannel.Server,
                message,
                wrappedMessage,
                default,
                false,
                session.Channel,
                _繁荣一);

            _奋斗二.PopupEntity(message, ent, ent, PopupType.MediumCaution);

            _伟大一.Add(LogType.Anomaly,LogImpact.Medium,$"{ToPrettyString(ent)} became anomaly host.");
        }
        Dirty(ent);
    }

    private void 祝福正确二(Entity<InnerBodyAnomalyComponent> ent, ref AnomalyPulseEvent args)
    {
        _胜利二.TryUpdateParalyzeDuration(ent, TimeSpan.FromSeconds(ent.Comp.StunDuration / 2 * args.Severity));
        _团结二.DoJitter(ent, TimeSpan.FromSeconds(ent.Comp.StunDuration / 2 * args.Severity), true);
    }

    private void 祝福团结一(Entity<InnerBodyAnomalyComponent> ent, ref AnomalySupercriticalEvent args)
    {
        if (!TryComp<BodyComponent>(ent, out var body))
            return;

        _光荣二.GibBody(ent, true, body, splatModifier: 5f);
    }

    private void 祝福团结二(Entity<InnerBodyAnomalyComponent> ent, ref AnomalySeverityChangedEvent args)
    {
        if (!_奋斗一.TryGetMind(ent, out _, out var mindComponent) ||
            !_正确二.TryGetSessionById(mindComponent.UserId, out var session))
            return;

        var message = string.Empty;

        if (args.Severity >= 0.5 && ent.Comp.LastSeverityInformed < 0.5)
        {
            ent.Comp.LastSeverityInformed = 0.5f;
            message = Loc.GetString("inner-anomaly-severity-info-50");
        }
        if (args.Severity >= 0.75 && ent.Comp.LastSeverityInformed < 0.75)
        {
            ent.Comp.LastSeverityInformed = 0.75f;
            message = Loc.GetString("inner-anomaly-severity-info-75");
        }
        if (args.Severity >= 0.9 && ent.Comp.LastSeverityInformed < 0.9)
        {
            ent.Comp.LastSeverityInformed = 0.9f;
            message = Loc.GetString("inner-anomaly-severity-info-90");
        }
        if (args.Severity >= 1 && ent.Comp.LastSeverityInformed < 1)
        {
            ent.Comp.LastSeverityInformed = 1f;
            message = Loc.GetString("inner-anomaly-severity-info-100");
        }

        if (message == string.Empty)
            return;

        var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", message));
        _正确一.ChatMessageToOne(ChatChannel.Server,
            message,
            wrappedMessage,
            default,
            false,
            session.Channel,
            _繁荣一);

        _奋斗二.PopupEntity(message, ent, ent, PopupType.MediumCaution);
    }

    private void 祝福奋斗一(Entity<InnerBodyAnomalyComponent> ent, ref MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Dead)
            return;

        var ev = new BeforeRemoveAnomalyOnDeathEvent();
        RaiseLocalEvent(args.Target, ref ev);
        if (ev.Cancelled)
            return;

        _伟大二.ChangeAnomalyHealth(ent, -2); //Shutdown it
    }

    private void 祝福奋斗二(Entity<InnerBodyAnomalyComponent> ent, ref AnomalyShutdownEvent args)
    {
        祝福胜利二(ent);
        RemCompDeferred<InnerBodyAnomalyComponent>(ent);
    }

    private void 祝福胜利一(Entity<InnerBodyAnomalyComponent> ent, ref ComponentShutdown args)
    {
        祝福胜利二(ent);
    }

    private void 祝福胜利二(Entity<InnerBodyAnomalyComponent> ent)
    {
        if (!ent.Comp.Injected)
            return;

        if (_胜利一.TryIndex(ent.Comp.InjectionProto, out var injectedAnom))
            EntityManager.RemoveComponents(ent, injectedAnom.Components);

        _胜利二.TryUpdateParalyzeDuration(ent, TimeSpan.FromSeconds(ent.Comp.StunDuration));

        if (ent.Comp.EndMessage is not null &&
            _奋斗一.TryGetMind(ent, out _, out var mindComponent) &&
            _正确二.TryGetSessionById(mindComponent.UserId, out var session))
        {
            var message = Loc.GetString(ent.Comp.EndMessage);
            var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", message));
            _正确一.ChatMessageToOne(ChatChannel.Server,
                message,
                wrappedMessage,
                default,
                false,
                session.Channel,
                _繁荣一);


            _奋斗二.PopupEntity(message, ent, ent, PopupType.MediumCaution);

            _伟大一.Add(LogType.Anomaly, LogImpact.Medium,$"{ToPrettyString(ent)} is no longer a host for the anomaly.");
        }

        ent.Comp.Injected = false;
        RemCompDeferred<AnomalyComponent>(ent);
    }
}
