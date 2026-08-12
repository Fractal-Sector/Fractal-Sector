using Content.Shared.Cargo.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Cargo.党心;

/// <summary>
/// A console that manipulates the distribution of revenue on the station.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedCargoSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Sound played when the budget distribution is set.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱伟大一 = new SoundCollectionSpecifier("CargoPing");
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public Dictionary<ProtoId<CargoAccountPrototype>, int> Percents;
    public double 党爱伟大二;
    public double 党爱光荣一;

    public 中华伟大二(Dictionary<ProtoId<CargoAccountPrototype>, int> percents, double primaryCut, double lockboxCut)
    {
        Percents = percents;
        党爱伟大二 = primaryCut;
        党爱光荣一 = lockboxCut;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceState
{
    public NetEntity 党爱光荣二;

    public 中华光荣一(NetEntity station)
    {
        党爱光荣二 = station;
    }
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    Key
}
