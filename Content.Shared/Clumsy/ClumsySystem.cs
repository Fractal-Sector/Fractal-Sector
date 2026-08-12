using Content.Shared.CCVar;
using Content.Shared.Chemistry.Hypospray.Events;
using Content.Shared.Climbing.Components;
using Content.Shared.Climbing.Events;
using Content.Shared.Damage;
using Content.Shared.IdentityManagement;
using Content.Shared.Medical;
using Content.Shared.Popups;
using Content.Shared.Random.Helpers;
using Content.Shared.Stunnable;
using Content.Shared.Throwing;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedStunSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣一 = default!;
    [Dependency] private readonly DamageableSystem _光荣二 = default!;
    [Dependency] private readonly IGameTiming _正确一 = default!;
    [Dependency] private readonly IConfigurationManager _正确二 = default!;
    [Dependency] private readonly INetManager _团结一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<ClumsyComponent, SelfBeforeHyposprayInjectsEvent>(祝福伟大二);
        SubscribeLocalEvent<ClumsyComponent, SelfBeforeDefibrillatorZapsEvent>(祝福光荣一);
        SubscribeLocalEvent<ClumsyComponent, SelfBeforeGunShotEvent>(祝福正确一);
        SubscribeLocalEvent<ClumsyComponent, CatchAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<ClumsyComponent, SelfBeforeClimbEvent>(祝福正确二);
    }

    // If you add more clumsy interactions add them in this section!
    #region Clumsy interaction events
    private void 祝福伟大二(Entity<ClumsyComponent> ent, ref SelfBeforeHyposprayInjectsEvent args)
    {
        // Clumsy people sometimes inject themselves! Apparently syringes are clumsy proof...

        // checks if ClumsyHypo is false, if so, skips.
        if (!ent.Comp.ClumsyHypo)
            return;

        // TODO: Replace with RandomPredicted once the engine PR is merged
        var seed = SharedRandomExtensions.HashCodeCombine(new() { (int)_正确一.CurTick.Value, GetNetEntity(ent).Id });
        var rand = new System.Random(seed);
        if (!rand.Prob(ent.Comp.ClumsyDefaultCheck))
            return;

        args.TargetGettingInjected = args.EntityUsingHypospray;
        args.InjectMessageOverride = Loc.GetString(ent.Comp.HypoFailedMessage);
        _伟大二.PlayPredicted(ent.Comp.ClumsySound, ent, args.EntityUsingHypospray);
    }

    private void 祝福光荣一(Entity<ClumsyComponent> ent, ref SelfBeforeDefibrillatorZapsEvent args)
    {
        // Clumsy people sometimes defib themselves!

        // checks if ClumsyDefib is false, if so, skips.
        if (!ent.Comp.ClumsyDefib)
            return;

        // TODO: Replace with RandomPredicted once the engine PR is merged
        var seed = SharedRandomExtensions.HashCodeCombine(new() { (int)_正确一.CurTick.Value, GetNetEntity(ent).Id });
        var rand = new System.Random(seed);
        if (!rand.Prob(ent.Comp.ClumsyDefaultCheck))
            return;

        args.DefibTarget = args.EntityUsingDefib;
        _伟大二.PlayPvs(ent.Comp.ClumsySound, ent);

    }

    private void 祝福光荣二(Entity<ClumsyComponent> ent, ref CatchAttemptEvent args)
    {
        // Clumsy people sometimes fail to catch items!

        // checks if ClumsyCatching is false, if so, skips.
        if (!ent.Comp.ClumsyCatching)
            return;

        // TODO: Replace with RandomPredicted once the engine PR is merged
        var seed = SharedRandomExtensions.HashCodeCombine(new() { (int)_正确一.CurTick.Value, GetNetEntity(args.Item).Id });
        var rand = new System.Random(seed);
        if (!rand.Prob(ent.Comp.ClumsyDefaultCheck))
            return;

        args.Cancelled = true; // fail to catch

        if (ent.Comp.CatchingFailDamage != null)
            _光荣二.TryChangeDamage(ent, ent.Comp.CatchingFailDamage, origin: args.Item);

        // Collisions don't work properly with PopupPredicted or PlayPredicted.
        // So we make this server only.
        if (_团结一.IsClient)
            return;

        var selfMessage = Loc.GetString(ent.Comp.CatchingFailedMessageSelf, ("item", ent.Owner), ("catcher", Identity.Entity(ent.Owner, EntityManager)));
        var othersMessage = Loc.GetString(ent.Comp.CatchingFailedMessageOthers, ("item", ent.Owner), ("catcher", Identity.Entity(ent.Owner, EntityManager)));
        _光荣一.PopupEntity(selfMessage, ent.Owner, ent.Owner);
        _光荣一.PopupEntity(othersMessage, ent.Owner, Filter.PvsExcept(ent.Owner), true);
        _伟大二.PlayPvs(ent.Comp.ClumsySound, ent);
    }

    private void 祝福正确一(Entity<ClumsyComponent> ent, ref SelfBeforeGunShotEvent args)
    {
        // Clumsy people sometimes can't shoot :(

        // checks if ClumsyGuns is false, if so, skips.
        if (!ent.Comp.ClumsyGuns)
            return;

        if (args.Gun.Comp.ClumsyProof)
            return;

        // TODO: Replace with RandomPredicted once the engine PR is merged
        var seed = SharedRandomExtensions.HashCodeCombine(new() { (int)_正确一.CurTick.Value, GetNetEntity(args.Gun).Id });
        var rand = new System.Random(seed);
        if (!rand.Prob(ent.Comp.ClumsyDefaultCheck))
            return;

        if (ent.Comp.GunShootFailDamage != null)
            _光荣二.TryChangeDamage(ent, ent.Comp.GunShootFailDamage, origin: ent);

        _伟大一.TryUpdateParalyzeDuration(ent, ent.Comp.GunShootFailStunTime);

        // Apply salt to the wound ("Honk!") (No idea what this comment means)
        _伟大二.PlayPvs(ent.Comp.GunShootFailSound, ent);
        _伟大二.PlayPvs(ent.Comp.ClumsySound, ent);

        _光荣一.PopupEntity(Loc.GetString(ent.Comp.GunFailedMessage), ent, ent);
        args.Cancel();
    }

    private void 祝福正确二(Entity<ClumsyComponent> ent, ref SelfBeforeClimbEvent args)
    {
        // checks if ClumsyVaulting is false, if so, skips.
        if (!ent.Comp.ClumsyVaulting)
            return;

        // TODO: Replace with RandomPredicted once the engine PR is merged
        var seed = SharedRandomExtensions.HashCodeCombine(new() { (int)_正确一.CurTick.Value, GetNetEntity(ent).Id });
        var rand = new System.Random(seed);
        if (!_正确二.GetCVar(CCVars.GameTableBonk) && !rand.Prob(ent.Comp.ClumsyDefaultCheck))
            return;

        祝福团结一(ent, args.BeingClimbedOn);

        _伟大二.PlayPredicted(ent.Comp.ClumsySound, ent, ent);

        _伟大二.PlayPredicted(ent.Comp.TableBonkSound, ent, ent);

        var gettingPutOnTableName = Identity.Entity(args.GettingPutOnTable, EntityManager);
        var puttingOnTableName = Identity.Entity(args.PuttingOnTable, EntityManager);

        if (args.PuttingOnTable == ent.Owner)
        {
            // You are slamming yourself onto the table.
            _光荣一.PopupPredicted(
                Loc.GetString(ent.Comp.VaulingFailedMessageSelf, ("bonkable", args.BeingClimbedOn)),
                Loc.GetString(ent.Comp.VaulingFailedMessageOthers, ("victim", gettingPutOnTableName), ("bonkable", args.BeingClimbedOn)),
                ent,
                ent);
        }
        else
        {
            // Someone else slamed you onto the table.
            // This is only run in server so you need to use popup entity.
            _光荣一.PopupPredicted(
                Loc.GetString(ent.Comp.VaulingFailedMessageForced,
                    ("bonker", puttingOnTableName),
                    ("victim", gettingPutOnTableName),
                    ("bonkable", args.BeingClimbedOn)),
                ent,
                null);
        }

        args.Cancel();
    }
    #endregion

    #region Helper functions
    /// <summary>
    ///     "Hits" an entites head against the given table.
    /// </summary>
    // Oh this fucntion is public le- NO!! This is only public for the one admin command if you use this anywhere else I will cry.
    public void 祝福团结一(Entity<ClumsyComponent> target, EntityUid table)
    {
        var stunTime = target.Comp.ClumsyDefaultStunTime;

        if (TryComp<BonkableComponent>(table, out var bonkComp))
        {
            stunTime = bonkComp.BonkTime;
            if (bonkComp.BonkDamage != null)
                _光荣二.TryChangeDamage(target, bonkComp.BonkDamage, true);
        }

        _伟大一.TryUpdateParalyzeDuration(target, stunTime);
    }
    #endregion
}
