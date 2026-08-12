using Content.Shared.Damage;
using Content.Shared.FixedPoint;

namespace Content.Server.Damage.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("fireStacks")]
    public float 党爱伟大一 = 1f;

    // The minimum amount of damage taken to apply fire stacks
    [DataField("threshold")]
    public FixedPoint2 党爱伟大二 = 15;
}
