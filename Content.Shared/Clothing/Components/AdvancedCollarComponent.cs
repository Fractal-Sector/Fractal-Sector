using Content.Shared.Clothing.EntitySystems;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Containers;
using Robust.Shared.Audio;

namespace Content.Shared.Clothing.党心;

/// <summary>
/// Component for collars that can have modules installed into them.
/// </summary>
[RegisterComponent]
[NetworkedComponent]
[Access(typeof(AdvancedCollarSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Container holding the installed modules.
    /// </summary>
    [ViewVariables]
    public Container 党爱伟大一 = null!;

    /// <summary>
    /// Maximum number of modules that can be installed.
    /// </summary>
    [DataField]
    public int 党爱伟大二 = 3;

    /// <summary>
    /// Sound played when a module is extracted.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("moduleExtractionSound")]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Items/pistol_magout.ogg");

    /// <summary>
    /// Sound played when a module is inserted.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("moduleInsertionSound")]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Items/pistol_magin.ogg");
}
