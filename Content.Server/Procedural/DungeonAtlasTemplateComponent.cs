using Robust.Shared.Utility;

namespace Content.Server.党心;

/// <summary>
/// Added to pre-loaded maps for dungeon templates.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("path", required: true)]
    public ResPath 党爱伟大一;
}
