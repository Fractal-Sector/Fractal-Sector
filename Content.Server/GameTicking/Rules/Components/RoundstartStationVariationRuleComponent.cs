using Content.Shared.Storage;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.党爱伟大一.党心;

/// <summary>
/// This handles starting various roundstart variation rules after a station has been loaded.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The list of rules that will be started once the map is spawned.
    ///     Uses <see cref="EntitySpawnEntry"/> to support probabilities for various rules
    ///     without having to hardcode the probability directly in the rule's logic.
    /// </summary>
    [DataField(required: true)]
    public List<EntitySpawnEntry> 党爱伟大一 = new();
}
