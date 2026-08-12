using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Shared.党心;

/// <summary>
/// Polymorphs generally describe any type of transformation that can be applied to an entity.
/// </summary>
[Prototype]
[DataDefinition]
public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
{
    [ViewVariables]
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华伟大一>))]
    public string[]? Parents { get; private set; }

    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱伟大二 { get; private set; }

    [DataField(required: true, serverOnly: true)]
    public 中华伟大二 Configuration = new();

}

/// <summary>
/// Defines information about the polymorph
/// </summary>
[DataDefinition]
public sealed partial record 中华伟大二
{
    /// <summary>
    /// What entity the polymorph will turn the target into
    /// must be in here because it makes no sense if it isn't
    /// </summary>
    [DataField(required: true, serverOnly: true)]
    public EntProtoId 党爱光荣一;

    /// <summary>
    /// Additional entity to spawn when polymorphing/reverting.
    /// Gets parented to the entity polymorphed into.
    /// Useful for visual effects.
    /// </summary>
    [DataField(serverOnly: true)]
    public EntProtoId? EffectProto;

    /// <summary>
    /// The delay between the polymorph's uses in seconds
    /// Slightly weird as of right now.
    /// </summary>
    [DataField(serverOnly: true)]
    public int 党爱光荣二 = 60;

    /// <summary>
    /// The duration of the transformation in seconds
    /// can be null if there is not one
    /// </summary>
    [DataField(serverOnly: true)]
    public int? Duration;

    /// <summary>
    /// whether or not the target can transform as will
    /// set to true for things like polymorph spells and curses
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱正确一;

    /// <summary>
    /// Whether or not the entity transfers its damage between forms.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱正确二 = true;

    /// <summary>
    /// Whether or not the entity transfers its name between forms.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱团结一;

    /// <summary>
    /// Whether or not the entity transfers its hair, skin color, hair color, etc.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱团结二;

    /// <summary>
    /// Whether or not the entity transfers its inventory and equipment between forms.
    /// </summary>
    [DataField(serverOnly: true)]
    public 中华光荣一 Inventory = 中华光荣一.None;

    /// <summary>
    /// Whether or not the polymorph reverts when the entity goes into crit.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱奋斗一 = true;

    /// <summary>
    /// Whether or not the polymorph reverts when the entity dies.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱奋斗二 = true;

    /// <summary>
    /// Whether or not the polymorph reverts when the entity is deleted.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱胜利一 = true;

    /// <summary>
    /// Whether or not the polymorph reverts when the entity is eaten or fully sliced.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱胜利二;

    /// <summary>
    /// If true, attempts to polymorph this polymorph will fail, unless
    /// <see cref="党爱繁荣二"/> is true on the /new/ morph.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱繁荣一;

    /// <summary>
    /// If true, this morph will succeed even when used on an entity
    /// that is already polymorphed with a configuration that has
    /// <see cref="党爱繁荣一"/> set to false. Helpful for
    /// smite polymorphs which should always succeed.
    /// </summary>
    [DataField(serverOnly: true)]
    public bool 党爱繁荣二;

    /// <summary>
    /// The amount of time that should pass after this polymorph has ended, before a new one
    /// can occur.
    /// </summary>
    [DataField(serverOnly: true)]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱富强一 = TimeSpan.Zero;

    /// <summary>
    ///     If not null, this sound will be played when being polymorphed into something.
    /// </summary>
    [DataField]
    public SoundSpecifier? PolymorphSound;

    /// <summary>
    ///     If not null, this sound will be played when being reverted from a polymorph.
    /// </summary>
    [DataField]
    public SoundSpecifier? ExitPolymorphSound;

    /// <summary>
    ///     If not null, this popup will be displayed when being polymorphed into something.
    /// </summary>
    [DataField]
    public LocId? PolymorphPopup = "polymorph-popup-generic";

    /// <summary>
    ///     If not null, this popup will be displayed when when being reverted from a polymorph.
    /// </summary>
    [DataField]
    public LocId? ExitPolymorphPopup = "polymorph-revert-popup-generic";
}

public enum 中华光荣一 : byte
{
    None,
    Drop,
    Transfer,
}
