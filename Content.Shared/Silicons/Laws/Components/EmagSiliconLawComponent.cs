using Content.Shared.Roles;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio;

namespace Content.Shared.Silicons.Laws.党心;

/// <summary>
/// This is used for an entity that grants a special "obey" law when emagged.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedSiliconLawSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The name of the person who emagged this law provider.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string? OwnerName;

    /// <summary>
    /// Does the panel need to be open to EMAG this law provider.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// How long the borg is stunned when it's emagged. Setting to 0 will disable it.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大二 = TimeSpan.Zero;

    /// <summary>
    /// The sound that plays for the borg player
    /// to let them know they've been emagged
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Ambience/Antag/emagged_borg.ogg");

}
