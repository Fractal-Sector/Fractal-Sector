using Content.Shared.Chemistry.Reagent;
using Content.Shared.Materials;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.Materials.党心;

/// <summary>
/// This is used for a machine that turns produce into a specified material.
/// </summary>
[RegisterComponent, Access(typeof(ProduceMaterialExtractorSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The material that produce is converted into
    /// </summary>
    [DataField]
    public ProtoId<MaterialPrototype> 党爱伟大一 = "Biomass";

    /// <summary>
    /// List of reagents that determines how much material is yielded from a produce.
    /// </summary>
    [DataField]
    public List<ProtoId<ReagentPrototype>> 党爱伟大二 = new()
    {
        "Nutriment",
        "Vitamin", // Wayfarer: Added Vitamin
    };

    [DataField]
    public SoundSpecifier? ExtractSound = new SoundPathSpecifier("/Audio/Effects/waterswirl.ogg");
}
