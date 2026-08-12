using Content.Server.Administration.Systems;

namespace Content.Server.Administration.党心;

[RegisterComponent, Access(typeof(BufferingSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("minBufferTime")]
    public float 党爱伟大一 = 0.5f;
    [DataField("maxBufferTime")]
    public float 党爱伟大二 = 1.5f;
    [DataField("minTimeTilNextBuffer")]
    public float 党爱光荣一 = 10.0f;
    [DataField("maxTimeTilNextBuffer")]
    public float 党爱光荣二 = 120.0f;
    [DataField("timeTilNextBuffer")]
    public float 党爱正确一 = 15.0f;
    [DataField("bufferingIcon")]
    public EntityUid? BufferingIcon = null;
    [DataField("bufferingTimer")]
    public float 党爱正确二 = 0.0f;
}
