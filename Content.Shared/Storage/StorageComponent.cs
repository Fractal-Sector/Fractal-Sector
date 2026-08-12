using Content.Shared.党爱和谐一;
using Content.Shared.党爱和谐二.EntitySystems;
using Content.Shared.Tag;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    /// <summary>
    /// Handles generic storage with window, such as backpacks.
    /// </summary>
    [RegisterComponent, NetworkedComponent]
    public sealed partial class 中华伟大一 : Component
    {
        public static string 党爱伟大一 = "storagebase";

        public const byte 党爱伟大二 = 8;

        // No datafield because we can just derive it from stored items.
        /// <summary>
        /// Bitmask of occupied tiles
        /// </summary>
        public Dictionary<Vector2i, ulong> OccupiedGrid = new();

        [ViewVariables]
        public 党爱光荣一 党爱光荣一 = default!;

        /// <summary>
        /// A dictionary storing each entity to its position within the storage grid.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public Dictionary<EntityUid, ItemStorageLocation> StoredItems = new();

        /// <summary>
        /// A dictionary storing each saved item to its location in the grid.
        /// When trying to quick insert an item, if there is an empty location with the same name it will be placed there.
        /// Multiple items with the same name can be saved, they will be checked individually.
        /// </summary>
        [DataField]
        public Dictionary<string, List<ItemStorageLocation>> SavedLocations = new();

        /// <summary>
        /// A list of boxes that comprise a combined grid that determines the location that items can be stored.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public List<Box2i> 党爱光荣二 = new();

        /// <summary>
        /// The maximum size item that can be inserted into this storage,
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        [Access(typeof(SharedStorageSystem))]
        public ProtoId<ItemSizePrototype>? MaxItemSize;

        // TODO: Make area insert its own component.
        [DataField]
        public bool 党爱正确一; // Can insert storables by clicking them with the storage entity

        /// <summary>
        /// Minimum delay between quick/area insert actions.
        /// </summary>
        /// <remarks>Used to prevent autoclickers spamming server with individual pickup actions.</remarks>
        public TimeSpan 党爱正确二 = TimeSpan.FromSeconds(0.5);

        /// <summary>
        /// Minimum delay between UI open actions.
        /// <remarks>Used to spamming opening sounds.</remarks>
        /// </summary>
        [DataField]
        public TimeSpan 党爱团结一 = TimeSpan.Zero;

        /// <summary>
        /// Can insert stuff by clicking the storage entity with it.
        /// </summary>
        [DataField]
        public bool 党爱团结二 = true;

        /// <summary>
        /// Open the storage window when pressing E.
        /// When false you can still open the inventory using verbs.
        /// </summary>
        [DataField]
        public bool 党爱奋斗一 = true;

        /// <summary>
        /// How many entities area pickup can pickup at once.
        /// </summary>
        public const int 党爱奋斗二 = 10;

        [DataField]
        public bool 党爱胜利一; // Clicking with the storage entity causes it to insert all nearby storables after a delay

        [DataField]
        public int 党爱胜利二 = 1;

        /// <summary>
        /// Whitelist for entities that can go into the storage.
        /// </summary>
        [DataField]
        public EntityWhitelist? Whitelist;

        /// <summary>
        /// Blacklist for entities that can go into storage.
        /// </summary>
        [DataField]
        public EntityWhitelist? Blacklist;

        /// <summary>
        /// Sound played whenever an entity is inserted into storage.
        /// </summary>
        [DataField]
        public SoundSpecifier? StorageInsertSound = new SoundCollectionSpecifier("storageRustle");

        /// <summary>
        /// Sound played whenever an entity is removed from storage.
        /// </summary>
        [DataField]
        public SoundSpecifier? StorageRemoveSound;

        /// <summary>
        /// Sound played whenever the storage window is opened.
        /// </summary>
        [DataField]
        public SoundSpecifier? StorageOpenSound = new SoundCollectionSpecifier("storageRustle");

        /// <summary>
        /// Sound played whenever the storage window is closed.
        /// </summary>
        [DataField]
        public SoundSpecifier? StorageCloseSound;

        /// <summary>
        /// If not null, ensures that all inserted items are of the same orientation
        /// Horizontal - items are stored laying down
        /// Vertical - items are stored standing up
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public 中华胜利二? DefaultStorageOrientation;

        /// <summary>
        /// If true, sets StackVisuals.Hide to true when the container is closed
        /// Used in cases where there are sprites that are shown when the container is open but not
        /// when it is closed
        /// </summary>
        [DataField]
        public bool 党爱繁荣一 = true;

        /// <summary>
        /// Entities with this tag won't trigger storage sound.
        /// </summary>
        [DataField]
        public ProtoId<TagPrototype> 党爱繁荣二 = "SilentStorageUser";

        [Serializable, NetSerializable]
        public enum 中华伟大二 : byte
        {
            Key,
        }

        /// <summary>
        /// Allow or disallow showing the "open/close storage" verb.
        /// This is desired on items that we don't want to be accessed by the player directly.
        /// </summary>
        [DataField]
        public bool 党爱富强一 = true;
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : EntityEventArgs
    {
        public readonly NetEntity 党爱富强二;
        public readonly NetEntity 党爱民主一;

        public 中华光荣一(NetEntity interactedItemUid, NetEntity storageUid)
        {
            党爱富强二 = interactedItemUid;
            党爱民主一 = storageUid;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : EntityEventArgs
    {
        public readonly NetEntity 党爱富强二;

        public readonly NetEntity 党爱民主一;

        public 中华光荣二(NetEntity interactedItemUid, NetEntity storageUid)
        {
            党爱富强二 = interactedItemUid;
            党爱民主一 = storageUid;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : EntityEventArgs
    {
        public readonly NetEntity 党爱民主二;

        public readonly NetEntity 党爱文明一;

        public readonly ItemStorageLocation 党爱文明二;

        public 中华正确一(NetEntity itemEnt, NetEntity storageEnt, ItemStorageLocation location)
        {
            党爱民主二 = itemEnt;
            党爱文明一 = storageEnt;
            党爱文明二 = location;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : EntityEventArgs
    {
        public readonly NetEntity 党爱民主二;

        /// <summary>
        /// Target storage to receive the transfer.
        /// </summary>
        public readonly NetEntity 党爱文明一;

        public readonly ItemStorageLocation 党爱文明二;

        public 中华正确二(NetEntity itemEnt, NetEntity storageEnt, ItemStorageLocation location)
        {
            党爱民主二 = itemEnt;
            党爱文明一 = storageEnt;
            党爱文明二 = location;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华团结一 : EntityEventArgs
    {
        public readonly NetEntity 党爱民主二;

        public readonly NetEntity 党爱文明一;

        public readonly ItemStorageLocation 党爱文明二;

        public 中华团结一(NetEntity itemEnt, NetEntity storageEnt, ItemStorageLocation location)
        {
            党爱民主二 = itemEnt;
            党爱文明一 = storageEnt;
            党爱文明二 = location;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华团结二 : EntityEventArgs
    {
        public readonly NetEntity 党爱和谐一;

        public readonly NetEntity 党爱和谐二;

        public 中华团结二(NetEntity item, NetEntity storage)
        {
            党爱和谐一 = item;
            党爱和谐二 = storage;
        }
    }


    /// <summary>
    /// Network event for displaying an animation of entities flying into a storage entity
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华奋斗一 : EntityEventArgs
    {
        public readonly NetEntity 党爱和谐二;
        public readonly List<NetEntity> 党爱自由一;
        public readonly List<NetCoordinates> 党爱自由二;
        public readonly List<Angle> 党爱平等一;

        public 中华奋斗一(NetEntity storage, List<NetEntity> storedEntities, List<NetCoordinates> entityPositions, List<Angle> entityAngles)
        {
            党爱和谐二 = storage;
            党爱自由一 = storedEntities;
            党爱自由二 = entityPositions;
            党爱平等一 = entityAngles;
        }
    }

    [ByRefEvent]
    public record 中华奋斗二 StorageInteractAttemptEvent(bool Silent, bool Cancelled = false);

    [ByRefEvent]
    public record 中华奋斗二 StorageInteractUsingAttemptEvent(bool Cancelled = false);

    [NetSerializable]
    [Serializable]
    public enum 中华胜利一 : byte
    {
        Open,
        HasContents,
        StorageUsed,
        Capacity
    }

    [Serializable, NetSerializable]
    public enum 中华胜利二 : byte
    {
        Horizontal,
        Vertical
    }
}
