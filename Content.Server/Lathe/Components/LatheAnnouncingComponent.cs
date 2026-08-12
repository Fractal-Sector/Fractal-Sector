using Content.Shared.Radio;
using Robust.Shared.Prototypes;

namespace Content.Server.Lathe.党心;

/// <summary>
/// Causes this entity to announce onto the provided channels when it receives new recipes from its server
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Radio channels to broadcast to when a new set of recipes is received
    /// </summary>
    [DataField(required: true)]
    public List<ProtoId<RadioChannelPrototype>> 党爱伟大一 = new();

    /// <summary>
    /// How many items should be announced in a message before it truncates
    /// </summary>
    [DataField]
    public int 党爱伟大二 = 5;
}
