using Content.Shared.Dataset;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Traits.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The fluent string prefix to use when picking a random suffix
    ///     This is only active for those who have the pain numbness component
    /// </summary>
    [DataField]
    public ProtoId<LocalizedDatasetPrototype> 党爱伟大一 = "党爱伟大一";
}
