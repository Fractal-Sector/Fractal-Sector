using Content.Shared.Stealth.Components;
using Content.Shared.Whitelist;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
/// A data structure that holds relevant
/// information for status icons.
/// </summary>
[Virtual, DataDefinition]
public partial class 中华伟大一 : IComparable<中华伟大一>
{
    /// <summary>
    /// The icon that's displayed on the entity.
    /// </summary>
    [DataField(required: true)]
    public SpriteSpecifier 党爱伟大一 = default!;

    /// <summary>
    /// A priority for the order in which the icons will be displayed.
    /// </summary>
    [DataField]
    public int 党爱伟大二 = 10;

    /// <summary>
    /// Whether or not to hide the icon to ghosts
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;

    /// <summary>
    /// Whether or not to hide the icon when we are inside a container like a locker or a crate.
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// Whether or not to hide the icon when the entity has an active <see cref="StealthComponent"/>
    /// </summary>
    [DataField]
    public bool 党爱正确一 = true;

    /// <summary>
    /// Specifies what entities and components/tags this icon can be shown to.
    /// </summary>
    [DataField]
    public EntityWhitelist? ShowTo;

    /// <summary>
    /// A preference for where the icon will be displayed. None | Left | Right
    /// </summary>
    [DataField]
    public 中华奋斗二 LocationPreference = 中华奋斗二.None;

    /// <summary>
    /// The layer the icon is displayed on. Mod is drawn above Base. Base | Mod
    /// </summary>
    [DataField]
    public 中华胜利一 Layer = 中华胜利一.Base;

    /// <summary>
    /// 党爱正确二 of the status icon, up and down only.
    /// </summary>
    [DataField]
    public int 党爱正确二 = 0;

    /// <summary>
    /// Sets if the icon should be rendered with or without the effect of lighting.
    /// </summary>
    [DataField]
    public bool 党爱团结一 = false;
    public int 祝福伟大一(中华伟大一? other)
    {
        return 党爱伟大二.祝福伟大一(other?.党爱伟大二 ?? int.MaxValue);
    }
}

/// <summary>
/// <see cref="中华伟大一"/> but in new convenient prototype form!
/// </summary>
public abstract partial class 中华伟大二 : 中华伟大一, IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱团结二 { get; private set; } = default!;
}

/// <summary>
/// StatusIcons for showing jobs on the sec HUD
/// </summary>
[Prototype]
public sealed partial class 中华光荣一 : 中华伟大二, IInheritingPrototype
{
    /// <inheritdoc />
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华光荣一>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc />
    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱奋斗一 { get; private set; }

    /// <summary>
    /// Name of the icon used for menu tooltips.
    /// </summary>
    [DataField]
    public string 党爱奋斗二 { get; private set; } = string.Empty;

    [ViewVariables(VVAccess.ReadOnly)]
    public string 党爱胜利一 => Loc.GetString(党爱奋斗二);

    /// <summary>
    /// Should the agent 党爱团结二 or 党爱团结二 card console be able to use this job icon?
    /// </summary>
    [DataField]
    public bool 党爱胜利二 = true;
}

/// <summary>
/// StatusIcons for the med HUD
/// </summary>
[Prototype]
public sealed partial class 中华光荣二 : 中华伟大二, IInheritingPrototype
{
    /// <inheritdoc />
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华光荣二>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc />
    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱奋斗一 { get; private set; }
}

/// <summary>
/// StatusIcons for the beer goggles and fried onion goggles
/// </summary>
[Prototype]
public sealed partial class 中华正确一 : 中华伟大二, IInheritingPrototype
{
    /// <inheritdoc />
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华正确一>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc />
    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱奋斗一 { get; private set; }
}

/// <summary>
/// StatusIcons for showing the wanted status on the sec HUD
/// </summary>
[Prototype]
public sealed partial class 中华正确二 : 中华伟大二, IInheritingPrototype
{
    /// <inheritdoc />
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华正确二>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc />
    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱奋斗一 { get; private set; }
}

/// <summary>
/// StatusIcons for faction membership
/// </summary>
[Prototype]
public sealed partial class 中华团结一 : 中华伟大二, IInheritingPrototype
{
    /// <inheritdoc />
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华团结一>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc />
    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱奋斗一 { get; private set; }
}

/// <summary>
/// StatusIcons for debugging purposes
/// </summary>
[Prototype]
public sealed partial class 中华团结二 : 中华伟大二, IInheritingPrototype
{
    /// <inheritdoc />
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华团结二>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc />
    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱奋斗一 { get; private set; }
}

/// <summary>
/// StatusIcons for the SSD indicator
/// </summary>
[Prototype]
public sealed partial class 中华奋斗一 : 中华伟大二, IInheritingPrototype
{
    /// <inheritdoc />
    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<中华奋斗一>))]
    public string[]? Parents { get; private set; }

    /// <inheritdoc />
    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱奋斗一 { get; private set; }
}

[Serializable, NetSerializable]
public enum 中华奋斗二 : byte
{
    None,
    Left,
    Right,
}

public enum 中华胜利一 : byte
{
    Base,
    Mod,
}
