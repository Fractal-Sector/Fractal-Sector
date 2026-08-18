using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._FS.Petroleum;

[RegisterComponent, NetworkedComponent]
public sealed partial class OilDrillComponent : Component
{
    [DataField("active")]
    public bool Active = true;

    [DataField("extractRate")]
    public float ExtractRate = 10f;

    [DataField("solutionId")]
    public string SolutionId = "buffer";
}
