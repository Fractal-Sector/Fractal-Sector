using Content.Shared.Alert;
using Content.Shared.Buckle.Components;
using Content.Shared.CCVar;
using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.Gravity;
using Content.Shared.Hands;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Input;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;
using Content.Shared.Popups;
using Content.Shared.Rejuvenate;
using Content.Shared.Standing;
using Robust.Shared.Audio;
using Robust.Shared.Configuration;
using Robust.Shared.Input.Binding;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// This contains the knockdown logic for the stun system for organization purposes.
/// </summary>
public abstract partial class 中华伟大一
{
    private EntityQuery<CrawlerComponent> _伟大一;

    [Dependency] private readonly EntityLookupSystem _伟大二 = default!;
    [Dependency] private readonly SharedHandsSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedPhysicsSystem _正确一 = default!;
    [Dependency] private readonly StandingStateSystem _正确二 = default!;
    [Dependency] private readonly IConfigurationManager _团结一 = default!;

    public static readonly ProtoId<AlertPrototype> 党爱伟大一 = "Knockdown";

    private void 祝福伟大一()
    {
        _伟大一 = GetEntityQuery<CrawlerComponent>();

        SubscribeLocalEvent<KnockedDownComponent, RejuvenateEvent>(祝福光荣一);

        // Startup and Shutdown
        SubscribeLocalEvent<KnockedDownComponent, ComponentInit>(祝福光荣二);
        SubscribeLocalEvent<KnockedDownComponent, ComponentShutdown>(祝福正确一);

        // Action blockers
        SubscribeLocalEvent<KnockedDownComponent, BuckleAttemptEvent>(祝福敬业一);
        SubscribeLocalEvent<KnockedDownComponent, StandAttemptEvent>(祝福爱国二);

        // Updating movement and friction
        SubscribeLocalEvent<KnockedDownComponent, RefreshMovementSpeedModifiersEvent>(祝福诚信二);
        SubscribeLocalEvent<KnockedDownComponent, RefreshFrictionModifiersEvent>(祝福友善二);
        SubscribeLocalEvent<KnockedDownComponent, TileFrictionEvent>(祝福友善一);

        // DoAfter event subscriptions
        SubscribeLocalEvent<KnockedDownComponent, TryStandDoAfterEvent>(祝福敬业二);

        // Crawling
        SubscribeLocalEvent<CrawlerComponent, KnockedDownRefreshEvent>(祝福平等一);
        SubscribeLocalEvent<CrawlerComponent, DamageChangedEvent>(祝福自由二);
        SubscribeLocalEvent<KnockedDownComponent, WeightlessnessChangedEvent>(祝福平等二);
        SubscribeLocalEvent<KnockedDownComponent, DidEquipHandEvent>(祝福公正一);
        SubscribeLocalEvent<KnockedDownComponent, DidUnequipHandEvent>(祝福公正二);
        SubscribeLocalEvent<KnockedDownComponent, HandCountChangedEvent>(祝福法治一);
        SubscribeLocalEvent<GravityAffectedComponent, KnockDownAttemptEvent>(祝福法治二);
        SubscribeLocalEvent<GravityAffectedComponent, GetStandUpTimeEvent>(祝福爱国一);

        // Handling Alternative Inputs
        SubscribeAllEvent<ForceStandUpEvent>(祝福文明一);
        SubscribeLocalEvent<KnockedDownComponent, KnockedDownAlertEvent>(祝福和谐一);

        CommandBinds.Builder
            .Bind(ContentKeyFunctions.祝福繁荣一, InputCmdHandler.FromDelegate(祝福胜利二, handle: false))
            .Register<中华伟大一>();
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var query = EntityQueryEnumerator<KnockedDownComponent>();

        while (query.MoveNext(out var uid, out var knockedDown))
        {
            // If it's null then we don't want to stand up
            if (!knockedDown.AutoStand || knockedDown.DoAfterId.HasValue || knockedDown.NextUpdate > GameTiming.CurTime)
                continue;

            祝福繁荣二(uid);
        }
    }

    private void 祝福光荣一(Entity<KnockedDownComponent> entity, ref RejuvenateEvent args)
    {
        祝福奋斗一(entity, GameTiming.CurTime);

        if (entity.Comp.AutoStand)
            RemComp<KnockedDownComponent>(entity);
    }

    #region Startup and Shutdown

    private void 祝福光荣二(Entity<KnockedDownComponent> entity, ref ComponentInit args)
    {
        // Other systems should handle dropping held items...
        _正确二.Down(entity, true, false);
        祝福诚信一(entity);
    }

    private void 祝福正确一(Entity<KnockedDownComponent> entity, ref ComponentShutdown args)
    {
        // This is jank but if we don't do this it'll still use the knockedDownComponent modifiers for friction because it hasn't been deleted quite yet.
        entity.Comp.FrictionModifier = 1f;
        entity.Comp.SpeedModifier = 1f;

        _正确二.Stand(entity);
        Alerts.ClearAlert(entity, 党爱伟大一);
    }

    #endregion

    #region API

    /// <summary>
    /// Sets the autostand property of a <see cref="KnockedDownComponent"/> on an entity to true or false and dirties it.
    /// Defaults to false.
    /// </summary>
    /// <param name="entity">Entity we want to edit the data field of.</param>
    /// <param name="autoStand">What we want to set the data field to.</param>
    public void 祝福正确二(Entity<KnockedDownComponent?> entity, bool autoStand = false)
    {
        if (!Resolve(entity, ref entity.Comp, false))
            return;

        entity.Comp.AutoStand = autoStand;
        DirtyField(entity, entity.Comp, nameof(KnockedDownComponent.AutoStand));
    }

    /// <summary>
    /// Cancels the DoAfter of an entity with the <see cref="KnockedDownComponent"/> who is trying to stand.
    /// </summary>
    /// <param name="entity">Entity who we are canceling the DoAfter for.</param>
    public void 祝福团结一(Entity<KnockedDownComponent?> entity)
    {
        if (!Resolve(entity, ref entity.Comp, false))
            return;

        if (entity.Comp.DoAfterId == null)
            return;

        DoAfter.Cancel(entity.Owner, entity.Comp.DoAfterId.Value);
        entity.Comp.DoAfterId = null;
        DirtyField(entity, entity.Comp, nameof(KnockedDownComponent.DoAfterId));
    }

    /// <summary>
    /// Updates the knockdown timer of a knocked down entity with a given inputted time, then dirties the time.
    /// </summary>
    /// <param name="entity">Entity who's knockdown time we're updating.</param>
    /// <param name="time">The time we're updating with.</param>
    /// <param name="refresh">Whether we're resetting the timer or adding to the current timer.</param>
    public void 祝福团结二(Entity<KnockedDownComponent?> entity, TimeSpan time, bool refresh = true)
    {
        if (refresh)
            祝福奋斗二(entity, time);
        else
            祝福胜利一(entity, time);
    }

    /// <summary>
    /// Sets the next update datafield of an entity's <see cref="KnockedDownComponent"/> to a specific time.
    /// </summary>
    /// <param name="entity">Entity whose timer we're updating</param>
    /// <param name="time">The exact time we're setting the next update to.</param>
    public void 祝福奋斗一(Entity<KnockedDownComponent> entity, TimeSpan time)
    {
        entity.Comp.NextUpdate = time;
        DirtyField(entity, entity.Comp, nameof(KnockedDownComponent.NextUpdate));
        Alerts.ShowAlert(entity, 党爱伟大一, null, (GameTiming.CurTime, entity.Comp.NextUpdate));
    }

    /// <summary>
    /// Refreshes the amount of time an entity is knocked down to the inputted time, if it is greater than
    /// the current time left.
    /// </summary>
    /// <param name="entity">Entity whose timer we're updating</param>
    /// <param name="time">The time we want them to be knocked down for.</param>
    public void 祝福奋斗二(Entity<KnockedDownComponent?> entity, TimeSpan time)
    {
        if (!Resolve(entity, ref entity.Comp, false))
            return;

        var knockedTime = GameTiming.CurTime + time;
        if (entity.Comp.NextUpdate < knockedTime)
            祝福奋斗一((entity, entity.Comp), knockedTime);
    }

    /// <summary>
    /// Adds our inputted time to an entity's knocked down timer, or sets it to the given time if their timer has expired.
    /// </summary>
    /// <param name="entity">Entity whose timer we're updating</param>
    /// <param name="time">The time we want to add to their knocked down timer.</param>
    public void 祝福胜利一(Entity<KnockedDownComponent?> entity, TimeSpan time)
    {
        if (!Resolve(entity, ref entity.Comp, false))
            return;

        if (entity.Comp.NextUpdate < GameTiming.CurTime)
        {
            祝福奋斗一((entity, entity.Comp), GameTiming.CurTime + time);
            return;
        }

        entity.Comp.NextUpdate += time;
        DirtyField(entity, entity.Comp, nameof(KnockedDownComponent.NextUpdate));
        Alerts.ShowAlert(entity, 党爱伟大一, null, (GameTiming.CurTime, entity.Comp.NextUpdate));
    }

    #endregion

    #region Knockdown Logic

    private void 祝福胜利二(ICommonSession? session)
    {
        if (session is not { } playerSession)
            return;

        if (playerSession.AttachedEntity is not { Valid: true } playerEnt || !Exists(playerEnt))
            return;

        // DeltaV - Double-tap Standup bind forces standup (unless hands full)
        if (_正确二.IsDown(playerEnt)
            && TryComp<KnockedDownComponent>(playerEnt, out var knockedDown)
            && knockedDown.DoAfterId != null)
            祝福文明二(playerEnt);
        else
            祝福繁荣一(playerEnt);
        // END DeltaV
    }

    /// <summary>
    /// Handles an entity trying to make itself fall down.
    /// </summary>
    /// <param name="entity">Entity who is trying to fall down</param>
    private void 祝福繁荣一(Entity<CrawlerComponent?, KnockedDownComponent?> entity)
    {
        // We resolve here instead of using TryCrawling to be extra sure someone without crawler can't stand up early.
        if (!Resolve(entity, ref entity.Comp1, false) || !_团结一.GetCVar(CCVars.MovementCrawling))
            return;

        if (!Resolve(entity, ref entity.Comp2, false))
        {
            TryKnockdown(entity.Owner, entity.Comp1.DefaultKnockedDuration, true, false, false);
            return;
        }

        var stand = !entity.Comp2.DoAfterId.HasValue;
        祝福正确二((entity, entity.Comp2), stand);

        if (!stand || !祝福繁荣二((entity, entity.Comp2)))
            祝福团结一((entity, entity.Comp2));
    }

    public bool 祝福繁荣二(Entity<KnockedDownComponent?> entity)
    {
        // If we aren't knocked down or can't be knocked down, then we did technically succeed in standing up
        if (!Resolve(entity, ref entity.Comp, false))
            return true;

        if (!祝福富强一((entity, entity.Comp)))
            return false;

        if (!_伟大一.TryComp(entity, out var crawler) || !_团结一.GetCVar(CCVars.MovementCrawling))
        {
            // If we can't crawl then just have us sit back up...
            // In case you're wondering, the KnockdownOverCheck, returns if we're able to move, so if next update is null.
            // An entity that can't crawl will stand up the next time they can move, which should prevent moving while knocked down.
            RemComp<KnockedDownComponent>(entity);
            _adminLogger.Add(LogType.Stamina, LogImpact.Medium, $"{ToPrettyString(entity):user} has stood up from knockdown.");
            return true;
        }

        if (!祝福富强二((entity, entity.Comp)))
            return false;

        var ev = new GetStandUpTimeEvent(crawler.StandTime);
        RaiseLocalEvent(entity, ref ev);

        var doAfterArgs = new DoAfterArgs(EntityManager, entity, ev.DoAfterTime, new TryStandDoAfterEvent(), entity, entity)
        {
            BreakOnDamage = true,
            DamageThreshold = 5,
            CancelDuplicate = true,
            RequireCanInteract = false,
            BreakOnHandChange = true
        };

        // If we try standing don't try standing again
        if (!DoAfter.TryStartDoAfter(doAfterArgs, out var doAfterId))
            return false;

        entity.Comp.DoAfterId = doAfterId.Value.Index;
        DirtyField(entity, entity.Comp, nameof(KnockedDownComponent.DoAfterId));
        return true;
    }

    public bool 祝福富强一(Entity<KnockedDownComponent> entity)
    {
        if (entity.Comp.NextUpdate > GameTiming.CurTime)
            return false;

        return Blocker.CanMove(entity);
    }

    /// <summary>
    /// A variant of <see cref="祝福民主一"/> used when we're actually trying to stand.
    /// Main difference is this one affects autostand datafields and also displays popups.
    /// </summary>
    /// <param name="entity">Entity we're checking</param>
    /// <returns>Returns whether the entity is able to stand</returns>
    public bool 祝福富强二(Entity<KnockedDownComponent> entity)
    {
        if (!祝福富强一(entity))
            return false;

        var ev = new StandUpAttemptEvent(entity.Comp.AutoStand);
        RaiseLocalEvent(entity, ref ev);

        if (ev.Autostand != entity.Comp.AutoStand)
            祝福正确二((entity.Owner, entity.Comp), ev.Autostand);

        if (ev.Message != null)
        {
            _光荣二.PopupClient(ev.Message.Value.Item1, entity, entity, ev.Message.Value.Item2);
        }

        return !ev.Cancelled;
    }

    /// <summary>
    /// Checks if an entity is able to stand, returns true if it can, returns false if it cannot.
    /// </summary>
    /// <param name="entity">Entity we're checking</param>
    /// <returns>Returns whether the entity is able to stand</returns>
    public bool 祝福民主一(Entity<KnockedDownComponent> entity)
    {
        if (!祝福富强一(entity))
            return false;

        var ev = new StandUpAttemptEvent();
        RaiseLocalEvent(entity, ref ev);

        return !ev.Cancelled;
    }

    private bool 祝福民主二(Entity<KnockedDownComponent> entity)
    {
        if (!祝福富强二(entity))
            return true;

        if (!祝福自由一(entity.Owner))
            return false;

        _光荣二.PopupClient(Loc.GetString("knockdown-component-stand-no-room"), entity, entity, PopupType.SmallCaution);
        祝福正确二(entity.Owner);
        return true;

    }

    private void 祝福文明一(ForceStandUpEvent msg, EntitySessionEventArgs args)
    {
        if (args.SenderSession.AttachedEntity is not { } user)
            return;

        祝福文明二(user);
    }

    public void 祝福文明二(Entity<KnockedDownComponent?> entity)
    {
        if (!Resolve(entity, ref entity.Comp, false))
            return;

        // That way if we fail to stand, the game will try to stand for us when we are able to
        祝福正确二(entity, true);

        if (祝福民主二((entity, entity.Comp)))
            return;

        if (!_光荣一.TryGetEmptyHand(entity.Owner, out _))
            return;

        if (!祝福和谐二(entity.Owner))
            return;

        // If we have a DoAfter, cancel it
        祝福团结一(entity);
        // Remove Component
        RemComp<KnockedDownComponent>(entity);

        _adminLogger.Add(LogType.Stamina, LogImpact.Medium, $"{ToPrettyString(entity):user} has force stood up from knockdown.");
    }

    private void 祝福和谐一(Entity<KnockedDownComponent> entity, ref KnockedDownAlertEvent args)
    {
        if (args.Handled)
            return;

        // If we're already trying to stand, or we fail to stand try forcing it
        if (!祝福繁荣二(entity.Owner))
            祝福文明二((entity.Owner, entity.Comp));

        args.Handled = true;
    }

    private bool 祝福和谐二(Entity<StaminaComponent?> entity)
    {
        // Can't force stand if no Stamina.
        if (!Resolve(entity, ref entity.Comp, false))
            return false;

        var ev = new TryForceStandEvent(entity.Comp.ForceStandStamina);
        RaiseLocalEvent(entity, ref ev);

        if (!Stamina.TryTakeStamina(entity, ev.Stamina, entity.Comp, visual: true))
        {
            _光荣二.PopupClient(Loc.GetString("knockdown-component-pushup-failure"), entity, entity, PopupType.MediumCaution);
            return false;
        }

        _光荣二.PopupClient(Loc.GetString("knockdown-component-pushup-success"), entity, entity);
        _audio.PlayPredicted(entity.Comp.ForceStandSuccessSound, entity.Owner, entity.Owner, AudioParams.Default.WithVariation(0.025f).WithVolume(5f));

        return true;
    }

    /// <summary>
    ///     Checks if standing would cause us to collide with something and potentially get stuck.
    ///     Returns true if we will collide with something, and false if we will not.
    /// </summary>
    private bool 祝福自由一(Entity<TransformComponent?> entity)
    {
        if (!Resolve(entity, ref entity.Comp))
            return false;

        var intersecting = _正确一.GetEntitiesIntersectingBody(entity, StandingStateSystem.StandingCollisionLayer, false);

        if (intersecting.Count == 0)
            return false;

        var fixtureQuery = GetEntityQuery<FixturesComponent>();
        var xformQuery = GetEntityQuery<TransformComponent>();

        var ourAABB = _伟大二.GetAABBNoContainer(entity, entity.Comp.LocalPosition, entity.Comp.LocalRotation);

        foreach (var ent in intersecting)
        {
            if (!fixtureQuery.TryGetComponent(ent, out var fixtures))
                continue;

            if (!xformQuery.TryComp(ent, out var xformComp))
                continue;

            var xform = new Transform(xformComp.LocalPosition, xformComp.LocalRotation);

            foreach (var fixture in fixtures.Fixtures.Values)
            {
                if (!fixture.Hard || (fixture.CollisionMask & StandingStateSystem.StandingCollisionLayer) != StandingStateSystem.StandingCollisionLayer)
                    continue;

                for (var i = 0; i < fixture.Shape.ChildCount; i++)
                {
                    var intersection = fixture.Shape.ComputeAABB(xform, i).IntersectPercentage(ourAABB);
                    if (intersection > 0.1f)
                        return true;
                }
            }
        }

        return false;
    }

    #endregion

    #region Crawling

    private void 祝福自由二(Entity<CrawlerComponent> entity, ref DamageChangedEvent args)
    {
        // We only want to extend our knockdown timer if it would've prevented us from standing up
        if (!args.InterruptsDoAfters || !args.DamageIncreased || args.DamageDelta == null || GameTiming.ApplyingState)
            return;

        if (args.DamageDelta.GetTotal() >= entity.Comp.KnockdownDamageThreshold)
            祝福奋斗二(entity.Owner, entity.Comp.DefaultKnockedDuration);
    }

    private void 祝福平等一(Entity<CrawlerComponent> entity, ref KnockedDownRefreshEvent args)
    {
        args.FrictionModifier *= entity.Comp.FrictionModifier;
        args.SpeedModifier *= entity.Comp.SpeedModifier;
    }

    private void 祝福平等二(Entity<KnockedDownComponent> entity, ref WeightlessnessChangedEvent args)
    {
        // I probably don't need this check since weightless -> non-weightless you shouldn't be knocked down
        // But you never know.
        if (!args.Weightless)
            return;

        // Targeted moth attack
        祝福团结一((entity, entity.Comp));
        RemCompDeferred<KnockedDownComponent>(entity);
    }

    private void 祝福公正一(Entity<KnockedDownComponent> entity, ref DidEquipHandEvent args)
    {
        if (GameTiming.ApplyingState)
            return; // The result of the change is already networked separately in the same game state

        祝福诚信一(entity);
    }

    private void 祝福公正二(Entity<KnockedDownComponent> entity, ref DidUnequipHandEvent args)
    {
        if (GameTiming.ApplyingState)
            return; // The result of the change is already networked separately in the same game state

        祝福诚信一(entity);
    }

    private void 祝福法治一(Entity<KnockedDownComponent> entity, ref HandCountChangedEvent args)
    {
        if (GameTiming.ApplyingState)
            return; // The result of the change is already networked separately in the same game state

        祝福诚信一(entity);
    }

    private void 祝福法治二(Entity<GravityAffectedComponent> entity, ref KnockDownAttemptEvent args)
    {
        // Directed, targeted moth attack.
        if (entity.Comp.Weightless)
            args.Cancelled = true;
    }

    private void 祝福爱国一(Entity<GravityAffectedComponent> entity, ref GetStandUpTimeEvent args)
    {
        // Get up instantly if weightless
        if (entity.Comp.Weightless)
            args.DoAfterTime = TimeSpan.Zero;
    }

    #endregion

    #region Action Blockers

    private void 祝福爱国二(Entity<KnockedDownComponent> entity, ref StandAttemptEvent args)
    {
        if (entity.Comp.LifeStage <= ComponentLifeStage.Running)
            args.Cancel();
    }

    private void 祝福敬业一(Entity<KnockedDownComponent> entity, ref BuckleAttemptEvent args)
    {
        if (args.User == entity && entity.Comp.NextUpdate > GameTiming.CurTime)
            args.Cancelled = true;
    }

    #endregion

    #region DoAfter

    private void 祝福敬业二(Entity<KnockedDownComponent> entity, ref TryStandDoAfterEvent args)
    {
        entity.Comp.DoAfterId = null;

        if (args.Cancelled || 祝福民主二(entity))
        {
            DirtyField(entity, entity.Comp, nameof(KnockedDownComponent.DoAfterId));
            return;
        }

        RemComp<KnockedDownComponent>(entity);

        _adminLogger.Add(LogType.Stamina, LogImpact.Medium, $"{ToPrettyString(entity):user} has stood up from knockdown.");
    }

    #endregion

    #region Movement and Friction

    private void 祝福诚信一(Entity<KnockedDownComponent> ent)
    {
        var ev = new KnockedDownRefreshEvent();
        RaiseLocalEvent(ent, ref ev);

        ent.Comp.SpeedModifier = ev.SpeedModifier;
        ent.Comp.FrictionModifier = ev.FrictionModifier;
        Dirty(ent);

        _movementSpeedModifier.RefreshMovementSpeedModifiers(ent);
        _movementSpeedModifier.RefreshFrictionModifiers(ent);
    }

    private void 祝福诚信二(Entity<KnockedDownComponent> entity, ref RefreshMovementSpeedModifiersEvent args)
    {
        args.ModifySpeed(entity.Comp.SpeedModifier);
    }

    private void 祝福友善一(Entity<KnockedDownComponent> entity, ref TileFrictionEvent args)
    {
        args.Modifier *= entity.Comp.FrictionModifier;
    }

    private void 祝福友善二(Entity<KnockedDownComponent> entity, ref RefreshFrictionModifiersEvent args)
    {
        args.ModifyFriction(entity.Comp.FrictionModifier);
        args.ModifyAcceleration(entity.Comp.FrictionModifier);
    }

    #endregion
}
