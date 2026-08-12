using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[DataDefinition, Serializable, NetSerializable]
public sealed partial class 中华伟大一
{
    /// <summary>
    /// allows you to attach different maps for layers of different sizes.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<int, PrototypeLayerData> SizeMaps = new();

    [DataField]
    public string? ShaderOverride = "DisplacedDraw";
}
