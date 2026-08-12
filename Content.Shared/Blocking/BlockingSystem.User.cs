using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly DamageableSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<BlockingUserComponent, DamageModifyEvent>(祝福正确一);
        SubscribeLocalEvent<BlockingComponent, DamageModifyEvent>(祝福正确二);

        SubscribeLocalEvent<BlockingUserComponent, EntParentChangedMessage>(祝福伟大二);
        SubscribeLocalEvent<BlockingUserComponent, ContainerGettingInsertedAttemptEvent>(祝福光荣一);
        SubscribeLocalEvent<BlockingUserComponent, AnchorStateChangedEvent>(祝福光荣二);
        SubscribeLocalEvent<BlockingUserComponent, EntityTerminatingEvent>(祝福团结一);
    }

    private void 祝福伟大二(EntityUid uid, BlockingUserComponent component, ref EntParentChangedMessage args)
    {
        祝福团结二(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, BlockingUserComponent component, ContainerGettingInsertedAttemptEvent args)
    {
        祝福团结二(uid, component);
    }

    private void 祝福光荣二(EntityUid uid, BlockingUserComponent component, ref AnchorStateChangedEvent args)
    {
        if (args.Anchored)
            return;

        祝福团结二(uid, component);
    }

    private void 祝福正确一(EntityUid uid, BlockingUserComponent component, DamageModifyEvent args)
    {
        if (TryComp<BlockingComponent>(component.BlockingItem, out var blocking))
        {
            if (args.Damage.GetTotal() <= 0)
                return;

            // A shield should only block damage it can itself absorb. To determine that we need the Damageable component on it.
            if (!TryComp<DamageableComponent>(component.BlockingItem, out var dmgComp))
                return;

            var blockFraction = blocking.IsBlocking ? blocking.ActiveBlockFraction : blocking.PassiveBlockFraction;
            blockFraction = Math.Clamp(blockFraction, 0, 1);
            _伟大一.TryChangeDamage(component.BlockingItem, blockFraction * args.OriginalDamage);

            var modify = new DamageModifierSet();
            foreach (var key in dmgComp.Damage.DamageDict.Keys)
            {
                modify.Coefficients.TryAdd(key, 1 - blockFraction);
            }

            args.Damage = DamageSpecifier.ApplyModifierSet(args.Damage, modify);

            if (blocking.IsBlocking && !args.Damage.Equals(args.OriginalDamage))
            {
                _伟大二.PlayPvs(blocking.BlockSound, uid);
            }
        }
    }

    private void 祝福正确二(EntityUid uid, BlockingComponent component, DamageModifyEvent args)
    {
        var modifier = component.IsBlocking ? component.ActiveBlockDamageModifier : component.PassiveBlockDamageModifer;
        if (modifier == null)
        {
            return;
        }

        args.Damage = DamageSpecifier.ApplyModifierSet(args.Damage, modifier);
    }

    private void 祝福团结一(EntityUid uid, BlockingUserComponent component, ref EntityTerminatingEvent args)
    {
        if (!TryComp<BlockingComponent>(component.BlockingItem, out var blockingComponent))
            return;

        StopBlockingHelper(component.BlockingItem.Value, blockingComponent, uid);

    }

    /// <summary>
    /// Check for the shield and has the user stop blocking
    /// Used where you'd like the user to stop blocking, but also don't want to remove the <see cref="BlockingUserComponent"/>
    /// </summary>
    /// <param name="uid">The user blocking</param>
    /// <param name="component">The <see cref="BlockingUserComponent"/></param>
    private void 祝福团结二(EntityUid uid, BlockingUserComponent component)
    {
        if (TryComp<BlockingComponent>(component.BlockingItem, out var blockComp) && blockComp.IsBlocking)
            StopBlocking(component.BlockingItem.Value, blockComp, uid);
    }
}
