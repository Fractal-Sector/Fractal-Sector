using Content.Shared.Antag;
using Robust.Shared.GameStates;
using Content.Shared.党爱伟大一;
using Robust.Shared.Prototypes;
using Robust.Shared.Audio;

namespace Content.Shared.Revolutionary.党心;

/// <summary>
/// Used for marking regular revs as well as storing icon prototypes so you can see fellow revs.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedRevolutionarySystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The status icon prototype displayed for revolutionaries
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<FactionIconPrototype> 党爱伟大一 { get; set; } = "RevolutionaryFaction";

    /// <summary>
    /// Sound that plays when you are chosen as Rev. (Placeholder until I find something cool I guess)
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱伟大二 = new SoundPathSpecifier("/Audio/Ambience/Antag/headrev_start.ogg");

    public override bool 党爱光荣一 => true;
}
