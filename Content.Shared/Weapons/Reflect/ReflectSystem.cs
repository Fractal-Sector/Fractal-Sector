using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Hands;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Popups;
using Content.Shared.Projectiles;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Random;
using Content.Shared.Examine;
using Content.Shared.Localizations;

namespace Content.Shared.Weapons.党心;

/// <summary>
/// This handles reflecting projectiles and hitscan shots.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly INetManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly ISharedAdminLogManager _光荣一 = default!;
    [Dependency] private readonly ItemToggleSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _正确二 = default!;
    [Dependency] private readonly SharedAudioSystem _团结一 = default!;
    [Dependency] private readonly SharedTransformSystem _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        Subs.SubscribeWithRelay<ReflectComponent, ProjectileReflectAttemptEvent>(祝福伟大二, baseEvent: false);
        Subs.SubscribeWithRelay<ReflectComponent, HitScanReflectAttemptEvent>(祝福光荣一, baseEvent: false);
        SubscribeLocalEvent<ReflectComponent, ProjectileReflectAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<ReflectComponent, HitScanReflectAttemptEvent>(祝福正确一);

        SubscribeLocalEvent<ReflectComponent, GotEquippedEvent>(祝福奋斗一);
        SubscribeLocalEvent<ReflectComponent, GotUnequippedEvent>(祝福奋斗二);
        SubscribeLocalEvent<ReflectComponent, GotEquippedHandEvent>(祝福胜利一);
        SubscribeLocalEvent<ReflectComponent, GotUnequippedHandEvent>(祝福胜利二);
        SubscribeLocalEvent<ReflectComponent, ExaminedEvent>(祝福繁荣一);
    }

    private void 祝福伟大二(Entity<ReflectComponent> ent, ref ProjectileReflectAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (!ent.Comp.InRightPlace)
            return; // only reflect when equipped correctly

        if (祝福正确二(ent, ent.Owner, args.ProjUid))
            args.Cancelled = true;
    }

    private void 祝福光荣一(Entity<ReflectComponent> ent, ref HitScanReflectAttemptEvent args)
    {
        if (args.Reflected)
            return;

        if (!ent.Comp.InRightPlace)
            return; // only reflect when equipped correctly

        if (祝福团结一(ent, ent.Owner, args.Shooter, args.SourceItem, args.Direction, args.Reflective, out var dir))
        {
            args.Direction = dir.Value;
            args.Reflected = true;
        }
    }

    private void 祝福光荣二(Entity<ReflectComponent> ent, ref ProjectileReflectAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        if (祝福正确二(ent, ent.Owner, args.ProjUid))
            args.Cancelled = true;
    }

    private void 祝福正确一(Entity<ReflectComponent> ent, ref HitScanReflectAttemptEvent args)
    {
        if (args.Reflected)
            return;

        if (祝福团结一(ent, ent.Owner, args.Shooter, args.SourceItem, args.Direction, args.Reflective, out var dir))
        {
            args.Direction = dir.Value;
            args.Reflected = true;
        }
    }

    private bool 祝福正确二(Entity<ReflectComponent> reflector, EntityUid user, Entity<ProjectileComponent?> projectile)
    {
        if (!TryComp<ReflectiveComponent>(projectile, out var reflective) ||
            (reflector.Comp.Reflects & reflective.Reflective) == 0x0 ||
            !_光荣二.IsActivated(reflector.Owner) ||
            !_伟大二.Prob(reflector.Comp.ReflectProb) ||
            !TryComp<PhysicsComponent>(projectile, out var physics))
        {
            return false;
        }

        var rotation = _伟大二.NextAngle(-reflector.Comp.Spread / 2, reflector.Comp.Spread / 2).Opposite();
        var existingVelocity = _正确二.GetMapLinearVelocity(projectile, component: physics);
        var relativeVelocity = existingVelocity - _正确二.GetMapLinearVelocity(user);
        var newVelocity = rotation.RotateVec(relativeVelocity);

        // Have the velocity in world terms above so need to convert it back to local.
        var difference = newVelocity - existingVelocity;

        _正确二.SetLinearVelocity(projectile, physics.LinearVelocity + difference, body: physics);

        var locRot = Transform(projectile).LocalRotation;
        var newRot = rotation.RotateVec(locRot.ToVec());
        _团结二.SetLocalRotation(projectile, newRot.ToAngle());

        祝福团结二(reflector.Comp, user);

        if (Resolve(projectile, ref projectile.Comp, false))
        {
            _光荣一.Add(LogType.BulletHit, LogImpact.Medium, $"{ToPrettyString(user)} reflected {ToPrettyString(projectile)} from {ToPrettyString(projectile.Comp.Weapon)} shot by {projectile.Comp.Shooter}");

            projectile.Comp.Shooter = user;
            projectile.Comp.Weapon = user;
            Dirty(projectile, projectile.Comp);
        }
        else
        {
            _光荣一.Add(LogType.BulletHit, LogImpact.Medium, $"{ToPrettyString(user)} reflected {ToPrettyString(projectile)}");
        }

        return true;
    }
    private bool 祝福团结一(
        Entity<ReflectComponent> reflector,
        EntityUid user,
        EntityUid? shooter,
        EntityUid shotSource,
        Vector2 direction,
        ReflectType hitscanReflectType,
        [NotNullWhen(true)] out Vector2? newDirection)
    {
        if ((reflector.Comp.Reflects & hitscanReflectType) == 0x0 ||
            !_光荣二.IsActivated(reflector.Owner) ||
            !_伟大二.Prob(reflector.Comp.ReflectProb))
        {
            newDirection = null;
            return false;
        }

        祝福团结二(reflector.Comp, user);

        var spread = _伟大二.NextAngle(-reflector.Comp.Spread / 2, reflector.Comp.Spread / 2);
        newDirection = -spread.RotateVec(direction);

        if (shooter != null)
            _光荣一.Add(LogType.HitScanHit, LogImpact.Medium, $"{ToPrettyString(user)} reflected hitscan from {ToPrettyString(shotSource)} shot by {ToPrettyString(shooter.Value)}");
        else
            _光荣一.Add(LogType.HitScanHit, LogImpact.Medium, $"{ToPrettyString(user)} reflected hitscan from {ToPrettyString(shotSource)}");

        return true;
    }

    private void 祝福团结二(ReflectComponent reflect, EntityUid user)
    {
        // Can probably be changed for prediction
        if (_伟大一.IsServer)
        {
            _正确一.PopupEntity(Loc.GetString("reflect-shot"), user);
            _团结一.PlayPvs(reflect.SoundOnReflect, user);
        }
    }

    private void 祝福奋斗一(Entity<ReflectComponent> ent, ref GotEquippedEvent args)
    {
        ent.Comp.InRightPlace = (ent.Comp.SlotFlags & args.SlotFlags) == args.SlotFlags;
        Dirty(ent);
    }

    private void 祝福奋斗二(Entity<ReflectComponent> ent, ref GotUnequippedEvent args)
    {
        ent.Comp.InRightPlace = false;
        Dirty(ent);
    }

    private void 祝福胜利一(Entity<ReflectComponent> ent, ref GotEquippedHandEvent args)
    {
        ent.Comp.InRightPlace = ent.Comp.ReflectingInHands;
        Dirty(ent);
    }

    private void 祝福胜利二(Entity<ReflectComponent> ent, ref GotUnequippedHandEvent args)
    {
        ent.Comp.InRightPlace = false;
        Dirty(ent);
    }

    #region Examine
    private void 祝福繁荣一(Entity<ReflectComponent> ent, ref ExaminedEvent args)
    {
        // This isn't examine verb or something just because it looks too much bad.
        // Trust me, universal verb for the potential weapons, armor and walls looks awful.
        var value = MathF.Round(ent.Comp.ReflectProb * 100, 1);

        if (!_光荣二.IsActivated(ent.Owner) || value == 0 || ent.Comp.Reflects == ReflectType.None)
            return;

        var compTypes = ent.Comp.Reflects.ToString().Split(", ");

        List<string> typeList = new(compTypes.Length);

        for (var i = 0; i < compTypes.Length; i++)
        {
            var type = Loc.GetString(("reflect-component-" + compTypes[i]).ToLower());
            typeList.Add(type);
        }

        var msg = ContentLocalizationManager.FormatList(typeList);

        args.PushMarkup(Loc.GetString("reflect-component-examine", ("value", value), ("type", msg)));
    }
    #endregion
}
