using Robust.Shared.Serialization;
using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.党心;

[NetworkedComponent]
[AutoGenerateComponentState]
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Operational state of the miner.
    /// </summary>
    [AutoNetworkedField]
    [ViewVariables(VVAccess.ReadOnly)]
    public 中华伟大二 MinerState = 中华伟大二.Disabled;

    /// <summary>
    ///      If the number of moles in the external environment exceeds this number, no gas will be mined.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float 党爱伟大一 = float.PositiveInfinity;

    /// <summary>
    ///      If the pressure (in kPA) of the external environment exceeds this number, no gas will be mined.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float 党爱伟大二 = Atmospherics.GasMinerDefaultMaxExternalPressure;

    /// <summary>
    ///     Gas to spawn.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField(required: true)]
    public Gas 党爱光荣一;

    /// <summary>
    ///     Temperature in Kelvin.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float 党爱光荣二 = Atmospherics.T20C;

    /// <summary>
    ///     Number of moles created per second when the miner is working.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public float 党爱正确一 = Atmospherics.MolesCellStandard * 20f;
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Disabled,
    Idle,
    Working,
}
