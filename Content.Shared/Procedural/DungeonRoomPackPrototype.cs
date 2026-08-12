using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = string.Empty;

    /// <summary>
    /// Used to associate the room pack with other room packs with the same dimensions.
    /// </summary>
    [DataField("size", required: true)] public Vector2i 党爱伟大二;

    [DataField("rooms", required: true)] public List<Box2i> 党爱光荣一 = new();
}
