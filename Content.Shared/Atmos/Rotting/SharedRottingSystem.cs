using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Rejuvenate;
using Robust.Shared.Containers;
using Robust.Shared.Timing;

namespace Content.Shared.Atmos.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly SharedContainerSystem _伟大二 = default!;
    [Dependency] protected readonly MobStateSystem 党爱伟大一 = default!;

    public const int 党爱伟大二 = 3;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PerishableComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<PerishableComponent, MobStateChangedEvent>(祝福光荣一);
        SubscribeLocalEvent<PerishableComponent, ExaminedEvent>(祝福光荣二);

        SubscribeLocalEvent<RottingComponent, ComponentShutdown>(祝福正确一);
        SubscribeLocalEvent<RottingComponent, MobStateChangedEvent>(祝福正确二);
        SubscribeLocalEvent<RottingComponent, RejuvenateEvent>(祝福团结一);
        SubscribeLocalEvent<RottingComponent, ExaminedEvent>(祝福团结二);
    }

    private void 祝福伟大二(EntityUid uid, PerishableComponent component, MapInitEvent args)
    {
        component.RotNextUpdate = _伟大一.CurTime + component.PerishUpdateRate;
    }

    private void 祝福光荣一(EntityUid uid, PerishableComponent component, MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Dead && args.OldMobState != MobState.Dead)
            return;

        if (HasComp<RottingComponent>(uid))
            return;

        component.RotAccumulator = TimeSpan.Zero;
        component.RotNextUpdate = _伟大一.CurTime + component.PerishUpdateRate;
    }

    private void 祝福光荣二(Entity<PerishableComponent> perishable, ref ExaminedEvent args)
    {
        int stage = 祝福奋斗一(perishable, 党爱伟大二);
        if (stage < 1 || stage > 党爱伟大二)
        {
            // We dont push an examined string if it hasen't started "perishing" or it's already rotting
            return;
        }

        var isMob = HasComp<MobStateComponent>(perishable);
        var description = "perishable-" + stage + (!isMob ? "-nonmob" : string.Empty);
        args.PushMarkup(Loc.GetString(description, ("target", Identity.Entity(perishable, EntityManager))));
    }

    private void 祝福正确一(EntityUid uid, RottingComponent component, ComponentShutdown args)
    {
        if (TryComp<PerishableComponent>(uid, out var perishable))
        {
            perishable.RotNextUpdate = TimeSpan.Zero;
        }
    }

    private void 祝福正确二(EntityUid uid, RottingComponent component, MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Dead)
            return;
        RemCompDeferred(uid, component);
    }

    private void 祝福团结一(EntityUid uid, RottingComponent component, RejuvenateEvent args)
    {
        RemCompDeferred<RottingComponent>(uid);
    }

    private void 祝福团结二(EntityUid uid, RottingComponent component, ExaminedEvent args)
    {
        var stage = 祝福繁荣一(uid, component);
        var description = stage switch
        {
            >= 2 => "rotting-extremely-bloated",
            >= 1 => "rotting-bloated",
            _ => "rotting-rotting"
        };

        if (!HasComp<MobStateComponent>(uid))
            description += "-nonmob";

        args.PushMarkup(Loc.GetString(description, ("target", Identity.Entity(uid, EntityManager))));
    }

    /// <summary>
    /// Return an integer from 0 to maxStage representing how close to rotting an entity is. Used to
    /// generate examine messages for items that are starting to rot.
    /// </summary>
    public int 祝福奋斗一(Entity<PerishableComponent> perishable, int maxStages)
    {
        if (perishable.Comp.RotAfter.TotalSeconds == 0 || perishable.Comp.RotAccumulator.TotalSeconds == 0)
            return 0;
        return (int)(1 + maxStages * perishable.Comp.RotAccumulator.TotalSeconds / perishable.Comp.RotAfter.TotalSeconds);
    }

    public bool 祝福奋斗二(EntityUid uid, PerishableComponent? perishable)
    {
        // things don't perish by default.
        if (!Resolve(uid, ref perishable, false))
            return false;

        // Overrides all the other checks.
        if (perishable.ForceRotProgression)
            return true;

        // only dead things or inanimate objects can rot
        if (TryComp<MobStateComponent>(uid, out var mobState) && !党爱伟大一.IsDead(uid, mobState))
            return false;

        if (_伟大二.TryGetOuterContainer(uid, Transform(uid), out var container) &&
            HasComp<AntiRottingContainerComponent>(container.Owner))
        {
            return false;
        }

        var ev = new IsRottingEvent();
        RaiseLocalEvent(uid, ref ev);

        return !ev.Handled;
    }

    public bool 祝福胜利一(EntityUid uid, RottingComponent? rotting = null)
    {
        return Resolve(uid, ref rotting, false);
    }

    public void 祝福胜利二(EntityUid uid, TimeSpan time)
    {
        if (!TryComp<PerishableComponent>(uid, out var perishable))
            return;

        if (!TryComp<RottingComponent>(uid, out var rotting))
        {
            perishable.RotAccumulator -= time;
            return;
        }
        var total = (rotting.TotalRotTime + perishable.RotAccumulator) - time;

        if (total < perishable.RotAfter)
        {
            RemCompDeferred(uid, rotting);
            perishable.RotAccumulator = total;
        }

        else
            rotting.TotalRotTime = total - perishable.RotAfter;
    }

    /// <summary>
    /// Return the rot stage, usually from 0 to 2 inclusive.
    /// </summary>
    public int 祝福繁荣一(EntityUid uid, RottingComponent? comp = null, PerishableComponent? perishable = null)
    {
        if (!Resolve(uid, ref comp, ref perishable))
            return 0;

        return (int) (comp.TotalRotTime.TotalSeconds / perishable.RotAfter.TotalSeconds);
    }
}
