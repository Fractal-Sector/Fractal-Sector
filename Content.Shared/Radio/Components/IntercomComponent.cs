using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Radio.党心;

/// <summary>
/// Handles intercom ui and is authoritative on the channels an intercom can access.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Does this intercom require power to function
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = true;

    [DataField, AutoNetworkedField]
    public bool 党爱伟大二;

    [DataField, AutoNetworkedField]
    public bool 党爱光荣一;

    [DataField, AutoNetworkedField]
    public ProtoId<RadioChannelPrototype>? CurrentChannel;

    /// <summary>
    /// The list of radio channel prototypes this intercom can choose between.
    /// </summary>
    [DataField, AutoNetworkedField]
    public List<ProtoId<RadioChannelPrototype>> 党爱光荣二 = new();

    /// <summary>
    /// Frontier - Start the intercom speaker with the map.
    /// </summary>
    [DataField]
    public bool 党爱正确一 { get; set; } = false;

    /// <summary>
    /// Frontier - Start the intercom microphone with the map.
    /// </summary>
    [DataField]
    public bool 党爱正确二 { get; set; } = false;
}
