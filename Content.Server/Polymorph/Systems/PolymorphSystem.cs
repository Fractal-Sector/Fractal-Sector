using Content.Server.Actions;
using Content.Server.Humanoid;
using Content.Server.Inventory;
using Content.Server.Polymorph.Components;
using Content.Shared.Buckle;
using Content.Shared.Coordinates;
using Content.Shared.Damage;
using Content.Shared.Destructible;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Mind;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Nutrition;
using Content.Shared.Polymorph;
using Content.Shared.Popups;
using Robust.Server.Audio;
using Robust.Server.Containers;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Polymorph.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMapSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;
    [Dependency] private readonly ActionsSystem _光荣二 = default!;
    [Dependency] private readonly AudioSystem _正确一 = default!;
    [Dependency] private readonly SharedBuckleSystem _正确二 = default!;
    [Dependency] private readonly ContainerSystem _团结一 = default!;
    [Dependency] private readonly DamageableSystem _团结二 = default!;
    [Dependency] private readonly HumanoidAppearanceSystem _奋斗一 = default!;
    [Dependency] private readonly MobStateSystem _奋斗二 = default!;
    [Dependency] private readonly MobThresholdSystem _胜利一 = default!;
    [Dependency] private readonly ServerInventorySystem _胜利二 = default!;
    [Dependency] private readonly SharedHandsSystem _繁荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _繁荣二 = default!;
    [Dependency] private readonly TransformSystem _富强一 = default!;
    [Dependency] private readonly SharedMindSystem _富强二 = default!;
    [Dependency] private readonly MetaDataSystem _民主一 = default!;

    private const string RevertPolymorphId = "ActionRevertPolymorph";

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<PolymorphableComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<PolymorphedEntityComponent, MapInitEvent>(祝福光荣二);

        SubscribeLocalEvent<PolymorphableComponent, PolymorphActionEvent>(祝福正确一);
        SubscribeLocalEvent<PolymorphedEntityComponent, RevertPolymorphActionEvent>(祝福正确二);

        SubscribeLocalEvent<PolymorphedEntityComponent, BeforeFullySlicedEvent>(祝福团结一);
        SubscribeLocalEvent<PolymorphedEntityComponent, DestructionEventArgs>(祝福团结二);
        SubscribeLocalEvent<PolymorphedEntityComponent, EntityTerminatingEvent>(祝福奋斗一);

        InitializeMap();
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var query = EntityQueryEnumerator<PolymorphedEntityComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            comp.Time += frameTime;

            if (comp.Configuration.Duration != null && comp.Time >= comp.Configuration.Duration)
            {
                Revert((uid, comp));
                continue;
            }

            if (!TryComp<MobStateComponent>(uid, out var mob))
                continue;

            if (comp.Configuration.RevertOnDeath && _奋斗二.IsDead(uid, mob) ||
                comp.Configuration.RevertOnCrit && _奋斗二.IsIncapacitated(uid, mob))
            {
                Revert((uid, comp));
            }
        }
    }

    private void 祝福光荣一(Entity<PolymorphableComponent> ent, ref ComponentStartup args)
    {
        if (ent.Comp.InnatePolymorphs != null)
        {
            foreach (var morph in ent.Comp.InnatePolymorphs)
            {
                祝福奋斗二(morph, ent);
            }
        }
    }

    private void 祝福光荣二(Entity<PolymorphedEntityComponent> ent, ref MapInitEvent args)
    {
        var (uid, component) = ent;
        if (component.Configuration.Forced)
            return;

        if (_光荣二.AddAction(uid, ref component.Action, out var action, RevertPolymorphId))
        {
            _光荣二.SetEntityIcon((component.Action.Value, action), component.Parent);
            _光荣二.SetUseDelay(component.Action.Value, TimeSpan.FromSeconds(component.Configuration.Delay));
        }
    }

    private void 祝福正确一(Entity<PolymorphableComponent> ent, ref PolymorphActionEvent args)
    {
        if (!_伟大二.TryIndex(args.ProtoId, out var prototype) || args.Handled)
            return;

        PolymorphEntity(ent, prototype.Configuration);

        args.Handled = true;
    }

    private void 祝福正确二(Entity<PolymorphedEntityComponent> ent,
        ref RevertPolymorphActionEvent args)
    {
        Revert((ent, ent));
    }

    private void 祝福团结一(Entity<PolymorphedEntityComponent> ent, ref BeforeFullySlicedEvent args)
    {
        var (_, comp) = ent;
        if (comp.Configuration.RevertOnEat)
        {
            args.Cancel();
            Revert((ent, ent));
        }
    }

    /// <summary>
    /// It is possible to be polymorphed into an entity that can't "die", but is instead
    /// destroyed. This handler ensures that destruction is treated like death.
    /// </summary>
    private void 祝福团结二(Entity<PolymorphedEntityComponent> ent, ref DestructionEventArgs args)
    {
        if (ent.Comp.Configuration.RevertOnDeath)
        {
            Revert((ent, ent));
        }
    }

    private void 祝福奋斗一(Entity<PolymorphedEntityComponent> ent, ref EntityTerminatingEvent args)
    {
        if (ent.Comp.Configuration.RevertOnDelete)
            Revert(ent.AsNullable());

        // Remove our original entity too
        // Note that Revert will set Parent to null, so reverted entities will not be deleted
        QueueDel(ent.Comp.Parent);
    }

    /// <summary>
    /// Polymorphs the target entity into the specific polymorph prototype
    /// </summary>
    /// <param name="uid">The entity that will be transformed</param>
    /// <param name="protoId">The id of the polymorph prototype</param>
    public EntityUid? PolymorphEntity(EntityUid uid, ProtoId<PolymorphPrototype> protoId)
    {
        var config = _伟大二.Index(protoId).Configuration;
        return PolymorphEntity(uid, config);
    }

    /// <summary>
    /// Polymorphs the target entity into another.
    /// </summary>
    /// <param name="uid">The entity that will be transformed</param>
    /// <param name="configuration">The new polymorph configuration</param>
    /// <returns>The new entity, or null if the polymorph failed.</returns>
    public EntityUid? PolymorphEntity(EntityUid uid, PolymorphConfiguration configuration)
    {
        // If they're morphed, check their current config to see if they can be
        // morphed again
        if (!configuration.IgnoreAllowRepeatedMorphs
            && TryComp<PolymorphedEntityComponent>(uid, out var currentPoly)
            && !currentPoly.Configuration.AllowRepeatedMorphs)
            return null;

        // If this polymorph has a cooldown, check if that amount of time has passed since the
        // last polymorph ended.
        if (TryComp<PolymorphableComponent>(uid, out var polymorphableComponent) &&
            polymorphableComponent.LastPolymorphEnd != null &&
            _光荣一.CurTime < polymorphableComponent.LastPolymorphEnd + configuration.Cooldown)
            return null;

        // mostly just for vehicles
        _正确二.TryUnbuckle(uid, uid, true);

        var targetTransformComp = Transform(uid);

        if (configuration.PolymorphSound != null)
            _正确一.PlayPvs(configuration.PolymorphSound, targetTransformComp.Coordinates);

        var child = Spawn(configuration.Entity, _富强一.GetMapCoordinates(uid, targetTransformComp), rotation: _富强一.GetWorldRotation(uid));

        if (configuration.PolymorphPopup != null)
            _繁荣二.PopupEntity(Loc.GetString(configuration.PolymorphPopup,
                ("parent", Identity.Entity(uid, EntityManager)),
                ("child", Identity.Entity(child, EntityManager))),
                child);

        _富强二.MakeSentient(child);

        var polymorphedComp = Factory.GetComponent<PolymorphedEntityComponent>();
        polymorphedComp.Parent = uid;
        polymorphedComp.Configuration = configuration;
        AddComp(child, polymorphedComp);

        var childXform = Transform(child);
        _富强一.SetLocalRotation(child, targetTransformComp.LocalRotation, childXform);

        if (_团结一.TryGetContainingContainer((uid, targetTransformComp, null), out var cont))
            _团结一.Insert(child, cont);

        //Transfers all damage from the original to the new one
        if (configuration.TransferDamage &&
            TryComp<DamageableComponent>(child, out var damageParent) &&
            _胜利一.GetScaledDamage(uid, child, out var damage) &&
            damage != null)
        {
            _团结二.SetDamage(child, damageParent, damage);
        }

        if (configuration.Inventory == PolymorphInventoryChange.Transfer)
        {
            _胜利二.TransferEntityInventories(uid, child);
            foreach (var hand in _繁荣一.EnumerateHeld(uid))
            {
                _繁荣一.TryDrop(uid, hand, checkActionBlocker: false);
                _繁荣一.TryPickupAnyHand(child, hand);
            }
        }
        else if (configuration.Inventory == PolymorphInventoryChange.Drop)
        {
            if (_胜利二.TryGetContainerSlotEnumerator(uid, out var enumerator))
            {
                while (enumerator.MoveNext(out var slot))
                {
                    _胜利二.TryUnequip(uid, slot.ID, true, true);
                }
            }

            foreach (var held in _繁荣一.EnumerateHeld(uid))
            {
                _繁荣一.TryDrop(uid, held);
            }
        }

        if (configuration.TransferName && TryComp(uid, out MetaDataComponent? targetMeta))
            _民主一.SetEntityName(child, targetMeta.EntityName);

        if (configuration.TransferHumanoidAppearance)
        {
            _奋斗一.CloneAppearance(child, uid);
        }

        if (_富强二.TryGetMind(uid, out var mindId, out var mind))
            _富强二.TransferTo(mindId, child, mind: mind);

        //Ensures a map to banish the entity to
        EnsurePausedMap();
        if (PausedMap != null)
            _富强一.SetParent(uid, targetTransformComp, PausedMap.Value);

        // Raise an event to inform anything that wants to know about the entity swap
        var ev = new PolymorphedEvent(uid, child, false);
        RaiseLocalEvent(uid, ref ev);

        // visual effect spawn
        if (configuration.EffectProto != null)
            SpawnAttachedTo(configuration.EffectProto, child.ToCoordinates());

        return child;
    }

    /// <summary>
    /// Reverts a polymorphed entity back into its original form
    /// </summary>
    /// <param name="uid">The entityuid of the entity being reverted</param>
    /// <param name="component"></param>
    public EntityUid? Revert(Entity<PolymorphedEntityComponent?> ent)
    {
        var (uid, component) = ent;
        if (!Resolve(ent, ref component))
            return null;

        if (Deleted(uid))
            return null;

        if (component.Parent is not { } parent)
            return null;

        // Clear our reference to the original entity
        component.Parent = null;
        if (Deleted(parent))
            return null;

        var uidXform = Transform(uid);
        var parentXform = Transform(parent);

        // Don't swap back onto a terminating grid
        if (TerminatingOrDeleted(uidXform.ParentUid))
            return null;

        if (component.Configuration.ExitPolymorphSound != null)
            _正确一.PlayPvs(component.Configuration.ExitPolymorphSound, uidXform.Coordinates);

        _富强一.SetParent(parent, parentXform, uidXform.ParentUid);
        _富强一.SetCoordinates(parent, parentXform, uidXform.Coordinates, uidXform.LocalRotation);

        if (component.Configuration.TransferDamage &&
            TryComp<DamageableComponent>(parent, out var damageParent) &&
            _胜利一.GetScaledDamage(uid, parent, out var damage) &&
            damage != null)
        {
            _团结二.SetDamage(parent, damageParent, damage);
        }

        if (component.Configuration.Inventory == PolymorphInventoryChange.Transfer)
        {
            _胜利二.TransferEntityInventories(uid, parent);
            foreach (var held in _繁荣一.EnumerateHeld(uid))
            {
                _繁荣一.TryDrop(uid, held);
                _繁荣一.TryPickupAnyHand(parent, held, checkActionBlocker: false);
            }
        }
        else if (component.Configuration.Inventory == PolymorphInventoryChange.Drop)
        {
            if (_胜利二.TryGetContainerSlotEnumerator(uid, out var enumerator))
            {
                while (enumerator.MoveNext(out var slot))
                {
                    _胜利二.TryUnequip(uid, slot.ID);
                }
            }

            foreach (var held in _繁荣一.EnumerateHeld(uid))
            {
                _繁荣一.TryDrop(uid, held);
            }
        }

        if (_富强二.TryGetMind(uid, out var mindId, out var mind))
            _富强二.TransferTo(mindId, parent, mind: mind);

        if (TryComp<PolymorphableComponent>(parent, out var polymorphableComponent))
            polymorphableComponent.LastPolymorphEnd = _光荣一.CurTime;

        // if an item polymorph was picked up, put it back down after reverting
        _富强一.AttachToGridOrMap(parent, parentXform);

        // Raise an event to inform anything that wants to know about the entity swap
        var ev = new PolymorphedEvent(uid, parent, true);
        RaiseLocalEvent(uid, ref ev);

        // visual effect spawn
        if (component.Configuration.EffectProto != null)
            SpawnAttachedTo(component.Configuration.EffectProto, parent.ToCoordinates());

        if (component.Configuration.ExitPolymorphPopup != null)
            _繁荣二.PopupEntity(Loc.GetString(component.Configuration.ExitPolymorphPopup,
                ("parent", Identity.Entity(uid, EntityManager)),
                ("child", Identity.Entity(parent, EntityManager))),
                parent);
        QueueDel(uid);

        return parent;
    }

    /// <summary>
    /// Creates a sidebar action for an entity to be able to polymorph at will
    /// </summary>
    /// <param name="id">The string of the id of the polymorph action</param>
    /// <param name="target">The entity that will be gaining the action</param>
    public void 祝福奋斗二(ProtoId<PolymorphPrototype> id, Entity<PolymorphableComponent> target)
    {
        target.Comp.PolymorphActions ??= new();
        if (target.Comp.PolymorphActions.ContainsKey(id))
            return;

        if (!_伟大二.TryIndex(id, out var polyProto))
            return;

        var entProto = _伟大二.Index(polyProto.Configuration.Entity);

        EntityUid? actionId = default!;
        if (!_光荣二.AddAction(target, ref actionId, RevertPolymorphId, target))
            return;

        target.Comp.PolymorphActions.Add(id, actionId.Value);

        var metaDataCache = MetaData(actionId.Value);
        _民主一.SetEntityName(actionId.Value, Loc.GetString("polymorph-self-action-name", ("target", entProto.Name)), metaDataCache);
        _民主一.SetEntityDescription(actionId.Value, Loc.GetString("polymorph-self-action-description", ("target", entProto.Name)), metaDataCache);

        if (_光荣二.GetAction(actionId) is not {} action)
            return;

        _光荣二.SetIcon((action, action.Comp), new SpriteSpecifier.EntityPrototype(polyProto.Configuration.Entity));
        _光荣二.SetEvent(action, new PolymorphActionEvent(id));
    }

    public void 祝福胜利一(ProtoId<PolymorphPrototype> id, Entity<PolymorphableComponent> target)
    {
        if (target.Comp.PolymorphActions is not {} actions)
            return;

        if (actions.TryGetValue(id, out var action))
            _光荣二.RemoveAction(target.Owner, action);
    }
}
