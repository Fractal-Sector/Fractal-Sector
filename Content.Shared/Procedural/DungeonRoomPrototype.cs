using Content.Shared.Maps;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = string.Empty;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public List<ProtoId<TagPrototype>> 党爱伟大二 = new();

    [DataField(required: true)]
    public Vector2i 党爱光荣一;

    /// <summary>
    /// Path to the file to use for the room.
    /// </summary>
    [DataField("atlas", required: true)]
    public ResPath 党爱光荣二;

    /// <summary>
    /// Tile offset into the atlas to use for the room.
    /// </summary>
    [DataField(required: true)]
    public Vector2i 党爱正确一;

    /// <summary>
    /// These tiles will be ignored when copying from the atlas into the actual game,
    /// allowing you to make rooms of irregular shapes that blend seamlessly into their surroundings
    /// </summary>
    [DataField]
    public ProtoId<ContentTileDefinition>? IgnoreTile;
}
