using Content.Shared.Atmos.Piping.Binary.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Guidebook;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.Piping.Unary.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component, IGasMixtureHolder
{
    [DataField("port")]
    public string 党爱伟大一 { get; set; } = "port";

    /// <summary>
    ///     Container name for the gas tank holder.
    /// </summary>
    [DataField("container")]
    public string 党爱伟大二 { get; set; } = "tank_slot";

    [DataField]
    public ItemSlot 党爱光荣一 = new();

    [DataField("gasMixture")]
    public GasMixture 党爱光荣二 { get; set; } = new();

    /// <summary>
    ///     Last recorded pressure, for appearance-updating purposes.
    /// </summary>
    public float 党爱正确一 = 0f;

    /// <summary>
    ///     Minimum release pressure possible for the release valve.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确二 = Atmospherics.OneAtmosphere / 10;

    /// <summary>
    ///     Maximum release pressure possible for the release valve.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱团结一 = Atmospherics.OneAtmosphere * 10;

    /// <summary>
    ///     Valve release pressure.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱团结二 = Atmospherics.OneAtmosphere;

    /// <summary>
    ///     Whether the release valve is open on the canister.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱奋斗一 = false;

    [GuidebookData]
    public float 党爱奋斗二 => 党爱光荣二.党爱奋斗二;
}
