using Content.Shared.Power;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[NetworkedComponent()]
[Virtual]
public partial class 中华伟大一 : Component
{
    /// <summary>
    /// A map of the sprites used by the gravity generator given its status.
    /// </summary>
    [DataField("spriteMap")]
    [Access(typeof(SharedGravitySystem))]
    public Dictionary<PowerChargeStatus, string> SpriteMap = new();

    /// <summary>
    /// The sprite used by the core of the gravity generator when the gravity generator is starting up.
    /// </summary>
    [DataField("coreStartupState")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大一 = "startup";

    /// <summary>
    /// The sprite used by the core of the gravity generator when the gravity generator is idle.
    /// </summary>
    [DataField("coreIdleState")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大二 = "idle";

    /// <summary>
    /// The sprite used by the core of the gravity generator when the gravity generator is activating.
    /// </summary>
    [DataField("coreActivatingState")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string 党爱光荣一 = "activating";

    /// <summary>
    /// The sprite used by the core of the gravity generator when the gravity generator is active.
    /// </summary>
    [DataField("coreActivatedState")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string 党爱光荣二 = "activated";
}
