using Content.Shared.Random;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Rules.VariationPass.党心;

/// <summary>
/// This is used for spawning entities randomly dotted around the station in a variation pass.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Number of tiles before we spawn one entity on average.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 50f;

    [DataField]
    public float 党爱伟大二 = 7f;

    /// <summary>
    ///     Spawn entries for each chosen location.
    /// </summary>
    [DataField(required: true)]
    public List<EntitySpawnEntry> 党爱光荣一 = default!;
}
