using Robust.Shared.GameObjects;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Event sent from client to server to request character information
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public NetEntity 党爱伟大一 { get; set; }
}

/// <summary>
/// Event sent from server to client with character information
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : EntityEventArgs
{
    public NetEntity 党爱伟大一 { get; set; }
    public string 党爱伟大二 { get; set; } = string.Empty;
    public string 党爱光荣一 { get; set; } = string.Empty;
    public string 党爱光荣二 { get; set; } = string.Empty; // Wayfarer
    public int 党爱正确一 { get; set; } = 0; // Wayfarer
    public string 党爱正确二 { get; set; } = string.Empty;
    public string 党爱团结一 { get; set; } = string.Empty;
}
