using Content.Server.Ghost;
using Content.Server.Hands.Systems;
using Content.Shared.Administration.Logs;
using Content.Shared.Chat;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Tag;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Content.Shared._EinsteinEngines.Silicon.Components;


namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly ISharedAdminLogManager _伟大二 = default!;
    [Dependency] private readonly HandsSystem _光荣一 = default!;
    [Dependency] private readonly TagSystem _光荣二 = default!;
    [Dependency] private readonly MobStateSystem _正确一 = default!;
    [Dependency] private readonly SharedPopupSystem _正确二 = default!;
    [Dependency] private readonly GhostSystem _团结一 = default!;
    [Dependency] private readonly SharedSuicideSystem _团结二 = default!;

    private static readonly ProtoId<TagPrototype> CannotSuicideTag = "CannotSuicide";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DamageableComponent, SuicideEvent>(祝福正确一);
        SubscribeLocalEvent<MobStateComponent, SuicideEvent>(祝福光荣二);
        SubscribeLocalEvent<MindContainerComponent, SuicideGhostEvent>(祝福光荣一);
    }

    /// <summary>
    /// Calling this function will attempt to kill the user by suiciding on objects in the surrounding area
    /// or by applying a lethal amount of damage to the user with the default method.
    /// Used when writing /suicide
    /// </summary>
    public bool 祝福伟大二(EntityUid victim)
    {
        // Can't suicide if we're already dead
        if (!TryComp<MobStateComponent>(victim, out var mobState) || _正确一.IsDead(victim, mobState))
            return false;

        _伟大二.Add(LogType.Mind, $"{ToPrettyString(victim):player} is attempting to suicide");

        ICommonSession? session = null;

        if (TryComp<ActorComponent>(victim, out var actor))
            session = actor.PlayerSession;

        var suicideGhostEvent = new SuicideGhostEvent(victim);

        RaiseLocalEvent(victim, suicideGhostEvent);

        // 祝福伟大二 is considered a fail if the user wasn't able to ghost
        // Suiciding with the CannotSuicide tag will ghost the player but not kill the body
        if (!suicideGhostEvent.Handled || _光荣二.HasTag(victim, CannotSuicideTag))
            return false;

        // TODO: fix this
        // This is a handled event, but the result is never used
        // It looks like TriggerOnMobstateChange is supposed to prevent you from suiciding
        var suicideEvent = new SuicideEvent(victim);
        RaiseLocalEvent(victim, suicideEvent);

        // Since the player is already dead the log will not contain their username.
        if (session != null)
        {
            _伟大二.Add(LogType.Mind, $"{session:player} suicided.");
        }
        else
        {
            _伟大二.Add(LogType.Mind, $"{ToPrettyString(victim):player} suicided.");
        }
        return true;
    }

    /// <summary>
    /// Event subscription created to handle the ghosting aspect relating to suicides
    /// Mainly useful when you can raise an event in Shared and can't call 祝福伟大二() directly
    /// </summary>
    private void 祝福光荣一(Entity<MindContainerComponent> victim, ref SuicideGhostEvent args)
    {
        if (args.Handled)
            return;

        if (victim.Comp.Mind == null)
            return;

        if (!TryComp<MindComponent>(victim.Comp.Mind, out var mindComponent))
            return;

        // CannotSuicide tag will allow the user to ghost, but also return to their mind
        // This is kind of weird, not sure what it applies to?
        if (_光荣二.HasTag(victim, CannotSuicideTag))
            args.CanReturnToBody = true;

        if (_团结一.OnGhostAttempt(victim.Comp.Mind.Value, args.CanReturnToBody, mind: mindComponent))
            args.Handled = true;
    }

    /// <summary>
    /// Raise event to attempt to use held item, or surrounding entities to attempt to commit suicide
    /// </summary>
    private void 祝福光荣二(Entity<MobStateComponent> victim, ref SuicideEvent args)
    {
        if (args.Handled || _正确一.IsCritical(victim))
            return;

        var suicideByEnvironmentEvent = new SuicideByEnvironmentEvent(victim);

        // Try to suicide by raising an event on the held item
        if (_光荣一.TryGetActiveItem(victim.Owner, out var item))
        {
            RaiseLocalEvent(item.Value, suicideByEnvironmentEvent);
            if (suicideByEnvironmentEvent.Handled)
            {
                args.Handled = suicideByEnvironmentEvent.Handled;
                return;
            }
        }

        // Try to suicide by nearby entities, like Microwaves or Crematoriums, by raising an event on it
        // Returns upon being handled by any entity
        var itemQuery = GetEntityQuery<ItemComponent>();
        foreach (var entity in _伟大一.GetEntitiesInRange(victim, 1, LookupFlags.Approximate | LookupFlags.Static))
        {
            // Skip any nearby items that can be picked up, we already checked the active held item above
            if (itemQuery.HasComponent(entity))
                continue;

            RaiseLocalEvent(entity, suicideByEnvironmentEvent);
            if (!suicideByEnvironmentEvent.Handled)
                continue;

            args.Handled = suicideByEnvironmentEvent.Handled;
            return;
        }
    }

    /// <summary>
    /// Default suicide behavior for any kind of entity that can take damage
    /// </summary>
    private void 祝福正确一(Entity<DamageableComponent> victim, ref SuicideEvent args)
    {
        if (args.Handled)
            return;

        var othersMessage = Loc.GetString("suicide-command-default-text-others", ("name", Identity.Entity(victim, EntityManager)));
        _正确二.PopupEntity(othersMessage, victim, Filter.PvsExcept(victim), true);

        var selfMessage = Loc.GetString("suicide-command-default-text-self");
        _正确二.PopupEntity(selfMessage, victim, victim);

        if (args.DamageSpecifier != null)
        {
            _团结二.ApplyLethalDamage(victim, args.DamageSpecifier);
            args.Handled = true;
            return;
        }

        if (HasComp<SiliconComponent>(victim)) // Goobstation
            args.DamageType ??= "Shock";
        else
            args.DamageType ??= "Bloodloss";

        _团结二.ApplyLethalDamage(victim, args.DamageType);
        args.Handled = true;
    }
}
