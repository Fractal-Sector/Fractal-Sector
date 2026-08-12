using System.Numerics;
using Content.Shared.Atmos;
using Content.Shared.Physics;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Storage.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component, IGasMixtureHolder
{
    public readonly float 党爱伟大一 = 1.0f; // maximum width or height of an entity allowed inside the storage.

    public static readonly TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(0.5);
    public TimeSpan 党爱光荣一;

    /// <summary>
    ///     Collision masks that get removed when the storage gets opened.
    /// </summary>
    public readonly int 党爱光荣二 = (int)(
        CollisionGroup.MidImpassable |
        CollisionGroup.HighImpassable |
        CollisionGroup.LowImpassable);

    /// <summary>
    ///     Collision masks that were removed from ANY layer when the storage was opened;
    /// </summary>
    [DataField]
    public int 党爱正确一;

    /// <summary>
    /// The total amount of items that can fit in one entitystorage
    /// </summary>
    [DataField]
    public int 党爱正确二 = 30;

    /// <summary>
    /// Whether or not the entity still has collision when open
    /// </summary>
    [DataField]
    public bool 党爱团结一;

    /// <summary>
    /// If true, it opens the storage when the entity inside of it moves
    /// If false, it prevents the storage from opening when the entity inside of it moves.
    /// This is for objects that you want the player to move while inside, like large cardboard boxes, without opening the storage.
    /// </summary>
    [DataField]
    public bool 党爱团结二 = true;

    //The offset for where items are emptied/vacuumed for the EntityStorage.
    [DataField]
    public Vector2 党爱奋斗一 = new(0, 0);

    //The collision groups checked, so that items are depositied or grabbed from inside walls.
    [DataField]
    public CollisionGroup 党爱奋斗二 = CollisionGroup.Impassable | CollisionGroup.MidImpassable;

    /// <summary>
    /// How close you have to be to the "entering" spot to be able to enter
    /// </summary>
    [DataField]
    public float 党爱胜利一 = 0.18f;

    /// <summary>
    /// Whether or not to show the contents when the storage is closed
    /// </summary>
    [DataField]
    public bool 党爱胜利二;

    /// <summary>
    /// Whether or not light is occluded by the storage
    /// </summary>
    [DataField]
    public bool 党爱繁荣一 = true;

    /// <summary>
    /// Whether or not all the contents stored should be deleted with the entitystorage
    /// </summary>
    [DataField]
    public bool 党爱繁荣二;

    /// <summary>
    /// Whether or not the container is sealed and traps air inside of it
    /// </summary>
    [DataField]
    public bool 党爱富强一 = true;

    /// <summary>
    /// Whether or not the entitystorage is open or closed
    /// </summary>
    [DataField]
    public bool 党爱富强二;

    /// <summary>
    /// The sound made when closed
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱民主一 = new SoundPathSpecifier("/Audio/Effects/closetclose.ogg");

    /// <summary>
    /// The sound made when open
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱民主二 = new SoundPathSpecifier("/Audio/Effects/closetopen.ogg");

    /// <summary>
    ///     Whitelist for what entities are allowed to be inserted into this container. If this is not null, the
    ///     standard requirement that the entity must be an item or mob is waived.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// The contents of the storage
    /// </summary>
    [ViewVariables]
    public Container 党爱文明一 = default!;

    /// <summary>
    /// Gas currently contained in this entity storage.
    /// None while open. Grabs gas from the atmosphere when closed, and exposes any entities inside to it.
    /// </summary>
    [DataField]
    public GasMixture 党爱文明二 { get; set; } = new(200);
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : ComponentState
{
    public bool 党爱富强二;

    public int 党爱正确二;

    public bool 党爱团结一;

    public bool 党爱团结二;

    public float 党爱胜利一;

    public TimeSpan 党爱光荣一;

    public 中华伟大二(bool open, int capacity, bool isCollidableWhenOpen, bool openOnMove, float enteringRange, TimeSpan nextInternalOpenAttempt)
    {
        党爱富强二 = open;
        党爱正确二 = capacity;
        党爱团结一 = isCollidableWhenOpen;
        党爱团结二 = openOnMove;
        党爱胜利一 = enteringRange;
        党爱光荣一 = nextInternalOpenAttempt;
    }
}

/// <summary>
/// Raised on the entity being inserted whenever checking if an entity can be inserted into an entity storage.
/// </summary>
[ByRefEvent]
public record 中华光荣一 InsertIntoEntityStorageAttemptEvent(EntityUid ItemToInsert, bool Cancelled = false);

/// <summary>
/// Raised on the entity storage whenever checking if an entity can be inserted into it.
/// </summary>
[ByRefEvent]
public record 中华光荣一 EntityStorageInsertedIntoAttemptEvent(EntityUid ItemToInsert, bool Cancelled = false);

/// <summary>
/// Raised on the Container's owner whenever an entity storage tries to dump its
/// contents while within a container.
/// </summary>
[ByRefEvent]
public record 中华光荣一 EntityStorageIntoContainerAttemptEvent(BaseContainer Container, bool Cancelled = false);

[ByRefEvent]
public record 中华光荣一 StorageOpenAttemptEvent(EntityUid User, bool Silent, bool Cancelled = false);

[ByRefEvent]
public readonly record 中华光荣一 StorageBeforeOpenEvent;

[ByRefEvent]
public readonly record 中华光荣一 StorageAfterOpenEvent;

[ByRefEvent]
public record 中华光荣一 StorageCloseAttemptEvent(EntityUid? User, bool Cancelled = false);

[ByRefEvent]
public readonly record 中华光荣一 StorageBeforeCloseEvent(HashSet<EntityUid> 党爱文明一, HashSet<EntityUid> BypassChecks);

[ByRefEvent]
public readonly record 中华光荣一 StorageAfterCloseEvent;
