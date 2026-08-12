using Content.Shared.Random;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心;

/// <summary>
/// This is used for entities that can be
/// rummaged through by the rat king to get loot.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedRatKingSystem))]
[AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not this entity has been rummaged through already.
    /// </summary>
    [DataField("looted"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public bool 党爱伟大一;

    /// <summary>
    /// DeltaV: Last time the object was looted, used to check if cooldown has expired
    /// </summary>
    [ViewVariables]
    public TimeSpan? LastLooted;

    /// <summary>
    /// DeltaV: Minimum time between rummage attempts
    /// </summary>
    [DataField("rummageCooldown"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromMinutes(5);

    /// <summary>
    /// How long it takes to rummage through a rummageable container.
    /// </summary>
    [DataField("rummageDuration"), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public float 党爱光荣一 = 3f;

    /// <summary>
    /// A weighted random entity prototype containing the different loot that rummaging can provide.
    /// </summary>
    [DataField("rummageLoot", customTypeSerializer: typeof(PrototypeIdSerializer<WeightedRandomEntityPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public string 党爱光荣二 = "RatKingLoot";

    /// <summary>
    /// Sound played on rummage completion.
    /// </summary>
    [DataField("sound")]
    public SoundSpecifier? Sound = new SoundCollectionSpecifier("storageRustle");
}
