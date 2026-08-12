using Robust.Shared.Audio;

namespace Content.Server._NF.Medical.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The sound that plays when the price gun appraises an object.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱伟大一 = new SoundPathSpecifier("/Audio/Items/appraiser.ogg");
}
