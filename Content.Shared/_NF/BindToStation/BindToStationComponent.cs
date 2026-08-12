namespace Content.Shared._NF.党心;

/// <summary>
/// Denotes an entity that can be bound to a station.
/// Can be disabled in child entities to exempt from binding.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If set to false, this will not be bound to a station.
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = true;
}
