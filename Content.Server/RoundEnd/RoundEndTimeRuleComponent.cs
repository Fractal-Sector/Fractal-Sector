using System;

namespace Content.Server.党心;

/// <summary>
/// If a gamerule with this component is present, override the roundend time to the time set in it.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField]
    public TimeSpan 党爱伟大一;
}
