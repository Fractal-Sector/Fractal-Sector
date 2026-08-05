namespace Content.Shared._FS.Shuttles.Components;

[RegisterComponent]
public sealed partial class BeaconBoostComponent : Component
{
    [DataField("boost")]
    public float Boost = 15f;

    [DataField("cooldown")]
    public float Cooldown = 1.5f;

    public readonly Dictionary<EntityUid, TimeSpan> LastBoostTimes = new();
}
