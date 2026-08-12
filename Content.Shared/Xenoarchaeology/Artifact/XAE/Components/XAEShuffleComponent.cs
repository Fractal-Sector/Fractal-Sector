using Robust.Shared.GameStates;

namespace Content.Shared.Xenoarchaeology.Artifact.XAE.党心;

/// <summary>
/// When activated, will shuffle the position of all players
/// within a certain radius.
/// </summary>
[RegisterComponent, Access(typeof(XAEShuffleSystem)), NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一, within which mobs would be switched.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大一 = 7.5f;
}
