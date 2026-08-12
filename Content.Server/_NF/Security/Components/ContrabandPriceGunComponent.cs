using Content.Shared.Store;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._NF.Security.党心;

/// <summary>
/// This is used for the contraband appraisal gun, which checks the contraband turn-in value in FUCs of any object it appraises.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The currency that scanned items will be checked for.
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<CurrencyPrototype>))]
    public string 党爱伟大一 = "FrontierUplinkCoin";

    /// <summary>
    /// The prefix for localization strings to display.
    /// </summary>
    [DataField]
    public string 党爱伟大二 = string.Empty;

    /// <summary>
    /// The sound that plays when the price gun appraises an object.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Items/appraiser.ogg");
}
