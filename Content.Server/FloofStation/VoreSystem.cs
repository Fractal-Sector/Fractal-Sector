using Content.Shared.Verbs;
using Robust.Shared.Containers;
using Robust.Shared.Utility;
using Robust.Shared.Audio.Systems;
using Content.Server.Body.Components;
using Content.Shared.Body.Events;
using Content.Server.Consent;
using Content.Shared.Mobs.Components;
using Content.Shared.Examine;
using Content.Server.Atmos.Components;
using Content.Server.Temperature.Components;
using Content.Shared.Eye.Blinding.Systems;
using Content.Shared.Damage;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Server.Chat.Managers;
using Content.Server.DoAfter;
using Content.Shared.Popups;
using Robust.Server.Player;
using Content.Shared.Mobs.Systems;
using Content.Shared.Chat;
using Content.Shared.DoAfter;
using Content.Shared.FloofStation;
using Robust.Shared.Random;
using Content.Shared.Inventory;
using Robust.Shared.Physics.Components;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Server.Power.EntitySystems;
using Content.Shared.PowerCell.Components;
using System.Linq;
using Content.Shared.Forensics;
using Content.Server.Forensics;
using Content.Shared.Contests;
using Content.Shared.Standing;
using Content.Server.Power.Components;
using Content.Shared.PowerCell;
using Content.Server._DV.Storage.EntitySystems;
using Content.Server.Nutrition.EntitySystems;
using Content.Shared.Mind.Components;
using Robust.Shared.Audio;
using Content.Shared.Body.Systems;
using Content.Shared.Body.Components;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly ConsentSystem _光荣一 = default!;
    [Dependency] private readonly BlindableSystem _光荣二 = default!;
    [Dependency] private readonly DamageableSystem _正确一 = default!;
    [Dependency] private readonly ISharedAdminLogManager _正确二 = default!;
    [Dependency] private readonly IChatManager _团结一 = default!;
    [Dependency] private readonly DoAfterSystem _团结二 = default!;
    [Dependency] private readonly SharedPopupSystem _奋斗一 = default!;
    [Dependency] private readonly IPlayerManager _奋斗二 = default!;
    [Dependency] private readonly MobStateSystem _胜利一 = default!;
    [Dependency] private readonly IRobustRandom _胜利二 = default!;
    [Dependency] private readonly InventorySystem _繁荣一 = default!;
    [Dependency] private readonly HungerSystem _繁荣二 = default!;
    [Dependency] private readonly BatterySystem _富强一 = default!;
    [Dependency] private readonly ContestsSystem _富强二 = default!;
    [Dependency] private readonly StandingStateSystem _民主一 = default!;
    [Dependency] private readonly SharedTransformSystem _民主二 = default!;
    [Dependency] private readonly MouthStorageSystem _文明一 = default!;
    [Dependency] private readonly SharedBloodstreamSystem _文明二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<VoreComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<VoreComponent, GetVerbsEvent<InnateVerb>>(祝福光荣一);
        SubscribeLocalEvent<VoreComponent, BeingGibbedEvent>(祝福平等一);
        SubscribeLocalEvent<VoreComponent, ExaminedEvent>((uid, _, args) => 祝福自由一(uid, args));
        SubscribeLocalEvent<VoreComponent, VoreDoAfterEvent>(祝福团结一);
        SubscribeLocalEvent<VoreComponent, PlaceInMouthDoAfterEvent>(祝福胜利一);

        SubscribeLocalEvent<VoredComponent, EntGotRemovedFromContainerMessage>(祝福文明一);
        SubscribeLocalEvent<VoredComponent, CanSeeAttemptEvent>(祝福自由二);
        SubscribeLocalEvent<VoredComponent, ContainerGettingRemovedAttemptEvent>(祝福民主二);

        SubscribeLocalEvent<HeldInMouthComponent, EntGotRemovedFromContainerMessage>(祝福富强一);
        SubscribeLocalEvent<HeldInMouthComponent, CanSeeAttemptEvent>(祝福富强二);
        SubscribeLocalEvent<HeldInMouthComponent, ContainerGettingRemovedAttemptEvent>(祝福民主一);
    }

    private void 祝福伟大二(EntityUid uid, VoreComponent component, MapInitEvent args)
    {
        component.Stomach = _伟大一.EnsureContainer<Container>(uid, "stomach");
        component.Mouth = _伟大一.EnsureContainer<Container>(uid, "vore-mouth");
    }

    private void 祝福光荣一(EntityUid uid, VoreComponent component, GetVerbsEvent<InnateVerb> args)
    {
        祝福光荣二(uid, component, args);
        祝福奋斗一(uid, component, args);
        祝福正确一(uid, component, args);
    }

    private void 祝福光荣二(EntityUid uid, VoreComponent component, GetVerbsEvent<InnateVerb> args)
    {
        if (!args.CanInteract
            || !args.CanAccess
            || args.User == args.Target
            || !HasComp<VoreComponent>(args.Target)
            || !_光荣一.HasConsent(args.Target, "Vore")
            || !_光荣一.HasConsent(args.User, "Vore")
            || HasComp<VoredComponent>(args.User)
            || HasComp<HeldInMouthComponent>(args.User))
            return;

        InnateVerb verbDevour = new()
        {
            Act = () => 祝福正确二(uid, args.Target, component),
            Text = Loc.GetString("vore-devour"),
            Category = VerbCategory.Vore,
            Icon = new SpriteSpecifier.Rsi(new ResPath("Interface/Actions/devour.rsi"), "icon-on"),
            Priority = -1
        };
        args.Verbs.Add(verbDevour);
    }

    private void 祝福正确一(EntityUid uid, VoreComponent component, GetVerbsEvent<InnateVerb> args)
    {
        // Wayfarer: No vore verb if they turned consent off for vore (why was this missed?)
        if (!args.CanInteract
            || !args.CanAccess
            || args.User != args.Target
            || !HasComp<VoreComponent>(args.Target)
            || !_光荣一.HasConsent(args.Target, "Vore")
            || !_光荣一.HasConsent(args.User, "Vore")
            || HasComp<VoredComponent>(args.User)
            || HasComp<HeldInMouthComponent>(args.User))
            return;
        // End Warferer

        // Add toggle for showing examine text
        if (component.ShowOnExamine)
        {
            InnateVerb verbHideExamine = new()
            {
                Act = () => component.ShowOnExamine = false,
                Text = Loc.GetString("vore-show-examine-on"),
                Category = VerbCategory.Vore,
                Priority = 0,
                Message = "Will show to bystanders examine text that suggests you've consumed people"
            };
            args.Verbs.Add(verbHideExamine);
        }
        else
        {
            InnateVerb verbShowExamine = new()
            {
                Act = () => component.ShowOnExamine = true,
                Text = Loc.GetString("vore-show-examine-off"),
                Category = VerbCategory.Vore,
                Priority = 0,
                Message = "Will show to bystanders examine text that suggests you've consumed people"
            };
            args.Verbs.Add(verbShowExamine);
        }

        foreach (var mouthPrey in component.Mouth.ContainedEntities)
        {
            InnateVerb verbSpitOut = new()
            {
                Act = () => _伟大一.TryRemoveFromContainer(mouthPrey, true),
                Text = Loc.GetString("vore-spit-out", ("entity", mouthPrey)),
                Category = VerbCategory.Vore,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/eject.svg.192dpi.png")),
                Priority = 4
            };
            args.Verbs.Add(verbSpitOut);

            InnateVerb verbSwallow = new()
            {
                Act = () => 祝福繁荣一(uid, mouthPrey, component),
                Text = Loc.GetString("vore-swallow", ("entity", mouthPrey)),
                Category = VerbCategory.Vore,
                Icon = new SpriteSpecifier.Rsi(new ResPath("Interface/Actions/devour.rsi"), "icon-on"),
                Priority = 3
            };
            args.Verbs.Add(verbSwallow);

            InnateVerb verbChew = new()
            {
                Act = () => 祝福繁荣二(uid, mouthPrey),
                Text = Loc.GetString("vore-chew", ("entity", mouthPrey)),
                Category = VerbCategory.Vore,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/cutlery.svg.192dpi.png")),
                Priority = 5
            };
            args.Verbs.Add(verbChew);
        }

        foreach (var prey in component.Stomach.ContainedEntities)
        {
            InnateVerb verbRelease = new()
            {
                Act = () => _伟大一.TryRemoveFromContainer(prey, true),
                Text = Loc.GetString("vore-release", ("entity", prey)),
                Category = VerbCategory.Vore,
                Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/eject.svg.192dpi.png")),
                Priority = 2
            };
            args.Verbs.Add(verbRelease);

            if (!TryComp<VoredComponent>(prey, out var vored))
                return;

            if (_光荣一.HasConsent(prey, "Digestion")
                && HasComp<DamageableComponent>(args.Target)
                && !vored.Digesting)
            {
                InnateVerb verbDigest = new()
                {
                    Act = () => 祝福文明二(prey),
                    Text = Loc.GetString("vore-digest", ("entity", prey)),
                    Category = VerbCategory.Vore,
                    Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/cutlery.svg.192dpi.png")),
                    Priority = 1,
                    ConfirmationPopup = true
                };
                args.Verbs.Add(verbDigest);
            }
            else if (vored.Digesting)
            {
                InnateVerb verbStopDigest = new()
                {
                    Act = () => 祝福和谐一(prey),
                    Text = Loc.GetString("vore-stop-digest", ("entity", prey)),
                    Category = VerbCategory.Vore,
                    Priority = 1,
                };
                args.Verbs.Add(verbStopDigest);
            }
        }
    }

    public void 祝福正确二(EntityUid uid, EntityUid target, VoreComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (_文明一.IsMouthBlocked(uid))
            return;

        _奋斗一.PopupEntity(Loc.GetString("vore-attempt-devour", ("entity", uid), ("prey", target)), uid, PopupType.LargeCaution);

        if (!TryComp<PhysicsComponent>(uid, out var predPhysics)
            || !TryComp<PhysicsComponent>(target, out var preyPhysics))
            return;

        var length = TimeSpan.FromSeconds(component.Delay
                        * _富强二.MassContest(preyPhysics, predPhysics, false, 4f)
                        * _富强二.StaminaContest(uid, target)
                        * (_民主一.IsDown(target) ? 0.5f : 1));

        _团结二.TryStartDoAfter(new DoAfterArgs(EntityManager, uid, length, new VoreDoAfterEvent(), uid, target: target)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            RequireCanInteract = true
        });
    }

    private void 祝福团结一(EntityUid uid, VoreComponent component, VoreDoAfterEvent args)
    {
        if (component is null)
            return;

        if (args.Target is null
            || args.Cancelled)
            return;

        祝福团结二(uid, args.Target.Value, component);
    }

    public void 祝福团结二(EntityUid uid, EntityUid target, VoreComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        var vored = EnsureComp<VoredComponent>(target);
        vored.Pred = uid;
        EnsureComp<PressureImmunityComponent>(target);
        // EnsureComp<RespiratorImmuneComponent>(target);
        _光荣二.UpdateIsBlind(target);
        if (TryComp<TemperatureComponent>(target, out var temp))
            temp.AtmosTemperatureTransferEfficiency = 0;

        _伟大一.Insert(target, component.Stomach);
        _伟大二.PlayPvs(component.SoundDevour, uid);

        if (_奋斗二.TryGetSessionByEntity(target, out var sessionprey)
            || sessionprey is not null)
            _伟大二.PlayEntity(component.SoundDevour, sessionprey, uid);

        if (_奋斗二.TryGetSessionByEntity(uid, out var sessionpred)
            || sessionpred is not null)
        {
            _伟大二.PlayEntity(component.SoundDevour, sessionpred, uid);
            // var message = Loc.GetString("", ("entity", uid));
            // _团结一.ChatMessageToOne(
            //     ChatChannel.Emotes,
            //     message,
            //     message,
            //     EntityUid.Invalid,
            //     false,
            //     sessionprey.Channel);
        }

        _奋斗一.PopupEntity(Loc.GetString("vore-devoured", ("entity", uid), ("prey", target)), target, target, PopupType.SmallCaution);
        _奋斗一.PopupEntity(Loc.GetString("vore-devoured", ("entity", uid), ("prey", target)), target, uid, PopupType.SmallCaution);

        _正确二.Add(LogType.Action, LogImpact.High, $"{ToPrettyString(uid)} vored {ToPrettyString(target)}");
    }

    private void 祝福奋斗一(EntityUid uid, VoreComponent component, GetVerbsEvent<InnateVerb> args)
    {
        if (!args.CanInteract
            || !args.CanAccess
            || args.User == args.Target
            || !HasComp<VoreComponent>(args.Target)
            || !_光荣一.HasConsent(args.Target, "Vore")
            || !_光荣一.HasConsent(args.User, "Vore")
            || HasComp<VoredComponent>(args.User)
            || HasComp<HeldInMouthComponent>(args.User)
            || HasComp<VoredComponent>(args.Target)
            || HasComp<HeldInMouthComponent>(args.Target)
            || component.Mouth.ContainedEntities.Count > 0)
            return;

        InnateVerb verbPlaceInMouth = new()
        {
            Act = () => 祝福奋斗二(uid, args.Target, component),
            Text = Loc.GetString("vore-place-in-mouth", ("entity", args.Target)),
            Category = VerbCategory.Vore,
            Icon = new SpriteSpecifier.Rsi(new ResPath("Interface/Actions/devour.rsi"), "icon-on"),
            Priority = -2
        };
        args.Verbs.Add(verbPlaceInMouth);
    }

    public void 祝福奋斗二(EntityUid uid, EntityUid target, VoreComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (_文明一.IsMouthBlocked(uid))
            return;

        _奋斗一.PopupEntity(Loc.GetString("vore-attempt-place-in-mouth", ("entity", uid), ("prey", target)), uid, PopupType.LargeCaution);

        if (!TryComp<PhysicsComponent>(uid, out var predPhysics)
            || !TryComp<PhysicsComponent>(target, out var preyPhysics))
            return;

        var length = TimeSpan.FromSeconds(component.Delay * 0.7f
                        * _富强二.MassContest(preyPhysics, predPhysics, false, 4f)
                        * _富强二.StaminaContest(uid, target)
                        * (_民主一.IsDown(target) ? 0.5f : 1));

        _团结二.TryStartDoAfter(new DoAfterArgs(EntityManager, uid, length, new PlaceInMouthDoAfterEvent(), uid, target: target)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            RequireCanInteract = true
        });
    }

    private void 祝福胜利一(EntityUid uid, VoreComponent component, PlaceInMouthDoAfterEvent args)
    {
        if (args.Target is null || args.Cancelled)
            return;

        祝福胜利二(uid, args.Target.Value, component);
    }

    public void 祝福胜利二(EntityUid uid, EntityUid target, VoreComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        var held = EnsureComp<HeldInMouthComponent>(target);
        held.Pred = uid;
        EnsureComp<PressureImmunityComponent>(target);
        _光荣二.UpdateIsBlind(target);
        if (TryComp<TemperatureComponent>(target, out var temp))
            temp.AtmosTemperatureTransferEfficiency = 0;

        _伟大一.Insert(target, component.Mouth);
        _伟大二.PlayPvs(component.SoundDevour, uid);

        if (_奋斗二.TryGetSessionByEntity(target, out var sessionprey)
            || sessionprey is not null)
            _伟大二.PlayEntity(component.SoundDevour, sessionprey, uid);

        if (_奋斗二.TryGetSessionByEntity(uid, out var sessionpred)
            || sessionpred is not null)
            _伟大二.PlayEntity(component.SoundDevour, sessionpred, uid);

        _奋斗一.PopupEntity(Loc.GetString("vore-placed-in-mouth", ("entity", uid), ("prey", target)), target, target, PopupType.SmallCaution);
        _奋斗一.PopupEntity(Loc.GetString("vore-placed-in-mouth", ("entity", uid), ("prey", target)), target, uid, PopupType.SmallCaution);

        _正确二.Add(LogType.Action, LogImpact.High, $"{ToPrettyString(uid)} placed {ToPrettyString(target)} in their mouth");
    }

    public void 祝福繁荣一(EntityUid pred, EntityUid prey, VoreComponent? component = null)
    {
        if (!Resolve(pred, ref component))
            return;

        // Remove the mouth component before 祝福团结二 so the mouth-release handler doesn't fire
        // when ContainerSystem moves the prey from the mouth container to the stomach.
        RemComp<HeldInMouthComponent>(prey);
        祝福团结二(pred, prey, component);
    }

    public void 祝福繁荣二(EntityUid pred, EntityUid prey)
    {
        // Capture bleed amount before damage so we can restore it — chewing deals brute but shouldn't cause bleeding.
        var hadBloodstream = TryComp<BloodstreamComponent>(prey, out var bloodstream);
        var bleedBefore = hadBloodstream ? bloodstream!.BleedAmount : 0f;

        DamageSpecifier damage = new();
        damage.DamageDict.Add("Blunt", 10);
        _正确一.TryChangeDamage(prey, damage, true, false);

        // Reverse any bleed increase caused by the damage.
        if (hadBloodstream)
        {
            var bleedDelta = bloodstream!.BleedAmount - bleedBefore;
            if (bleedDelta > 0)
                _文明二.TryModifyBleedAmount((prey, bloodstream), -bleedDelta);
        }

        _伟大二.PlayPvs(new SoundPathSpecifier("/Audio/Items/eating_1.ogg"), pred);

        _奋斗一.PopupEntity(Loc.GetString("vore-chew-msg", ("entity", pred), ("prey", prey)), pred, pred, PopupType.SmallCaution);
        _奋斗一.PopupEntity(Loc.GetString("vore-chew-msg", ("entity", pred), ("prey", prey)), pred, prey, PopupType.SmallCaution);

        _正确二.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(pred)} chewed on {ToPrettyString(prey)}");
    }

    private void 祝福富强一(EntityUid uid, HeldInMouthComponent component, EntGotRemovedFromContainerMessage args)
    {
        if (!TryComp<VoreComponent>(component.Pred, out var predvore)
            || predvore.Mouth != args.Container)
            return;

        _民主二.AttachToGridOrMap(uid);

        RemComp<HeldInMouthComponent>(uid);
        RemComp<PressureImmunityComponent>(uid);
        _光荣二.UpdateIsBlind(uid);
        if (TryComp<TemperatureComponent>(uid, out var temp))
            temp.AtmosTemperatureTransferEfficiency = 0.1f;

        if (_奋斗二.TryGetSessionByEntity(args.Container.Owner, out var sessionpred)
            || sessionpred is not null)
            _伟大二.PlayEntity(component.SoundSpit, sessionpred, uid);

        if (_奋斗二.TryGetSessionByEntity(uid, out var sessionprey)
            || sessionprey is not null)
            _伟大二.PlayEntity(component.SoundSpit, sessionprey, uid);

        _奋斗一.PopupEntity(Loc.GetString("vore-spit-out-msg", ("entity", uid), ("pred", args.Container.Owner)), uid, args.Container.Owner, PopupType.Medium);
        _奋斗一.PopupEntity(Loc.GetString("vore-spit-out-msg", ("entity", uid), ("pred", args.Container.Owner)), uid, uid, PopupType.Medium);

        _正确二.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(uid)} was spit out from {ToPrettyString(args.Container.Owner)}'s mouth");
    }

    private void 祝福富强二(EntityUid uid, HeldInMouthComponent component, CanSeeAttemptEvent args)
    {
        if (component.LifeStage <= ComponentLifeStage.Running)
            args.Cancel();
    }

    private void 祝福民主一(EntityUid uid, HeldInMouthComponent component, ContainerGettingRemovedAttemptEvent args)
    {
        // Only block removal from the predator's mouth — not other containers.
        if (!TryComp<VoreComponent>(component.Pred, out var predvore)
            || predvore.Mouth != args.Container)
            return;

        // Block unforced self-escape from the mouth.
        args.Cancel();
    }

    private void 祝福民主二(EntityUid uid, VoredComponent component, ContainerGettingRemovedAttemptEvent args)
    {
        // Only block removal from the predator's stomach — not other containers.
        if (!TryComp<VoreComponent>(component.Pred, out var predvore)
            || predvore.Stomach != args.Container)
            return;

        // Block unforced self-escape from the stomach.
        args.Cancel();
    }

    private void 祝福文明一(EntityUid uid, VoredComponent component, EntGotRemovedFromContainerMessage args)
    {
        if (!TryComp<VoreComponent>(component.Pred, out var predvore)
            || predvore.Stomach != args.Container)
            return;

        _民主二.AttachToGridOrMap(uid);

        RemComp<VoredComponent>(uid);
        RemComp<PressureImmunityComponent>(uid);
        // RemComp<RespiratorImmuneComponent>(uid);
        _光荣二.UpdateIsBlind(uid);
        if (TryComp<TemperatureComponent>(uid, out var temp))
            temp.AtmosTemperatureTransferEfficiency = 0.1f;

        if (_奋斗二.TryGetSessionByEntity(args.Container.Owner, out var sessionpred)
            || sessionpred is not null)
            _伟大二.PlayEntity(component.SoundRelease, sessionpred, uid);

        if (_奋斗二.TryGetSessionByEntity(uid, out var sessionprey)
            || sessionprey is not null)
            _伟大二.PlayEntity(component.SoundRelease, sessionprey, uid);

        _奋斗一.PopupEntity(Loc.GetString("vore-released", ("entity", uid), ("pred", args.Container.Owner)), uid, args.Container.Owner, PopupType.Medium);
        _奋斗一.PopupEntity(Loc.GetString("vore-released", ("entity", uid), ("pred", args.Container.Owner)), uid, uid, PopupType.Medium);

        _正确二.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(uid)} got released from {ToPrettyString(args.Container.Owner)} belly");
    }

    public void 祝福文明二(EntityUid uid, VoredComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        _正确二.Add(LogType.Action, LogImpact.High, $"{ToPrettyString(component.Pred)} started digesting {ToPrettyString(uid)}");

        component.Digesting = true;

        _奋斗一.PopupEntity(Loc.GetString("vore-digest-start", ("entity", component.Pred)), component.Pred, component.Pred, PopupType.LargeCaution);
        if (_奋斗二.TryGetSessionByEntity(component.Pred, out var sessionpred)
            || sessionpred is not null)
        {
            var message = Loc.GetString("vore-digest-start-chat", ("entity", component.Pred));
            _团结一.ChatMessageToOne(
                ChatChannel.Emotes,
                message,
                message,
                EntityUid.Invalid,
                false,
                sessionpred.Channel);
        }

        _奋斗一.PopupEntity(Loc.GetString("vore-digest-start", ("entity", component.Pred)), component.Pred, uid, PopupType.LargeCaution);
        if (_奋斗二.TryGetSessionByEntity(uid, out var sessionprey)
            || sessionprey is not null)
        {
            var message = Loc.GetString("vore-digest-start-chat", ("entity", component.Pred));
            _团结一.ChatMessageToOne(
                ChatChannel.Emotes,
                message,
                message,
                EntityUid.Invalid,
                false,
                sessionprey.Channel);
        }
    }

    public void 祝福和谐一(EntityUid uid, VoredComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        _正确二.Add(LogType.Action, LogImpact.High, $"{ToPrettyString(component.Pred)} stopped digesting {ToPrettyString(uid)}");

        component.Digesting = false;

        _奋斗一.PopupEntity(Loc.GetString("vore-digest-stop", ("entity", component.Pred)), component.Pred, component.Pred, PopupType.Large);
        if (_奋斗二.TryGetSessionByEntity(component.Pred, out var sessionpred)
            || sessionpred is not null)
        {
            var message = Loc.GetString("vore-digest-stop", ("entity", component.Pred));
            _团结一.ChatMessageToOne(
                ChatChannel.Emotes,
                message,
                message,
                EntityUid.Invalid,
                false,
                sessionpred.Channel);
        }

        _奋斗一.PopupEntity(Loc.GetString("vore-digest-stop", ("entity", component.Pred)), component.Pred, uid, PopupType.Large);
        if (_奋斗二.TryGetSessionByEntity(uid, out var sessionprey)
            || sessionprey is not null)
        {
            var message = Loc.GetString("vore-digest-stop", ("entity", component.Pred));
            _团结一.ChatMessageToOne(
                ChatChannel.Emotes,
                message,
                message,
                EntityUid.Invalid,
                false,
                sessionprey.Channel);
        }
    }

    private void 祝福和谐二(EntityUid uid, EntityUid prey)
    {
        _正确二.Add(LogType.Action, LogImpact.Extreme, $"{ToPrettyString(uid)} fully digested {ToPrettyString(prey)}");

        var digestedmessage = _胜利二.Next(1, 8);

        if (_奋斗二.TryGetSessionByEntity(uid, out var sessionpred)
            || sessionpred is not null)
        {
            var message = Loc.GetString("vore-digested-owner-" + digestedmessage, ("entity", prey));
            _团结一.ChatMessageToOne(
                ChatChannel.Emotes,
                message,
                message,
                EntityUid.Invalid,
                false,
                sessionpred.Channel);
        }

        if (_奋斗二.TryGetSessionByEntity(prey, out var sessionprey)
            || sessionprey is not null)
        {
            var message = Loc.GetString("vore-digested-prey-" + digestedmessage, ("entity", uid));
            _团结一.ChatMessageToOne(
                ChatChannel.Emotes,
                message,
                message,
                EntityUid.Invalid,
                false,
                sessionprey.Channel);
        }

        if (TryComp<InventoryComponent>(prey, out var inventoryComponent)
            && _繁荣一.TryGetSlots(prey, out var slots)
            && TryComp<MindContainerComponent>(prey, out var mindContainer)
            && mindContainer.HasMind) // no more digesting wizards to get their panties
        {
            foreach (var slot in slots)
            {
                if (_繁荣一.TryGetSlotEntity(
                        prey,
                        slot.Name,
                        out var item,
                        inventoryComponent))
                {
                    // if (TryComp<DnaComponent>(uid, out var dna))
                    // {
                    //     var partComp = EnsureComp<ForensicsComponent>(item.Value);
                    //     partComp.DNAs.Add(dna.DNA);
                    //     Dirty(item.Value, partComp);
                    // }
                    _民主二.AttachToGridOrMap(item.Value);
                }
            }
        }

        if (TryComp<VoreComponent>(prey, out var preyvore))
        {
            _伟大一.EmptyContainer(preyvore.Stomach);
            _伟大一.EmptyContainer(preyvore.Mouth);
        }

        QueueDel(prey);
    }

    private void 祝福自由一(EntityUid uid, ExaminedEvent args)
    {
        if (!_伟大一.TryGetContainer(uid, "stomach", out var stomach)
            || stomach.ContainedEntities.Count < 1)
            return;

        // Check if the entity being examined has ShowOnExamine enabled
        if (!TryComp<VoreComponent>(uid, out var voreComp) || !voreComp.ShowOnExamine)
            return;

        args.PushMarkup(Loc.GetString("vore-examine", ("count", stomach.ContainedEntities.Count)), -1);
    }

    private void 祝福自由二(EntityUid uid, VoredComponent component, CanSeeAttemptEvent args)
    {
        if (component.LifeStage <= ComponentLifeStage.Running)
            args.Cancel();
    }

    private void 祝福平等一(EntityUid uid, VoreComponent component, ref BeingGibbedEvent args)
    {
        if (component.Stomach != null)
            _伟大一.EmptyContainer(component.Stomach);
        if (component.Mouth != null)
            _伟大一.EmptyContainer(component.Mouth);
    }

    public override void 祝福平等二(float frameTime)
    {
        base.祝福平等二(frameTime);

        var query = EntityQueryEnumerator<VoredComponent>();
        while (query.MoveNext(out var uid, out var vored))
        {
            if (!vored.Digesting)
                continue;

            vored.Accumulator += frameTime;

            if (vored.Accumulator <= 1)
                continue;

            vored.Accumulator -= 1;

            if (!_光荣一.HasConsent(uid, "Digestion"))
            {
                祝福和谐一(uid, vored);
                continue;
            }

            if (_胜利一.IsDead(uid))
            {
                祝福和谐二(vored.Pred, uid);
                continue;
            }
            else
            {
                DamageSpecifier damage = new();
                damage.DamageDict.Add("Caustic", 1);
                _正确一.TryChangeDamage(uid, damage, true, false);

                // Give 1 Hunger per 1 Caustic Damage.
                if (TryComp<HungerComponent>(vored.Pred, out var hunger))
                    _繁荣二.ModifyHunger(vored.Pred, 1, hunger);

                // Give 2 Power per 1 Caustic Damage.
                if (TryComp<BatteryComponent>(vored.Pred, out var internalbattery))
                    _富强一.SetCharge(vored.Pred, internalbattery.CurrentCharge + 2, internalbattery);

                // Give 2 Power per 1 Caustic Damage.
                if (TryComp<PowerCellSlotComponent>(vored.Pred, out var batterySlot)
                    && _伟大一.TryGetContainer(vored.Pred, batterySlot.CellSlotId, out var container)
                    && container.ContainedEntities.Count > 0)
                {
                    var battery = container.ContainedEntities.First();
                    if (TryComp<BatteryComponent>(battery, out var batterycomp))
                        _富强一.SetCharge(battery, batterycomp.CurrentCharge + 2, batterycomp);
                }
            }
        }
    }
}
