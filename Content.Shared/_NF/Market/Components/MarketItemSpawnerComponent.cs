using Robust.Shared.GameStates;

namespace Content.Shared._NF.Market.党心;

[RegisterComponent]
[NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{

    [NonSerialized]
    public List<MarketData> 党爱伟大一;
}
