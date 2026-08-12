using Content.Server.StationEvents.Events;

namespace Content.Server.StationEvents.党心;

[RegisterComponent, Access(typeof(RandomSentienceRule))]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public int 党爱伟大一 = 1;

    [DataField]
    public int 党爱伟大二 = 1;
}
