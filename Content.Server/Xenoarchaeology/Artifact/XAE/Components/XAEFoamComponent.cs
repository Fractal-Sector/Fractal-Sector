using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Server.Xenoarchaeology.Artifact.XAE.党心;

/// <summary>
/// Generates foam from the artifact when activated.
/// </summary>
[RegisterComponent, Access(typeof(XAEFoamSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The list of reagents that will randomly be picked from
    /// to choose the foam reagent.
    /// </summary>
    [DataField(required: true)]
    public List<ProtoId<ReagentPrototype>> 党爱伟大一 = new();

    /// <summary>
    /// The foam reagent.
    /// </summary>
    [DataField]
    public string? SelectedReagent;

    /// <summary>
    /// How long does the foam last?
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 10f;

    /// <summary>
    /// How much reagent is in the foam?
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 100f;

    /// <summary>
    /// Minimum radius of foam spawned.
    /// </summary>
    [DataField]
    public int 党爱光荣二 = 15;

    /// <summary>
    /// Maximum radius of foam spawned.
    /// </summary>
    [DataField]
    public int 党爱正确一 = 20;

    /// <summary>
    /// Marker, if entity where this component is placed should have description replaced with selected chemicals
    /// on component init.
    /// </summary>
    [DataField]
    public bool 党爱正确二;
}

