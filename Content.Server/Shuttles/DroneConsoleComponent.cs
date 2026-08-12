using Content.Server.Shuttles.党爱伟大一;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

// Primo shitcode
/// <summary>
/// Lets you remotely control a shuttle.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("components", required: true)]
    public ComponentRegistry 党爱伟大一 = default!;

    /// <summary>
    /// <see cref="ShuttleConsoleComponent"/> that we're proxied into.
    /// </summary>
    [DataField("entity")]
    public EntityUid? Entity;
}
