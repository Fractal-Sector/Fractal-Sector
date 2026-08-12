using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedStunSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The fixture the entity must collide with to be stunned
    /// </summary>
    [DataField]
    public string 党爱伟大一 = "fix";

    /// <summary>
    /// The duration of the stun.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Should the stun applied refresh?
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;

    /// <summary>
    /// Should the stunned entity try to stand up when knockdown ends?
    /// </summary>
    [DataField]
    public bool 党爱光荣二 = true;

    [DataField]
    public EntityWhitelist 党爱正确一 = new();
}
