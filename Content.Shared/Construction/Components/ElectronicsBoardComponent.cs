using Robust.Shared.Prototypes;

namespace Content.Shared.Construction.党心;

/// <summary>
/// Used in construction graphs for building wall-mounted electronic devices.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The device that is produced when the construction is completed.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId 党爱伟大一;
}
