using Content.Server.StationEvents.Events;
using Robust.Shared.Audio;
using Robust.Shared.Collections;

namespace Content.Server.StationEvents.党心;

[RegisterComponent, Access(typeof(MassHallucinationsRule))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The maximum time between incidents in seconds
    /// </summary>
    [DataField("maxTimeBetweenIncidents", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一;

    /// <summary>
    /// The minimum time between incidents in seconds
    /// </summary>
    [DataField("minTimeBetweenIncidents", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二;

    [DataField("maxSoundDistance", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一;

    [DataField("sounds", required: true)]
    public SoundSpecifier 党爱光荣二 = default!;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public List<EntityUid> 党爱正确一 = new();
}
