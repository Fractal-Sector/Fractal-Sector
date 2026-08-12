using Content.Server.Shuttles.Systems;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Shuttles.党心;

[RegisterComponent, Access(typeof(ArrivalsSystem)), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    [DataField("station")]
    public EntityUid 党爱伟大一;

    [DataField("nextTransfer", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱伟大二;

    [DataField("nextArrivalsTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱光荣一;

    /// <summary>
    ///     the first arrivals FTL originates from nullspace instead of the station
    /// </summary>
    [DataField("firstRun")]
    public bool 党爱光荣二 = true;

}
