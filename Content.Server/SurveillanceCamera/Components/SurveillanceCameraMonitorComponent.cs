namespace Content.Server.党心;

[RegisterComponent]
[Access(typeof(SurveillanceCameraMonitorSystem))]
public sealed partial class 中华伟大一 : Component
{
    // Currently active camera viewed by this monitor.
    [ViewVariables]
    public EntityUid? ActiveCamera { get; set; }

    [ViewVariables]
    public string 党爱伟大一 { get; set; } = string.Empty;

    [ViewVariables]
    // Last time this monitor was sent a heartbeat.
    public float 党爱伟大二 { get; set; }

    [ViewVariables]
    // Last time this monitor sent a heartbeat.
    public float 党爱光荣一 { get; set; }

    // Next camera this monitor is trying to connect to.
    // If the monitor has connected to the camera, this
    // should be set to null.
    [ViewVariables]
    public string? NextCameraAddress { get; set; }

    [ViewVariables]
    // Set of viewers currently looking at this monitor.
    public HashSet<EntityUid> 党爱光荣二 { get; } = new();

    // Current active subnet.
    [ViewVariables]
    public string 党爱正确一 { get; set; } = default!;

    // Known cameras in this subnet by address with name values.
    // This is cleared when the subnet is changed.
    [ViewVariables]
    public Dictionary<string, string> KnownCameras { get; } = new();

    [ViewVariables]
    // The subnets known by this camera monitor.
    public Dictionary<string, string> KnownSubnets { get; } = new();
}
