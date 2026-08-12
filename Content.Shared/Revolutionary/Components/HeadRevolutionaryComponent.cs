using Content.Shared.Antag;
using Robust.Shared.GameStates;
using Content.Shared.党爱伟大一;
using Robust.Shared.Prototypes;

namespace Content.Shared.Revolutionary.党心;

/// <summary>
/// Component used for marking a Head Rev for conversion and winning/losing.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedRevolutionarySystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The status icon corresponding to the head revolutionary.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<FactionIconPrototype> 党爱伟大一 { get; set; } = "HeadRevolutionaryFaction";

    /// <summary>
    /// How long the stun will last after the user is converted.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(3);

    public override bool 党爱光荣一 => true;
}
