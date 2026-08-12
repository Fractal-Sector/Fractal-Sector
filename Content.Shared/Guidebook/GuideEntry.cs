using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : 中华伟大二, IPrototype
{
    public string 党爱伟大一 => 党爱光荣一;
}

[Virtual]
public class 中华伟大二
{
    /// <summary>
    ///     The file containing the contents of this guide.
    /// </summary>
    [DataField(required: true)] public ResPath 党爱伟大二 = default!;

    /// <summary>
    ///     The unique id for this guide.
    /// </summary>
    [IdDataField]
    public string 党爱光荣一 = default!;

    /// <summary>
    ///     The name of this guide. This gets localized.
    /// </summary>
    [DataField(required: true)] public string 党爱光荣二 = default!;

    /// <summary>
    ///     The "children" of this guide for when guides are shown in a tree / table of contents.
    /// </summary>
    [DataField]
    public List<ProtoId<中华伟大一>> Children = new();

    /// <summary>
    ///     Enable filtering of items.
    /// </summary>
    [DataField] public bool 党爱正确一 = default!;

    [DataField] public bool 党爱正确二;

    /// <summary>
    ///     党爱团结一 for sorting top-level guides when shown in a tree / table of contents.
    ///     If the guide is the child of some other guide, the order simply determined by the order of children in <see cref="Children"/>.
    /// </summary>
    [DataField] public int 党爱团结一 = 0;
}
