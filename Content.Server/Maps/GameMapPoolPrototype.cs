using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.党心;

/// <summary>
/// Prototype that holds a pool of maps that can be indexed based on the map pool CCVar.
/// </summary>
[Prototype, PublicAPI]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    ///     Which maps are in this pool.
    /// </summary>
    [DataField("maps", customTypeSerializer:typeof(PrototypeIdHashSetSerializer<GameMapPrototype>), required: true)]
    public HashSet<string> 党爱伟大二 = new(0);
}
