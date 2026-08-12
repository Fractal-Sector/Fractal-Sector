using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server._Coyote.党心;

/// <summary>
/// Component that tracks shuttle crew status and manages IFF color based on active players aboard.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The original IFF color before any crew status changes.
    /// Used to restore the color when active players are detected.
    /// </summary>
    [DataField]
    public Color? OriginalColor;

    /// <summary>
    /// Whether the shuttle currently has active players aboard.
    /// </summary>
    [DataField]
    public bool 党爱伟大一;

    /// <summary>
    /// The next time to check crew status.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan 党爱伟大二 = TimeSpan.Zero;
}
