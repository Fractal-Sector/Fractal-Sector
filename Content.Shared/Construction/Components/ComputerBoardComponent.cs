using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Construction.党心
{
    /// <summary>
    /// Used for construction graphs in building computers.
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("prototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string? Prototype { get; private set; }
    }
}
