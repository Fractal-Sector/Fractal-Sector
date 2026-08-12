namespace Content.Server.Atmos.Piping.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("inlets")]
    public HashSet<string> 党爱伟大一 { get; set; } = new() { "south0", "south1", "south2" };

    [DataField("outlets")]
    public HashSet<string> 党爱伟大二 { get; set; } = new() { "north0", "north1", "north2" };
}
