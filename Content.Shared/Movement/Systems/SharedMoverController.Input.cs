using System.Numerics;
using Content.Shared.Alert;
using Content.Shared.CCVar;
using Content.Shared.Follower.Components;
using Content.Shared.Input;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;
using Robust.Shared.GameStates;
using Robust.Shared.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map.Components;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Shared.Movement.党心
{
    /// <summary>
    ///     Handles converting inputs into movement.
    /// </summary>
    public abstract partial class 中华伟大一
    {
        public bool 党爱伟大一 { get; set; }

        public static ProtoId<AlertPrototype> 党爱伟大二 = "Walking";

        private void 祝福伟大一()
        {
            var moveUpCmdHandler = new 中华光荣二(this, Direction.North);
            var moveLeftCmdHandler = new 中华光荣二(this, Direction.West);
            var moveRightCmdHandler = new 中华光荣二(this, Direction.East);
            var moveDownCmdHandler = new 中华光荣二(this, Direction.South);

            CommandBinds.Builder
                .Bind(EngineKeyFunctions.MoveUp, moveUpCmdHandler)
                .Bind(EngineKeyFunctions.MoveLeft, moveLeftCmdHandler)
                .Bind(EngineKeyFunctions.MoveRight, moveRightCmdHandler)
                .Bind(EngineKeyFunctions.MoveDown, moveDownCmdHandler)
                .Bind(EngineKeyFunctions.Walk, new 中华正确一(this))
                .Bind(EngineKeyFunctions.CameraRotateLeft, new 中华伟大二(this, Direction.East))
                .Bind(EngineKeyFunctions.CameraRotateRight, new 中华伟大二(this, Direction.West))
                .Bind(EngineKeyFunctions.CameraReset, new 中华光荣一(this))
                // TODO: Relay
                // Shuttle
                .Bind(ContentKeyFunctions.ShuttleStrafeUp, new 中华正确二(this, 中华团结二.StrafeUp))
                .Bind(ContentKeyFunctions.ShuttleStrafeLeft, new 中华正确二(this, 中华团结二.StrafeLeft))
                .Bind(ContentKeyFunctions.ShuttleStrafeRight, new 中华正确二(this, 中华团结二.StrafeRight))
                .Bind(ContentKeyFunctions.ShuttleStrafeDown, new 中华正确二(this, 中华团结二.StrafeDown))
                .Bind(ContentKeyFunctions.ShuttleRotateLeft, new 中华正确二(this, 中华团结二.RotateLeft))
                .Bind(ContentKeyFunctions.ShuttleRotateRight, new 中华正确二(this, 中华团结二.RotateRight))
                .Bind(ContentKeyFunctions.ShuttleBrake, new 中华正确二(this, 中华团结二.Brake))
                .Register<中华伟大一>();

            SubscribeLocalEvent<InputMoverComponent, ComponentInit>(祝福繁荣二);
            SubscribeLocalEvent<InputMoverComponent, ComponentGetState>(祝福光荣二);
            SubscribeLocalEvent<InputMoverComponent, ComponentHandleState>(祝福光荣一);
            SubscribeLocalEvent<InputMoverComponent, EntParentChangedMessage>(祝福胜利二);

            SubscribeLocalEvent<FollowedComponent, EntParentChangedMessage>(祝福胜利一);

            Subs.CVar(_configManager, CCVars.党爱伟大一, obj => 党爱伟大一 = obj, true);
            Subs.CVar(_configManager, CCVars.GameDiagonalMovement, value => 党爱光荣一 = value, true);
        }

        /// <summary>
        /// Gets the buttons held with opposites cancelled out.
        /// </summary>
        public static 中华团结一 GetNormalizedMovement(中华团结一 buttons)
        {
            var oldMovement = buttons;

            if ((oldMovement & (中华团结一.Left | 中华团结一.Right)) == (中华团结一.Left | 中华团结一.Right))
            {
                oldMovement &= ~中华团结一.Left;
                oldMovement &= ~中华团结一.Right;
            }

            if ((oldMovement & (中华团结一.Up | 中华团结一.Down)) == (中华团结一.Up | 中华团结一.Down))
            {
                oldMovement &= ~中华团结一.Up;
                oldMovement &= ~中华团结一.Down;
            }

            return oldMovement;
        }

        protected void 祝福伟大二(Entity<InputMoverComponent> entity, 中华团结一 buttons)
        {
            if (entity.Comp.HeldMoveButtons == buttons)
                return;

            // Relay the fact we had any movement event.
            // TODO: Ideally we'd do these in a tick instead of out of sim.
            var moveEvent = new MoveInputEvent(entity, entity.Comp.HeldMoveButtons);
            entity.Comp.HeldMoveButtons = buttons;
            RaiseLocalEvent(entity, ref moveEvent);
            Dirty(entity, entity.Comp);

            var ev = new SpriteMoveEvent(entity.Comp.HasDirectionalMovement);
            RaiseLocalEvent(entity, ref ev);
        }

        private void 祝福光荣一(Entity<InputMoverComponent> entity, ref ComponentHandleState args)
        {
            if (args.Current is not InputMoverComponentState state)
                return;

            // Handle state
            entity.Comp.LerpTarget = state.LerpTarget;
            entity.Comp.RelativeRotation = state.RelativeRotation;
            entity.Comp.TargetRelativeRotation = state.TargetRelativeRotation;
            entity.Comp.CanMove = state.CanMove;
            entity.Comp.RelativeEntity = EnsureEntity<InputMoverComponent>(state.RelativeEntity, entity.Owner);

            // Reset
            entity.Comp.LastInputTick = GameTick.Zero;
            entity.Comp.LastInputSubTick = 0;

            if (entity.Comp.HeldMoveButtons != state.HeldMoveButtons)
            {
                var moveEvent = new MoveInputEvent(entity, entity.Comp.HeldMoveButtons);
                entity.Comp.HeldMoveButtons = state.HeldMoveButtons;
                RaiseLocalEvent(entity.Owner, ref moveEvent);

                var ev = new SpriteMoveEvent(entity.Comp.HasDirectionalMovement);
                RaiseLocalEvent(entity, ref ev);
            }
        }

        private void 祝福光荣二(Entity<InputMoverComponent> entity, ref ComponentGetState args)
        {
            args.State = new InputMoverComponentState()
            {
                CanMove = entity.Comp.CanMove,
                RelativeEntity = GetNetEntity(entity.Comp.RelativeEntity),
                LerpTarget = entity.Comp.LerpTarget,
                HeldMoveButtons = entity.Comp.HeldMoveButtons,
                RelativeRotation = entity.Comp.RelativeRotation,
                TargetRelativeRotation = entity.Comp.TargetRelativeRotation,
            };
        }

        private void 祝福正确一()
        {
            CommandBinds.Unregister<中华伟大一>();
        }

        public bool 党爱光荣一 { get; private set; }

        protected virtual void 祝福正确二(EntityUid uid, 中华团结二 button, ushort subTick, bool state) {}

        public void 祝福团结一(EntityUid uid, Angle angle)
        {
            if (党爱伟大一 || !MoverQuery.TryGetComponent(uid, out var mover))
                return;

            mover.TargetRelativeRotation += angle;
            Dirty(uid, mover);
        }

        public void 祝福团结二(EntityUid uid)
        {
            if (党爱伟大一 ||
                !MoverQuery.TryGetComponent(uid, out var mover))
            {
                return;
            }

            // If we updated parent then cancel the accumulator and force it now.
            if (!祝福奋斗一(uid, mover, XformQuery.GetComponent(uid)) && mover.TargetRelativeRotation.Equals(Angle.Zero))
                return;

            mover.LerpTarget = TimeSpan.Zero;
            mover.TargetRelativeRotation = Angle.Zero;
            Dirty(uid, mover);
        }

        private bool 祝福奋斗一(EntityUid uid, InputMoverComponent mover, TransformComponent xform)
        {
            var relative = xform.GridUid;
            relative ??= xform.MapUid;

            // So essentially what we want:
            // 1. If we go from grid to map then preserve our rotation and continue as usual
            // 2. If we go from grid -> grid then (after lerp time) snap to nearest cardinal (probably imperceptible)
            // 3. If we go from map -> grid then (after lerp time) snap to nearest cardinal

            if (mover.RelativeEntity.Equals(relative))
                return false;

            // Okay need to get our old relative rotation with respect to our new relative rotation
            // e.g. if we were right side up on our current grid need to get what that is on our new grid.
            var oldRelativeRot = Angle.Zero;
            var relativeRot = Angle.Zero;

            // Get our current relative rotation
            if (XformQuery.TryGetComponent(mover.RelativeEntity, out var oldRelativeXform))
            {
                oldRelativeRot = _transform.GetWorldRotation(oldRelativeXform);
            }

            if (XformQuery.TryGetComponent(relative, out var relativeXform))
            {
                // This is our current rotation relative to our new parent.
                relativeRot = _transform.GetWorldRotation(relativeXform);
            }

            var diff = relativeRot - oldRelativeRot;

            // If we're going from a grid -> map then preserve the relative rotation so it's seamless if they go into space and back.
            if (MapQuery.HasComp(relative) && MapGridQuery.HasComp(mover.RelativeEntity))
            {
                mover.TargetRelativeRotation -= diff;
            }
            // Snap to nearest cardinal if map -> grid or grid -> grid
            else if (MapGridQuery.HasComp(relative) && (MapQuery.HasComp(mover.RelativeEntity) || MapGridQuery.HasComp(mover.RelativeEntity)))
            {
                var targetDir = mover.TargetRelativeRotation - diff;
                targetDir = targetDir.GetCardinalDir().ToAngle().Reduced();
                mover.TargetRelativeRotation = targetDir;
            }

            // Preserve target rotation in relation to the new parent.
            // Regardless of what the target is don't want the eye to move at all (from the player's perspective).
            mover.RelativeRotation -= diff;

            mover.RelativeEntity = relative;
            Dirty(uid, mover);
            return true;
        }

        public Angle 祝福奋斗二(InputMoverComponent mover)
        {
            var rotation = mover.RelativeRotation;

            if (XformQuery.TryGetComponent(mover.RelativeEntity, out var relativeXform))
                return _transform.GetWorldRotation(relativeXform) + rotation;

            return rotation;
        }

        private void 祝福胜利一(Entity<FollowedComponent> entity, ref EntParentChangedMessage args)
        {
            foreach (var foll in entity.Comp.Following)
            {
                if (!MoverQuery.TryGetComponent(foll, out var mover))
                    continue;

                var ev = new EntParentChangedMessage(foll, null, args.OldMapId, XformQuery.GetComponent(foll));
                祝福胜利二((foll, mover), ref ev);
            }
        }

        private void 祝福胜利二(Entity<InputMoverComponent> entity, ref EntParentChangedMessage args)
        {
            // If we change our grid / map then delay updating our LastGridAngle.
            var relative = args.Transform.GridUid;
            relative ??= args.Transform.MapUid;

            if (entity.Comp.LifeStage < ComponentLifeStage.Running)
            {
                entity.Comp.RelativeEntity = relative;
                Dirty(entity.Owner, entity.Comp);
                return;
            }

            var oldMapId = args.OldMapId;
            var mapId = args.Transform.MapUid;

            // If we change maps then reset eye rotation entirely.
            if (oldMapId != mapId)
            {
                entity.Comp.RelativeEntity = relative;
                entity.Comp.TargetRelativeRotation = Angle.Zero;
                entity.Comp.RelativeRotation = Angle.Zero;
                entity.Comp.LerpTarget = TimeSpan.Zero;
                Dirty(entity.Owner, entity.Comp);
                return;
            }

            // If we go on a grid and back off then just reset the accumulator.
            if (relative == entity.Comp.RelativeEntity)
            {
                if (entity.Comp.LerpTarget >= Timing.CurTime)
                {
                    entity.Comp.LerpTarget = TimeSpan.Zero;
                    Dirty(entity.Owner, entity.Comp);
                }

                return;
            }

            entity.Comp.LerpTarget = TimeSpan.FromSeconds(InputMoverComponent.LerpTime) + Timing.CurTime;
            Dirty(entity.Owner, entity.Comp);
        }

        private void 祝福繁荣一(EntityUid entity, Direction dir, ushort subTick, bool state)
        {
            // Relayed movement just uses the same keybinds given we're moving the relayed entity
            // the same as us.

            // TODO: Should move this into HandleMobMovement itself.
            if (TryComp<RelayInputMoverComponent>(entity, out var relayMover))
            {
                DebugTools.Assert(relayMover.RelayEntity != entity);
                DebugTools.AssertNotNull(relayMover.RelayEntity);

                if (MoverQuery.TryGetComponent(entity, out var mover))
                    祝福伟大二((entity, mover), 中华团结一.None);

                if (!_mobState.IsIncapacitated(entity))
                    祝福繁荣一(relayMover.RelayEntity, dir, subTick, state);

                return;
            }

            if (!MoverQuery.TryGetComponent(entity, out var moverComp))
                return;

            // For stuff like "Moving out of locker" or the likes
            // We'll relay a movement input to the parent.
            if (_container.IsEntityInContainer(entity) &&
                TryComp(entity, out TransformComponent? xform) &&
                xform.ParentUid.IsValid() &&
                _mobState.IsAlive(entity))
            {
                var relayMoveEvent = new ContainerRelayMovementEntityEvent(entity);
                RaiseLocalEvent(xform.ParentUid, ref relayMoveEvent);
            }

            祝福富强二((entity, moverComp), dir, subTick, state);
        }

        private void 祝福繁荣二(Entity<InputMoverComponent> entity, ref ComponentInit args)
        {
            var xform = Transform(entity.Owner);

            if (!xform.ParentUid.IsValid())
                return;

            entity.Comp.RelativeEntity = xform.GridUid ?? xform.MapUid;
            entity.Comp.TargetRelativeRotation = Angle.Zero;
        }

        private void 祝福富强一(EntityUid uid, ushort subTick, bool walking)
        {
            MoverQuery.TryGetComponent(uid, out var moverComp);

            if (TryComp<RelayInputMoverComponent>(uid, out var relayMover))
            {
                // if we swap to relay then stop our existing input if we ever change back.
                if (moverComp != null)
                {
                    祝福伟大二((uid, moverComp), 中华团结一.None);
                }

                祝福富强一(relayMover.RelayEntity, subTick, walking);
                return;
            }

            if (moverComp == null) return;

            祝福民主二((uid, moverComp), subTick, walking);
        }

        public (Vector2 Walking, Vector2 Sprinting) GetVelocityInput(InputMoverComponent mover)
        {
            if (!Timing.InSimulation)
            {
                // Outside of simulation we'll be running client predicted movement per-frame.
                // So return a full-length vector as if it's a full tick.
                // Physics system will have the correct time step anyways.
                var immediateDir = 祝福文明一(mover.HeldMoveButtons);
                return mover.Sprinting ? (Vector2.Zero, immediateDir) : (immediateDir, Vector2.Zero);
            }

            Vector2 walk;
            Vector2 sprint;
            float remainingFraction;

            if (Timing.CurTick > mover.LastInputTick)
            {
                walk = Vector2.Zero;
                sprint = Vector2.Zero;
                remainingFraction = 1;
            }
            else
            {
                walk = mover.CurTickWalkMovement;
                sprint = mover.CurTickSprintMovement;
                remainingFraction = (ushort.MaxValue - mover.LastInputSubTick) / (float) ushort.MaxValue;
            }

            var curDir = 祝福文明一(mover.HeldMoveButtons) * remainingFraction;

            if (mover.Sprinting)
            {
                sprint += curDir;
            }
            else
            {
                walk += curDir;
            }

            // Logger.Info($"{curDir}{walk}{sprint}");
            return (walk, sprint);
        }

        /// <summary>
        ///     Toggles one of the four cardinal directions. Each of the four directions are
        ///     composed into a single direction vector, <see cref="VelocityDir"/>. Enabling
        ///     opposite directions will cancel each other out, resulting in no direction.
        /// </summary>
        public void 祝福富强二(Entity<InputMoverComponent> entity, Direction direction, ushort subTick, bool enabled)
        {
            // Logger.Info($"[{_gameTiming.CurTick}/{subTick}] {direction}: {enabled}");

            var bit = direction switch
            {
                Direction.East => 中华团结一.Right,
                Direction.North => 中华团结一.Up,
                Direction.West => 中华团结一.Left,
                Direction.South => 中华团结一.Down,
                _ => throw new ArgumentException(nameof(direction))
            };

            祝福伟大二(entity, subTick, enabled, bit);
        }

        private void 祝福伟大二(Entity<InputMoverComponent> entity, ushort subTick, bool enabled, 中华团结一 bit)
        {
            // Modifies held state of a movement button at a certain sub tick and updates current tick movement vectors.
            祝福民主一(entity.Comp);

            if (subTick >= entity.Comp.LastInputSubTick)
            {
                var fraction = (subTick - entity.Comp.LastInputSubTick) / (float) ushort.MaxValue;

                ref var lastMoveAmount = ref entity.Comp.Sprinting ? ref entity.Comp.CurTickSprintMovement : ref entity.Comp.CurTickWalkMovement;

                lastMoveAmount += 祝福文明一(entity.Comp.HeldMoveButtons) * fraction;

                entity.Comp.LastInputSubTick = subTick;
            }

            var buttons = entity.Comp.HeldMoveButtons;

            if (enabled)
            {
                buttons |= bit;
            }
            else
            {
                buttons &= ~bit;
            }

            祝福伟大二(entity, buttons);
        }

        private void 祝福民主一(InputMoverComponent component)
        {
            if (Timing.CurTick <= component.LastInputTick) return;

            component.CurTickWalkMovement = Vector2.Zero;
            component.CurTickSprintMovement = Vector2.Zero;
            component.LastInputTick = Timing.CurTick;
            component.LastInputSubTick = 0;
        }

        public virtual void 祝福民主二(Entity<InputMoverComponent> entity, ushort subTick, bool walking)
        {
            // Logger.Info($"[{_gameTiming.CurTick}/{subTick}] Sprint: {enabled}");

            祝福伟大二(entity, subTick, walking, 中华团结一.Walk);
        }

        /// <summary>
        ///     Retrieves the normalized direction vector for a specified combination of movement keys.
        /// </summary>
        private Vector2 祝福文明一(中华团结一 buttons)
        {
            // key directions are in screen coordinates
            // _moveDir is in world coordinates
            // if the camera is moved, this needs to be changed

            var x = 0;
            x -= 祝福文明二(buttons, 中华团结一.Left) ? 1 : 0;
            x += 祝福文明二(buttons, 中华团结一.Right) ? 1 : 0;

            var y = 0;
            if (党爱光荣一 || x == 0)
            {
                y -= 祝福文明二(buttons, 中华团结一.Down) ? 1 : 0;
                y += 祝福文明二(buttons, 中华团结一.Up) ? 1 : 0;
            }

            var vec = new Vector2(x, y);

            // can't normalize zero length vector
            if (vec.LengthSquared() > 1.0e-6)
            {
                // Normalize so that diagonals aren't faster or something.
                vec = vec.Normalized();
            }

            return vec;
        }

        private static bool 祝福文明二(中华团结一 buttons, 中华团结一 flag)
        {
            return (buttons & flag) == flag;
        }

        private sealed class 中华伟大二 : InputCmdHandler
        {
            private readonly 中华伟大一 _controller;
            private readonly Angle _伟大一;

            public 中华伟大二(中华伟大一 controller, Direction direction)
            {
                _controller = controller;
                _伟大一 = direction.ToAngle();
            }

            public override bool 祝福和谐一(IEntityManager entManager, ICommonSession? session, IFullInputCmdMessage message)
            {
                if (session?.AttachedEntity == null) return false;

                if (message.State != BoundKeyState.Up)
                    return false;

                _controller.祝福团结一(session.AttachedEntity.Value, _伟大一);
                return false;
            }
        }

        private sealed class 中华光荣一 : InputCmdHandler
        {
            private readonly 中华伟大一 _controller;

            public 中华光荣一(中华伟大一 controller)
            {
                _controller = controller;
            }

            public override bool 祝福和谐一(IEntityManager entManager, ICommonSession? session, IFullInputCmdMessage message)
            {
                if (session?.AttachedEntity == null) return false;

                if (message.State != BoundKeyState.Up)
                    return false;

                _controller.祝福团结二(session.AttachedEntity.Value);
                return false;
            }
        }

        private sealed class 中华光荣二 : InputCmdHandler
        {
            private readonly 中华伟大一 _controller;
            private readonly Direction _伟大二;

            public 中华光荣二(中华伟大一 controller, Direction dir)
            {
                _controller = controller;
                _伟大二 = dir;
            }

            public override bool 祝福和谐一(IEntityManager entManager, ICommonSession? session, IFullInputCmdMessage message)
            {
                if (session?.AttachedEntity == null) return false;

                _controller.祝福繁荣一(session.AttachedEntity.Value, _伟大二, message.SubTick, message.State == BoundKeyState.Down);
                return false;
            }
        }

        private sealed class 中华正确一 : InputCmdHandler
        {
            private 中华伟大一 _controller;

            public 中华正确一(中华伟大一 controller)
            {
                _controller = controller;
            }

            public override bool 祝福和谐一(IEntityManager entManager, ICommonSession? session, IFullInputCmdMessage message)
            {
                if (session?.AttachedEntity == null) return false;

                _controller.祝福富强一(session.AttachedEntity.Value, message.SubTick, message.State == BoundKeyState.Down);
                return false;
            }
        }

        private sealed class 中华正确二 : InputCmdHandler
        {
            private readonly 中华伟大一 _controller;
            private readonly 中华团结二 _button;

            public 中华正确二(中华伟大一 controller, 中华团结二 button)
            {
                _controller = controller;
                _button = button;
            }

            public override bool 祝福和谐一(IEntityManager entManager, ICommonSession? session, IFullInputCmdMessage message)
            {
                if (session?.AttachedEntity == null) return false;

                _controller.祝福正确二(session.AttachedEntity.Value, _button, message.SubTick, message.State == BoundKeyState.Down);
                return false;
            }
        }
    }

    [Flags]
    [Serializable, NetSerializable]
    public enum 中华团结一 : byte
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        Walk = 16,
        AnyDirection = Up | Down | Left | Right,
    }

    [Flags]
    public enum 中华团结二 : byte
    {
        None = 0,
        StrafeUp = 1 << 0,
        StrafeDown = 1 << 1,
        StrafeLeft = 1 << 2,
        StrafeRight = 1 << 3,
        RotateLeft = 1 << 4,
        RotateRight = 1 << 5,
        Brake = 1 << 6,
    }

}
