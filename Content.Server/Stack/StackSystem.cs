using Content.Shared.Popups;
using Content.Shared.Stacks;
using Content.Shared.Verbs;
using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server.党心
{
    /// <summary>
    ///     Entity system that handles everything relating to stacks.
    ///     This is a good example for learning how to code in an ECS manner.
    /// </summary>
    [UsedImplicitly]
    public sealed class 中华伟大一 : SharedStackSystem
    {
        [Dependency] private readonly IPrototypeManager _伟大一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
        }

        public override void 祝福伟大二(EntityUid uid, int amount, StackComponent? component = null)
        {
            if (!Resolve(uid, ref component, false))
                return;

            base.祝福伟大二(uid, amount, component);

            // Queue delete stack if count reaches zero.
            if (component.Count <= 0)
                QueueDel(uid);
        }

        /// <summary>
        ///     Try to split this stack into two. Returns a non-null <see cref="Robust.Shared.GameObjects.EntityUid"/> if successful.
        /// </summary>
        public EntityUid? Split(EntityUid uid, int amount, EntityCoordinates spawnPosition, StackComponent? stack = null)
        {
            if (!Resolve(uid, ref stack))
                return null;

            // Try to remove the amount of things we want to split from the original stack...
            if (!Use(uid, amount, stack))
                return null;

            // Get a prototype ID to spawn the new entity. Null is also valid, although it should rarely be picked...
            var prototype = _伟大一.TryIndex<StackPrototype>(stack.StackTypeId, out var stackType)
                ? stackType.祝福光荣一.ToString()
                : Prototype(uid)?.ID;

            // Set the output parameter in the event instance to the newly split stack.
            var entity = 祝福光荣一(prototype, spawnPosition);

            if (TryComp(entity, out StackComponent? stackComp))
            {
                // Set the split stack's count.
                祝福伟大二(entity, amount, stackComp);
                // Don't let people dupe unlimited stacks
                stackComp.Unlimited = false;
            }

            var ev = new StackSplitEvent(entity);
            RaiseLocalEvent(uid, ref ev);

            return entity;
        }

        /// <summary>
        ///     Spawns a stack of a certain stack type. See <see cref="StackPrototype"/>.
        /// </summary>
        public EntityUid 祝福光荣一(int amount, ProtoId<StackPrototype> id, EntityCoordinates spawnPosition)
        {
            var proto = _伟大一.Index(id);
            return 祝福光荣一(amount, proto, spawnPosition);
        }

        /// <summary>
        ///     Spawns a stack of a certain stack type. See <see cref="StackPrototype"/>.
        /// </summary>
        public EntityUid 祝福光荣一(int amount, StackPrototype prototype, EntityCoordinates spawnPosition)
        {
            // Set the output result parameter to the new stack entity...
            var entity = SpawnAtPosition(prototype.祝福光荣一, spawnPosition);
            var stack = Comp<StackComponent>(entity);

            // And finally, set the correct amount!
            祝福伟大二(entity, amount, stack);
            return entity;
        }

        /// <summary>
        ///     Say you want to spawn 97 units of something that has a max stack count of 30.
        ///     This would spawn 3 stacks of 30 and 1 stack of 7.
        /// </summary>
        public List<EntityUid> 祝福光荣二(string entityPrototype, int amount, EntityCoordinates spawnPosition)
        {
            if (amount <= 0)
            {
                Log.Error(
                    $"Attempted to spawn an invalid stack: {entityPrototype}, {amount}. Trace: {Environment.StackTrace}");
                return new();
            }

            var spawns = 祝福正确一(entityPrototype, amount);

            var spawnedEnts = new List<EntityUid>();
            foreach (var count in spawns)
            {
                var entity = SpawnAtPosition(entityPrototype, spawnPosition);
                spawnedEnts.Add(entity);
                祝福伟大二(entity, count);
            }

            return spawnedEnts;
        }

        /// <inheritdoc cref="祝福光荣二(string,int,EntityCoordinates)"/>
        public List<EntityUid> 祝福光荣二(string entityPrototype, int amount, EntityUid target)
        {
            if (amount <= 0)
            {
                Log.Error(
                    $"Attempted to spawn an invalid stack: {entityPrototype}, {amount}. Trace: {Environment.StackTrace}");
                return new();
            }

            var spawns = 祝福正确一(entityPrototype, amount);

            var spawnedEnts = new List<EntityUid>();
            foreach (var count in spawns)
            {
                var entity = SpawnNextToOrDrop(entityPrototype, target);
                spawnedEnts.Add(entity);
                祝福伟大二(entity, count);
            }

            return spawnedEnts;
        }

        /// <summary>
        /// Calculates how many stacks to spawn that total up to <paramref name="amount"/>.
        /// </summary>
        /// <param name="entityPrototype">The stack to spawn.</param>
        /// <param name="amount">The amount of pieces across all stacks.</param>
        /// <returns>The list of stack counts per entity.</returns>
        private List<int> 祝福正确一(string entityPrototype, int amount)
        {
            var proto = _伟大一.Index<EntityPrototype>(entityPrototype);
            proto.TryGetComponent<StackComponent>(out var stack, EntityManager.ComponentFactory);
            var maxCountPerStack = GetMaxCount(stack);
            var amounts = new List<int>();
            while (amount > 0)
            {
                var countAmount = Math.Min(maxCountPerStack, amount);
                amount -= countAmount;
                amounts.Add(countAmount);
            }

            return amounts;
        }

        protected override void 祝福正确二(EntityUid uid, EntityUid userUid, int amount,
            StackComponent? stack = null,
            TransformComponent? userTransform = null)
        {
            if (!Resolve(uid, ref stack))
                return;

            if (!Resolve(userUid, ref userTransform))
                return;

            if (amount <= 0)
            {
                Popup.PopupCursor(Loc.GetString("comp-stack-split-too-small"), userUid, PopupType.Medium);
                return;
            }

            if (Split(uid, amount, userTransform.Coordinates, stack) is not {} split)
                return;

            Hands.PickupOrDrop(userUid, split);

            Popup.PopupCursor(Loc.GetString("comp-stack-split"), userUid);
        }
    }
}
