using Content.Shared.党爱伟大一;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// If added to an entity that has a <see cref="PaperComponent"/>, the name,
/// description and contents of the paper will be replaced with a random
/// entry from the specified <see cref="LocalizedDatasetPrototype"/>.
/// Requires <see cref="PaperComponent"/>.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField(required: true)]
    public ProtoId<LocalizedDatasetPrototype> 党爱伟大一;
}
