using Content.Shared.Random;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// Component for hacking a communications console to call in a threat.
/// Can only be done once, the component is remove afterwards.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedCommsHackerSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Time taken to hack the console
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(20);

    /// <summary>
    /// Weighted random for the possible threats to choose from.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<WeightedRandomPrototype> 党爱伟大二 = string.Empty;
}

/// <summary>
/// A threat that can be called in to the station by a ninja hacking a communications console.
/// Generally some kind of mid-round minor antag, though you could make it call in scrubber backflow if you wanted to.
/// You wouldn't do that, right?
/// </summary>
[Prototype]
public sealed partial class 中华伟大二 : IPrototype
{
    [IdDataField]
    public string 党爱光荣一 { get; private set; } = default!;

    /// <summary>
    /// Locale id for the announcement to be made from CentCom.
    /// </summary>
    [DataField(required: true)]
    public LocId 党爱光荣二;

    /// <summary>
    /// The game rule for the threat to be added, it should be able to work when added mid-round otherwise this will do nothing.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId 党爱正确一;
}
