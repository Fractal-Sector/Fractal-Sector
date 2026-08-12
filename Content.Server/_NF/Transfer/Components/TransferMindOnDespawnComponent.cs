using Robust.Shared.Prototypes;

namespace Content.Server._NF.Transfer.党心;
/// <summary>
/// Its not fancy but it works for an in-between animations used on
/// hatching animation of the baby dragon
/// </summary>

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The entity prototype to move the mind to after the animation.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId 党爱伟大一 = default!;
}
