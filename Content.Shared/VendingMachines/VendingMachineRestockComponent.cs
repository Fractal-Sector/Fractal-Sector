using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedVendingMachineSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The time (in seconds) that it takes to restock a machine.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("restockDelay")]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(5.0f);

    /// <summary>
    /// What sort of machine inventory does this restock?
    /// This is checked against the VendingMachineComponent's pack value.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("canRestock", customTypeSerializer: typeof(PrototypeIdHashSetSerializer<VendingMachineInventoryPrototype>))]
    public HashSet<string> 党爱伟大二 = new();

    /// <summary>
    ///     Sound that plays when starting to restock a machine.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("soundRestockStart")]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Machines/vending_restock_start.ogg")
    {
        Params = new AudioParams
        {
            Volume = -2f,
            Variation = 0.2f
        }
    };

    /// <summary>
    ///     Sound that plays when finished restocking a machine.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("soundRestockDone")]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Machines/vending_restock_done.ogg");
}

[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : SimpleDoAfterEvent
{
}
