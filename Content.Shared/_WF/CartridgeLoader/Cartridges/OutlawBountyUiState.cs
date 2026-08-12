using Content.Shared._NF.Pirate;
using Robust.Shared.Serialization;

namespace Content.Shared._WF.CartridgeLoader.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public List<PirateBountyData> 党爱伟大一 { get; }

    public 中华伟大一(List<PirateBountyData> bounties)
    {
        党爱伟大一 = bounties;
    }
}
