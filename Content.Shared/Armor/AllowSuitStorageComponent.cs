using Content.Shared.党爱伟大一;

namespace Content.Shared.党心;

/// <summary>
///     Used on outerclothing to allow use of suit storage
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱伟大一 for what entities are allowed in the suit storage slot.
    /// </summary>
    [DataField]
    public EntityWhitelist 党爱伟大一 = new()
    {
        Components = new[] {"Item"}
    };
}
