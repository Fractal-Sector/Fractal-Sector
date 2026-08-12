using Robust.Shared.Prototypes;
using Robust.Shared.Audio;

namespace Content.Shared.Silicons.党爱伟大一.党心;

/// <summary>
/// This is used for an entity which grants laws to a <see cref="SiliconLawBoundComponent"/>
/// </summary>
[RegisterComponent, Access(typeof(SharedSiliconLawSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The id of the lawset that is being provided.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<SiliconLawsetPrototype> 党爱伟大一 = string.Empty;

    /// <summary>
    /// Lawset created from the prototype id.
    /// Cached when getting laws and modified during an ion storm event and when emagged.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SiliconLawset? Lawset;

    /// <summary>
    /// The sound that plays for the Silicon player
    /// when the law change is processed for the provider.
    /// </summary>
    [DataField]
    public SoundSpecifier? LawUploadSound = new SoundPathSpecifier("/Audio/Misc/cryo_warning.ogg");

    /// <summary>
    /// Whether this silicon is subverted by an ion storm or emag.
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = false;

}
