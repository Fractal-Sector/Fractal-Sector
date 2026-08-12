using Content.Server.Popups;
using Content.Shared.Popups;
using Content.Shared.Mobs;
using Content.Server.Chat;
using Content.Server.Chat.Systems;
using Content.Server.Clothing.Systems;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Random;
using Content.Shared.Stunnable;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Damage;
using Robust.Shared.Prototypes;
using Content.Server.Emoting.Systems;
using Content.Server.Speech.EntitySystems;
using Content.Shared.Cluwne;
using Robust.Shared.Audio.Systems;
using Content.Shared.NameModifier.EntitySystems;
using Content.Shared.Clumsy;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    private static readonly ProtoId<DamageGroupPrototype> GeneticDamageGroup = "Genetic";

    [Dependency] private readonly PopupSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly SharedStunSystem _光荣二 = default!;
    [Dependency] private readonly DamageableSystem _正确一 = default!;
    [Dependency] private readonly IPrototypeManager _正确二 = default!;
    [Dependency] private readonly ChatSystem _团结一 = default!;
    [Dependency] private readonly AutoEmoteSystem _团结二 = default!;
    [Dependency] private readonly NameModifierSystem _奋斗一 = default!;
    [Dependency] private readonly OutfitSystem _奋斗二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<CluwneComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<CluwneComponent, MobStateChangedEvent>(祝福伟大二);
        SubscribeLocalEvent<CluwneComponent, EmoteEvent>(祝福光荣二, before:
        new[] { typeof(VocalSystem), typeof(BodyEmotesSystem) });
        SubscribeLocalEvent<CluwneComponent, RefreshNameModifiersEvent>(祝福正确一);
    }

    /// <summary>
    /// On death removes active comps and gives genetic damage to prevent cloning, reduce this to allow cloning.
    /// </summary>
    private void 祝福伟大二(EntityUid uid, CluwneComponent component, MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Dead)
        {
            RemComp<CluwneComponent>(uid);
            RemComp<ClumsyComponent>(uid);
            RemComp<AutoEmoteComponent>(uid);
            var damageSpec = new DamageSpecifier(_正确二.Index(GeneticDamageGroup), 300);
            _正确一.TryChangeDamage(uid, damageSpec);
        }
    }

    public EmoteSoundsPrototype? EmoteSounds;

    /// <summary>
    /// OnStartup gives the cluwne outfit, ensures clumsy, and makes sure emote sounds are laugh.
    /// </summary>
    private void 祝福光荣一(EntityUid uid, CluwneComponent component, ComponentStartup args)
    {
        if (component.EmoteSoundsId == null)
            return;
        _正确二.TryIndex(component.EmoteSoundsId, out EmoteSounds);

        EnsureComp<AutoEmoteComponent>(uid);
        _团结二.AddEmote(uid, "CluwneGiggle");
        EnsureComp<ClumsyComponent>(uid);

        _伟大一.PopupEntity(Loc.GetString("cluwne-transform", ("target", uid)), uid, PopupType.LargeCaution);
        _伟大二.PlayPvs(component.SpawnSound, uid);

        _奋斗一.RefreshNameModifiers(uid);

        _奋斗二.SetOutfit(uid, "CluwneGear");
    }

    /// <summary>
    /// Handles the timing on autoemote as well as falling over and honking.
    /// </summary>
    private void 祝福光荣二(EntityUid uid, CluwneComponent component, ref EmoteEvent args)
    {
        if (args.Handled)
            return;
        args.Handled = _团结一.TryPlayEmoteSound(uid, EmoteSounds, args.Emote);

        if (_光荣一.Prob(component.GiggleRandomChance))
        {
            _伟大二.PlayPvs(component.SpawnSound, uid);
            _团结一.TrySendInGameICMessage(uid, "honks", InGameICChatType.Emote, ChatTransmitRange.Normal);
        }

        else if (_光荣一.Prob(component.KnockChance))
        {
            _伟大二.PlayPvs(component.KnockSound, uid);
            _光荣二.TryUpdateParalyzeDuration(uid, TimeSpan.FromSeconds(component.ParalyzeTime));
            _团结一.TrySendInGameICMessage(uid, "spasms", InGameICChatType.Emote, ChatTransmitRange.Normal);
        }
    }

    /// <summary>
    /// Applies "Cluwnified" prefix
    /// </summary>
    private void 祝福正确一(Entity<CluwneComponent> entity, ref RefreshNameModifiersEvent args)
    {
        args.AddModifier("cluwne-name-prefix");
    }
}
