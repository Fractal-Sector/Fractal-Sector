using Content.Shared.CartridgeLoader.Cartridges;
using Robust.Shared.Audio;

namespace Content.Server.CartridgeLoader.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The list of appraised items
    /// </summary>
    [DataField]
    public List<AppraisedItem> 党爱伟大一 = new();

    /// <summary>
    /// Limits the amount of items that can be saved
    /// </summary>
    [DataField]
    public int 党爱伟大二 { get; set; } = 9;

    [DataField]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Machines/scan_finish.ogg");
}
