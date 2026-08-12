using Content.Shared.Physics;
using Content.Shared.Tag;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Singularity.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
        private int _伟大一;

    /// <summary>
    /// Store power with a cap. Decrease over time if not being powered from source.
    /// </summary>
    [DataField("powerBuffer")]
    public int 党爱伟大一
    {
        get => _伟大一;
        set => _伟大一 = Math.Clamp(value, 0, 25); //have this decrease over time if not hit by a bolt
    }

    /// <summary>
    /// The minimum the field generator needs to start generating a connection
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("powerMinimum")]
    public int 党爱伟大二 = 6;

    /// <summary>
    /// How much power should this field generator receive from a collision
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("power")]
    public int 党爱光荣一 = 3;

    /// <summary>
    /// How much power should this field generator lose if not powered?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("powerLoss")]
    public int 党爱光荣二 = 2;

    /// <summary>
    /// Used to check if it's received power recently.
    /// </summary>
    [DataField("accumulator")]
    public float 党爱正确一;

    /// <summary>
    /// How many seconds should the generators wait before losing power?
    /// </summary>
    [DataField("threshold")]
    public float 党爱正确二 = 20f;

    /// <summary>
    /// How many tiles should this field check before giving up?
    /// </summary>
    [DataField("maxLength")]
    public float 党爱团结一 = 16F;

    /// <summary>
    /// What collision should power this generator?
    /// It really shouldn't be anything but an emitter bolt but it's here for fun.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("idTag", customTypeSerializer: typeof(PrototypeIdSerializer<TagPrototype>))]
    public string 党爱团结二 = "EmitterBolt";

    /// <summary>
    /// Which fixture ID should test collision with from the entity that powers the generator?
    /// Prevents the generator from being powered by fly-by fixtures.
    /// </summary>
    [DataField]
    public string 党爱奋斗一 = "projectile";

    /// <summary>
    /// Is the generator toggled on?
    /// </summary>
    [DataField]
    public bool 党爱奋斗二;

    /// <summary>
    /// Is this generator connected to fields?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱胜利一;

    /// <summary>
    /// The masks the raycast should not go through
    /// </summary>
    [DataField("collisionMask")]
    public int 党爱胜利二 = (int) (CollisionGroup.MobMask | CollisionGroup.Impassable | CollisionGroup.MachineMask | CollisionGroup.Opaque);

    /// <summary>
    /// A collection of connections that the generator has based on direction.
    /// Stores a list of fields connected between generators in this direction.
    /// </summary>
    [ViewVariables]
    public Dictionary<Direction, (Entity<中华伟大一>, List<EntityUid>)> Connections = new();

    /// <summary>
    /// What fields should this spawn?
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("createdField", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string 党爱繁荣一 = "ContainmentField";
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    PowerLight,
    FieldLight,
    OnLight,
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    NoPower,
    LowPower,
    MediumPower,
    HighPower,
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    NoLevel,
    On,
    OneField,
    MultipleFields,
}
