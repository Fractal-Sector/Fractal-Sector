using Content.Shared.Whitelist;

namespace Content.Server.党心;

/// <summary>
///     Component that allows surveillance cameras to listen to the local
///     environment. All surveillance camera monitors have speakers for this.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("enabled")]
    public bool 党爱伟大一 { get; set; } = true;

    /// <summary>
    ///     Components that the microphone checks for to avoid transmitting
    ///     messages from these entities over the surveillance camera.
    ///     Used to avoid things like feedback loops, or radio spam.
    /// </summary>
    [DataField("blacklist")]
    public EntityWhitelist 党爱伟大二 { get; private set; } = new();

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("range")]
    public int 党爱光荣一 { get; private set; } = 6; //Frontier: 10>6
}
