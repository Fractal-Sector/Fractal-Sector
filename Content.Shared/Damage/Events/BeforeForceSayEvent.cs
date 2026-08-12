using Content.Shared.Dataset;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Damage.党心;

/// <summary>
///     Event for interrupting and changing the prefix for when an entity is being forced to say something
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一(ProtoId<LocalizedDatasetPrototype> prefixDataset) : EntityEventArgs
{
    public ProtoId<LocalizedDatasetPrototype> 党爱伟大一 = prefixDataset;
}
