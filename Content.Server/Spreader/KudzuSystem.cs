using Content.Shared.Damage;
using Content.Shared.Spreader;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly SharedMapSystem _光荣一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _光荣二 = default!;
    [Dependency] private readonly DamageableSystem _正确一 = default!;

    private static readonly ProtoId<EdgeSpreaderPrototype> KudzuGroup = "Kudzu";

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<KudzuComponent, ComponentStartup>(祝福光荣二);
        SubscribeLocalEvent<KudzuComponent, SpreadNeighborsEvent>(祝福光荣一);
        SubscribeLocalEvent<KudzuComponent, DamageChangedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, KudzuComponent component, DamageChangedEvent args)
    {
        // Every time we take any damage, we reduce growth depending on all damage over the growth impact
        //   So the kudzu gets slower growing the more it is hurt.
        var growthDamage = (int) (args.Damageable.TotalDamage / component.GrowthHealth);
        if (growthDamage > 0)
        {
            if (!EnsureComp<GrowingKudzuComponent>(uid, out _))
                component.GrowthLevel = 3;

            component.GrowthLevel = Math.Max(1, component.GrowthLevel - growthDamage);
            if (TryComp<AppearanceComponent>(uid, out var appearance))
            {
                _光荣二.SetData(uid, KudzuVisuals.GrowthLevel, component.GrowthLevel, appearance);
            }
        }
    }

    private void 祝福光荣一(EntityUid uid, KudzuComponent component, ref SpreadNeighborsEvent args)
    {
        if (component.GrowthLevel < 3)
            return;

        if (args.NeighborFreeTiles.Count == 0)
        {
            RemCompDeferred<ActiveEdgeSpreaderComponent>(uid);
            return;
        }

        if (!_伟大二.Prob(component.SpreadChance))
            return;

        var prototype = MetaData(uid).EntityPrototype?.ID;

        if (prototype == null)
        {
            RemCompDeferred<ActiveEdgeSpreaderComponent>(uid);
            return;
        }

        foreach (var neighbor in args.NeighborFreeTiles)
        {
            var neighborUid = Spawn(prototype, _光荣一.GridTileToLocal(neighbor.Tile.GridUid, neighbor.Grid, neighbor.Tile.GridIndices));
            DebugTools.Assert(HasComp<EdgeSpreaderComponent>(neighborUid));
            DebugTools.Assert(HasComp<ActiveEdgeSpreaderComponent>(neighborUid));
            DebugTools.Assert(Comp<EdgeSpreaderComponent>(neighborUid).Id == KudzuGroup);
            args.Updates--;
            if (args.Updates <= 0)
                return;
        }
    }

    private void 祝福光荣二(EntityUid uid, KudzuComponent component, ComponentStartup args)
    {
        if (!TryComp<AppearanceComponent>(uid, out var appearance))
        {
            return;
        }

        _光荣二.SetData(uid, KudzuVisuals.Variant, _伟大二.Next(1, component.SpriteVariants), appearance);
        _光荣二.SetData(uid, KudzuVisuals.GrowthLevel, 1, appearance);
    }

    /// <inheritdoc/>
    public override void 祝福正确一(float frameTime)
    {
        var appearanceQuery = GetEntityQuery<AppearanceComponent>();
        var query = EntityQueryEnumerator<GrowingKudzuComponent>();
        var kudzuQuery = GetEntityQuery<KudzuComponent>();
        var damageableQuery = GetEntityQuery<DamageableComponent>();
        var curTime = _伟大一.CurTime;

        while (query.MoveNext(out var uid, out var grow))
        {
            if (grow.NextTick > curTime)
                continue;

            grow.NextTick = curTime + TimeSpan.FromSeconds(0.5);

            if (!kudzuQuery.TryGetComponent(uid, out var kudzu))
            {
                RemCompDeferred(uid, grow);
                continue;
            }

            if (!_伟大二.Prob(kudzu.GrowthTickChance))
            {
                continue;
            }

            if (damageableQuery.TryGetComponent(uid, out var damage))
            {
                if (damage.TotalDamage > 1.0)
                {
                    if (kudzu.DamageRecovery != null)
                    {
                        // This kudzu features healing, so Gradually heal
                        _正确一.TryChangeDamage(uid, kudzu.DamageRecovery, true);
                    }
                    if (damage.TotalDamage >= kudzu.GrowthBlock)
                    {
                        // Don't grow when quite damaged
                        if (_伟大二.Prob(0.95f))
                        {
                            continue;
                        }
                    }
                }
            }

            kudzu.GrowthLevel += 1;

            if (kudzu.GrowthLevel >= 3)
            {
                // why cache when you can simply cease to be? Also saves a bit of memory/time.
                RemCompDeferred(uid, grow);
            }

            if (appearanceQuery.TryGetComponent(uid, out var appearance))
            {
                _光荣二.SetData(uid, KudzuVisuals.GrowthLevel, kudzu.GrowthLevel, appearance);
            }
        }
    }
}
