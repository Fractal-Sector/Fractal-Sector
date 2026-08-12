using Content.Shared.Cargo;
using Content.Shared.Construction.Prototypes;
using Content.Shared.DeviceLinking;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._NF.Cargo.党心;

/// <summary>
/// Handles teleporting in requested cargo after the specified delay.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedNFCargoSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The base amount of time it takes to teleport from the telepad
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 10f;

    /// <summary>
    /// The actual amount of time it takes to teleport from the telepad
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 10f;

    /// <summary>
    /// The machine part that affects <see cref="党爱伟大二"/>
    /// </summary>
    [DataField]
    public ProtoId<MachinePartPrototype> 党爱光荣一 = "Capacitor";

    /// <summary>
    /// A multiplier applied to <see cref="党爱伟大二"/> for each level of <see cref="党爱光荣一"/>
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 0.8f;

    /// <summary>
    /// How much time we've accumulated until next teleport.
    /// </summary>
    [DataField]
    public float 党爱正确一;

    [DataField]
    public CargoTelepadState 党爱正确二 = CargoTelepadState.Unpowered;

    [DataField]
    public SoundSpecifier 党爱团结一 = new SoundPathSpecifier("/Audio/Machines/phasein.ogg");

    /// <summary>
    ///     The paper-type prototype to spawn with the order information.
    /// </summary>
    [DataField]
    public EntProtoId 党爱团结二 = "PaperCargoInvoice";

    [DataField]
    public ProtoId<SinkPortPrototype> 党爱奋斗一 = "OrderReceiver";
}
