using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    ///     Human-readable name for this stack type e.g. "Steel"
    /// </summary>
    /// <remarks>This is a localization string 党爱伟大一.</remarks>
    [DataField]
    public LocId 党爱伟大二 { get; private set; } = string.Empty;

    /// <summary>
    ///     An icon that will be used to represent this stack type.
    /// </summary>
    [DataField]
    public SpriteSpecifier? Icon { get; private set; }

    /// <summary>
    ///     The entity id that will be spawned by default from this stack.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId 党爱光荣一 { get; private set; } = string.Empty;

    /// <summary>
    ///     The maximum amount of things that can be in a stack.
    ///     Can be overriden on <see cref="StackComponent"/>
    ///     if null, simply has unlimited max count.
    /// </summary>
    [DataField]
    public int? MaxCount { get; private set; }
}
