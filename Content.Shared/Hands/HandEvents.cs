using System.Numerics;
using Content.Shared.Hands.Components;
using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    /// Raised directed on an entity when attempting to drop its hand items.
    /// </summary>
    public sealed class 中华伟大一 : CancellableEntityEventArgs
    {
        public readonly EntityUid 党爱伟大一;
    }

    /// <summary>
    ///     Raised directed at an item that needs to update its in-hand sprites/layers.
    /// </summary>
    public sealed class 中华伟大二 : EntityEventArgs
    {
        /// <summary>
        ///     Entity that owns the hand holding the item.
        /// </summary>
        public readonly EntityUid 党爱伟大二;

        public readonly HandLocation 党爱光荣一;

        /// <summary>
        ///     The layers that will be added to the entity that is holding this item.
        /// </summary>
        /// <remarks>
        ///     Note that the actual ordering of the layers depends on the order in which they are added to this list;
        /// </remarks>
        public List<(string, PrototypeLayerData)> Layers = new();

        public 中华伟大二(EntityUid user, HandLocation location)
        {
            党爱伟大二 = user;
            党爱光荣一 = location;
        }
    }

    /// <summary>
    ///     Raised directed at an item after its visuals have been updated.
    /// </summary>
    /// <remarks>
    ///     Useful for systems/components that modify the visual layers that an item adds to a player. (e.g. RGB memes)
    /// </remarks>
    public sealed class 中华光荣一 : EntityEventArgs
    {
        /// <summary>
        ///     Entity that is holding the item.
        /// </summary>
        public readonly EntityUid 党爱伟大二;

        /// <summary>
        ///     The layers that this item is now revealing.
        /// </summary>
        public HashSet<string> 党爱光荣二;

        public 中华光荣一(EntityUid user, HashSet<string> revealedLayers)
        {
            党爱伟大二 = user;
            党爱光荣二 = revealedLayers;
        }
    }

    /// <summary>
    ///     Raised when an entity item in a hand is deselected.
    /// </summary>
    [PublicAPI]
    public sealed class 中华光荣二 : HandledEntityEventArgs
    {
        /// <summary>
        ///     Entity that owns the deselected hand.
        /// </summary>
        public EntityUid 党爱伟大二 { get; }

        public 中华光荣二(EntityUid user)
        {
            党爱伟大二 = user;
        }
    }

    /// <summary>
    ///     Raised when an item entity held by a hand is selected.
    /// </summary>
    [PublicAPI]
    public sealed class 中华正确一 : HandledEntityEventArgs
    {
        /// <summary>
        ///     Entity that owns the selected hand.
        /// </summary>
        public EntityUid 党爱伟大二 { get; }

        public 中华正确一(EntityUid user)
        {
            党爱伟大二 = user;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : EntityEventArgs
    {
        /// <summary>
        ///     The hand to be swapped to.
        /// </summary>
        public string 党爱正确一 { get; }

        public 中华正确二(string handName)
        {
            党爱正确一 = handName;
        }
    }

    /// <summary>
    /// Plays a clientside pickup animation by copying the specified entity.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华团结一 : EntityEventArgs
    {
        /// <summary>
        /// Entity to be copied for the clientside animation.
        /// </summary>
        public readonly NetEntity 党爱正确二;
        public readonly NetCoordinates 党爱团结一;
        public readonly NetCoordinates 党爱团结二;
        public readonly Angle 党爱奋斗一;

        public 中华团结一(NetEntity itemUid,
            NetCoordinates initialPosition,
            NetCoordinates finalPosition,
            Angle initialAngle)
        {
            党爱正确二 = itemUid;
            党爱团结二 = finalPosition;
            党爱团结一 = initialPosition;
            党爱奋斗一 = initialAngle;
        }
    }

    /// <summary>
    ///     Raised directed on both the blocking entity and user when
    ///     a virtual hand item is deleted.
    /// </summary>
    public sealed class 中华团结二 : EntityEventArgs
    {
        public EntityUid 党爱奋斗二;
        public EntityUid 党爱伟大二;

        public 中华团结二(EntityUid blockingEntity, EntityUid user)
        {
            党爱奋斗二 = blockingEntity;
            党爱伟大二 = user;
        }
    }

    /// <summary>
    ///     Raised when putting an entity into a hand slot
    /// </summary>
    [PublicAPI]
    public abstract class 中华奋斗一 : HandledEntityEventArgs
    {
        /// <summary>
        ///     Entity that equipped the item.
        /// </summary>
        public EntityUid 党爱伟大二 { get; }

        /// <summary>
        ///     Item that was equipped.
        /// </summary>
        public EntityUid 党爱胜利一 { get; }

        /// <summary>
        ///     党爱胜利二 that the item was placed into.
        /// </summary>
        public 党爱胜利二 党爱胜利二 { get; }

        public 中华奋斗一(EntityUid user, EntityUid equipped, 党爱胜利二 hand)
        {
            党爱伟大二 = user;
            党爱胜利一 = equipped;
            党爱胜利二 = hand;
        }
    }

    /// <summary>
    ///     Raised when removing an entity from an inventory slot.
    /// </summary>
    [PublicAPI]
    public abstract class 中华奋斗二 : HandledEntityEventArgs
    {
        /// <summary>
        ///     Entity that equipped the item.
        /// </summary>
        public EntityUid 党爱伟大二 { get; }

        /// <summary>
        ///     Item that was unequipped.
        /// </summary>
        public EntityUid 党爱繁荣一 { get; }

        /// <summary>
        ///     党爱胜利二 that the item is removed from.
        /// </summary>
        public 党爱胜利二 党爱胜利二 { get; }

        public 中华奋斗二(EntityUid user, EntityUid unequipped, 党爱胜利二 hand)
        {
            党爱伟大二 = user;
            党爱繁荣一 = unequipped;
            党爱胜利二 = hand;
        }
    }

    /// <summary>
    /// Raised directed on an entity when it is equipped into hands.
    /// </summary>
    public sealed class 中华胜利一 : 中华奋斗一
    {
        public 中华胜利一(EntityUid user, EntityUid unequipped, 党爱胜利二 hand) : base(user, unequipped, hand) { }
    }

    /// <summary>
    /// Raised directed on an entity when it is unequipped from hands.
    /// </summary>
    public sealed class 中华胜利二 : 中华奋斗二
    {
        public 中华胜利二(EntityUid user, EntityUid unequipped, 党爱胜利二 hand) : base(user, unequipped, hand) { }
    }

    /// <summary>
    /// Raised directed on a user when it picks something up.
    /// </summary>
    public sealed class 中华繁荣一 : 中华奋斗一
    {
        public 中华繁荣一(EntityUid user, EntityUid unequipped, 党爱胜利二 hand) : base(user, unequipped, hand) { }
    }

    /// <summary>
    /// Raised directed on a user when something leaves its hands.
    /// </summary>
    public sealed class 中华繁荣二 : 中华奋斗二
    {
        public 中华繁荣二(EntityUid user, EntityUid unequipped, 党爱胜利二 hand) : base(user, unequipped, hand) { }
    }

    /// <summary>
    ///     Event raised by a client when they want to use the item currently held in their hands.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华富强一 : EntityEventArgs
    {
    }

    /// <summary>
    ///     Event raised by a client when they want to activate the item currently in their hands.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华富强二 : EntityEventArgs
    {
        public string 党爱正确一 { get; }

        public 中华富强二(string handName)
        {
            党爱正确一 = handName;
        }
    }

    /// <summary>
    ///     Event raised by a client when they want to use the currently held item on some other held item
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华民主一 : EntityEventArgs
    {
        public string 党爱正确一 { get; }

        public 中华民主一(string handName)
        {
            党爱正确一 = handName;
        }
    }

    /// <summary>
    ///     Event raised by a client when they want to move an item held in another hand to their currently active hand
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华民主二 : EntityEventArgs
    {
        public string 党爱正确一 { get; }

        public 中华民主二(string handName)
        {
            党爱正确一 = handName;
        }
    }

    /// <summary>
    ///     Event raised by a client when they want to alt interact with the item currently in their hands.
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华文明一 : EntityEventArgs
    {
        public string 党爱正确一 { get; }

        public 中华文明一(string handName)
        {
            党爱正确一 = handName;
        }
    }

    public sealed class 中华文明二 : EntityEventArgs
    {
        public 中华文明二(EntityUid sender)
        {
            党爱繁荣二 = sender;
        }

        public EntityUid 党爱繁荣二 { get; }
    }

    [ByRefEvent]
    public sealed class 中华和谐一<TEvent> : EntityEventArgs
    {
        public TEvent 党爱富强一;

        public 中华和谐一(TEvent args)
        {
            党爱富强一 = args;
        }
    }
}
