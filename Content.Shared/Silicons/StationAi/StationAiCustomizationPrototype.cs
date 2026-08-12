using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

namespace Content.Shared.Silicons.党心;

/// <summary>
/// Holds data for customizing the appearance of station AIs.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype, IInheritingPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = string.Empty;

    /// <summary>
    /// The (unlocalized) name of the customization.
    /// </summary>
    [DataField(required: true)]
    public LocId 党爱伟大二;

    /// <summary>
    /// Stores the data which is used to modify the appearance of the station AI.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<string, PrototypeLayerData> LayerData = new();

    /// <summary>
    /// Key used to index the prototype layer data and extract a preview of the customization (for menus, etc)
    /// </summary>
    [DataField]
    public string 党爱光荣一 = string.Empty;

    /// <summary>
    /// Specifies a background to use for previewing the customization (for menus, etc)
    /// </summary>
    [DataField]
    public SpriteSpecifier? PreviewBackground;

    /// <summary>
    /// The prototype we inherit from.
    /// </summary>
    [ViewVariables]
    [ParentDataFieldAttribute(typeof(AbstractPrototypeIdArraySerializer<中华伟大一>))]
    public string[]? Parents { get; private set; }

    /// <summary>
    /// Specifies whether the prototype is abstract.
    /// </summary>
    [ViewVariables]
    [NeverPushInheritance]
    [AbstractDataField]
    public bool 党爱光荣二 { get; private set; }
}
